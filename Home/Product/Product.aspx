<%@ Page Title="Sản phẩm" Language="C#" MasterPageFile="~/Home/Site.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="WebBanLapTop.Home.Products" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div style="max-width: 1200px; margin: 0 auto; padding: 30px 0;">
		<h2 style="text-align: center; margin-bottom: 30px; font-size: 28px; color: #333;">Danh sách sản phẩm</h2>

		<!-- Bộ lọc theo danh mục -->
		<div class="filter-section" style="text-align: center; margin-bottom: 25px;">
			<label for="ddlBrand" style="font-size: 16px; margin-right: 10px;">Lọc theo danh mục:</label>
			<asp:DropDownList ID="ddlBrand" runat="server" AutoPostBack="true" CssClass="form-select" Style="width: 220px; display: inline-block;"
				OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged">
			</asp:DropDownList>
		</div>

		<!-- Danh sách sản phẩm -->
		<div class="product-list">
			<asp:Repeater ID="rptProducts" runat="server">
				<ItemTemplate>
					<div class="product-card">
						<a href='<%# "ProductDetail.aspx?id=" + Eval("id") %>'>
							<img src='<%# Eval("image_url") %>' alt='<%# Eval("name") %>' class="product-img" />
						</a>
						<h3 class="product-name"><%# Eval("name") %></h3>
						<p class="product-desc"><%# Eval("description") %></p>
						<p class="product-price"><%# string.Format("{0:N0} đ", Eval("price")) %></p>
						<div class="button">
							<span>
								<a href='<%# "ProductDetail.aspx?id=" + Eval("id") %>' class="details">Chi tiết</a>
							</span>
						</div>
					</div>
				</ItemTemplate>
			</asp:Repeater>
		</div>
	</div>

	<style>
		.product-list {
			display: grid;
			grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
			gap: 25px;
			padding: 10px;
		}

		.product-card {
			border: 1px solid #ddd;
			border-radius: 8px;
			padding: 15px;
			text-align: center;
			background-color: #fff;
			box-shadow: 0 2px 5px rgba(0,0,0,0.08);
			transition: transform 0.2s, box-shadow 0.2s;
		}

			.product-card:hover {
				transform: translateY(-3px);
				box-shadow: 0 4px 12px rgba(0,0,0,0.15);
			}

		.product-img {
			width: 100%;
			height: 180px;
			object-fit: contain;
			margin-bottom: 10px;
		}

		.product-name {
			font-size: 16px;
			font-weight: 600;
			color: #333;
			margin-bottom: 8px;
			min-height: 40px;
		}

		.product-desc {
			font-size: 13px;
			color: #777;
			min-height: 40px;
			margin-bottom: 10px;
		}

		.product-price {
			color: #dc3545;
			font-weight: bold;
			font-size: 15px;
			margin-bottom: 12px;
		}

		.button {
			display: inline-block;
			margin-top: 10px;
		}

			.button span {
				display: inline-block;
				background: linear-gradient(to bottom, #f5f5f5 0%, #ddd 100%);
				border: 1px solid #ccc;
				border-radius: 4px;
				padding: 6px 12px;
				transition: all 0.3s ease;
			}

				.button span:hover {
					background: linear-gradient(to bottom, #e6e6e6 0%, #ccc 100%);
				}

			.button a.details {
				color: #737370;
				text-decoration: none;
				font-size: 14px;
				display: inline-block;
			}

			.button span:hover a.details {
				color: #000;
			}
	</style>
</asp:Content>
