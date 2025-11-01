<%@ Page Title="Đăng nhập" Language="C#" MasterPageFile="~/Home/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebBanLapTop.Home.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    Đăng nhập
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="login-page" style="max-width: 400px; margin: 0 auto; padding: 40px 0;">
        <h3 class="text-center" style="text-align:center; margin-bottom:20px;">Đăng nhập tài khoản</h3>

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Style="display:block; text-align:center; margin-bottom:10px;"></asp:Label>

        <div class="form-group" style="margin-bottom:10px;">
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Placeholder="Tên đăng nhập" Style="width:377px; padding:8px;"/>
        </div>

        <div class="form-group" style="margin-bottom:15px;">
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Mật khẩu" Style="width:377px; padding:8px;"/>
        </div>

        <asp:Button ID="btnLogin" runat="server" Text="Đăng nhập" CssClass="btn btn-primary btn-block"
            Style="width:398px; background-color:#007bff; border:none; padding:8px; color:white; cursor:pointer;"
            OnClick="btnLogin_Click" />

        <div style="text-align:center; margin-top:15px;">
            <a href="register.aspx">Chưa có tài khoản? Đăng ký ngay</a>
        </div>
    </div>
</asp:Content>
