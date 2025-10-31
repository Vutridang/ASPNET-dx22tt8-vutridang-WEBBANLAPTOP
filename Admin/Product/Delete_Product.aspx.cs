using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace WebBanLapTop.Admin.Product
{
	public partial class Delete_Product : System.Web.UI.Page
	{
		// Khai báo connection string
		string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Chặn truy cập nếu chưa đăng nhập
			if (Session["AdminUser"] == null)
			{
				Response.Redirect("~/Admin/Login.aspx");
				return;
			}

			if (!int.TryParse(Request.QueryString["id"], out int productId))
			{
				Response.Redirect("Product.aspx");
				return;
			}

			string imageUrl = null;

			// Lấy image_url hiện tại để xóa file
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sqlGet = "SELECT image_url FROM product WHERE id=@id";
				SqlCommand cmdGet = new SqlCommand(sqlGet, conn);
				cmdGet.Parameters.AddWithValue("@id", productId);
				conn.Open();
				object result = cmdGet.ExecuteScalar();
				imageUrl = result?.ToString();
			}

			// Xóa sản phẩm
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "DELETE FROM product WHERE id=@id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@id", productId);
				conn.Open();
				cmd.ExecuteNonQuery();
			}

			// Xóa file hình ảnh nếu tồn tại
			if (!string.IsNullOrEmpty(imageUrl))
			{
				string filePath = Server.MapPath(imageUrl);
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
				}
			}

			// Lưu thông báo vào Session
			Session["ToastMessage"] = "Xóa sản phẩm thành công!";

			// Quay lại trang danh sách sản phẩm
			Response.Redirect("Product.aspx", false);
			Context.ApplicationInstance.CompleteRequest();
		}
	}
}
