using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebBanLapTop.Admin.Category
{
	public partial class Category : System.Web.UI.Page
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

				// Load danh sách Category
				LoadCategories();

				// Set tên Admin lên MasterPage
				SetAdminNameFromSession();
			}
		}

		/// <summary>
		/// Gán tên Admin lên MasterPage
		/// </summary>
		private void SetAdminNameFromSession()
		{
			if (this.Master == null) return;

			SiteMaster master = this.Master as SiteMaster;
			if (master == null) return;
			if (master.AdminNameLabel == null) return;

			master.AdminNameLabel.Text = Session["AdminUser"] != null
				? Session["AdminUser"].ToString()
				: "Admin";
		}

		/// <summary>
		/// Load danh sách Category lên GridView
		/// </summary>
		private void LoadCategories()
		{
			if (gvCategory == null) return; // GridView null-safe

			try
			{
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					string sql = "SELECT * FROM category ORDER BY id DESC";
					SqlDataAdapter da = new SqlDataAdapter(sql, conn);
					DataTable dt = new DataTable();
					da.Fill(dt);

					gvCategory.DataSource = dt.Rows.Count > 0 ? dt : null;
					gvCategory.DataBind();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Lỗi khi load dữ liệu Category: " + ex.Message);
			}
		}
	}
}
