using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebBanLapTop.Admin.Order
{
	public partial class Order : System.Web.UI.Page
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
				((SiteMaster)this.Master).ShowToastFromSession(this);
				LoadOrders();
				SetAdminNameFromSession();
			}
		}

		private void LoadOrders()
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = @"
                    SELECT o.id, u.username AS customer_name, o.total_amount, o.status, o.created_at, o.updated_at
                    FROM [order] o
                    INNER JOIN [user] u ON o.user_id = u.id
                    ORDER BY o.id DESC";

				SqlDataAdapter da = new SqlDataAdapter(sql, conn);
				DataTable dt = new DataTable();
				da.Fill(dt);

				gvOrders.DataSource = dt.Rows.Count > 0 ? dt : null;
				gvOrders.DataBind();
			}
		}

		protected void gvOrders_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType != DataControlRowType.DataRow) return;

			string status = DataBinder.Eval(e.Row.DataItem, "status").ToString();

			// Label hiển thị trạng thái tiếng Việt và màu
			Label lblStatus = (Label)e.Row.FindControl("lblStatusVN");
			if (lblStatus != null)
			{
				lblStatus.Text = GetStatusVN(status);
				switch (status)
				{
					case "pending":
						lblStatus.ForeColor = System.Drawing.Color.Orange;
						break;
					case "paid":
						lblStatus.ForeColor = System.Drawing.Color.Green;
						break;
					case "shipped":
						lblStatus.ForeColor = System.Drawing.Color.Blue;
						break;
					case "cancelled":
						lblStatus.ForeColor = System.Drawing.Color.Red;
						break;
					default:
						lblStatus.ForeColor = System.Drawing.Color.Black;
						break;
				}
			}

			// Dropdown để cập nhật trạng thái
			DropDownList ddl = (DropDownList)e.Row.FindControl("ddlUpdateStatus");
			if (ddl != null)
			{
				ddl.Items.Clear();
				ddl.Items.Add(new ListItem("Chờ thanh toán", "pending"));
				ddl.Items.Add(new ListItem("Đã thanh toán", "paid"));
				ddl.Items.Add(new ListItem("Đã giao hàng", "shipped"));
				ddl.Items.Add(new ListItem("Đã hủy", "cancelled"));

				ddl.SelectedValue = status;
			}
		}

		protected string GetStatusVN(string status)
		{
			switch (status)
			{
				case "pending":
					return "Chờ thanh toán";
				case "paid":
					return "Đã thanh toán";
				case "shipped":
					return "Đã giao hàng";
				case "cancelled":
					return "Đã hủy";
				default:
					return status;
			}
		}

		protected void ddlUpdateStatus_SelectedIndexChanged(object sender, EventArgs e)
		{
			DropDownList ddl = (DropDownList)sender;
			GridViewRow row = (GridViewRow)ddl.NamingContainer;
			int orderId = Convert.ToInt32(gvOrders.DataKeys[row.RowIndex].Value);
			string newStatus = ddl.SelectedValue;

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "UPDATE [order] SET status=@status, updated_at=GETDATE() WHERE id=@id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@status", newStatus);
				cmd.Parameters.AddWithValue("@id", orderId);
				conn.Open();
				cmd.ExecuteNonQuery();
			}

			// Toast thông báo
			Session["ToastMessage"] = "Cập nhật trạng thái đơn hàng thành công!";
			((SiteMaster)this.Master).ShowToastFromSession(this);

			LoadOrders(); // reload để label + dropdown hiển thị chính xác
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
