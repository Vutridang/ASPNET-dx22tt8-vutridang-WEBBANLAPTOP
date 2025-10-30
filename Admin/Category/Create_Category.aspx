<%@ Page Title="Thêm Danh Mục" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Create_Category.aspx.cs" Inherits="WebBanLapTop.Admin.Category.Create_Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Thêm Danh Mục Mới</h2>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

    <asp:Panel ID="pnlForm" runat="server">
        <div class="form-group">
            <label>Tên Danh Mục</label>
            <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" />
        </div>

        <asp:Button ID="btnCreate" runat="server" Text="Tạo" CssClass="btn btn-success" OnClick="btnCreate_Click" />
        <a href="Category.aspx" class="btn btn-secondary">Hủy</a>
    </asp:Panel>
</asp:Content>
