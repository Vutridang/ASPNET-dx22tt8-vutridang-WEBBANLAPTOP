using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebBanLapTop.Admin
{
	public partial class Dashboard : System.Web.UI.Page
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
				LoadSummary();
			}
		}

		private void LoadSummary()
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("STT", typeof(int));
			dt.Columns.Add("Name", typeof(string));
			dt.Columns.Add("Quantity", typeof(int));

			int stt = 1;

			dt.Rows.Add(stt++, "Người dùng", GetCount("user"));
			dt.Rows.Add(stt++, "Sản phẩm", GetCount("product"));
			dt.Rows.Add(stt++, "Danh mục", GetCount("category"));
			dt.Rows.Add(stt++, "Đơn hàng", GetCount("order"));

			gvSummary.DataSource = dt;
			gvSummary.DataBind();
		}

		private int GetCount(string tableName)
		{
			string sql = $"SELECT COUNT(*) FROM [{tableName}]";
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				SqlCommand cmd = new SqlCommand(sql, conn);
				conn.Open();
				return (int)cmd.ExecuteScalar();
			}
		}
	}
}
