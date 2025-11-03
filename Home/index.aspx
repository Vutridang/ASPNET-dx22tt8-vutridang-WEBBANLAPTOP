<%@ Page Title="Home" Language="C#" MasterPageFile="~/Home/Site.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WebBanLapTop.Home.index" %>
<asp:Content ID="FeaturedProductsContent" ContentPlaceHolderID="FeaturedProducts" runat="server">
	<div class="featured-wrapper">
		<asp:Repeater ID="rptFeaturedProducts" runat="server">
			<ItemTemplate>
				<div class="product-box">
					<div class="product-img">
						<a href='product_detail.aspx?id=<%# Eval("id") %>'>
							<img src='<%# Eval("image_url") %>' alt='<%# Eval("name") %>' />
						</a>
					</div>
					<div class="product-info">
						<h3><%# Eval("name") %></h3>
						<p class="desc"><%# Eval("description") %></p>
						<div class="price">
							<span class="price-text"><%# String.Format("{0:N0}₫", Eval("price")) %></span>
						</div>
						<span>
							<%# RenderAddToCartButton((int)Eval("id"), Convert.ToInt32(Eval("stock")), "FeaturedProductsContent") %>
						</span>
					</div>
				</div>
			</ItemTemplate>
		</asp:Repeater>
	</div>
</asp:Content>

<asp:Content ID="MainContentContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="content_bottom">
		<div class="heading">
			<h3>Danh sách sản phẩm</h3>
		</div>
		<div class="clear"></div>
		<div class="section group" id="ProductsList">
			<asp:Repeater ID="rptProducts" runat="server">
				<ItemTemplate>
					<div class="grid_1_of_4 images_1_of_4">
						<a href='product_detail.aspx?id=<%# Eval("id") %>'>
							<img src='<%# Eval("image_url") %>' alt='<%# Eval("name") %>' />
						</a>
						<h2><%# Eval("name") %></h2>
						<p><%# Eval("description") %></p>
						<p><span class="price"><%# String.Format("{0:N0}₫", Eval("price")) %></span></p>
						<div class="button">
							<span>
								<a href='/Home/Product/ProductDetail.aspx?id=<%# Eval("id") %>' style="background: #0d6efd; color: white; padding: 8px 14px; border: none; border-radius: 5px; font-size: 14px; font-weight: 600; cursor: not-allowed; display: inline-block; opacity: 0.9;"
									onmouseover="this.style.background='#0b5ed7';"
									onmouseout="this.style.background='#0d6efd';">Chi tiết</a>
							</span>
							<span>
								<%# RenderAddToCartButton((int)Eval("id"), Convert.ToInt32(Eval("stock")), "MainContentContent") %>
							</span>
						</div>
					</div>
				</ItemTemplate>
			</asp:Repeater>
		</div>
	</div>
	<script type="text/javascript">
		function addToCart(productId) {
			// Gửi request đến index.aspx để thêm giỏ hàng
			window.location.href = 'index.aspx?add=' + productId;
		}
	</script>
</asp:Content>

