using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace WebBanLapTop.Admin.Product
{
	public partial class Edit_Product : System.Web.UI.Page
	{
		private string connStr;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["AdminUser"] == null)
			{
				Response.Redirect("~/Admin/Login.aspx");
				return;
			}

			ConnectionStringSettings connSetting = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"];
			if (connSetting == null)
				throw new Exception("ConnectionString 'WebBanLapTopConnection' chưa được khai báo trong web.config.");
			connStr = connSetting.ConnectionString;

			if (!IsPostBack)
			{
				LoadCategories();
				LoadBrands();
				LoadProduct();
			}
		}

		private void LoadCategories()
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "SELECT id, category_name FROM category ORDER BY category_name";
				SqlDataAdapter da = new SqlDataAdapter(sql, conn);
				DataTable dt = new DataTable();
				da.Fill(dt);

				ddlCategory.DataSource = dt;
				ddlCategory.DataTextField = "category_name";
				ddlCategory.DataValueField = "id";
				ddlCategory.DataBind();
			}
		}

		private void LoadBrands()
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "SELECT id, name FROM brand ORDER BY name";
				SqlCommand cmd = new SqlCommand(sql, conn);
				conn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				ddlBrand.DataSource = dr;
				ddlBrand.DataTextField = "name";
				ddlBrand.DataValueField = "id";
				ddlBrand.DataBind();
				dr.Close();
			}
		}

		private void LoadProduct()
		{
			if (!int.TryParse(Request.QueryString["id"], out int productId))
			{
				Response.Redirect("Product.aspx");
				return;
			}

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "SELECT * FROM product WHERE id=@id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@id", productId);

				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					txtName.Text = reader["name"].ToString();
					txtDescription.Text = reader["description"].ToString();
					txtPrice.Text = reader["price"].ToString();
					txtStock.Text = reader["stock"].ToString();
					ddlCategory.SelectedValue = reader["category_id"].ToString();
					ddlBrand.SelectedValue = reader["brand_id"].ToString();
					string imageUrl = reader["image_url"].ToString();
					if (!string.IsNullOrEmpty(imageUrl))
					{
						imgCurrent.ImageUrl = imageUrl;
					}
				}
				else
				{
					Response.Redirect("Product.aspx");
				}
			}
		}

		protected void btnUpdate_Click(object sender, EventArgs e)
		{
			if (!int.TryParse(Request.QueryString["id"], out int productId))
			{
				Response.Redirect("Product.aspx");
				return;
			}

			string name = txtName.Text.Trim();
			string description = txtDescription.Text.Trim();
			string priceText = txtPrice.Text.Trim();
			string stockText = txtStock.Text.Trim();
			int brandId = int.Parse(ddlBrand.SelectedValue);
			int categoryId = int.Parse(ddlCategory.SelectedValue);

			if (name == "" || priceText == "" || stockText == "")
			{
				lblMessage.Text = "Vui lòng điền đầy đủ thông tin.";
				return;
			}

			if (!decimal.TryParse(priceText, out decimal price))
			{
				lblMessage.Text = "Giá không hợp lệ.";
				return;
			}

			if (!int.TryParse(stockText, out int stock))
			{
				lblMessage.Text = "Tồn kho không hợp lệ.";
				return;
			}

			string newImageUrl = null;

			// Xử lý upload hình mới
			if (fileUpload.HasFile)
			{
				string fileName = Path.GetFileName(fileUpload.PostedFile.FileName);
				string savePath = Server.MapPath("~/images/products/") + fileName;

				// Lấy image cũ để xóa
				string oldImageUrl = null;
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					string sqlGet = "SELECT image_url FROM product WHERE id=@id";
					SqlCommand cmdGet = new SqlCommand(sqlGet, conn);
					cmdGet.Parameters.AddWithValue("@id", productId);
					conn.Open();
					object result = cmdGet.ExecuteScalar();
					oldImageUrl = result?.ToString();
				}

				try
				{
					fileUpload.SaveAs(savePath);
					newImageUrl = "/images/products/" + fileName;

					// Xóa hình cũ
					if (!string.IsNullOrEmpty(oldImageUrl))
					{
						string oldFilePath = Server.MapPath(oldImageUrl);
						if (File.Exists(oldFilePath))
						{
							File.Delete(oldFilePath);
						}
					}
				}
				catch (Exception ex)
				{
					lblMessage.Text = "Lỗi upload hình: " + ex.Message;
					return;
				}
			}

			// Update product
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = @"UPDATE product SET 
						category_id=@category_id,
						brand_id=@brand_id,
						name=@name,
						description=@description,
						price=@price,
						stock=@stock,
						updated_at=GETDATE()";

				if (newImageUrl != null)
					sql += ", image_url=@image_url";

				sql += " WHERE id=@id";

				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@id", productId);
				cmd.Parameters.AddWithValue("@category_id", categoryId);
				cmd.Parameters.AddWithValue("@brand_id", brandId);
				cmd.Parameters.AddWithValue("@name", name);
				cmd.Parameters.AddWithValue("@description", description);
				cmd.Parameters.AddWithValue("@price", price);
				cmd.Parameters.AddWithValue("@stock", stock);

				if (newImageUrl != null)
					cmd.Parameters.AddWithValue("@image_url", newImageUrl);

				try
				{
					conn.Open();
					cmd.ExecuteNonQuery();
					// Lưu thông báo vào Session
					Session["ToastMessage"] = "Cập nhật sản phẩm thành công!";

					// Redirect sang User.aspx
					Response.Redirect("Product.aspx", false);
					Context.ApplicationInstance.CompleteRequest();
				}
				catch (Exception ex)
				{
					lblMessage.Text = "Lỗi: " + ex.Message;
				}
			}
		}
	}
}
