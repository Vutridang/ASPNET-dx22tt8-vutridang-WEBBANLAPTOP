using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace WebBanLapTop.Home
{
	public partial class Register : System.Web.UI.Page
	{
		protected void btnRegister_Click(object sender, EventArgs e)
		{
			string username = txtUsername.Text.Trim();
			string email = txtEmail.Text.Trim();
			string password = txtPassword.Text;
			string confirmPassword = txtConfirmPassword.Text;

			lblMessage.Text = "";

			// Validate input
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
				string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
			{
				lblMessage.Text = "Vui lòng điền đầy đủ thông tin.";
				return;
			}

			if (password != confirmPassword)
			{
				lblMessage.Text = "Mật khẩu và xác nhận mật khẩu không khớp.";
				return;
			}

			// Hash password (SHA1 example - bạn có thể đổi thuật toán khác tốt hơn như SHA256)
			string hashedPassword = HashPassword(password);

			string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string queryCheck = "SELECT COUNT(*) FROM [user] WHERE username = @username OR email = @email";
				SqlCommand cmdCheck = new SqlCommand(queryCheck, conn);
				cmdCheck.Parameters.AddWithValue("@username", username);
				cmdCheck.Parameters.AddWithValue("@email", email);

				conn.Open();
				int count = (int)cmdCheck.ExecuteScalar();
				if (count > 0)
				{
					lblMessage.Text = "Tên đăng nhập hoặc email đã tồn tại.";
					return;
				}

				string queryInsert = @"INSERT INTO [user] (username, password, email, role, created_at, updated_at) 
                                       VALUES (@username, @password, @email, 'customer', GETDATE(), GETDATE())";

				SqlCommand cmdInsert = new SqlCommand(queryInsert, conn);
				cmdInsert.Parameters.AddWithValue("@username", username);
				cmdInsert.Parameters.AddWithValue("@password", hashedPassword);
				cmdInsert.Parameters.AddWithValue("@email", email);

				int rows = cmdInsert.ExecuteNonQuery();

				if (rows > 0)
				{
					// Đăng ký thành công, chuyển hướng tới trang đăng nhập
					Response.Redirect("/Home/Login.aspx?register=success");
				}
				else
				{
					lblMessage.Text = "Đăng ký thất bại, vui lòng thử lại.";
				}
			}
		}

		private string HashPassword(string password)
		{
			using (SHA1 sha1 = SHA1.Create())
			{
				byte[] inputBytes = Encoding.UTF8.GetBytes(password);
				byte[] hashBytes = sha1.ComputeHash(inputBytes);

				StringBuilder sb = new StringBuilder();
				foreach (byte b in hashBytes)
					sb.Append(b.ToString("x2"));
				return sb.ToString();
			}
		}
	}
}
