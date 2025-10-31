<%@ Page Title="Chi Tiết Đơn Hàng" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Order_Item.aspx.cs" Inherits="WebBanLapTop.Admin.Order_Item.Order_Item" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Chi Tiết Đơn Hàng - Order ID:
		<asp:Label ID="lblOrderID" runat="server" /></h2>

	<asp:GridView ID="gvOrderItems" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped">
		<Columns>
			<asp:TemplateField HeaderText="STT">
				<ItemTemplate>
					<%# Container.DataItemIndex + 1 %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:BoundField DataField="product_name" HeaderText="Sản Phẩm" />
			<asp:BoundField DataField="quantity" HeaderText="Số Lượng" />
			<asp:BoundField DataField="price" HeaderText="Giá" DataFormatString="{0:N0} VND" />
			<asp:BoundField DataField="total" HeaderText="Thành Tiền" DataFormatString="{0:N0} VND" />
		</Columns>
	</asp:GridView>
</asp:Content>
