using System;
using System.Data.SqlClient;
using System.Configuration;

namespace WebBanLapTop.Home
{
	public partial class index : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				((SiteMaster)this.Master).ShowToastFromSession(this);
				LoadFeaturedProducts();
				LoadProducts();
			}
		}

		private void LoadFeaturedProducts()
		{
			string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				// Lấy 2 sản phẩm nổi bật mới nhất
				string sql = "SELECT TOP 2 id, name, description, price, image_url FROM product ORDER BY created_at DESC";
				SqlCommand cmd = new SqlCommand(sql, conn);
				conn.Open();

				SqlDataReader reader = cmd.ExecuteReader();
				rptFeaturedProducts.DataSource = reader;
				rptFeaturedProducts.DataBind();

				reader.Close();
			}
		}


		private void LoadProducts()
		{
			string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "SELECT TOP 4 id, name, description, price, image_url FROM product ORDER BY created_at DESC";
				SqlCommand cmd = new SqlCommand(sql, conn);
				conn.Open();

				SqlDataReader reader = cmd.ExecuteReader();
				rptProducts.DataSource = reader;
				rptProducts.DataBind();

				reader.Close();
			}
		}
	}
}
