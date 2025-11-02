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
					Response.Redirect("/Home/Account/Login.aspx?register=success");
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

		private void SyncCartItemToDatabase(int userId, int productId, int quantity, decimal price)
		{
			string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();

				// 1️⃣ Kiểm tra user đã có cart chưa
				string checkCartSql = "SELECT TOP 1 id FROM cart WHERE user_id=@user_id AND is_checked_out=0";
				SqlCommand checkCartCmd = new SqlCommand(checkCartSql, conn);
				checkCartCmd.Parameters.AddWithValue("@user_id", userId);
				object result = checkCartCmd.ExecuteScalar();

				int cartId;
				if (result == null)
				{
					// 2️⃣ Tạo mới cart nếu chưa có
					string createCartSql = "INSERT INTO cart (user_id) OUTPUT INSERTED.id VALUES (@user_id)";
					SqlCommand createCartCmd = new SqlCommand(createCartSql, conn);
					createCartCmd.Parameters.AddWithValue("@user_id", userId);
					cartId = (int)createCartCmd.ExecuteScalar();
				}
				else
				{
					cartId = Convert.ToInt32(result);
				}

				// 3️⃣ Kiểm tra item trong cart_item
				string checkItemSql = "SELECT id FROM cart_item WHERE cart_id=@cart_id AND product_id=@product_id";
				SqlCommand checkItemCmd = new SqlCommand(checkItemSql, conn);
				checkItemCmd.Parameters.AddWithValue("@cart_id", cartId);
				checkItemCmd.Parameters.AddWithValue("@product_id", productId);
				object existingItem = checkItemCmd.ExecuteScalar();

				if (existingItem != null)
				{
					// 4️⃣ Cập nhật số lượng
					string updateSql = "UPDATE cart_item SET quantity = quantity + @quantity WHERE id=@id";
					SqlCommand updateCmd = new SqlCommand(updateSql, conn);
					updateCmd.Parameters.AddWithValue("@quantity", quantity);
					updateCmd.Parameters.AddWithValue("@id", (int)existingItem);
					updateCmd.ExecuteNonQuery();
				}
				else
				{
					// 5️⃣ Thêm mới item
					string insertSql = @"INSERT INTO cart_item (cart_id, product_id, quantity, price_at_time)
                                 VALUES (@cart_id, @product_id, @quantity, @price)";
					SqlCommand insertCmd = new SqlCommand(insertSql, conn);
					insertCmd.Parameters.AddWithValue("@cart_id", cartId);
					insertCmd.Parameters.AddWithValue("@product_id", productId);
					insertCmd.Parameters.AddWithValue("@quantity", quantity);
					insertCmd.Parameters.AddWithValue("@price", price);
					insertCmd.ExecuteNonQuery();
				}
			}
		}

	}
}
