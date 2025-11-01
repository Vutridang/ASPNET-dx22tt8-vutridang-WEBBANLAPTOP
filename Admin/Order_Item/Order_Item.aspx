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
			<asp:BoundField DataField="brand_name" HeaderText="Thương Hiệu" />
			<asp:BoundField DataField="quantity" HeaderText="Số Lượng" />
			<asp:BoundField DataField="price" HeaderText="Giá" DataFormatString="{0:N0} VND" />
			<asp:BoundField DataField="total" HeaderText="Thành Tiền" DataFormatString="{0:N0} VND" />
		</Columns>
	</asp:GridView>

	<h3 class="mt-4">Chi Tiết Khách Hàng</h3>
	<table class="table table-bordered" style="width: 100%;">
		<tbody>
			<tr>
				<th>Username</th>
				<td>
					<asp:Label ID="lblUsername" runat="server" /></td>
			</tr>
			<tr>
				<th>Email</th>
				<td>
					<asp:Label ID="lblEmail" runat="server" /></td>
			</tr>
			<tr>
				<th>Quyền</th>
				<td>
					<asp:Label ID="lblRole" runat="server" /></td>
			</tr>
			<tr>
				<th>Address</th>
				<td>
					<asp:Label ID="lblAddress" runat="server" /></td>
			</tr>
			<tr>
				<th>Zipcode</th>
				<td>
					<asp:Label ID="lblZipcode" runat="server" /></td>
			</tr>
			<tr>
				<th>Phương Thức Thanh Toán</th>
				<td>
					<asp:Label ID="lblPaymentMethod" runat="server" /></td>
			</tr>
			<tr>
				<th>Ngày Tạo</th>
				<td>
					<asp:Label ID="lblCreatedAt" runat="server" /></td>
			</tr>
			<tr>
				<th>Cập Nhật Lần Cuối</th>
				<td>
					<asp:Label ID="lblUpdatedAt" runat="server" /></td>
			</tr>
		</tbody>
	</table>

</asp:Content>
