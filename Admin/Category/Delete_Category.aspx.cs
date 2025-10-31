using System;
using System.Data.SqlClient;
using System.Configuration;

namespace WebBanLapTop.Admin.Category
{
	public partial class Delete_Category : System.Web.UI.Page
	{
		// Khai báo connection string cấp lớp
		string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Chặn truy cập nếu chưa đăng nhập
			if (Session["AdminUser"] == null)
			{
				Response.Redirect("~/Admin/Login.aspx");
				return;
			}

			int categoryId;
			if (!int.TryParse(Request.QueryString["id"], out categoryId))
			{
				Response.Redirect("Category.aspx");
				return;
			}

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "DELETE FROM category WHERE id = @id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@id", categoryId);
				conn.Open();
				cmd.ExecuteNonQuery();
			}

			// Lưu thông báo vào Session
			Session["ToastMessage"] = "Xóa danh mục thành công!";

			// Quay lại trang danh sách danh mục
			Response.Redirect("Category.aspx", false);
			Context.ApplicationInstance.CompleteRequest();
		}
	}
}
