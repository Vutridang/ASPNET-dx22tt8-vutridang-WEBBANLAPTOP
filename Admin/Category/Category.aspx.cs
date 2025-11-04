using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebBanLapTop.Admin.Category
{
	public partial class Category : System.Web.UI.Page
	{
		// Connection string ở class-level
		private string connStr;
		private int pageSize = 10;

		private int CurrentPage
		{
			get { return (int)(ViewState["CurrentPage"] ?? 1); }
			set { ViewState["CurrentPage"] = value; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			// Chặn truy cập nếu chưa đăng nhập
			if (Session["AdminUser"] == null)
			{
				Response.Redirect("~/Admin/Login.aspx");
				return;
			}

			// Lấy connection string từ web.config và check null
			ConnectionStringSettings connSetting = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"];
			if (connSetting == null)
			{
				throw new Exception("ConnectionString 'WebBanLapTopConnection' chưa được khai báo trong web.config.");
			}

			connStr = connSetting.ConnectionString;

			if (!IsPostBack)
			{
				((SiteMaster)this.Master).ShowToastFromSession(this);

				// Load danh sách Category
				LoadCategories(1);

				// Set tên Admin lên MasterPage
				SetAdminNameFromSession();
			}
		}

		/// <summary>
		/// Gán tên Admin lên MasterPage
		/// </summary>
		private void SetAdminNameFromSession()
		{
			if (this.Master == null) return;

			SiteMaster master = this.Master as SiteMaster;
			if (master == null) return;
			if (master.AdminNameLabel == null) return;

			master.AdminNameLabel.Text = Session["AdminUser"] != null
				? Session["AdminUser"].ToString()
				: "Admin";
		}

		/// <summary>
		/// Load danh sách Category lên GridView
		/// </summary>
		private void LoadCategories(int page)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					conn.Open();

					// 1️⃣ Đếm tổng số danh mục
					int totalRecords = 0;
					using (SqlCommand countCmd = new SqlCommand("SELECT COUNT(*) FROM [category]", conn))
					{
						totalRecords = (int)countCmd.ExecuteScalar();
					}

					int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
					if (totalPages == 0) totalPages = 1;

					// 2️⃣ Lấy danh mục theo trang
					string sql = @"
                        SELECT id, category_name, created_at, updated_at
                        FROM [category]
                        ORDER BY id DESC
                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                    ";

					int offset = (page - 1) * pageSize;

					using (SqlCommand cmd = new SqlCommand(sql, conn))
					{
						cmd.Parameters.AddWithValue("@Offset", offset);
						cmd.Parameters.AddWithValue("@PageSize", pageSize);

						using (SqlDataReader reader = cmd.ExecuteReader())
						{
							DataTable dt = new DataTable();
							dt.Load(reader);
							gvCategory.DataSource = dt.Rows.Count > 0 ? dt : null;
							gvCategory.DataBind();
						}
					}

					// 3️⃣ Cập nhật thông tin phân trang
					lblPageInfo.Text = "Trang " + page + " / " + totalPages;
					btnPrev.Enabled = (page > 1);
					btnNext.Enabled = (page < totalPages);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Lỗi khi load dữ liệu Category: " + ex.Message);
			}
		}

		protected void btnPrev_Click(object sender, EventArgs e)
		{
			if (CurrentPage > 1)
			{
				CurrentPage--;
				LoadCategories(CurrentPage);
			}
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{
			CurrentPage++;
			LoadCategories(CurrentPage);
		}
	}
}
