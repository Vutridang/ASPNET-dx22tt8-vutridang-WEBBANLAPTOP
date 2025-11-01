using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace WebBanLapTop.Home
{
	public partial class Topbrands : System.Web.UI.Page
	{
		private string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				LoadTopBrands();
		}

		private void LoadTopBrands()
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sqlBrands = "SELECT id, name, logo_url, description FROM brand WHERE is_top = 1";
				SqlDataAdapter daBrand = new SqlDataAdapter(sqlBrands, conn);
				DataTable dtBrands = new DataTable();
				daBrand.Fill(dtBrands);

				List<dynamic> topBrandList = new List<dynamic>();

				foreach (DataRow brandRow in dtBrands.Rows)
				{
					int brandId = Convert.ToInt32(brandRow["id"]);

					string sqlProducts = @"SELECT TOP 4 p.id, p.name, p.price, p.image_url
										   FROM product p
										   WHERE p.brand_id = @brand_id
										   ORDER BY p.created_at DESC";

					SqlCommand cmd = new SqlCommand(sqlProducts, conn);
					cmd.Parameters.AddWithValue("@brand_id", brandId);

					SqlDataAdapter daProduct = new SqlDataAdapter(cmd);
					DataTable dtProducts = new DataTable();
					daProduct.Fill(dtProducts);

					topBrandList.Add(new
					{
						id = brandRow["id"],
						name = brandRow["name"].ToString(),
						logo_url = brandRow["logo_url"].ToString(),
						description = brandRow["description"].ToString(),
						Products = dtProducts
					});
				}

				rptTopBrands.DataSource = topBrandList;
				rptTopBrands.DataBind();
			}
		}
	}
}
