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


		// Logout
		protected void lnkLogout_Click(object sender, EventArgs e)
		{
			Session.Clear(); // Xóa session
			Response.Redirect("Login.aspx");
		}
	}
}
