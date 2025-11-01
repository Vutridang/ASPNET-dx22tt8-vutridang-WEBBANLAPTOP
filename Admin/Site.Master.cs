using System;
using System.Web.UI;

namespace WebBanLapTop.Admin
{
	public partial class SiteMaster : MasterPage
	{
		public System.Web.UI.WebControls.Label AdminNameLabel
		{
			get { return lblAdminNameTop; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				// Nếu đã login → hiển thị tên admin
				if (Session["AdminUser"] != null)
				{
					lblAdminNameTop.Text = Session["AdminUser"].ToString();
					liAdminDropdown.Visible = true;
				}
				else
				{
					// Chưa login → ẩn dropdown
					liAdminDropdown.Visible = false;
				}
			}
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



		// Logout
		protected void lnkLogout_Click(object sender, EventArgs e)
		{
			Session.Clear(); // Xóa session
			Session["ToastMessage"] = "Đăng xuất thành công!";
			Response.Redirect("~/Admin/Login.aspx");
		}
	}
}
