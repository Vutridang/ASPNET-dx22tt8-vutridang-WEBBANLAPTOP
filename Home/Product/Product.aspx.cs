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

		private int pageSize = 8; // Số sản phẩm mỗi trang

		private int CurrentPage
		{
			get { return (int)(ViewState["CurrentPage"] ?? 1); }
			set { ViewState["CurrentPage"] = value; }
		}

		private int CurrentCategory
		{
			get { return (int)(ViewState["CurrentCategory"] ?? 0); }
			set { ViewState["CurrentCategory"] = value; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				LoadCategories();
				LoadProducts(CurrentPage);
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
				ddlBrand.Items.Add(new ListItem("Tất cả", "0")); // tùy chọn mặc định

				while (reader.Read())
				{
					ddlBrand.Items.Add(
						new ListItem(reader["category_name"].ToString(), reader["id"].ToString())
					);
				}

				reader.Close();
			}
		}

		private void LoadProducts(int page)
		{
			// bảo đảm page hợp lệ
			if (page < 1) page = 1;

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();

				// 1) Đếm tổng theo category hiện tại
				int totalRecords = 0;
				string countSql = "SELECT COUNT(*) FROM product";
				if (CurrentCategory != 0)
					countSql += " WHERE category_id = @categoryId";

				using (SqlCommand countCmd = new SqlCommand(countSql, conn))
				{
					if (CurrentCategory != 0)
						countCmd.Parameters.AddWithValue("@categoryId", CurrentCategory);

					object obj = countCmd.ExecuteScalar();
					totalRecords = obj != DBNull.Value ? Convert.ToInt32(obj) : 0;
				}

				int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
				if (totalPages == 0) totalPages = 1;

				// Nếu page vượt quá tổng trang -> set về cuối
				if (page > totalPages) page = totalPages;

				// 2) Lấy dữ liệu chính xác theo category (nếu có)
				string sql = @"
            SELECT p.*, c.category_name
            FROM product p
            INNER JOIN category c ON p.category_id = c.id";

				if (CurrentCategory != 0)
					sql += " WHERE p.category_id = @categoryId";

				sql += " ORDER BY p.id DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

				int offset = (page - 1) * pageSize;

				using (SqlCommand cmd = new SqlCommand(sql, conn))
				{
					// nếu cần filter theo category thì luôn thêm parameter @categoryId
					if (CurrentCategory != 0)
						cmd.Parameters.AddWithValue("@categoryId", CurrentCategory);

					cmd.Parameters.AddWithValue("@Offset", offset);
					cmd.Parameters.AddWithValue("@PageSize", pageSize);

					DataTable dt = new DataTable();
					using (SqlDataAdapter da = new SqlDataAdapter(cmd))
					{
						da.Fill(dt);
					}

					rptProducts.DataSource = dt.Rows.Count > 0 ? dt : null;
					rptProducts.DataBind();
				}

				// 3) Cập nhật UI phân trang và lưu trang hiện tại
				lblPageInfo.Text = "Trang " + page + " / " + totalPages;
				btnPrev.Enabled = (page > 1);
				btnNext.Enabled = (page < totalPages);

				CurrentPage = page;
			}
		}


		protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Lưu category hiện tại (0 = tất cả)
			CurrentCategory = int.Parse(ddlBrand.SelectedValue);

			// Luôn quay về trang đầu
			CurrentPage = 1;

			// Load trang 1 với category hiện tại (nếu 0 thì toàn bộ)
			LoadProducts(1);
		}

		protected void btnPrev_Click(object sender, EventArgs e)
		{
			if (CurrentPage > 1)
			{
				CurrentPage--;
				LoadProducts(CurrentPage);
			}
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{
			CurrentPage++;
			LoadProducts(CurrentPage);
		}
	}
}
