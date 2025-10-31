using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace WebBanLapTop.Admin.Brand
{
	public partial class Create_Brand : System.Web.UI.Page
	{
		private string connStr;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["AdminUser"] == null)
			{
				Response.Redirect("~/Admin/Login.aspx");
				return;
			}

			ConnectionStringSettings connSetting = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"];
			if (connSetting == null)
				throw new Exception("ConnectionString 'WebBanLapTopConnection' chưa được khai báo trong web.config.");
			connStr = connSetting.ConnectionString;
		}

		protected void btnCreate_Click(object sender, EventArgs e)
		{
			string name = txtName.Text.Trim();
			string description = txtDescription.Text.Trim();
			bool isTop = chkIsTop.Checked;

			if (string.IsNullOrEmpty(name))
			{
				lblMessage.Text = "Vui lòng nhập tên thương hiệu.";
				return;
			}

			// Xử lý logo
			string logoUrl = null;
			if (fileUpload.HasFile)
			{
				string fileName = Path.GetFileName(fileUpload.PostedFile.FileName);
				string folderPath = Server.MapPath("~/images/brands/");
				if (!Directory.Exists(folderPath))
					Directory.CreateDirectory(folderPath);

				string savePath = Path.Combine(folderPath, fileName);
				try
				{
					fileUpload.SaveAs(savePath);
					logoUrl = "/images/brands/" + fileName;
				}
				catch (Exception ex)
				{
					lblMessage.Text = "Lỗi upload hình: " + ex.Message;
					return;
				}
			}

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "INSERT INTO brand (name, logo_url, description, is_top) VALUES (@name, @logo_url, @description, @is_top)";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@name", name);
				cmd.Parameters.AddWithValue("@logo_url", (object)logoUrl ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@description", (object)description ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@is_top", isTop);

				try
				{
					conn.Open();
					cmd.ExecuteNonQuery();
					Session["ToastMessage"] = "Thêm thương hiệu thành công!";
					Response.Redirect("Brand.aspx", false);
					Context.ApplicationInstance.CompleteRequest();
				}
				catch (Exception ex)
				{
					lblMessage.Text = "Lỗi: " + ex.Message;
				}
			}
		}
	}
}
