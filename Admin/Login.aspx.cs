using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
				Response.Redirect("Dashboard.aspx");
			}
		}

		protected void btnLogin_Click(object sender, EventArgs e)
		{
			string username = txtUsername.Text.Trim();
			string password = txtPassword.Text.Trim();

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "SELECT COUNT(*) FROM [user] WHERE username=@username AND password=@password AND role='admin'";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@username", username);
				cmd.Parameters.AddWithValue("@password", password);

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
	}
}