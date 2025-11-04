using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Net;
using System.Net.Mail;

namespace WebBanLapTop.Home.Cart
{
	public partial class Checkout : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Session["UserId"] == null)
				{
					Response.Redirect("/Home/Account/Login.aspx");
					return;
				}

				int userId = Convert.ToInt32(Session["UserId"]);
				LoadUserInfo(userId);
			}
		}

		private void LoadUserInfo(int userId)
		{
			string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();
				string sql = "SELECT username, email, role FROM [user] WHERE id = @id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@id", userId);
				SqlDataReader reader = cmd.ExecuteReader();

				if (reader.Read())
				{
					txtUsername.Text = reader["username"].ToString();
					txtEmail.Text = reader["email"].ToString();
					txtRole.Text = reader["role"].ToString();
				}
			}
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				int userId = Convert.ToInt32(Session["UserId"]);

				// 1️⃣ Tạo đơn hàng
				int orderId = CreateOrder(userId);

				// 2️⃣ Lưu thông tin user_detail
				InsertUserDetail(userId, orderId);

				// 3️⃣ Lấy sản phẩm và update stock (dùng out)
				DataTable dtItems;
				decimal totalAmount;
				AddOrderItemsAndUpdateStock(userId, orderId, out dtItems, out totalAmount);

				// 4️⃣ Đánh dấu cart đã thanh toán
				MarkCartAsCheckedOut(userId);

				// 5️⃣ Gửi mail xác nhận có danh sách sản phẩm
				SendMail(txtEmail.Text, txtUsername.Text, txtAddress.Text, ddlPayment.SelectedValue, dtItems, totalAmount);

				// 6️⃣ Hiển thị thông báo và redirect
				string script = @"Swal.fire({
                icon: 'success',
                title: 'Đặt hàng thành công!',
                showConfirmButton: false,
                timer: 2000
            }).then(() => { window.location='/Home/index.aspx'; });";

				ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMsg", script, true);
			}
		}



		private int CreateOrder(int userId)
		{
			int newOrderId = 0;
			string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();

				string sql = @"
                    INSERT INTO [order] (user_id, total_amount)
                    OUTPUT INSERTED.id
                    VALUES (@user_id, 0);
                ";

				using (SqlCommand cmd = new SqlCommand(sql, conn))
				{
					cmd.Parameters.AddWithValue("@user_id", userId);
					newOrderId = Convert.ToInt32(cmd.ExecuteScalar());
				}
			}

			return newOrderId;
		}

		private void InsertUserDetail(int userId, int orderId)
		{
			string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();
				string sql = @"
                    INSERT INTO user_detail (user_id, order_id, address, zipcode, payment_method, created_at, updated_at)
                    VALUES (@user_id, @order_id, @address, @zipcode, @payment, GETDATE(), GETDATE());
                ";

				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@user_id", userId);
				cmd.Parameters.AddWithValue("@order_id", orderId);
				cmd.Parameters.AddWithValue("@address", txtAddress.Text);
				cmd.Parameters.AddWithValue("@zipcode", txtZipcode.Text);
				cmd.Parameters.AddWithValue("@payment", ddlPayment.SelectedValue);
				cmd.ExecuteNonQuery();
			}
		}

		private void AddOrderItemsAndUpdateStock(int userId, int orderId, out DataTable orderItems, out decimal total)
		{
			string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
			orderItems = new DataTable();
			total = 0m;

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();

				// 1) Lấy cart_id hiện tại (chưa checkout)
				string getCartSql = "SELECT TOP 1 id FROM cart WHERE user_id = @uid AND is_checked_out = 0";
				SqlCommand getCartCmd = new SqlCommand(getCartSql, conn);
				getCartCmd.Parameters.AddWithValue("@uid", userId);

				object cartIdObj = getCartCmd.ExecuteScalar();
				if (cartIdObj == null) return; // Không có giỏ hàng

				int cartId = Convert.ToInt32(cartIdObj);

				// 2) Lấy danh sách sản phẩm trong cart_item + tên sản phẩm
				string getItemsSql = @"
            SELECT ci.product_id, p.name AS product_name, ci.quantity, ci.price_at_time AS price
            FROM cart_item ci
            INNER JOIN product p ON ci.product_id = p.id
            WHERE ci.cart_id = @cartId";
				SqlCommand getItemsCmd = new SqlCommand(getItemsSql, conn);
				getItemsCmd.Parameters.AddWithValue("@cartId", cartId);

				using (SqlDataReader reader = getItemsCmd.ExecuteReader())
				{
					orderItems.Load(reader);
				}

				// 3) Duyệt từng sản phẩm
				foreach (DataRow row in orderItems.Rows)
				{
					int productId = Convert.ToInt32(row["product_id"]);
					int quantity = Convert.ToInt32(row["quantity"]);
					decimal price = Convert.ToDecimal(row["price"]);
					total += price * quantity;

					// Thêm vào order_item
					string insertOrderItem = @"
                INSERT INTO order_item (order_id, product_id, quantity, price)
                VALUES (@order_id, @product_id, @quantity, @price)";
					using (SqlCommand insertCmd = new SqlCommand(insertOrderItem, conn))
					{
						insertCmd.Parameters.AddWithValue("@order_id", orderId);
						insertCmd.Parameters.AddWithValue("@product_id", productId);
						insertCmd.Parameters.AddWithValue("@quantity", quantity);
						insertCmd.Parameters.AddWithValue("@price", price);
						insertCmd.ExecuteNonQuery();
					}

					// Trừ stock (nên kiểm tra stock >= quantity nếu cần)
					string updateStock = "UPDATE product SET stock = stock - @quantity WHERE id = @pid";
					using (SqlCommand stockCmd = new SqlCommand(updateStock, conn))
					{
						stockCmd.Parameters.AddWithValue("@quantity", quantity);
						stockCmd.Parameters.AddWithValue("@pid", productId);
						stockCmd.ExecuteNonQuery();
					}
				}

				// Cập nhật tổng tiền đơn hàng
				string updateOrder = "UPDATE [order] SET total_amount = @total, updated_at = GETDATE() WHERE id = @oid";
				using (SqlCommand updateCmd = new SqlCommand(updateOrder, conn))
				{
					updateCmd.Parameters.AddWithValue("@total", total);
					updateCmd.Parameters.AddWithValue("@oid", orderId);
					updateCmd.ExecuteNonQuery();
				}

				// Xóa cart_item
				string deleteCartItems = "DELETE FROM cart_item WHERE cart_id = @cid";
				using (SqlCommand delCmd = new SqlCommand(deleteCartItems, conn))
				{
					delCmd.Parameters.AddWithValue("@cid", cartId);
					delCmd.ExecuteNonQuery();
				}
			}
		}



		private void MarkCartAsCheckedOut(int userId)
		{
			string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();
				string sql = "UPDATE cart SET is_checked_out = 1, updated_at = GETDATE() WHERE user_id = @uid AND is_checked_out = 0";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@uid", userId);
				cmd.ExecuteNonQuery();
			}
		}

		private string GenerateOrderEmailHtml(string username, string address, string paymentMethod, DataTable orderItems, decimal totalAmount)
		{
			string rowsHtml = "";
			foreach (DataRow row in orderItems.Rows)
			{
				string productName = row["product_name"].ToString();
				int quantity = Convert.ToInt32(row["quantity"]);
				decimal price = Convert.ToDecimal(row["price"]);

				rowsHtml += $@"
            <tr style='background:#f8f9fa; text-align:center;'>
                <td style='padding:10px; border:1px solid #dee2e6;'>{productName}</td>
                <td style='padding:10px; border:1px solid #dee2e6;'>{quantity}</td>
                <td style='padding:10px; border:1px solid #dee2e6;'>{price:n0} ₫</td>
            </tr>";
			}

			return $@"
    <html>
    <head>
        <meta charset='UTF-8'>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f5f7fa;
                color: #333;
                line-height: 1.6;
                padding: 20px;
            }}
            .container {{
                max-width: 700px;
                margin: 0 auto;
                background: #fff;
                border-radius: 10px;
                box-shadow: 0 3px 10px rgba(0,0,0,0.1);
                overflow: hidden;
            }}
            .header {{
                background-color: #198754;
                color: #fff;
                text-align: center;
                padding: 20px 0;
            }}
            .header h2 {{
                margin: 0;
            }}
            .content {{
                padding: 25px 35px;
            }}
            .info-box {{
                background: #f8f9fa;
                border: 1px solid #dee2e6;
                border-radius: 8px;
                padding: 15px 20px;
                margin-bottom: 20px;
            }}
            .info-box p {{
                margin: 5px 0;
            }}
            .info-box strong {{
                color: #198754;
            }}
            table {{
                width: 100%;
                border-collapse: collapse;
                margin-top: 15px;
                font-size: 15px;
            }}
            th {{
                background: #198754;
                color: white;
                padding: 10px;
                border: 1px solid #dee2e6;
            }}
            td {{
                border: 1px solid #dee2e6;
                padding: 10px;
                text-align: center;
            }}
            .total {{
                text-align: right;
                padding: 15px 10px;
                font-size: 16px;
                font-weight: bold;
                color: #198754;
            }}
            .footer {{
                background-color: #f1f1f1;
                text-align: center;
                padding: 15px;
                font-size: 13px;
                color: #666;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h2>✅ Xác nhận đơn hàng thành công</h2>
            </div>
            <div class='content'>
                <p>Xin chào <strong>{username}</strong>,</p>
                <p>Cảm ơn bạn đã mua hàng tại <b>Web Bán Laptop</b>! Đơn hàng của bạn đã được xác nhận thành công.</p>

                <div class='info-box'>
                    <p><strong>Địa chỉ giao hàng:</strong> {address}</p>
                    <p><strong>Phương thức thanh toán:</strong> {paymentMethod}</p>
                    <p><strong>Ngày đặt hàng:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</p>
                </div>

                <table>
                    <tr>
                        <th>Sản phẩm</th>
                        <th>Số lượng</th>
                        <th>Giá</th>
                    </tr>
                    {rowsHtml}
                </table>

                <div class='total'>
                    Tổng cộng: {totalAmount:n0} ₫
                </div>

                <p style='margin-top:20px;'>Chúng tôi sẽ liên hệ với bạn khi đơn hàng được giao cho đơn vị vận chuyển.</p>
                <p>Trân trọng,<br/><b>Đội ngũ Web Bán Laptop</b></p>
            </div>
            <div class='footer'>
                © {DateTime.Now.Year} Web Bán Laptop. Mọi quyền được bảo lưu.
            </div>
        </div>
    </body>
    </html>";
		}


		private void SendMail(string toEmail, string username, string address, string paymentMethod, DataTable orderItems, decimal totalAmount)
		{
			try
			{
				System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

				string bodyHtml = GenerateOrderEmailHtml(username, address, paymentMethod, orderItems, totalAmount);

				MailMessage mail = new MailMessage();
				mail.From = new MailAddress("no-reply@webbanlaptop.com", "Web Bán Laptop");
				mail.To.Add(toEmail);
				mail.Subject = "Xác nhận đơn hàng thành công";
				mail.Body = bodyHtml;
				mail.IsBodyHtml = true;

				var client = new SmtpClient("smtp.mailtrap.io", 587)
				{
					Credentials = new NetworkCredential("7f27b16b4de167", "6825ccb974ca89"),
					EnableSsl = true
				};

				client.Send(mail);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Lỗi gửi mail: " + ex.Message);
			}
		}

	}
}
