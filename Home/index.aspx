<%@ Page Title="Home" Language="C#" MasterPageFile="~/Home/Site.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WebBanLapTop.Home.index" %>

<asp:Content ID="FeaturedProductsContent" ContentPlaceHolderID="FeaturedProducts" runat="server">
	<asp:Repeater ID="rptFeaturedProducts" runat="server">
		<ItemTemplate>
			<div class="listview_1_of_2 images_1_of_2">
				<div class="listimg listimg_2_of_1">
					<a href='product_detail.aspx?id=<%# Eval("id") %>'>
						<img src='<%# Eval("image_url") %>' alt='<%# Eval("name") %>' />
					</a>
				</div>
				<div class="text list_2_of_1">
					<h2><%# Eval("name") %></h2>
					<p><%# Eval("description") %></p>
					<div class="button">
						<span>
							<a href='cart.aspx?add=<%# Eval("id") %>' class="details">Giỏ hàng</a>
						</span>
					</div>
				</div>
			</div>
		</ItemTemplate>
	</asp:Repeater>
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
								<a href='/Home/Product/ProductDetail.aspx?id=<%# Eval("id") %>' class="details">Chi tiết</a>
							</span>
							<span>
								<a href='cart.aspx?add=<%# Eval("id") %>' class="details">Giỏ hàng</a>
							</span>
						</div>
					</div>
				</ItemTemplate>
			</asp:Repeater>
		</div>
	</div>
</asp:Content>

