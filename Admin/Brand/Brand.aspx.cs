using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebBanLapTop.Admin.Brand
{
	public partial class Brand : System.Web.UI.Page
	{
		private string connStr;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Chặn truy cập nếu chưa đăng nhập Admin
			if (Session["AdminUser"] == null)
			{
				Response.Redirect("~/Admin/Login.aspx");
				return;
			}

			// Lấy connection string
			ConnectionStringSettings connSetting = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"];
			if (connSetting == null)
			{
				throw new Exception("ConnectionString 'WebBanLapTopConnection' chưa được khai báo trong web.config.");
			}
			connStr = connSetting.ConnectionString;

			if (!IsPostBack)
			{
				((SiteMaster)this.Master).ShowToastFromSession(this);
				LoadBrands();
				SetAdminNameFromSession();
			}
		}

		private void SetAdminNameFromSession()
		{
			if (this.Master == null) return;
			SiteMaster master = this.Master as SiteMaster;
			if (master == null || master.AdminNameLabel == null) return;
			master.AdminNameLabel.Text = Session["AdminUser"] != null ? Session["AdminUser"].ToString() : "Admin";
		}

		private void LoadBrands()
		{
			if (gvBrands == null) return;

			try
			{
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					string sql = "SELECT * FROM brand ORDER BY id DESC";
					SqlDataAdapter da = new SqlDataAdapter(sql, conn);
					DataTable dt = new DataTable();
					da.Fill(dt);
					gvBrands.DataSource = dt.Rows.Count > 0 ? dt : null;
					gvBrands.DataBind();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Lỗi khi load dữ liệu thương hiệu: " + ex.Message);
			}
		}
	}
}
