using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

namespace WebBanLapTop.Admin
{
	public partial class Edit_User : System.Web.UI.Page
	{
		protected int userId;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Chặn truy cập nếu chưa đăng nhập
			if (Session["AdminUser"] == null)
			{
				Response.Redirect("~/Admin/Login.aspx");
				return;
			}

			if (!int.TryParse(Request.QueryString["id"], out userId))
			{
				Response.Redirect("User.aspx");
				return;
			}

			if (!IsPostBack)
			{
				LoadUser();
			}
		}

		private void LoadUser()
		{
			string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "SELECT * FROM [user] WHERE id=@id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@id", userId);
				conn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.Read())
				{
					txtUsername.Text = dr["username"].ToString();
					txtEmail.Text = dr["email"].ToString();
					ddlRole.SelectedValue = dr["role"].ToString();
					pnlForm.Visible = true;
				}
				else
				{
					Response.Redirect("User.aspx");
				}
			}
		}

		protected void btnUpdate_Click(object sender, EventArgs e)
		{
			string username = txtUsername.Text.Trim();
			string email = txtEmail.Text.Trim();
			string password = txtPassword.Text.Trim();
			string role = ddlRole.SelectedValue;

			string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = conn;
				string sql;

				if (!string.IsNullOrEmpty(password))
				{
					string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
					sql = "UPDATE [user] SET username=@username, email=@email, password=@password, role=@role, updated_at=GETDATE() WHERE id=@id";
					cmd.Parameters.AddWithValue("@password", hashedPassword);
				}
				else
				{
					sql = "UPDATE [user] SET username=@username, email=@email, role=@role, updated_at=GETDATE() WHERE id=@id";
				}

				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue("@username", username);
				cmd.Parameters.AddWithValue("@email", email);
				cmd.Parameters.AddWithValue("@role", role);
				cmd.Parameters.AddWithValue("@id", userId);

				conn.Open();
				cmd.ExecuteNonQuery();
				Response.Redirect("User.aspx");
			}
		}
	}
}
