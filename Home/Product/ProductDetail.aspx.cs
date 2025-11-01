using System;
using System.Configuration;
using System.Data.SqlClient;

namespace WebBanLapTop.Home
{
	public partial class ProductDetail : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				LoadCategories();
				string id = Request.QueryString["id"];
				if (!string.IsNullOrEmpty(id))
				{
					LoadProductDetail(id);
				}
			}
		}

		private void LoadCategories()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				string query = "SELECT id, category_name FROM category ORDER BY category_name";
				SqlCommand cmd = new SqlCommand(query, conn);
				conn.Open();

				SqlDataReader reader = cmd.ExecuteReader();
				rptCategories.DataSource = reader;
				rptCategories.DataBind();

				reader.Close();
			}
		}

		private void LoadProductDetail(string id)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
			string query = @"
        SELECT 
            p.name, 
            p.description, 
            p.price, 
            p.stock, 
            p.image_url, 
            b.name AS brand_name,
            c.category_name
        FROM product p
        LEFT JOIN brand b ON p.brand_id = b.id
        LEFT JOIN category c ON p.category_id = c.id
        WHERE p.id = @id";

			using (SqlConnection conn = new SqlConnection(connectionString))
			using (SqlCommand cmd = new SqlCommand(query, conn))
			{
				cmd.Parameters.AddWithValue("@id", id);
				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					lblName.Text = reader["name"].ToString();
					lblDescription.Text = reader["description"].ToString();
					lblPrice.Text = string.Format("{0:N0} VNĐ", reader["price"]);
					lblStock.Text = reader["stock"].ToString();
					lblBrand.Text = reader["brand_name"].ToString();
					lblCategory.Text = reader["category_name"].ToString();  // ✅ Thêm dòng này
					imgProduct.ImageUrl = reader["image_url"].ToString();
				}
				conn.Close();
			}
		}


		protected void btnAddToCart_Click(object sender, EventArgs e)
		{
			// TODO: xử lý thêm giỏ hàng (session hoặc database)
			int quantity = int.Parse(txtQuantity.Text);
			string productName = lblName.Text;
			string price = lblPrice.Text;

			// Ví dụ: thông báo tạm
			ClientScript.RegisterStartupScript(this.GetType(), "alert",
				$"Swal.fire('Đã thêm vào giỏ hàng', '{productName} x{quantity}', 'success');", true);
		}
	}
}
