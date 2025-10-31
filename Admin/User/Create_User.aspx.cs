using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;

namespace WebBanLapTop.Admin
{
	public partial class Create_User : System.Web.UI.Page
	{
		// Connection string ở class-level
		private string connStr;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Chặn truy cập nếu chưa đăng nhập
			if (Session["AdminUser"] == null)
			{
				Response.Redirect("~/Admin/Login.aspx");
				return;
			}

			// Lấy connection string từ web.config và check null
			ConnectionStringSettings connSetting = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"];
			if (connSetting == null)
			{
				throw new Exception("ConnectionString 'WebBanLapTopConnection' chưa được khai báo trong web.config.");
			}
			connStr = connSetting.ConnectionString;
		}

		protected void btnCreate_Click(object sender, EventArgs e)
		{
			string username = txtUsername.Text.Trim();
			string email = txtEmail.Text.Trim();
			string password = txtPassword.Text.Trim();
			string role = ddlRole.SelectedValue;

			if (username == "" || email == "" || password == "")
			{
				lblMessage.Text = "Vui Lòng Điền Đầy Đủ Thông Tin.";
				return;
			}

			string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "INSERT INTO [user] (username,password,email,role) VALUES (@username,@password,@email,@role)";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@username", username);
				cmd.Parameters.AddWithValue("@password", hashedPassword);
				cmd.Parameters.AddWithValue("@email", email);
				cmd.Parameters.AddWithValue("@role", role);

				try
				{
					conn.Open();
					cmd.ExecuteNonQuery();

					// Lưu thông báo vào Session
					Session["ToastMessage"] = "Thêm người dùng thành công!";

					// Redirect sang User.aspx
					Response.Redirect("User.aspx", false);
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
