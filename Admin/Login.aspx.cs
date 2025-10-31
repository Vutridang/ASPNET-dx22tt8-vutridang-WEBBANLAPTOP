using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;

namespace WebBanLapTop.Admin
{
	public partial class Login : System.Web.UI.Page
	{
		private string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Nếu đã login, tự động chuyển đến Dashboard
			if (Session["AdminUser"] != null)
			{
				Response.Redirect("~/Admin/Dashboard.aspx");
			}
		}

		protected void btnLogin_Click(object sender, EventArgs e)
		{
			string username = txtUsername.Text.Trim();
			string password = txtPassword.Text.Trim();

			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				lblMessage.Text = "Vui lòng nhập đầy đủ thông tin.";
				return;
			}

			// Hash password SHA1 giống Edit_User
			string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");

			try
			{
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					string sql = "SELECT COUNT(*) FROM [user] WHERE username=@username AND password=@password AND role='admin'";
					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.Parameters.AddWithValue("@username", username);
					cmd.Parameters.AddWithValue("@password", hashedPassword);

					conn.Open();
					int count = (int)cmd.ExecuteScalar();
					if (count > 0)
					{
						Session["AdminUser"] = username;
						Response.Redirect("Dashboard.aspx");
					}
					else
					{
						lblMessage.Text = "Tên đăng nhập hoặc mật khẩu không đúng.";
					}
				}
			}
			catch (Exception ex)
			{
				lblMessage.Text = "Lỗi kết nối hoặc truy vấn: " + ex.Message;
			}
		}
	}
}
