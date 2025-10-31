using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebBanLapTop.Admin.Product
{
	public partial class Product : System.Web.UI.Page
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

				
				LoadProducts();

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

		private void LoadProducts()
		{
			if (gvProducts == null) return;

			try
			{
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					string sql = "SELECT * FROM product ORDER BY id DESC";
					SqlDataAdapter da = new SqlDataAdapter(sql, conn);
					DataTable dt = new DataTable();
					da.Fill(dt);
					gvProducts.DataSource = dt.Rows.Count > 0 ? dt : null;
					gvProducts.DataBind();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Lỗi khi load dữ liệu sản phẩm: " + ex.Message);
			}
		}
	}
}
