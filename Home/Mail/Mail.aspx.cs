using System;
using System.Net;
using System.Net.Mail;

namespace WebBanLapTop.Home.Mail
{
	public partial class Mail : System.Web.UI.Page
	{
		protected void btnSend_Click(object sender, EventArgs e)
		{
			try
			{
				System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

				MailMessage mail = new MailMessage();
				mail.From = new MailAddress("no-reply@webbanlaptop.com", "Web Bán Laptop");
				mail.To.Add(txtTo.Text);
				mail.Subject = "Test gửi mail từ MailTrap";
				mail.Body = "Đây là mail test từ dự án Web Bán Laptop (.NET 3.5).";

				var client = new SmtpClient("smtp.mailtrap.io", 587)
				{
					Credentials = new NetworkCredential("7f27b16b4de167", "6825ccb974ca89"),
					EnableSsl = true
				};
				client.Send(mail);

				lblMsg.Text = "✅ Gửi mail thành công!";
				lblMsg.ForeColor = System.Drawing.Color.Green;
			}
			catch (Exception ex)
			{
				string msg = ex.Message;
				if (ex.InnerException != null)
					msg += " | Inner: " + ex.InnerException.Message;

				Response.Write("<b style='color:red'>Lỗi gửi mail:</b> " + msg);
			}
		}
	}
}
