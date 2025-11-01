<%@ Page Title="Chi tiết sản phẩm" Language="C#" MasterPageFile="~/Home/Site.Master"
	AutoEventWireup="true" CodeBehind="ProductDetail.aspx.cs" Inherits="WebBanLapTop.Home.ProductDetail" %>

<asp:Content ID="HeadStyle" ContentPlaceHolderID="HeadContent" runat="server">
	<style>
		body {
			background-color: #f5f5f5;
			font-family: 'Segoe UI', sans-serif;
		}

		.product-detail-container {
			max-width: 1100px;
			margin: 40px auto;
			background: #fff;
			border-radius: 10px;
			box-shadow: 0 4px 10px rgba(0,0,0,0.1);
			padding: 30px 40px;
		}

		.product-detail {
			display: flex;
			flex-wrap: wrap;
			gap: 40px;
		}

		.product-image {
			flex: 1 1 40%;
			text-align: center;
		}

			.product-image img {
				max-width: 100%;
				border-radius: 10px;
				border: 1px solid #eee;
			}

		.product-info {
			flex: 1 1 55%;
		}

			.product-info h2 {
				font-size: 28px;
				font-weight: bold;
				color: #222;
				margin-bottom: 10px;
			}

		.rating {
			color: #f4c150;
			font-size: 18px;
			margin-bottom: 10px;
		}

		.price {
			font-size: 24px;
			color: #ee4d2d;
			font-weight: bold;
			margin-bottom: 8px;
		}

		.stock {
			font-size: 15px;
			color: #555;
			margin-bottom: 15px;
		}

		.desc {
			font-size: 15px;
			color: #333;
			line-height: 1.6;
			margin-bottom: 20px;
		}

		/* Ô nhập số lượng */
		.quantity-container {
			display: flex;
			align-items: center;
			margin-bottom: 25px;
		}

			.quantity-container label {
				margin-right: 10px;
				font-weight: 600;
			}

		.quantity-input {
			display: flex;
			align-items: center;
			border: 1px solid #ccc;
			border-radius: 5px;
		}

			.quantity-input button {
				background-color: #198754;
				color: white;
				border: none;
				width: 35px;
				height: 35px;
				cursor: pointer;
				font-size: 18px;
			}

			.quantity-input input {
				width: 60px;
				height: 35px;
				text-align: center;
				border: none;
				font-size: 16px;
			}

		/* Nút Thêm vào giỏ hàng */
		.add-to-cart {
			text-align: center;
			margin-top: 30px;
		}

		.btn-add-cart {
			background-color: #198754;
			color: white;
			border: none;
			padding: 14px 50px;
			font-size: 17px;
			border-radius: 8px;
			cursor: pointer;
			transition: 0.2s;
		}

			.btn-add-cart:hover {
				background-color: #157347;
				transform: translateY(-2px);
			}

		@media (max-width: 768px) {
			.product-detail {
				flex-direction: column;
				text-align: center;
			}
		}
	</style>
</asp:Content>

<asp:Content ID="MainDetail" ContentPlaceHolderID="MainContent" runat="server">
	<div class="product-detail-container">
		<div class="product-detail">
			<div class="product-image">
				<asp:Image ID="imgProduct" runat="server" AlternateText="Hình sản phẩm" />
			</div>

			<div class="product-info">
				<h2>
					<asp:Label ID="lblName" runat="server" /></h2>
				<div class="rating">★★★★☆</div>
				<p class="price">
					<asp:Label ID="lblPrice" runat="server" />
				</p>
				<p class="stock">
					Số lượng còn lại:
                    <span style="color: #ee4d2d; font-weight: bold">
						<asp:Label ID="lblStock" runat="server" />
					</span>
				</p>
				<p class="brand">
					Hãng:
					<asp:Label ID="lblBrand" runat="server" />
				</p>
				<p class="desc">
					<asp:Label ID="lblDescription" runat="server" />
				</p>
			</div>

			<div class="quantity-container">
				<label for="txtQuantity">Số lượng:</label>
				<div class="quantity-input">
					<button type="button" onclick="changeQty(-1)">-</button>
					<asp:TextBox ID="txtQuantity" runat="server" Text="1" CssClass="qty" />
					<button type="button" onclick="changeQty(1)">+</button>
				</div>
			</div>
		</div>



		<div class="add-to-cart">
			<asp:Button ID="btnAddToCart" runat="server" CssClass="btn-add-cart"
				Text="Thêm vào giỏ hàng" OnClick="btnAddToCart_Click" />
		</div>
	</div>

	<script>
		function changeQty(val) {
			var qtyInput = document.getElementById('<%= txtQuantity.ClientID %>');
			var current = parseInt(qtyInput.value) || 1;
			current += val;
			if (current < 1) current = 1;
			qtyInput.value = current;
		}
	</script>
</asp:Content>
