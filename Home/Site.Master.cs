using System;
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
			}
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
	}
}
