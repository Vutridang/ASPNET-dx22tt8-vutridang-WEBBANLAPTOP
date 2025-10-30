using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebBanLapTop
{
	public partial class index : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				lblMessage.Text = "Chào bạn! Đây là lần đầu tải trang.";
			}
		}

		protected void btnClick_Click(object sender, EventArgs e)
		{
			lblMessage.Text = "Bạn đã nhấn nút thành công!";
		}
	}
}