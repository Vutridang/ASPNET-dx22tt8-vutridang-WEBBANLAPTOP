using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace WebBanLapTop.Admin
{
	public partial class User : System.Web.UI.Page
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

			if (!IsPostBack)
			{
				((SiteMaster)this.Master).ShowToastFromSession(this);

				// Load danh sách user
				LoadUsers();

				// Set tên Admin lên MasterPage
				SetAdminNameFromSession();
			}
		}

		/// <summary>
		/// Gán tên Admin lên MasterPage, tránh null
		/// </summary>
		private void SetAdminNameFromSession()
		{
			if (this.Master == null) return; // Master null-safe

			SiteMaster master = this.Master as SiteMaster;
			if (master == null) return;       // Chuyển kiểu an toàn
			if (master.AdminNameLabel == null) return;

			master.AdminNameLabel.Text = Session["AdminUser"] != null
				? Session["AdminUser"].ToString()
				: "Admin";
		}

		/// <summary>
		/// Load danh sách User lên GridView
		/// </summary>
		private void LoadUsers()
		{
			if (gvUsers == null) return; // GridView null-safe

			try
			{
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					string sql = "SELECT * FROM [user] ORDER BY id DESC";
					SqlDataAdapter da = new SqlDataAdapter(sql, conn);
					DataTable dt = new DataTable();
					da.Fill(dt);

					gvUsers.DataSource = dt.Rows.Count > 0 ? dt : null;
					gvUsers.DataBind();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Lỗi khi load dữ liệu User: " + ex.Message);
			}
		}
	}
}
