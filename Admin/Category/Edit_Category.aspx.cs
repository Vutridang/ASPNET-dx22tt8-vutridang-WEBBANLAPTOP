using System;
using System.Data.SqlClient;
using System.Configuration;

namespace WebBanLapTop.Admin.Category
{
	public partial class Edit_Category : System.Web.UI.Page
	{
		protected int categoryId;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Chặn truy cập nếu chưa đăng nhập
			if (Session["AdminUser"] == null)
			{
				Response.Redirect("~/Admin/Login.aspx");
				return;
			}

			// Lấy ID từ query string
			if (!int.TryParse(Request.QueryString["id"], out categoryId))
			{
				Response.Redirect("Category.aspx");
				return;
			}

			if (!IsPostBack)
			{
				LoadCategory();
			}
		}

		private void LoadCategory()
		{
			string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "SELECT * FROM category WHERE id=@id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@id", categoryId);

				conn.Open();
				SqlDataReader dr = cmd.ExecuteReader();

				if (dr.Read())
				{
					txtCategoryName.Text = dr["category_name"].ToString();
					pnlForm.Visible = true;
				}
				else
				{
					Response.Redirect("Category.aspx");
				}
			}
		}

		protected void btnUpdate_Click(object sender, EventArgs e)
		{
			string categoryName = txtCategoryName.Text.Trim();

			if (string.IsNullOrEmpty(categoryName))
			{
				lblMessage.Text = "Vui lòng nhập tên danh mục.";
				return;
			}

			string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "UPDATE category SET category_name=@category_name, updated_at=GETDATE() WHERE id=@id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@category_name", categoryName);
				cmd.Parameters.AddWithValue("@id", categoryId);

				try
				{
					conn.Open();
					cmd.ExecuteNonQuery();
					Response.Redirect("Category.aspx");
				}
				catch (Exception ex)
				{
					lblMessage.Text = "Lỗi khi cập nhật danh mục: " + ex.Message;
				}
			}
		}
	}
}
