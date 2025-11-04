<%@ Page Title="Quản Lý Đơn Hàng" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="WebBanLapTop.Admin.Order.Order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Quản Lý Đơn Hàng</h2>

	<asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" DataKeyNames="id" OnRowDataBound="gvOrders_RowDataBound">
		<Columns>
			<asp:TemplateField HeaderText="STT">
				<ItemTemplate>
					<%# Container.DataItemIndex + 1 %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:BoundField DataField="customer_name" HeaderText="Người Đặt" />
			<asp:BoundField DataField="total_amount" HeaderText="Tổng Tiền" DataFormatString="{0:N0} VND" />
			<asp:TemplateField HeaderText="Trạng Thái">
				<ItemTemplate>
					<asp:Label ID="lblStatusVN" runat="server"></asp:Label>
				</ItemTemplate>
			</asp:TemplateField>


			<asp:TemplateField HeaderText="Cập nhật trạng thái">
				<ItemTemplate>
					<asp:DropDownList ID="ddlUpdateStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUpdateStatus_SelectedIndexChanged"></asp:DropDownList>
				</ItemTemplate>
			</asp:TemplateField>

			<asp:BoundField DataField="created_at" HeaderText="Ngày Tạo" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
			<asp:BoundField DataField="updated_at" HeaderText="Ngày Cập Nhật" DataFormatString="{0:dd/MM/yyyy HH:mm}" />

			<asp:TemplateField HeaderText="Hành Động">
				<ItemTemplate>
					<a href='../Order_Item/Order_Item.aspx?order_id=<%# Eval("id") %>' class="btn btn-info btn-sm">Chi tiết</a>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
	<!-- ✅ Phân trang -->
	<div class="pagination text-center">
		<asp:LinkButton ID="btnPrev" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnPrev_Click">« Trước</asp:LinkButton>
		<asp:Label ID="lblPageInfo" runat="server" CssClass="mx-2" />
		<asp:LinkButton ID="btnNext" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnNext_Click">Sau »</asp:LinkButton>
	</div>
</asp:Content>
