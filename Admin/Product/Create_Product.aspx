<%@ Page Title="Thêm Sản Phẩm" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Create_Product.aspx.cs" Inherits="WebBanLapTop.Admin.Product.Create_Product" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Thêm Sản Phẩm Mới</h2>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

    <asp:Panel ID="pnlForm" runat="server">
        <div class="form-group">
            <label>Tên Sản Phẩm</label>
            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label>Mô Tả</label>
            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
        </div>

        <div class="form-group">
            <label>Giá (VND)</label>
            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label>Tồn Kho</label>
            <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label>Danh Mục</label>
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>

        <div class="form-group">
            <label>Hình Ảnh</label>
            <asp:FileUpload ID="fileUpload" runat="server" CssClass="form-control" />
        </div>

        <asp:Button ID="btnCreate" runat="server" Text="Tạo" CssClass="btn btn-success" OnClick="btnCreate_Click" />
        <a href="Product.aspx" class="btn btn-secondary">Hủy</a>
    </asp:Panel>
</asp:Content>
