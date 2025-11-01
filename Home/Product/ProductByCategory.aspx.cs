using System;
using System.Data.SqlClient;
using System.Configuration;

namespace WebBanLapTop.Home
{
	public partial class ProductByCategory : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				LoadProductsByCategory();
			}
		}

		private void LoadProductsByCategory()
		{
			string catId = Request.QueryString["catid"];
			if (string.IsNullOrEmpty(catId))
			{
				lblNoResults.Text = "Không tìm thấy danh mục hợp lệ.";
				lblNoResults.Visible = true;
				return;
			}

			string connectionString = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();

				// Lấy tên danh mục
				string catNameQuery = "SELECT category_name FROM category WHERE id = @id";
				SqlCommand catCmd = new SqlCommand(catNameQuery, conn);
				catCmd.Parameters.AddWithValue("@id", catId);

				object catName = catCmd.ExecuteScalar();
				if (catName != null)
				{
					lblCategoryName.Text = catName.ToString();
				}
				else
				{
					lblCategoryName.Text = "Danh mục không tồn tại";
					lblNoResults.Visible = true;
					return;
				}

				// Lấy danh sách sản phẩm thuộc danh mục
				string query = "SELECT id, name, price, image_url FROM product WHERE category_id = @id";
				SqlCommand cmd = new SqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@id", catId);

				SqlDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					rptProductsByCategory.DataSource = reader;
					rptProductsByCategory.DataBind();
				}
				else
				{
					lblNoResults.Visible = true;
				}

				reader.Close();
			}
		}
	}
}
