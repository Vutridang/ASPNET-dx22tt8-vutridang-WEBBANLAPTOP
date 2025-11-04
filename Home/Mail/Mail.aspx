<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mail.aspx.cs" Inherits="WebBanLapTop.Home.Mail.Mail" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Test MailTrap</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:TextBox ID="txtTo" runat="server" Placeholder="Nhập email nhận" /><br />
        <asp:Button ID="btnSend" runat="server" Text="Gửi mail test" OnClick="btnSend_Click" /><br />
        <asp:Label ID="lblMsg" runat="server" ForeColor="Green"></asp:Label>
    </form>
</body>
</html>
