using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace WebBanLapTop.Admin.Order_Item
{
	public partial class Order_Item : System.Web.UI.Page
	{
		private string connStr;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["AdminUser"] == null)
			{
				Response.Redirect("~/Admin/Login.aspx");
				return;
			}

			connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"]?.ConnectionString
					  ?? throw new Exception("ConnectionString 'WebBanLapTopConnection' chưa được khai báo.");

			if (!IsPostBack)
			{
				int orderId = 0;
				if (!int.TryParse(Request.QueryString["order_id"], out orderId) || orderId <= 0)
				{
					// Nếu order_id không hợp lệ, quay về trang quản lý Orders
					Response.Redirect("../Order/Order.aspx");
					return;
				}

				lblOrderID.Text = orderId.ToString();
				LoadOrderItems(orderId);
				LoadCustomerDetails(orderId.ToString());

				// Set tên Admin lên MasterPage
				SetAdminNameFromSession();
			}
		}

		private void LoadOrderItems(int orderId)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					string sql = @"
						SELECT 
							p.name AS product_name,
							b.name AS brand_name,
							oi.quantity,
							oi.price,
							(oi.quantity * oi.price) AS total,
							u.username AS customer_name,
							o.id AS order_id,
							o.status
						FROM order_item oi
						INNER JOIN [order] o ON oi.order_id = o.id
						INNER JOIN product p ON oi.product_id = p.id
						LEFT JOIN brand b ON p.brand_id = b.id
						INNER JOIN [user] u ON o.user_id = u.id
						WHERE o.id = @orderId
						ORDER BY oi.id";

					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.Parameters.AddWithValue("@orderId", orderId);

					SqlDataAdapter da = new SqlDataAdapter(cmd);
					DataTable dt = new DataTable();
					da.Fill(dt);

					gvOrderItems.DataSource = dt.Rows.Count > 0 ? dt : null;
					gvOrderItems.DataBind();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Lỗi khi load chi tiết đơn hàng: " + ex.Message);
			}
		}

		private void LoadCustomerDetails(string orderId)
		{
			string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
			string query = @"
                SELECT u.username, u.email, u.role,
                       ud.address, ud.zipcode, ud.payment_method,
                       ud.created_at, ud.updated_at
                FROM [order] o
                INNER JOIN [user] u ON o.user_id = u.id
                INNER JOIN user_detail ud ON o.id = ud.order_id AND u.id = ud.user_id
                WHERE o.id = @orderId";

			using (SqlConnection conn = new SqlConnection(connStr))
			using (SqlCommand cmd = new SqlCommand(query, conn))
			{
				cmd.Parameters.AddWithValue("@orderId", orderId);
				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					lblUsername.Text = reader["username"].ToString();
					lblEmail.Text = reader["email"].ToString();
					lblRole.Text = reader["role"].ToString();
					lblAddress.Text = reader["address"].ToString();
					lblZipcode.Text = reader["zipcode"].ToString();
					lblPaymentMethod.Text = reader["payment_method"].ToString();
					lblCreatedAt.Text = Convert.ToDateTime(reader["created_at"]).ToString("dd/MM/yyyy HH:mm");
					lblUpdatedAt.Text = Convert.ToDateTime(reader["updated_at"]).ToString("dd/MM/yyyy HH:mm");
				}
			}
		}

		private void SetAdminNameFromSession()
		{
			if (this.Master == null) return;

			var master = this.Master as SiteMaster;
			if (master?.AdminNameLabel != null)
				master.AdminNameLabel.Text = Session["AdminUser"]?.ToString() ?? "Admin";
		}
	}
}
