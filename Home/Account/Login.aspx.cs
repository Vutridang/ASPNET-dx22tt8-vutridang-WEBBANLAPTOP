using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;

namespace WebBanLapTop.Home
{
	public partial class Login : Page
	{
		private string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Nếu đã đăng nhập thì về trang chủ
			if (Session["AdminUser"] != null || Session["CustomerUser"] != null)
			{
				Response.Redirect("~/Home/index.aspx", false);
				Context.ApplicationInstance.CompleteRequest();
				return;
			}

			// ==> LUÔN gọi ShowToastFromSession ngay (không phụ thuộc IsPostBack)
			// đảm bảo toast từ Cart (đã set Session["ToastMessage"]) được hiển thị trên Login
			if (!IsPostBack)
			{
				((Home.SiteMaster)this.Master).ShowToastFromSession(this);
			}
		}

		protected void btnLogin_Click(object sender, EventArgs e)
		{
			string username = txtUsername.Text.Trim();
			string password = txtPassword.Text.Trim();

			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				lblMessage.Text = "⚠️ Vui lòng nhập đầy đủ thông tin.";
				return;
			}

			// ✅ Mã hoá mật khẩu SHA1
			string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");

			try
			{
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					string sql = "SELECT id, role FROM [user] WHERE username=@username AND password=@password";
					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.Parameters.AddWithValue("@username", username);
					cmd.Parameters.AddWithValue("@password", hashedPassword);

					conn.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							int userId = reader.GetInt32(0);
							string role = reader["role"].ToString().Trim().ToLower();

							// ✅ Ghi session
							Session["UserId"] = userId;
							if (role == "admin")
								Session["AdminUser"] = username;
							else
								Session["CustomerUser"] = username;

							Session["ToastMessage"] = "Đăng nhập thành công!";

							// ✅ Nếu có giỏ hàng tạm (Session["Cart"]) → đồng bộ sang DB
							var cart = Session["Cart"] as List<CartItem>;
							if (cart != null && cart.Count > 0)
							{
								foreach (var item in cart)
								{
									SyncCartItemToDatabase(userId, item.ProductId, item.Quantity, item.Price);
								}
								Session["Cart"] = null; // Xoá giỏ tạm
							}

							// ✅ Chuyển hướng an toàn
							Response.Redirect("~/Home/index.aspx", false);
							Context.ApplicationInstance.CompleteRequest();
						}
						else
						{
							lblMessage.Text = "❌ Tên đăng nhập hoặc mật khẩu không đúng.";
						}
					}
				}
			}
			catch (Exception ex)
			{
				lblMessage.Text = "Lỗi kết nối: " + ex.Message;
			}
		}

		// HÀM ĐỒNG BỘ GIỎ HÀNG VÀO DATABASE

		private void SyncCartItemToDatabase(int userId, int productId, int quantity, decimal price)
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();

				// 1️⃣ Tìm cart hiện tại của user (chưa thanh toán)
				string checkCartSql = "SELECT TOP 1 id FROM cart WHERE user_id=@user_id AND is_checked_out=0";
				SqlCommand checkCartCmd = new SqlCommand(checkCartSql, conn);
				checkCartCmd.Parameters.AddWithValue("@user_id", userId);
				object result = checkCartCmd.ExecuteScalar();

				int cartId;
				if (result == null)
				{
					// 2️⃣ Nếu chưa có → tạo mới
					string createCartSql = "INSERT INTO cart (user_id) OUTPUT INSERTED.id VALUES (@user_id)";
					SqlCommand createCartCmd = new SqlCommand(createCartSql, conn);
					createCartCmd.Parameters.AddWithValue("@user_id", userId);
					cartId = (int)createCartCmd.ExecuteScalar();
				}
				else
				{
					cartId = Convert.ToInt32(result);
				}

				// 3️⃣ Kiểm tra sản phẩm đã có trong giỏ chưa
				string checkItemSql = "SELECT id FROM cart_item WHERE cart_id=@cart_id AND product_id=@product_id";
				SqlCommand checkItemCmd = new SqlCommand(checkItemSql, conn);
				checkItemCmd.Parameters.AddWithValue("@cart_id", cartId);
				checkItemCmd.Parameters.AddWithValue("@product_id", productId);
				object existingItem = checkItemCmd.ExecuteScalar();

				if (existingItem != null)
				{
					// 4️⃣ Cập nhật số lượng
					string updateSql = "UPDATE cart_item SET quantity = quantity + @quantity WHERE id=@id";
					SqlCommand updateCmd = new SqlCommand(updateSql, conn);
					updateCmd.Parameters.AddWithValue("@quantity", quantity);
					updateCmd.Parameters.AddWithValue("@id", (int)existingItem);
					updateCmd.ExecuteNonQuery();
				}
				else
				{
					// 5️⃣ Thêm mới item
					string insertSql = @"INSERT INTO cart_item (cart_id, product_id, quantity, price_at_time)
										 VALUES (@cart_id, @product_id, @quantity, @price)";
					SqlCommand insertCmd = new SqlCommand(insertSql, conn);
					insertCmd.Parameters.AddWithValue("@cart_id", cartId);
					insertCmd.Parameters.AddWithValue("@product_id", productId);
					insertCmd.Parameters.AddWithValue("@quantity", quantity);
					insertCmd.Parameters.AddWithValue("@price", price);
					insertCmd.ExecuteNonQuery();
				}
			}
		}
	}
}
