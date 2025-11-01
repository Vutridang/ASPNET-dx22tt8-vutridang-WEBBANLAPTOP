using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebBanLapTop.Home
{
	public partial class SearchProduct : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				string keyword = Request.QueryString["keyword"];
				if (!string.IsNullOrEmpty(keyword))
				{
					LoadSearchResults(keyword);
				}
			}
		}

		private void LoadSearchResults(string keyword)
		{
			try
			{
				string connectionString = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = @"
                        SELECT p.id, p.name, p.price, p.image_url, b.name AS brand_name
                        FROM product p
                        LEFT JOIN brand b ON p.brand_id = b.id
                        WHERE p.name LIKE @keyword OR b.name LIKE @keyword
                        ORDER BY p.created_at DESC;
                    ";

					using (SqlCommand cmd = new SqlCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

						SqlDataAdapter da = new SqlDataAdapter(cmd);
						DataTable dt = new DataTable();
						da.Fill(dt);

						rptSearchResults.DataSource = dt;
						rptSearchResults.DataBind();

						lblNoResults.Visible = (dt.Rows.Count == 0);
					}
				}
			}
			catch (Exception ex)
			{
				lblNoResults.Visible = true;
				lblNoResults.Text = "Lỗi khi tải dữ liệu: " + ex.Message;
			}
		}
	}
}
