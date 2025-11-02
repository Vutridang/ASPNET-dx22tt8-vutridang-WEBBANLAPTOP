using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBanLapTop;

namespace WebBanLapTop
{
	public partial class Cart : System.Web.UI.Page
	{
		private string connectionString =
			System.Configuration.ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				bool isLoggedIn = Session["UserId"] != null;

				// 🧩 Nếu user đã đăng nhập → load từ DB
				if (isLoggedIn)
				{
					int userId = Convert.ToInt32(Session["UserId"]);
					List<CartItem> dbCart = new List<CartItem>();

					using (SqlConnection conn = new SqlConnection(connectionString))
					{
						string query = @"SELECT p.id, p.name, p.image_url, ci.price_at_time, ci.quantity
                                 FROM cart_item ci
                                 JOIN cart c ON ci.cart_id = c.id
                                 JOIN product p ON ci.product_id = p.id
                                 WHERE c.user_id = @user_id AND c.is_checked_out = 0";
						SqlCommand cmd = new SqlCommand(query, conn);
						cmd.Parameters.AddWithValue("@user_id", userId);
						conn.Open();
						SqlDataReader reader = cmd.ExecuteReader();

						while (reader.Read())
						{
							dbCart.Add(new CartItem
							{
								ProductId = reader.GetInt32(0),
								Name = reader.GetString(1),
								ImageUrl = reader["image_url"].ToString(),
								Price = reader.GetDecimal(3),
								Quantity = reader.GetInt32(4)
							});
						}
					}

					Session["Cart"] = dbCart;
				}

				LoadCart();
				((Home.SiteMaster)this.Master).ShowToastFromSession(this);
			}
		}


		private CartItem AddToCart(int productId)
		{
			bool isLoggedIn = Session["UserId"] != null;
			int userId = isLoggedIn ? Convert.ToInt32(Session["UserId"]) : 0;

			List<CartItem> cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

			if (!isLoggedIn)
			{
				var existing = cart.FirstOrDefault(x => x.ProductId == productId);
				if (existing != null)
				{
					existing.Quantity += 1;
					Session["Cart"] = cart;
					return existing;
				}
			}

			CartItem newItem = null;

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				string query = "SELECT id, name, price, image_url FROM product WHERE id = @id";
				SqlCommand cmd = new SqlCommand(query, conn);
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
						Quantity = 1
					};
				}
				reader.Close();

				if (isLoggedIn && newItem != null)
				{
					int cartId;
					string checkCart = "SELECT TOP 1 id FROM cart WHERE user_id = @user_id AND is_checked_out = 0";
					SqlCommand checkCmd = new SqlCommand(checkCart, conn);
					checkCmd.Parameters.AddWithValue("@user_id", userId);
					object result = checkCmd.ExecuteScalar();

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

					string checkItem = "SELECT id FROM cart_item WHERE cart_id = @cart_id AND product_id = @product_id";
					SqlCommand checkItemCmd = new SqlCommand(checkItem, conn);
					checkItemCmd.Parameters.AddWithValue("@cart_id", cartId);
					checkItemCmd.Parameters.AddWithValue("@product_id", productId);
					object existItem = checkItemCmd.ExecuteScalar();

					if (existItem != null)
					{
						string update = "UPDATE cart_item SET quantity = quantity + 1 WHERE id = @id";
						SqlCommand updateCmd = new SqlCommand(update, conn);
						updateCmd.Parameters.AddWithValue("@id", (int)existItem);
						updateCmd.ExecuteNonQuery();
					}
					else
					{
						string insert = "INSERT INTO cart_item (cart_id, product_id, quantity, price_at_time) VALUES (@cart_id, @product_id, 1, @price)";
						SqlCommand insertCmd = new SqlCommand(insert, conn);
						insertCmd.Parameters.AddWithValue("@cart_id", cartId);
						insertCmd.Parameters.AddWithValue("@product_id", productId);
						insertCmd.Parameters.AddWithValue("@price", newItem.Price);
						insertCmd.ExecuteNonQuery();
					}
				}
				else if (!isLoggedIn && newItem != null)
				{
					cart.Add(newItem);
					Session["Cart"] = cart;
				}
			}

			return newItem;
		}

		private void LoadCart()
		{
			var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

			gvCart.DataSource = cart;
			gvCart.DataBind();

			if (cart.Count > 0)
			{
				pnlTotal.Visible = true;

				decimal subtotal = cart.Sum(x => x.Price * x.Quantity);
				decimal vat = subtotal * 0.10m;
				decimal grandTotal = subtotal + vat;

				lblSubTotal.Text = subtotal.ToString("N0") + " VNĐ";
				lblVAT.Text = vat.ToString("N0") + " VNĐ";
				lblGrandTotal.Text = grandTotal.ToString("N0") + " VNĐ";
			}
			else
			{
				pnlTotal.Visible = false;
			}
		}



		protected void gvCart_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (Session["Cart"] == null) return;
			var cart = (List<CartItem>)Session["Cart"];
			int productId = Convert.ToInt32(e.CommandArgument);

			string toastMessage = "";

			if (e.CommandName == "UpdateItem")
			{
				string qtyKey = "qty_" + productId;
				string newQtyValue = Request.Form[qtyKey];
				int newQty;

				if (int.TryParse(newQtyValue, out newQty) && newQty > 0)
				{
					var item = cart.FirstOrDefault(x => x.ProductId == productId);
					if (item != null)
					{
						item.Quantity = newQty;

						if (Session["UserId"] != null)
							UpdateCartItemInDatabase(Convert.ToInt32(Session["UserId"]), productId, newQty);

						toastMessage = "Cập nhật số lượng thành công!";
					}
				}
				else
				{
					toastMessage = "Số lượng không hợp lệ!";
				}
			}
			else if (e.CommandName == "RemoveItem")
			{
				cart.RemoveAll(x => x.ProductId == productId);

				if (Session["UserId"] != null)
					DeleteCartItemInDatabase(Convert.ToInt32(Session["UserId"]), productId);

				toastMessage = "Đã xóa sản phẩm khỏi giỏ hàng!";
			}

			Session["Cart"] = cart;
			LoadCart();

			// ✅ Lưu thông báo để hiển thị toast sau khi reload
			if (!string.IsNullOrEmpty(toastMessage))
			{
				Session["ToastMessage"] = toastMessage;
				Response.Redirect(Request.RawUrl, false); // 🔥 reload lại trang
				Context.ApplicationInstance.CompleteRequest();
			}
		}


		private void UpdateCartItemInDatabase(int userId, int productId, int newQty)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				string query = @"UPDATE ci
                                 SET ci.quantity = @qty
                                 FROM cart_item ci
                                 INNER JOIN cart c ON ci.cart_id = c.id
                                 WHERE c.user_id = @user_id AND ci.product_id = @product_id AND c.is_checked_out = 0";
				SqlCommand cmd = new SqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@qty", newQty);
				cmd.Parameters.AddWithValue("@user_id", userId);
				cmd.Parameters.AddWithValue("@product_id", productId);
				cmd.ExecuteNonQuery();
			}
		}

		private void DeleteCartItemInDatabase(int userId, int productId)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				string query = @"DELETE ci
                                 FROM cart_item ci
                                 INNER JOIN cart c ON ci.cart_id = c.id
                                 WHERE c.user_id = @user_id AND ci.product_id = @product_id AND c.is_checked_out = 0";
				SqlCommand cmd = new SqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@user_id", userId);
				cmd.Parameters.AddWithValue("@product_id", productId);
				cmd.ExecuteNonQuery();
			}
		}
	}
}
