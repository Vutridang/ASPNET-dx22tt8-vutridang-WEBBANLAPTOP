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
							oi.quantity,
							oi.price,
							(oi.quantity * oi.price) AS total,
							u.username AS customer_name,
							o.id AS order_id,
							o.status
						FROM order_item oi
						INNER JOIN [order] o ON oi.order_id = o.id
						INNER JOIN product p ON oi.product_id = p.id
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

		private void SetAdminNameFromSession()
		{
			if (this.Master == null) return;

			var master = this.Master as SiteMaster;
			if (master?.AdminNameLabel != null)
				master.AdminNameLabel.Text = Session["AdminUser"]?.ToString() ?? "Admin";
		}
	}
}
