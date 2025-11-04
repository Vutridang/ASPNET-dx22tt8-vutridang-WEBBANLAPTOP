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
	<!-- ========== DANH SÁCH SẢN PHẨM ========== -->
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
								<a href='/Home/Product/ProductDetail.aspx?id=<%# Eval("id") %>'
									style="background: #0d6efd; color: white; padding: 8px 14px; border-radius: 5px; font-size: 14px; font-weight: 600;">Chi tiết</a>
							</span>
							<span>
								<%# RenderAddToCartButton((int)Eval("id"), Convert.ToInt32(Eval("stock")), "MainContentContent") %>
							</span>
						</div>
					</div>
				</ItemTemplate>
			</asp:Repeater>
		</div>
		<!-- 🔹 Pagination -->
		<div style="text-align: center; margin-top: 15px;">
			<asp:Button ID="btnPrevProducts" runat="server" Text="« Trước" CssClass="btn-pagination" OnClick="btnPrevProducts_Click" />
			<asp:Label ID="lblPageProducts" runat="server" Text="Trang 1" Style="margin: 0 8px; font-weight: bold;" />
			<asp:Button ID="btnNextProducts" runat="server" Text="Sau »" CssClass="btn-pagination" OnClick="btnNextProducts_Click" />
		</div>
	</div>

	<!-- ========== DANH SÁCH THƯƠNG HIỆU ========== -->
	<div class="content_bottom">
		<div class="heading">
			<h3>Danh sách thương hiệu</h3>
		</div>
		<div class="clear"></div>
		<div class="section group" id="BrandsList">
			<asp:Repeater ID="rptBrands" runat="server">
				<ItemTemplate>
					<div class="grid_1_of_4 images_1_of_4">
						<a href='/Home/Product/ProductByBrand.aspx?id=<%# Eval("id") %>'>
							<img src='<%# Eval("logo_url") %>' alt='<%# Eval("name") %>' style="width: 200px; height: 200px; object-fit: contain; border: 1px solid #eee; border-radius: 10px; padding: 10px; background: #fff;" />
						</a>
						<h2 style="margin-top: 10px; font-size: 16px; font-weight: 600; text-align: center;"><%# Eval("name") %></h2>
						<p style="font-size: 13px; color: #777; text-align: center;"><%# Eval("description") %></p>
						<asp:Panel Visible='<%# Convert.ToBoolean(Eval("is_top")) %>' runat="server">
							<p style="color: #0d6efd; font-weight: 600; text-align: center;">⭐ Thương hiệu nổi bật</p>
						</asp:Panel>
					</div>
				</ItemTemplate>
			</asp:Repeater>
		</div>
		<!-- 🔹 Pagination -->
		<div style="text-align: center; margin-top: 15px;">
			<asp:Button ID="btnPrevBrands" runat="server" Text="« Trước" CssClass="btn-pagination" OnClick="btnPrevBrands_Click" />
			<asp:Label ID="lblPageBrands" runat="server" Text="Trang 1" Style="margin: 0 8px; font-weight: bold;" />
			<asp:Button ID="btnNextBrands" runat="server" Text="Sau »" CssClass="btn-pagination" OnClick="btnNextBrands_Click" />
		</div>
	</div>

	<script type="text/javascript">
		function addToCart(productId) {
			window.location.href = 'index.aspx?add=' + productId;
		}
	</script>

	<style>
		.pagination-wrapper {
			display: flex;
			justify-content: center;
			align-items: center;
			gap: 12px;
			margin-top: 30px;
			clear: both;
		}

		.btn-pagination {
			background-color: #28a745;
			color: #fff !important;
			border: none;
			padding: 6px 18px;
			border-radius: 25px;
			font-weight: 500;
			transition: all 0.2s ease;
			cursor: pointer;
		}

			.btn-pagination:hover {
				background-color: #218838;
				transform: translateY(-1px);
				box-shadow: 0 2px 6px rgba(0, 0, 0, 0.15);
			}

			.btn-pagination:disabled,
			.btn-pagination[disabled] {
				opacity: 0.6;
				cursor: not-allowed;
				background-color: #6c757d !important;
			}
	</style>
</asp:Content>
