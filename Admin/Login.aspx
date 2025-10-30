<%@ Page Title="Admin Login" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebBanLapTop.Admin.Login" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <h3 class="text-center mb-4">Đăng nhập</h3>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control mb-2" Placeholder="Username"></asp:TextBox>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control mb-2" TextMode="Password" Placeholder="Password"></asp:TextBox>
            <asp:Button ID="btnLogin" runat="server" Text="Đăng nhập" CssClass="btn btn-primary btn-block" OnClick="btnLogin_Click"/>
        </div>
    </div>
</asp:Content>
