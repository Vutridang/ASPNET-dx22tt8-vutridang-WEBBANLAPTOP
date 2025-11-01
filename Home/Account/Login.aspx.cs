using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;

namespace WebBanLapTop.Home
{
	public partial class Login : Page
	{
		private string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Nếu đã đăng nhập → quay lại trang chủ
			if (Session["AdminUser"] != null || Session["CustomerUser"] != null)
			{
				Response.Redirect("~/Home/index.aspx", false);
				Context.ApplicationInstance.CompleteRequest();
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

			// ✅ Mã hoá mật khẩu SHA1
			string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");

			try
			{
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					string sql = "SELECT role FROM [user] WHERE username=@username AND password=@password";
					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.Parameters.AddWithValue("@username", username);
					cmd.Parameters.AddWithValue("@password", hashedPassword);

					conn.Open();
					object result = cmd.ExecuteScalar();

					if (result != null)
					{
						string role = result.ToString().Trim().ToLower();

						// ✅ Ghi session dùng chung
						if (role == "admin")
							Session["AdminUser"] = username;
						else
							Session["CustomerUser"] = username;

						Session["ToastMessage"] = "Đăng nhập thành công!";

						// ✅ Chuyển hướng an toàn, không bị chặn giữa chừng
						Response.Redirect("~/Home/index.aspx", false);
						Context.ApplicationInstance.CompleteRequest();
					}
					else
					{
						lblMessage.Text = "Tên đăng nhập hoặc mật khẩu không đúng.";
					}
				}
			}
			catch (Exception ex)
			{
				lblMessage.Text = "Lỗi kết nối: " + ex.Message;
			}
		}
	}
}
