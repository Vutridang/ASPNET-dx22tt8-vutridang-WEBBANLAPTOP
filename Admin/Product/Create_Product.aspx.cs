using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace WebBanLapTop.Admin.Product
{
	public partial class Create_Product : System.Web.UI.Page
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
				LoadCategories(); // Load danh mục đúng
			}
		}


		private void LoadCategories()
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					string sql = "SELECT id, category_name FROM category ORDER BY category_name";
					SqlDataAdapter da = new SqlDataAdapter(sql, conn);
					DataTable dt = new DataTable();
					da.Fill(dt);

					ddlCategory.DataSource = dt;
					ddlCategory.DataTextField = "category_name"; // Hiển thị tên danh mục
					ddlCategory.DataValueField = "id";           // Lưu giá trị id
					ddlCategory.DataBind();
				}
			}
			catch (Exception ex)
			{
				lblMessage.Text = "Lỗi load danh mục: " + ex.Message;
			}
		}


		protected void btnCreate_Click(object sender, EventArgs e)
		{
			string name = txtName.Text.Trim();
			string description = txtDescription.Text.Trim();
			string priceText = txtPrice.Text.Trim();
			string stockText = txtStock.Text.Trim();
			int categoryId = int.Parse(ddlCategory.SelectedValue);

			if (name == "" || priceText == "" || stockText == "")
			{
				lblMessage.Text = "Vui lòng điền đầy đủ thông tin.";
				return;
			}

			decimal price;
			int stock;

			if (!decimal.TryParse(priceText, out price))
			{
				lblMessage.Text = "Giá không hợp lệ.";
				return;
			}

			if (!int.TryParse(stockText, out stock))
			{
				lblMessage.Text = "Tồn kho không hợp lệ.";
				return;
			}

			// Xử lý hình ảnh
			string imageUrl = null;
			if (fileUpload.HasFile)
			{
				string fileName = Path.GetFileName(fileUpload.PostedFile.FileName);
				string savePath = Server.MapPath("~/images/") + fileName;
				try
				{
					fileUpload.SaveAs(savePath);
					imageUrl = "/images/" + fileName;
				}
				catch (Exception ex)
				{
					lblMessage.Text = "Lỗi upload hình: " + ex.Message;
					return;
				}
			}

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "INSERT INTO product (category_id, name, description, price, stock, image_url) " +
							 "VALUES (@category_id, @name, @description, @price, @stock, @image_url)";

				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@category_id", categoryId);
				cmd.Parameters.AddWithValue("@name", name);
				cmd.Parameters.AddWithValue("@description", description);
				cmd.Parameters.AddWithValue("@price", price);
				cmd.Parameters.AddWithValue("@stock", stock);
				cmd.Parameters.AddWithValue("@image_url", (object)imageUrl ?? DBNull.Value);

				try
				{
					conn.Open();
					cmd.ExecuteNonQuery();
					// Lưu thông báo vào Session
					Session["ToastMessage"] = "Thêm sản phẩm thành công!";

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
