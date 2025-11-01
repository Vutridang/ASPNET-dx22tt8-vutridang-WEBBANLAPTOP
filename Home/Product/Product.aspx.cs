using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace WebBanLapTop.Home
{
	public partial class Products : System.Web.UI.Page
	{
		private string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				LoadCategories();
				LoadProducts();
			}
		}

		private void LoadCategories()
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "SELECT id, category_name FROM category ORDER BY category_name";
				SqlCommand cmd = new SqlCommand(sql, conn);
				conn.Open();

				SqlDataReader reader = cmd.ExecuteReader();

				ddlBrand.Items.Clear();
				ddlBrand.Items.Add(new ListItem("Tất cả", "0"));

				while (reader.Read())
				{
					ddlBrand.Items.Add(
						new ListItem(reader["category_name"].ToString(), reader["id"].ToString())
					);
				}

				reader.Close();
			}
		}

		private void LoadProducts(int categoryId = 0)
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = @"SELECT p.*, c.category_name 
							   FROM product p 
							   INNER JOIN category c ON p.category_id = c.id";
				if (categoryId != 0)
					sql += " WHERE p.category_id = @categoryId";

				SqlCommand cmd = new SqlCommand(sql, conn);
				if (categoryId != 0)
					cmd.Parameters.AddWithValue("@categoryId", categoryId);

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable();
				da.Fill(dt);

				rptProducts.DataSource = dt;
				rptProducts.DataBind();
			}
		}

		protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedId = int.Parse(ddlBrand.SelectedValue);
			LoadProducts(selectedId);
		}
	}
}
