<%@ Page Title="Quản Lý Người Dùng" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="WebBanLapTop.Admin.User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Quản Lý Người Dùng</h2>

    <p>
        <a href="Create_User.aspx" class="btn btn-success">Thêm Người Dùng Mới</a>
    </p>
    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" DataKeyNames="id">
        <Columns>
            <asp:BoundField DataField="id" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="username" HeaderText="Tên Đăng Nhập" />
            <asp:BoundField DataField="email" HeaderText="Email" />
            <asp:BoundField DataField="role" HeaderText="Vai Trò" />
            <asp:BoundField DataField="created_at" HeaderText="Ngày Tạo" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
            <asp:BoundField DataField="updated_at" HeaderText="Ngày Cập Nhật" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
            <asp:TemplateField HeaderText="Hành Động">
                <ItemTemplate>
                    <a href='Edit_User.aspx?id=<%# Eval("id") %>' class="btn btn-primary btn-sm">Sửa</a>
                    <a href='Delete_User.aspx?id=<%# Eval("id") %>' class="btn btn-danger btn-sm" onclick="return confirm('Bạn có chắc muốn xóa người dùng này?');">Xóa</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
