using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace WebBanLapTop.Admin.Brand
{
	public partial class Delete_Brand : System.Web.UI.Page
	{
		private string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Kiểm tra đăng nhập
			if (Session["AdminUser"] == null)
			{
				Response.Redirect("~/Admin/Login.aspx");
				return;
			}

			// Lấy id thương hiệu từ query string
			if (!int.TryParse(Request.QueryString["id"], out int brandId))
			{
				Response.Redirect("Brand.aspx");
				return;
			}

			string logoUrl = null;

			// Lấy logo_url hiện tại để xóa file
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sqlGet = "SELECT logo_url FROM brand WHERE id=@id";
				SqlCommand cmdGet = new SqlCommand(sqlGet, conn);
				cmdGet.Parameters.AddWithValue("@id", brandId);
				conn.Open();
				object result = cmdGet.ExecuteScalar();
				logoUrl = result?.ToString();
			}

			// Xóa thương hiệu
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "DELETE FROM brand WHERE id=@id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@id", brandId);
				conn.Open();
				cmd.ExecuteNonQuery();
			}

			// Xóa file logo nếu tồn tại
			if (!string.IsNullOrEmpty(logoUrl))
			{
				string filePath = Server.MapPath(logoUrl);
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
				}
			}

			// Lưu thông báo vào Session
			Session["ToastMessage"] = "Xóa thương hiệu thành công!";

			// Quay lại trang danh sách thương hiệu
			Response.Redirect("Brand.aspx", false);
			Context.ApplicationInstance.CompleteRequest();
		}
	}
}
