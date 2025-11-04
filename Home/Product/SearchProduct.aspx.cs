using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebBanLapTop.Home
{
	public partial class SearchProduct : System.Web.UI.Page
	{
		private string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
		private int pageSize = 8; // số sản phẩm mỗi trang

		private int CurrentPage
		{
			get { return (int)(ViewState["CurrentPage"] ?? 1); }
			set { ViewState["CurrentPage"] = value; }
		}

		private string CurrentKeyword
		{
			get { return (string)(ViewState["CurrentKeyword"] ?? ""); }
			set { ViewState["CurrentKeyword"] = value; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				string keyword = Request.QueryString["keyword"];
				if (!string.IsNullOrEmpty(keyword))
				{
					CurrentKeyword = keyword;
					LoadSearchResults(keyword, 1);
				}
				else
				{
					lblNoResults.Text = "Không có từ khóa tìm kiếm.";
					lblNoResults.Visible = true;
				}
			}
		}

		private void LoadSearchResults(string keyword, int page)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					conn.Open();

					// 1️⃣ Đếm tổng số sản phẩm phù hợp
					string countSql = @"
						SELECT COUNT(*) 
						FROM product p 
						LEFT JOIN brand b ON p.brand_id = b.id
						WHERE p.name LIKE @keyword OR b.name LIKE @keyword";

					int totalRecords;
					using (SqlCommand countCmd = new SqlCommand(countSql, conn))
					{
						countCmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
						totalRecords = (int)countCmd.ExecuteScalar();
					}

					int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
					if (totalPages == 0) totalPages = 1;

					if (page < 1) page = 1;
					if (page > totalPages) page = totalPages;

					int offset = (page - 1) * pageSize;

					// 2️⃣ Truy vấn sản phẩm trong trang hiện tại
					string sql = @"
						SELECT p.id, p.name, p.price, p.image_url, b.name AS brand_name
						FROM product p
						LEFT JOIN brand b ON p.brand_id = b.id
						WHERE p.name LIKE @keyword OR b.name LIKE @keyword
						ORDER BY p.created_at DESC
						OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

					using (SqlCommand cmd = new SqlCommand(sql, conn))
					{
						cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
						cmd.Parameters.AddWithValue("@Offset", offset);
						cmd.Parameters.AddWithValue("@PageSize", pageSize);

						SqlDataAdapter da = new SqlDataAdapter(cmd);
						DataTable dt = new DataTable();
						da.Fill(dt);

						rptSearchResults.DataSource = dt.Rows.Count > 0 ? dt : null;
						rptSearchResults.DataBind();

						lblNoResults.Visible = (dt.Rows.Count == 0);
					}

					// 3️⃣ Phân trang
					lblPageInfo.Text = "Trang " + page + " / " + totalPages;
					btnPrev.Enabled = (page > 1);
					btnNext.Enabled = (page < totalPages);
					CurrentPage = page;
				}
			}
			catch (Exception ex)
			{
				lblNoResults.Visible = true;
				lblNoResults.Text = "Lỗi khi tải dữ liệu: " + ex.Message;
			}
		}

		protected void btnPrev_Click(object sender, EventArgs e)
		{
			if (CurrentPage > 1)
			{
				CurrentPage--;
				LoadSearchResults(CurrentKeyword, CurrentPage);
			}
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{
			CurrentPage++;
			LoadSearchResults(CurrentKeyword, CurrentPage);
		}
	}
}
