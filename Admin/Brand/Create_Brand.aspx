<%@ Page Title="Thêm Thương Hiệu" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Create_Brand.aspx.cs" Inherits="WebBanLapTop.Admin.Brand.Create_Brand" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Thêm Thương Hiệu Mới</h2>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

    <asp:Panel ID="pnlForm" runat="server">
        <div class="form-group">
            <label>Tên Thương Hiệu</label>
            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label>Mô Tả</label>
            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
        </div>

        <div class="form-group">
            <label>Logo</label>
            <asp:FileUpload ID="fileUpload" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group form-check">
            <asp:CheckBox ID="chkIsTop" runat="server" CssClass="form-check-input" />
            <label class="form-check-label" for="chkIsTop">Là thương hiệu nổi bật (Top Brand)</label>
        </div>

        <asp:Button ID="btnCreate" runat="server" Text="Tạo" CssClass="btn btn-success" OnClick="btnCreate_Click" />
        <a href="Brand.aspx" class="btn btn-secondary">Hủy</a>
    </asp:Panel>
</asp:Content>
