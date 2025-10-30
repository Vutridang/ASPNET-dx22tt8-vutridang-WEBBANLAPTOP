<%@ Page Title="Sửa Danh Mục" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Edit_Category.aspx.cs" Inherits="WebBanLapTop.Admin.Category.Edit_Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Sửa Danh Mục</h2>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

    <asp:Panel ID="pnlForm" runat="server" Visible="false">
        <div class="form-group">
            <label>Tên Danh Mục</label>
            <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" />
        </div>

        <asp:Button ID="btnUpdate" runat="server" Text="Cập Nhật" CssClass="btn btn-primary" OnClick="btnUpdate_Click" />
        <a href="Category.aspx" class="btn btn-secondary">Hủy</a>
    </asp:Panel>
</asp:Content>
