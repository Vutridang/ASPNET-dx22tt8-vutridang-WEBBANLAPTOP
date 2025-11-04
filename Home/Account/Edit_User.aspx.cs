using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Web.UI;

namespace WebBanLapTop.Home.Account
{
	public partial class Edit_User : System.Web.UI.Page
	{
		string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
		const int PageSize = 5; // 5 đơn hàng mỗi trang

		int CurrentPage
		{
			get { return ViewState["CurrentPage"] == null ? 1 : (int)ViewState["CurrentPage"]; }
			set { ViewState["CurrentPage"] = value; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Session["UserId"] == null)
				{
					Response.Redirect("/Home/Account/Login.aspx");
					return;
				}
				LoadUserInfo();
				LoadOrders(CurrentPage);
			}
		}

		private void LoadUserInfo()
		{
			int userId = Convert.ToInt32(Session["UserId"]);
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "SELECT username, email FROM [user] WHERE id=@id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@id", userId);
				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					txtUsername.Text = reader["username"].ToString();
					txtEmail.Text = reader["email"].ToString();
				}
				reader.Close();
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			int userId = Convert.ToInt32(Session["UserId"]);
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "UPDATE [user] SET username=@u, email=@e, updated_at=GETDATE() WHERE id=@id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@u", txtUsername.Text.Trim());
				cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
				cmd.Parameters.AddWithValue("@id", userId);
				conn.Open();
				cmd.ExecuteNonQuery();
			}

			ScriptManager.RegisterStartupScript(this, GetType(), "alert",
				"Swal.fire('Thành công','Thông tin đã được cập nhật!','success');", true);
		}

		// 🔹 Load danh sách đơn hàng có phân trang
		private void LoadOrders(int page)
		{
			int userId = Convert.ToInt32(Session["UserId"]);
			int offset = (page - 1) * PageSize;
			int totalRecords = 0;

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();

				// Đếm tổng số đơn
				SqlCommand countCmd = new SqlCommand("SELECT COUNT(*) FROM [order] WHERE user_id=@uid", conn);
				countCmd.Parameters.AddWithValue("@uid", userId);
				totalRecords = Convert.ToInt32(countCmd.ExecuteScalar());

				int totalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
				if (totalPages == 0) totalPages = 1;

				if (page < 1) page = 1;
				if (page > totalPages) page = totalPages;

				string sql = @"
                    SELECT id, total_amount, status, created_at
                    FROM [order]
                    WHERE user_id=@uid
                    ORDER BY id DESC
                    OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";

				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@uid", userId);
				cmd.Parameters.AddWithValue("@offset", offset);
				cmd.Parameters.AddWithValue("@limit", PageSize);

				SqlDataReader reader = cmd.ExecuteReader();
				gvOrders.DataSource = reader;
				gvOrders.DataBind();
				reader.Close();

				// Cập nhật phân trang
				lblPageInfo.Text = $"Trang {page} / {Math.Max(totalPages, 1)}";
				btnPrev.Enabled = page > 1;
				btnNext.Enabled = page < totalPages;

				CurrentPage = page;
			}
		}

		protected void btnPrev_Click(object sender, EventArgs e)
		{
			if (CurrentPage > 1)
				LoadOrders(--CurrentPage);
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{
			LoadOrders(++CurrentPage);
		}

		protected void btnViewDetail_Click(object sender, EventArgs e)
		{
			System.Web.UI.WebControls.Button btn = (System.Web.UI.WebControls.Button)sender;
			int orderId = Convert.ToInt32(btn.CommandArgument);

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = @"
                    SELECT p.name, p.image_url, b.name AS brand_name, oi.quantity, oi.price
                    FROM order_item oi
                    INNER JOIN product p ON oi.product_id = p.id
                    LEFT JOIN brand b ON p.brand_id = b.id
                    WHERE oi.order_id = @oid";

				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@oid", orderId);
				conn.Open();

				SqlDataReader reader = cmd.ExecuteReader();
				StringBuilder html = new StringBuilder();
				while (reader.Read())
				{
					string img = reader["image_url"] != DBNull.Value ? reader["image_url"].ToString() : "/Home/images/no_image.png";
					html.Append("<tr>");
					html.Append("<td style='border:1px solid #ddd; padding:8px; text-align:center;'><img src='" + img + "' style='width:80px; height:80px; object-fit:cover; border-radius:5px;'></td>");
					html.Append("<td style='border:1px solid #ddd; padding:8px;'>" + reader["name"] + "</td>");
					html.Append("<td style='border:1px solid #ddd; padding:8px;'>" + (reader["brand_name"] ?? "Không rõ") + "</td>");
					html.Append("<td style='border:1px solid #ddd; padding:8px; text-align:center;'>" + reader["quantity"] + "</td>");
					html.Append("<td style='border:1px solid #ddd; padding:8px; text-align:right;'>" + String.Format("{0:N0}₫", reader["price"]) + "</td>");
					html.Append("</tr>");
				}
				tblOrderItems.InnerHtml = html.ToString();
				reader.Close();
			}

			orderModal.Style["display"] = "block";
		}

		protected void btnCloseModal_Click(object sender, EventArgs e)
		{
			orderModal.Style["display"] = "none";
		}
	}
}
