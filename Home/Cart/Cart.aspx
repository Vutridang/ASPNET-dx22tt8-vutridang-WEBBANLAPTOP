<%@ Page Title="Giỏ hàng" Language="C#" MasterPageFile="~/Home/Site.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="WebBanLapTop.Cart" %>

<asp:Content ID="CartContent" ContentPlaceHolderID="MainContent" runat="server">
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
										<input type="number"
											id="qty_<%# Eval("ProductId") %>"
											name="qty"
											value='<%# Eval("Quantity") %>'
											min="1"
											style="width: 60px; height: 32px; text-align: center; border: 1px solid #ccc; border-radius: 5px; font-size: 14px;" />

										<asp:LinkButton ID="btnUpdate" runat="server"
											CommandName="UpdateItem"
											CommandArgument='<%# Eval("ProductId") %>'
											CssClass="btn-update-qty"
											Text="⟳"
											ToolTip="Cập nhật" />
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
									<asp:LinkButton ID="btnRemove" runat="server" CommandName="RemoveItem" CommandArgument='<%# Eval("ProductId") %>' Text="X" ForeColor="Red" />
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

						<a href="/Home/Account/Login.aspx" class="btn-shop right">
							<i class="bi bi-credit-card-fill"></i>
							<span>Thanh toán</span>
						</a>
					</div>


					<div class="clear"></div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
