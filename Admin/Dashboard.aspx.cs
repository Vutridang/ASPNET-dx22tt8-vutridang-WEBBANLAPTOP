using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebBanLapTop.Admin
{
	public partial class Dashboard : Page
	{
		private string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				// Kiểm tra session admin
				if (Session["AdminUser"] == null)
				{
					Response.Redirect("Login.aspx");
					return;
				}

				LoadAdmins();
			}
		}

		private void LoadAdmins()
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "SELECT id, username, email FROM [user] WHERE role='admin'";
				SqlDataAdapter da = new SqlDataAdapter(sql, conn);
				DataTable dt = new DataTable();
				da.Fill(dt);

				gvAdmins.DataSource = dt;
				gvAdmins.DataBind();
			}
		}
	}
}
