using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace WebBanLapTop.Home
{
	public partial class ProductByBrand : System.Web.UI.Page
	{
		private string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
		private int pageSize = 8; // mỗi trang 9 sản phẩm

		private int CurrentPage
		{
			get { return (int)(ViewState["CurrentPage"] ?? 1); }
			set { ViewState["CurrentPage"] = value; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				LoadProductsByBrand(1);
		}

		private void LoadProductsByBrand(int page)
		{
			string brandId = Request.QueryString["id"];
			if (string.IsNullOrEmpty(brandId))
			{
				lblNoResults.Text = "Không tìm thấy thương hiệu hợp lệ.";
				lblNoResults.Visible = true;
				return;
			}

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();

				// 🔹 Lấy tên thương hiệu
				string brandNameQuery = "SELECT name FROM brand WHERE id = @id";
				using (SqlCommand brandCmd = new SqlCommand(brandNameQuery, conn))
				{
					brandCmd.Parameters.AddWithValue("@id", brandId);
					object brandName = brandCmd.ExecuteScalar();
					lblBrandName.Text = brandName != null ? brandName.ToString() : "Thương hiệu không tồn tại";
				}

				// 🔹 Đếm tổng sản phẩm thuộc thương hiệu
				int totalRecords = 0;
				using (SqlCommand countCmd = new SqlCommand("SELECT COUNT(*) FROM product WHERE brand_id = @id", conn))
				{
					countCmd.Parameters.AddWithValue("@id", brandId);
					totalRecords = Convert.ToInt32(countCmd.ExecuteScalar());
				}

				int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
				if (totalPages == 0) totalPages = 1;

				if (page < 1) page = 1;
				if (page > totalPages) page = totalPages;

				int startRow = (page - 1) * pageSize + 1;
				int endRow = startRow + pageSize - 1;

				// 🔹 Lấy danh sách sản phẩm theo thương hiệu (phân trang với ROW_NUMBER())
				string query = @"
					WITH BrandProducts AS (
						SELECT id, name, price, image_url,
							   ROW_NUMBER() OVER (ORDER BY id DESC) AS RowNum
						FROM product
						WHERE brand_id = @id
					)
					SELECT id, name, price, image_url
					FROM BrandProducts
					WHERE RowNum BETWEEN @StartRow AND @EndRow;";

				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@id", brandId);
					cmd.Parameters.AddWithValue("@StartRow", startRow);
					cmd.Parameters.AddWithValue("@EndRow", endRow);

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.HasRows)
						{
							rptProductsByBrand.DataSource = reader;
							rptProductsByBrand.DataBind();
							lblNoResults.Visible = false;
						}
						else
						{
							rptProductsByBrand.DataSource = null;
							rptProductsByBrand.DataBind();
							lblNoResults.Visible = true;
						}
					}
				}

				lblPageInfo.Text = "Trang " + page + " / " + totalPages;
				btnPrev.Enabled = (page > 1);
				btnNext.Enabled = (page < totalPages);
				CurrentPage = page;
			}
		}

		protected void btnPrev_Click(object sender, EventArgs e)
		{
			if (CurrentPage > 1)
			{
				CurrentPage--;
				LoadProductsByBrand(CurrentPage);
			}
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{
			CurrentPage++;
			LoadProductsByBrand(CurrentPage);
		}
	}
}
