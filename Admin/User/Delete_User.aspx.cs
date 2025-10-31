using System;
using System.Data.SqlClient;
using System.Configuration;

namespace WebBanLapTop.Admin
{
	public partial class Delete_User : System.Web.UI.Page
	{
		// Khai báo class-level
		string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Chặn truy cập nếu chưa đăng nhập
			if (Session["AdminUser"] == null)
			{
				Response.Redirect("~/Admin/Login.aspx");
				return;
			}

			int userId;
			if (!int.TryParse(Request.QueryString["id"], out userId))
			{
				Response.Redirect("User.aspx");
				return;
			}

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "DELETE FROM [user] WHERE id=@id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@id", userId);
				conn.Open();
				cmd.ExecuteNonQuery();
			}

			// Lưu thông báo vào Session
			Session["ToastMessage"] = "Xóa người dùng thành công!";

			// Redirect sang User.aspx
			Response.Redirect("User.aspx", false);
			Context.ApplicationInstance.CompleteRequest();
		}
	}
}
