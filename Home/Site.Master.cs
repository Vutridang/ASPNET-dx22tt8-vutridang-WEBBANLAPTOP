using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebBanLapTop.Home
{
	public partial class SiteMaster : MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				string currentPage = System.IO.Path.GetFileName(Request.Path);
				headerBottom.Visible = string.Equals(currentPage, "index.aspx", StringComparison.OrdinalIgnoreCase);

				string loggedUser = null;
				if (Session["CustomerUser"] != null)
					loggedUser = Session["CustomerUser"].ToString();
				else if (Session["AdminUser"] != null)
					loggedUser = Session["AdminUser"].ToString();

				if (!string.IsNullOrEmpty(loggedUser))
				{
					divAuthButtons.Visible = false;
					divUserInfo.Visible = true;
					lblUserNameTop.Text = loggedUser;
				}
				else
				{
					divAuthButtons.Visible = true;
					divUserInfo.Visible = false;
				}

				UpdateCartCount();
			}
		}

		private void UpdateCartCount()
		{
			int totalItems = 0;

			// 🧩 Trường hợp CHƯA đăng nhập → lấy từ Session
			if (Session["Cart"] != null)
			{
				var cart = Session["Cart"] as List<CartItem>;
				if (cart != null)
				{
					foreach (var item in cart)
						totalItems += item.Quantity;
				}
			}
			else if (Session["UserId"] != null)
			{
				// 🧩 Trường hợp ĐÃ đăng nhập → lấy từ DB
				string connStr = ConfigurationManager.ConnectionStrings["WebBanLapTopConnection"].ConnectionString;
				int userId = Convert.ToInt32(Session["UserId"]);

				using (SqlConnection conn = new SqlConnection(connStr))
				{
					string sql = @"
						SELECT ISNULL(SUM(ci.quantity), 0)
						FROM cart c
						JOIN cart_item ci ON c.id = ci.cart_id
						WHERE c.user_id = @user_id AND c.is_checked_out = 0";

					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.Parameters.AddWithValue("@user_id", userId);
					conn.Open();

					object result = cmd.ExecuteScalar();
					if (result != DBNull.Value)
						totalItems = Convert.ToInt32(result);
				}
			}

			lblCartCount.Text = totalItems > 9 ? "9+" : totalItems.ToString();
		}


		protected void btnSearch_Click(object sender, EventArgs e)
		{
			string keyword = txtSearch.Text.Trim();

			if (keyword == "Tìm kiếm sản phẩm" || string.IsNullOrEmpty(keyword))
				return;

			string encodedKeyword = Server.UrlEncode(keyword);
			Response.Redirect("~/Home/Product/SearchProduct.aspx?keyword=" + encodedKeyword);
		}



		public void ShowToastFromSession(Page page, string sessionKey = "ToastMessage")
		{
			if (page.Session[sessionKey] != null)
			{
				string msg = page.Session[sessionKey].ToString();
				page.Session[sessionKey] = null;

				string script = $@"
            Swal.fire({{
                toast: true,
                position: 'top-end',
                icon: 'success',
                title: '{msg}',
                showConfirmButton: false,
                timer: 2000,
                timerProgressBar: true
            }});";

				ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), script, true);
			}
		}

		protected void btnLogout_Click(object sender, EventArgs e)
		{
			Session.Clear();
			Session["ToastMessage"] = "Đăng xuất thành công!";
			Response.Redirect("~/Home/index.aspx");
		}

		public string CartCountClientID
		{
			get { return lblCartCount.ClientID; }
		}
	}
}
