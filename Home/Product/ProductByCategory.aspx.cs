using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace WebBanLapTop.Home
{
	public partial class ProductByCategory : System.Web.UI.Page
	{
		private string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
		private int pageSize = 9; // số sản phẩm mỗi trang

		private int CurrentPage
		{
			get { return (int)(ViewState["CurrentPage"] ?? 1); }
			set { ViewState["CurrentPage"] = value; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				LoadProductsByCategory(1);
			}
		}

		private void LoadProductsByCategory(int page)
		{
			string catId = Request.QueryString["catid"];
			if (string.IsNullOrEmpty(catId))
			{
				lblNoResults.Text = "Không tìm thấy danh mục hợp lệ.";
				lblNoResults.Visible = true;
				return;
			}

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();

				// 🔹 Lấy tên danh mục
				string catNameQuery = "SELECT category_name FROM category WHERE id = @id";
				using (SqlCommand catCmd = new SqlCommand(catNameQuery, conn))
				{
					catCmd.Parameters.AddWithValue("@id", catId);
					object catName = catCmd.ExecuteScalar();
					lblCategoryName.Text = catName != null ? catName.ToString() : "Danh mục không tồn tại";
				}

				// 🔹 Đếm tổng số sản phẩm trong danh mục
				int totalRecords = 0;
				using (SqlCommand countCmd = new SqlCommand("SELECT COUNT(*) FROM product WHERE category_id = @id", conn))
				{
					countCmd.Parameters.AddWithValue("@id", catId);
					totalRecords = Convert.ToInt32(countCmd.ExecuteScalar());
				}

				int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
				if (totalPages == 0) totalPages = 1;

				if (page < 1) page = 1;
				if (page > totalPages) page = totalPages;

				int offset = (page - 1) * pageSize;

				// 🔹 Lấy danh sách sản phẩm theo trang
				string query = @"
					SELECT id, name, price, image_url
					FROM product
					WHERE category_id = @id
					ORDER BY id DESC
					OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@id", catId);
					cmd.Parameters.AddWithValue("@Offset", offset);
					cmd.Parameters.AddWithValue("@PageSize", pageSize);

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.HasRows)
						{
							rptProductsByCategory.DataSource = reader;
							rptProductsByCategory.DataBind();
							lblNoResults.Visible = false;
						}
						else
						{
							rptProductsByCategory.DataSource = null;
							rptProductsByCategory.DataBind();
							lblNoResults.Visible = true;
						}
					}
				}

				lblPageInfo.Text = $"Trang {page} / {totalPages}";
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
				LoadProductsByCategory(CurrentPage);
			}
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{
			CurrentPage++;
			LoadProductsByCategory(CurrentPage);
		}
	}
}
