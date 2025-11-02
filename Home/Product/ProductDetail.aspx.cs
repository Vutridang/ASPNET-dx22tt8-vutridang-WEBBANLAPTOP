using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebBanLapTop.Home
{
	public partial class ProductDetail : System.Web.UI.Page
	{
		private string connectionString = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				LoadCategories();
				string id = Request.QueryString["id"];
				if (!string.IsNullOrEmpty(id))
				{
					LoadProductDetail(id);
				}
			}
		}

		private void LoadCategories()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				string query = "SELECT id, category_name FROM category ORDER BY category_name";
				SqlCommand cmd = new SqlCommand(query, conn);
				conn.Open();

				SqlDataReader reader = cmd.ExecuteReader();
				rptCategories.DataSource = reader;
				rptCategories.DataBind();

				reader.Close();
			}
		}

		private void LoadProductDetail(string id)
		{
			string query = @"
        SELECT 
            p.name, 
            p.description, 
            p.price, 
            p.stock, 
            p.image_url, 
            b.name AS brand_name,
            c.category_name
        FROM product p
        LEFT JOIN brand b ON p.brand_id = b.id
        LEFT JOIN category c ON p.category_id = c.id
        WHERE p.id = @id";

			using (SqlConnection conn = new SqlConnection(connectionString))
			using (SqlCommand cmd = new SqlCommand(query, conn))
			{
				cmd.Parameters.AddWithValue("@id", id);
				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					lblName.Text = reader["name"].ToString();
					lblDescription.Text = reader["description"].ToString();
					lblPrice.Text = string.Format("{0:N0} VNĐ", reader["price"]);
					lblStock.Text = reader["stock"].ToString();
					lblBrand.Text = reader["brand_name"].ToString();
					lblCategory.Text = reader["category_name"].ToString();  // ✅ Thêm dòng này
					imgProduct.ImageUrl = reader["image_url"].ToString();
				}
			}
		}


		// 🟩 Nút Mua Ngay
		protected void btnAddToCart_Click(object sender, EventArgs e)
		{
			int productId = Convert.ToInt32(Request.QueryString["id"]);
			int quantity;
			if (!int.TryParse(txtQuantity.Text.Trim(), out quantity) || quantity < 1)
				quantity = 1;

			var addedProduct = AddToCart(productId, quantity);

			if (addedProduct != null)
			{
				string script = $@"
					Swal.fire({{
						title: 'Đã thêm vào giỏ hàng!',
						html: `
							<div style=""display:flex;align-items:center;gap:10px;justify-content:center;margin-top:10px;"">
								<img src='{addedProduct.ImageUrl}' style=""width:70px;height:70px;border-radius:8px;object-fit:cover;border:1px solid #ddd;"" />
								<div style=""text-align:left;"">
									<div style=""font-size:16px;font-weight:600;color:#198754;"">{addedProduct.Name}</div>
									<div style=""font-size:14px;color:#555;margin-top:4px;"">{addedProduct.Price:N0} ₫ × {quantity}</div>
								</div>
							</div>`,
						icon: 'success',
						showCancelButton: true,
						confirmButtonText: 'Xem giỏ hàng',
						cancelButtonText: 'Tiếp tục mua',
						confirmButtonColor: '#198754',
						cancelButtonColor: '#6c757d'
					}}).then((result) => {{
						if (result.isConfirmed) {{
							window.location.href = '/Home/Cart/Cart.aspx';
						}} else if (result.dismiss === Swal.DismissReason.cancel) {{
							// 🧩 Cập nhật giỏ hàng realtime
							let cartLabel = document.getElementById('{((SiteMaster)this.Master).CartCountClientID}');
							if (cartLabel) {{
								let current = parseInt(cartLabel.innerText) || 0;
								let newCount = current + {quantity}
								if (newCount > 9) {{
									cartLabel.innerText = ""9+"";
								}} else {{
									cartLabel.innerText = current + {quantity};
								}}			
							}}
						}}
					}});";
				ScriptManager.RegisterStartupScript(this, GetType(), "added", script, true);


			}
		}

		// 🧩 Hàm AddToCart tương tự như trong Cart.aspx.cs, chỉ khác là thêm tham số quantity
		private CartItem AddToCart(int productId, int quantity)
		{
			CartItem newItem = null;
			bool isLoggedIn = Session["UserId"] != null;
			int userId = isLoggedIn ? Convert.ToInt32(Session["UserId"]) : 0;
			List<CartItem> cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				string sql = "SELECT id, name, price, image_url FROM product WHERE id = @id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@id", productId);
				SqlDataReader reader = cmd.ExecuteReader();

				if (reader.Read())
				{
					newItem = new CartItem
					{
						ProductId = reader.GetInt32(0),
						Name = reader.GetString(1),
						Price = reader.GetDecimal(2),
						ImageUrl = reader["image_url"] != DBNull.Value ? reader["image_url"].ToString() : "/Home/images/no_image.png",
						Quantity = quantity
					};
				}
				reader.Close();

				if (newItem == null) return null;

				if (isLoggedIn)
				{
					// tìm hoặc tạo cart trong DB
					int cartId;
					string findCart = "SELECT TOP 1 id FROM cart WHERE user_id=@user_id AND is_checked_out=0";
					SqlCommand findCartCmd = new SqlCommand(findCart, conn);
					findCartCmd.Parameters.AddWithValue("@user_id", userId);
					object result = findCartCmd.ExecuteScalar();

					if (result == null)
					{
						string createCart = "INSERT INTO cart (user_id) OUTPUT INSERTED.id VALUES (@user_id)";
						SqlCommand createCmd = new SqlCommand(createCart, conn);
						createCmd.Parameters.AddWithValue("@user_id", userId);
						cartId = (int)createCmd.ExecuteScalar();
					}
					else
					{
						cartId = Convert.ToInt32(result);
					}

					// kiểm tra sản phẩm
					string findItem = "SELECT id FROM cart_item WHERE cart_id=@cart_id AND product_id=@product_id";
					SqlCommand findItemCmd = new SqlCommand(findItem, conn);
					findItemCmd.Parameters.AddWithValue("@cart_id", cartId);
					findItemCmd.Parameters.AddWithValue("@product_id", productId);
					object existing = findItemCmd.ExecuteScalar();

					if (existing != null)
					{
						string update = "UPDATE cart_item SET quantity = quantity + @qty WHERE id=@id";
						SqlCommand updateCmd = new SqlCommand(update, conn);
						updateCmd.Parameters.AddWithValue("@qty", quantity);
						updateCmd.Parameters.AddWithValue("@id", (int)existing);
						updateCmd.ExecuteNonQuery();
					}
					else
					{
						string insert = @"INSERT INTO cart_item (cart_id, product_id, quantity, price_at_time)
										  VALUES (@cart_id, @product_id, @qty, @price)";
						SqlCommand insertCmd = new SqlCommand(insert, conn);
						insertCmd.Parameters.AddWithValue("@cart_id", cartId);
						insertCmd.Parameters.AddWithValue("@product_id", productId);
						insertCmd.Parameters.AddWithValue("@qty", quantity);
						insertCmd.Parameters.AddWithValue("@price", newItem.Price);
						insertCmd.ExecuteNonQuery();
					}
				}
				else
				{
					// session cart
					var existingItem = cart.Find(x => x.ProductId == productId);
					if (existingItem != null)
						existingItem.Quantity += quantity;
					else
						cart.Add(newItem);

					Session["Cart"] = cart;
				}
			}

			return newItem;
		}
	}
}
