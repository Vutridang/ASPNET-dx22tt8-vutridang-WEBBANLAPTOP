<%@ Page Title="Thêm Người Dùng" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Create_User.aspx.cs" Inherits="WebBanLapTop.Admin.Create_User" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Thêm Người Dùng Mới</h2>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    <asp:Panel ID="pnlForm" runat="server">
        <div class="form-group">
            <label>Tên Đăng Nhập</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
        </div>
        <div class="form-group">
            <label>Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
        </div>
        <div class="form-group">
            <label>Mật Khẩu</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
        </div>
        <div class="form-group">
            <label>Vai Trò</label>
            <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
                <asp:ListItem Text="Customer" Value="customer" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Admin" Value="admin"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <asp:Button ID="btnCreate" runat="server" Text="Tạo" CssClass="btn btn-success" OnClick="btnCreate_Click" />
        <a href="User.aspx" class="btn btn-secondary">Hủy</a>
    </asp:Panel>
</asp:Content>
