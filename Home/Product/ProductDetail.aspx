<%@ Page Title="Chi tiết sản phẩm" Language="C#" MasterPageFile="~/Home/Site.Master"
	AutoEventWireup="true" CodeBehind="ProductDetail.aspx.cs" Inherits="WebBanLapTop.Home.ProductDetail" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="main">
		<div class="content">
			<div class="section group">
				<!-- Bên trái: mô tả chi tiết sản phẩm -->
				<div class="cont-desc span_1_of_2">
					<div class="grid images_3_of_2">
						<asp:Image ID="imgProduct" runat="server" AlternateText="Hình sản phẩm" />
					</div>

					<div class="desc span_3_of_2">
						<h2>
							<asp:Label ID="lblName" runat="server" /></h2>

						<p>
							<asp:Label ID="lblShortDesc" runat="server" />
						</p>

						<div class="price">
							<p>
								Giá: <span>
									<asp:Label ID="lblPrice" runat="server" /></span>
							</p>
							<p>
								Danh mục: <span>
									<asp:Label ID="lblCategory" runat="server" /></span>
							</p>
							<p>
								Thương hiệu: <span>
									<asp:Label ID="lblBrand" runat="server" /></span>
							</p>
							<p>
								Số lượng còn lại: <span>
									<asp:Label ID="lblStock" runat="server" /></span>
							</p>
						</div>

						<div class="add-cart">
							<div class="quantity-box">
								<label for="txtQuantity">Số lượng:</label>
								<div class="quantity-control">
									<button type="button" onclick="changeQty(-1)">−</button>
									<asp:TextBox ID="txtQuantity" runat="server" CssClass="buyfield" Text="1" TextMode="SingleLine" />
									<button type="button" onclick="changeQty(1)">+</button>
								</div>
							</div>

							<script type="text/javascript">
								function changeQty(val) {
									var qty = document.getElementById("<%= txtQuantity.ClientID %>");
									var current = parseInt(qty.value) || 1;
									current += val;
									if (current < 1) current = 1;
									qty.value = current;
								}
							</script>

						</div>
						<br />
						<br />
						<asp:Button ID="btnAddToCart" runat="server"
							CssClass="buysubmit" Text="Mua ngay"
							OnClick="btnAddToCart_Click" />
					</div>

					<!-- Phần mô tả chi tiết -->
					<div class="product-desc">
						<h2>Chi tiết sản phẩm</h2>
						<p>
							<asp:Label ID="lblDescription" runat="server" />
						</p>
					</div>
				</div>

				<!-- Bên phải: sidebar danh mục -->
				<div class="rightsidebar span_3_of_1">
					<h2>Danh mục</h2>
					<ul>
						<asp:Repeater ID="rptCategories" runat="server">
							<ItemTemplate>
								<li>
									<a href='ProductByCategory.aspx?catid=<%# Eval("id") %>'>
										<%# Eval("category_name") %>
									</a>
								</li>
							</ItemTemplate>
						</asp:Repeater>
					</ul>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
