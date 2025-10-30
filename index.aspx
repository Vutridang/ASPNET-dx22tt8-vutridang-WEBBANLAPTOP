<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WebBanLapTop.index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Trang chủ Laptop Store</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align:center; margin-top:50px;">
            <h1>Chào mừng đến với Laptop Store</h1>
            <asp:Label ID="lblMessage" runat="server" Text="Click nút bên dưới để thử postback" />
            <br /><br />
            <asp:Button ID="btnClick" runat="server" Text="Nhấn vào đây" OnClick="btnClick_Click" />
        </div>
    </form>
</body>
</html>
