using System;
using System.Data.SqlClient;
using System.Configuration;

namespace WebBanLapTop.Admin.Category
{
	public partial class Create_Category : System.Web.UI.Page
	{
		private string connStr;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Chặn truy cập nếu chưa đăng nhập
			if (Session["AdminUser"] == null)
			{
				Response.Redirect("~/Admin/Login.aspx");
				return;
			}

			// Lấy connection string từ web.config và kiểm tra
			ConnectionStringSettings connSetting = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"];
			if (connSetting == null)
			{
				throw new Exception("ConnectionString 'WebBanLapTopConnection' chưa được khai báo trong web.config.");
			}
			connStr = connSetting.ConnectionString;
		}

		protected void btnCreate_Click(object sender, EventArgs e)
		{
			string categoryName = txtCategoryName.Text.Trim();

			if (string.IsNullOrEmpty(categoryName))
			{
				lblMessage.Text = "Vui lòng nhập tên danh mục.";
				return;
			}

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "INSERT INTO category (category_name) VALUES (@category_name)";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@category_name", categoryName);

				try
				{
					conn.Open();
					cmd.ExecuteNonQuery();

					// Lưu thông báo vào Session
					Session["ToastMessage"] = "Thêm danh mục thành công!";

					// Quay lại trang danh sách danh mục
					Response.Redirect("Category.aspx", false);
					Context.ApplicationInstance.CompleteRequest();
				}
				catch (Exception ex)
				{
					lblMessage.Text = "Lỗi khi thêm danh mục: " + ex.Message;
				}
			}
		}
	}
}
