using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebBanLapTop.Home
{
	public partial class index : System.Web.UI.Page
	{
		string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
		const int PageSize = 4;

		int CurrentPageProducts
		{
			get { return ViewState["CurrentPageProducts"] == null ? 1 : (int)ViewState["CurrentPageProducts"]; }
			set { ViewState["CurrentPageProducts"] = value; }
		}
		int CurrentPageBrands
		{
			get { return ViewState["CurrentPageBrands"] == null ? 1 : (int)ViewState["CurrentPageBrands"]; }
			set { ViewState["CurrentPageBrands"] = value; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				((SiteMaster)this.Master).ShowToastFromSession(this);
				LoadFeaturedProducts();
				LoadProducts();
				LoadBrands();

				// 🟢 Nếu URL có ?add=ID → thêm sản phẩm
				if (Request.QueryString["add"] != null)
				{
					int productId;
					if (int.TryParse(Request.QueryString["add"], out productId))
					{
						var addedProduct = AddToCart(productId);
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
							let cartLabel = document.getElementById('...');
							if (cartLabel) {{
								let current = parseInt(cartLabel.innerText.replace(/\D/g, '')) || 0;
								let newCount = current + 1;
								if (newCount > 9) {{
									cartLabel.innerText = ""9+"";
								}} else {{
									cartLabel.innerText = newCount;
								}}
							}}
						}}
					}});";
							ScriptManager.RegisterStartupScript(this, GetType(), "added", script, true);
						}
					}
				}
			}
		}

		private void LoadFeaturedProducts()
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "SELECT TOP 4 id, name, description, price, image_url, stock FROM product ORDER BY created_at DESC";
				SqlCommand cmd = new SqlCommand(sql, conn);
				conn.Open();
				rptFeaturedProducts.DataSource = cmd.ExecuteReader();
				rptFeaturedProducts.DataBind();
			}
		}

		private void LoadProducts()
		{
			int startRow = (CurrentPageProducts - 1) * PageSize + 1;
			int endRow = startRow + PageSize - 1;

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = @"
				WITH ProductPaged AS (
					SELECT id, name, description, price, image_url, stock,
					ROW_NUMBER() OVER (ORDER BY created_at DESC) AS RowNum
					FROM product
				)
				SELECT * FROM ProductPaged WHERE RowNum BETWEEN @start AND @end;
				SELECT COUNT(*) FROM product;";

				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@start", startRow);
				cmd.Parameters.AddWithValue("@end", endRow);
				conn.Open();

				SqlDataReader reader = cmd.ExecuteReader();
				rptProducts.DataSource = reader;
				rptProducts.DataBind();

				if (reader.NextResult() && reader.Read())
				{
					int total = reader.GetInt32(0);
					int totalPages = (int)Math.Ceiling(total / (double)PageSize);
					lblPageProducts.Text = "Trang " + CurrentPageProducts + " / " + totalPages;
					btnPrevProducts.Enabled = CurrentPageProducts > 1;
					btnNextProducts.Enabled = CurrentPageProducts < totalPages;
				}
			}
		}

		private void LoadBrands()
		{
			int startRow = (CurrentPageBrands - 1) * PageSize + 1;
			int endRow = startRow + PageSize - 1;

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = @"
				WITH BrandPaged AS (
					SELECT id, name, logo_url, description, is_top,
					ROW_NUMBER() OVER (ORDER BY is_top DESC, id ASC) AS RowNum
					FROM brand
				)
				SELECT * FROM BrandPaged WHERE RowNum BETWEEN @start AND @end;
				SELECT COUNT(*) FROM brand;";

				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@start", startRow);
				cmd.Parameters.AddWithValue("@end", endRow);
				conn.Open();

				SqlDataReader reader = cmd.ExecuteReader();
				rptBrands.DataSource = reader;
				rptBrands.DataBind();

				if (reader.NextResult() && reader.Read())
				{
					int total = reader.GetInt32(0);
					int totalPages = (int)Math.Ceiling(total / (double)PageSize);
					lblPageBrands.Text = "Trang " + CurrentPageBrands + " / " + totalPages;
					btnPrevBrands.Enabled = CurrentPageBrands > 1;
					btnNextBrands.Enabled = CurrentPageBrands < totalPages;
				}
			}
		}

		// 🔹 Sự kiện nút phân trang
		protected void btnPrevProducts_Click(object sender, EventArgs e)
		{
			if (CurrentPageProducts > 1) CurrentPageProducts--;
			LoadProducts();
		}
		protected void btnNextProducts_Click(object sender, EventArgs e)
		{
			CurrentPageProducts++;
			LoadProducts();
		}
		protected void btnPrevBrands_Click(object sender, EventArgs e)
		{
			if (CurrentPageBrands > 1) CurrentPageBrands--;
			LoadBrands();
		}
		protected void btnNextBrands_Click(object sender, EventArgs e)
		{
			CurrentPageBrands++;
			LoadBrands();
		}

		// 🧩 Thêm sản phẩm vào giỏ hàng (Session hoặc DB)
		// 🧩 Thêm sản phẩm vào giỏ hàng (Session hoặc DB)
		private CartItem AddToCart(int productId)
		{
			bool isLoggedIn = Session["UserId"] != null;
			int userId = isLoggedIn ? Convert.ToInt32(Session["UserId"]) : 0;
			List<CartItem> cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
			CartItem newItem = null;

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();

				// 🔹 Lấy thông tin sản phẩm
				string sql = "SELECT id, name, price, image_url, stock FROM product WHERE id=@id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@id", productId);
				SqlDataReader reader = cmd.ExecuteReader();

				int stock = 0;
				if (reader.Read())
				{
					stock = reader.GetInt32(4);
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

				if (newItem == null || stock <= 0)
					return null; // ❌ Hết hàng hoặc không tìm thấy sản phẩm

				if (isLoggedIn)
				{
					// 🔹 Lấy hoặc tạo cart_id
					int cartId;
					string findCart = "SELECT TOP 1 id FROM cart WHERE user_id=@uid AND is_checked_out=0";
					SqlCommand findCmd = new SqlCommand(findCart, conn);
					findCmd.Parameters.AddWithValue("@uid", userId);
					object result = findCmd.ExecuteScalar();

					if (result == null)
					{
						var createCmd = new SqlCommand("INSERT INTO cart (user_id) OUTPUT INSERTED.id VALUES (@uid)", conn);
						createCmd.Parameters.AddWithValue("@uid", userId);
						cartId = (int)createCmd.ExecuteScalar();
					}
					else
					{
						cartId = Convert.ToInt32(result);
					}

					// 🔹 Kiểm tra xem sản phẩm đã có trong giỏ chưa
					string checkItem = "SELECT id FROM cart_item WHERE cart_id=@c AND product_id=@p";
					SqlCommand checkCmd = new SqlCommand(checkItem, conn);
					checkCmd.Parameters.AddWithValue("@c", cartId);
					checkCmd.Parameters.AddWithValue("@p", productId);
					object exist = checkCmd.ExecuteScalar();

					if (exist != null)
					{
						// Tăng số lượng trong giỏ
						SqlCommand updateCmd = new SqlCommand("UPDATE cart_item SET quantity = quantity + 1 WHERE id=@id", conn);
						updateCmd.Parameters.AddWithValue("@id", (int)exist);
						updateCmd.ExecuteNonQuery();
					}
					else
					{
						// Thêm mới vào giỏ
						SqlCommand insertCmd = new SqlCommand("INSERT INTO cart_item (cart_id, product_id, quantity, price_at_time) VALUES (@c, @p, 1, @price)", conn);
						insertCmd.Parameters.AddWithValue("@c", cartId);
						insertCmd.Parameters.AddWithValue("@p", productId);
						insertCmd.Parameters.AddWithValue("@price", newItem.Price);
						insertCmd.ExecuteNonQuery();
					}

					// 🔻 Giảm tồn kho
					SqlCommand updateStockCmd = new SqlCommand("UPDATE product SET stock = stock - 1 WHERE id = @id AND stock > 0", conn);
					updateStockCmd.Parameters.AddWithValue("@id", productId);
					updateStockCmd.ExecuteNonQuery();
				}
				else
				{
					// 🔹 Nếu chưa login → session cart
					var existing = cart.Find(x => x.ProductId == productId);
					if (existing != null)
						existing.Quantity += 1;
					else
						cart.Add(newItem);
					Session["Cart"] = cart;
				}
			}

			return newItem;
		}




		protected string RenderAddToCartButton(int id, int stock, string containerId)
		{
			if (stock > 0)
				return "<a href='/Home/Cart/Cart.aspx?add=" + id + "' onclick='event.preventDefault(); addToCart(" + id + ");' style=\"background:#198754;color:white;padding:6px 12px;border-radius:4px;\">Giỏ hàng</a>";
			else
				return "<button disabled style=\"background:#ccc;color:#666;padding:6px 12px;border-radius:4px;\">Hết hàng</button>";
		}
	}
}
