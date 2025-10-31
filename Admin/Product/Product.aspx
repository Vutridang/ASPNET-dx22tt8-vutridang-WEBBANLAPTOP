<%@ Page Title="Quản Lý Sản Phẩm" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="WebBanLapTop.Admin.Product.Product" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Quản Lý Sản Phẩm</h2>
	<p>
		<a href="Create_Product.aspx" class="btn btn-success">Thêm Sản Phẩm Mới</a>
	</p>
	<asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" DataKeyNames="id">
		<Columns>
			<asp:TemplateField HeaderText="STT">
				<ItemTemplate>
					<%# Container.DataItemIndex + 1 %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:BoundField DataField="name" HeaderText="Tên Sản Phẩm" />
			<asp:BoundField DataField="description" HeaderText="Mô Tả" />
			<asp:BoundField DataField="price" HeaderText="Giá" DataFormatString="{0:N0} VND" />
			<asp:BoundField DataField="stock" HeaderText="Tồn Kho" />
			<asp:TemplateField HeaderText="Hình Ảnh">
				<ItemTemplate>
					<asp:Image ID="imgProduct" runat="server" Width="100px" ImageUrl='<%# Eval("image_url") %>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:BoundField DataField="created_at" HeaderText="Ngày Tạo" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
			<asp:BoundField DataField="updated_at" HeaderText="Ngày Cập Nhật" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
			<asp:TemplateField HeaderText="Hành Động">
				<ItemTemplate>
					<a href='Edit_Product.aspx?id=<%# Eval("id") %>' class="btn btn-primary btn-sm">Sửa</a>
					<a href="javascript:void(0);"
						class="btn btn-danger btn-sm btn-delete"
						data-id='<%# Eval("id") %>'
						data-type="product"
						data-name='<%# Eval("name") %>'>Xóa
					</a>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
</asp:Content>
