using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace WebBanLapTop.Admin.Brand
{
	public partial class Edit_Brand : System.Web.UI.Page
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

			if (!IsPostBack)
			{
				LoadBrand();
			}
		}

		private void LoadBrand()
		{
			if (!int.TryParse(Request.QueryString["id"], out int brandId))
			{
				Response.Redirect("Brand.aspx");
				return;
			}

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "SELECT * FROM brand WHERE id=@id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@id", brandId);

				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					txtName.Text = reader["name"].ToString();
					txtDescription.Text = reader["description"].ToString();
					chkIsTop.Checked = Convert.ToBoolean(reader["is_top"]);

					string logoUrl = reader["logo_url"].ToString();
					if (!string.IsNullOrEmpty(logoUrl))
						imgCurrent.ImageUrl = logoUrl;
				}
				else
				{
					Response.Redirect("Brand.aspx");
				}
			}
		}

		protected void btnUpdate_Click(object sender, EventArgs e)
		{
			if (!int.TryParse(Request.QueryString["id"], out int brandId))
			{
				Response.Redirect("Brand.aspx");
				return;
			}

			string name = txtName.Text.Trim();
			string description = txtDescription.Text.Trim();
			bool isTop = chkIsTop.Checked;
			string newLogoUrl = null;

			if (string.IsNullOrEmpty(name))
			{
				lblMessage.Text = "Vui lòng nhập tên thương hiệu.";
				return;
			}

			// Upload logo mới
			if (fileUpload.HasFile)
			{
				string fileName = Path.GetFileName(fileUpload.PostedFile.FileName);
				string folderPath = Server.MapPath("~/images/brands/");
				if (!Directory.Exists(folderPath))
					Directory.CreateDirectory(folderPath);

				string savePath = Path.Combine(folderPath, fileName);

				// Lấy logo cũ để xóa
				string oldLogoUrl = null;
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					string sqlGet = "SELECT logo_url FROM brand WHERE id=@id";
					SqlCommand cmdGet = new SqlCommand(sqlGet, conn);
					cmdGet.Parameters.AddWithValue("@id", brandId);
					conn.Open();
					object result = cmdGet.ExecuteScalar();
					oldLogoUrl = result?.ToString();
				}

				try
				{
					fileUpload.SaveAs(savePath);
					newLogoUrl = "/images/brands/" + fileName;

					if (!string.IsNullOrEmpty(oldLogoUrl))
					{
						string oldFilePath = Server.MapPath(oldLogoUrl);
						if (File.Exists(oldFilePath))
							File.Delete(oldFilePath);
					}
				}
				catch (Exception ex)
				{
					lblMessage.Text = "Lỗi upload hình: " + ex.Message;
					return;
				}
			}

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				string sql = "UPDATE brand SET name=@name, description=@description, is_top=@is_top";
				if (newLogoUrl != null)
					sql += ", logo_url=@logo_url";
				sql += " WHERE id=@id";

				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@id", brandId);
				cmd.Parameters.AddWithValue("@name", name);
				cmd.Parameters.AddWithValue("@description", (object)description ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@is_top", isTop);
				if (newLogoUrl != null)
					cmd.Parameters.AddWithValue("@logo_url", newLogoUrl);

				try
				{
					conn.Open();
					cmd.ExecuteNonQuery();
					Session["ToastMessage"] = "Cập nhật thương hiệu thành công!";
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
