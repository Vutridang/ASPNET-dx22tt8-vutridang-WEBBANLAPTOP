<%@ Page Title="Quản Lý Danh Mục" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="WebBanLapTop.Admin.Category.Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Quản Lý Danh Mục</h2>

	<p>
		<a href="Create_Category.aspx" class="btn btn-success">Thêm Danh Mục Mới</a>
	</p>

	<asp:GridView ID="gvCategory" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" DataKeyNames="id">
		<Columns>
			<asp:BoundField DataField="id" HeaderText="ID" ReadOnly="True" />
			<asp:BoundField DataField="category_name" HeaderText="Tên Danh Mục" />
			<asp:BoundField DataField="created_at" HeaderText="Ngày Tạo" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
			<asp:BoundField DataField="updated_at" HeaderText="Ngày Cập Nhật" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
			<asp:TemplateField HeaderText="Hành Động">
				<ItemTemplate>
					<a href='Edit_Category.aspx?id=<%# Eval("id") %>' class="btn btn-primary btn-sm">Sửa</a>
					<a href="javascript:void(0);"
						class="btn btn-danger btn-sm btn-delete"
						data-id='<%# Eval("id") %>'
						data-type="category"
						data-name='<%# Eval("category_name") %>'>Xóa
					</a>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
</asp:Content>
