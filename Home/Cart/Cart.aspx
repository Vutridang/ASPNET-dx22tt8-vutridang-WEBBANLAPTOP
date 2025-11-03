<%@ Page Title="Giỏ hàng" Language="C#" MasterPageFile="~/Home/Site.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="WebBanLapTop.Cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
	<div class="main">
		<div class="content">
			<div class="cartoption">
				<div class="cartpage">
					<h2>Giỏ hàng</h2>

					<asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" CssClass="tblone" OnRowCommand="gvCart_RowCommand">
						<Columns>
							<asp:BoundField DataField="Name" HeaderText="Tên sản phẩm" />

							<asp:TemplateField HeaderText="Hình ảnh">
								<ItemTemplate>
									<img src='<%# Eval("ImageUrl") %>' alt="product" style="width: 80px; height: 80px;" />
								</ItemTemplate>
							</asp:TemplateField>

							<asp:BoundField DataField="Price" HeaderText="Giá (VNĐ)" DataFormatString="{0:N0}" />

							<asp:TemplateField HeaderText="Số lượng">
								<ItemTemplate>
									<div style="display: flex; align-items: center; gap: 6px;">
										<asp:TextBox ID="txtQty" runat="server"
											Text='<%# Eval("Quantity") %>'
											TextMode="SingleLine"
											onkeypress="return event.charCode >= 48 && event.charCode <= 57"
											Style="width: 60px; height: 32px; text-align: center; border: 1px solid #ccc; border-radius: 5px; font-size: 14px;" />
										<asp:LinkButton ID="btnUpdate" runat="server"
											CommandName="UpdateItem"
											CommandArgument='<%# Eval("ProductId") %>'
											CssClass="btn-update-qty"
											ToolTip="Cập nhật"
											CausesValidation="false">
											<span class="bi bi-arrow-repeat"></span>
										</asp:LinkButton>
									</div>
								</ItemTemplate>
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Thành tiền (VNĐ)">
								<ItemTemplate>
									<%# String.Format("{0:N0}", Convert.ToDecimal(Eval("Price")) * Convert.ToInt32(Eval("Quantity"))) %>
								</ItemTemplate>
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Thao tác">
								<ItemTemplate>
									<asp:LinkButton ID="btnRemove" runat="server"
										CommandName="RemoveItem"
										CommandArgument='<%# Eval("ProductId") %>'
										Text="X"
										ForeColor="Red" />
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>

						<EmptyDataTemplate>
							<table class="tblone" style="width: 100%;">
								<thead>
									<tr>
										<th>Tên sản phẩm</th>
										<th>Hình ảnh</th>
										<th>Giá (VNĐ)</th>
										<th>Số lượng</th>
										<th>Thành tiền (VNĐ)</th>
										<th>Thao tác</th>
									</tr>
								</thead>
								<tbody>
									<tr>
										<td colspan="6" style="text-align: center; color: #999; padding: 20px;">🛒 Giỏ hàng của bạn hiện đang trống.
										</td>
									</tr>
								</tbody>
							</table>
						</EmptyDataTemplate>
					</asp:GridView>


					<br />

					<asp:Panel ID="pnlTotal" runat="server" Visible="false">
						<table style="float: right; text-align: left;" width="40%">
							<tr>
								<th>Tổng cộng:</th>
								<td>
									<asp:Label ID="lblSubTotal" runat="server" /></td>
							</tr>
							<tr>
								<th>VAT (10%):</th>
								<td>
									<asp:Label ID="lblVAT" runat="server" /></td>
							</tr>
							<tr>
								<th>Thành tiền:</th>
								<td>
									<asp:Label ID="lblGrandTotal" runat="server" /></td>
							</tr>
						</table>
					</asp:Panel>

					<div class="clear"></div>

					<div class="shopping">
						<a href="/Home/index.aspx" class="btn-shop left">
							<i class="bi bi-arrow-left-circle"></i>
							<span>Tiếp tục mua hàng</span>
						</a>

						<asp:UpdatePanel ID="upCheckout" runat="server">
							<ContentTemplate>
								<asp:LinkButton ID="btnCheckout" runat="server" CssClass="btn-shop right"
									OnClick="btnCheckout_Click">
									<i class="bi bi-credit-card-fill"></i>
									<span>Thanh toán</span>
								</asp:LinkButton>
							</ContentTemplate>
						</asp:UpdatePanel>
					</div>


					<div class="clear"></div>
				</div>
			</div>
		</div>
	</div>

	<!-- Popup kiểm tra tồn kho -->
	<div id="stockModal" runat="server" class="modal" style="display: none;">
		<div class="modal-content"
			style="background: #fff; width: 600px; margin: 100px auto; border-radius: 10px; padding: 20px; box-shadow: 0 0 15px rgba(0,0,0,0.3);">
			<h3 style="text-align: center;">Danh sách sản phẩm không đủ hàng</h3>

			<table style="width: 100%; border-collapse: collapse; margin-top: 15px;">
				<thead>
					<tr style="background: #f5f5f5;">
						<th style="padding: 8px; border: 1px solid #ddd;">Hình ảnh</th>
						<th style="padding: 8px; border: 1px solid #ddd;">Tên sản phẩm</th>
						<th style="padding: 8px; border: 1px solid #ddd;">Tồn kho</th>
					</tr>
				</thead>
				<tbody id="tblStockBody" runat="server"></tbody>
			</table>

			<div style="text-align: center; margin-top: 20px;">
				<asp:Button ID="btnUpdateCart" runat="server" Text="Cập nhật giỏ hàng"
					OnClick="btnUpdateCart_Click"
					Style="background: #28a745; color: #fff; border: none; padding: 10px 20px; border-radius: 5px; margin-right: 10px;" />

				<asp:Button ID="btnCancelCheckout" runat="server" Text="Hủy"
					OnClick="btnCancelCheckout_Click"
					Style="background: #dc3545; color: #fff; border: none; padding: 10px 20px; border-radius: 5px;" />
			</div>
		</div>
	</div>


</asp:Content>
