<%@ Page Title="Đăng ký" Language="C#" MasterPageFile="~/Home/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WebBanLapTop.Home.Register" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="register-page" style="max-width: 400px; margin: 0 auto; padding: 40px 0;">
        <h3 class="text-center" style="text-align:center; margin-bottom:20px;">Đăng ký tài khoản</h3>

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Style="display:block; text-align:center; margin-bottom:10px;"></asp:Label>

        <div class="form-group" style="margin-bottom:10px;">
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Placeholder="Tên đăng nhập" Style="width:377px; padding:8px;" />
        </div>

        <div class="form-group" style="margin-bottom:10px;">
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Email" Style="width:377px; padding:8px;" />
        </div>

        <div class="form-group" style="margin-bottom:10px;">
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Mật khẩu" Style="width:377px; padding:8px;" />
        </div>

        <div class="form-group" style="margin-bottom:15px;">
            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Xác nhận mật khẩu" Style="width:377px; padding:8px;" />
        </div>

        <asp:Button ID="btnRegister" runat="server" Text="Đăng ký" CssClass="btn btn-success btn-block"
            Style="width:398px; background-color:#28a745; border:none; padding:8px; color:white; cursor:pointer;"
            OnClick="btnRegister_Click" />

        <div style="text-align:center; margin-top:15px;">
            <a href="Login.aspx">Đã có tài khoản? Đăng nhập ngay</a>
        </div>
    </div>
</asp:Content>
