<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="WebBanLapTop.Home.Cart.Checkout" MasterPageFile="~/Home/Site.Master" %>

<asp:Content ID="CheckoutContent" ContentPlaceHolderID="MainContent" runat="server">

	<style>
	    /* ======= FORM CONTAINER ======= */
	    .checkout-container {
	        max-width: 600px;
	        margin: 40px auto;
	        background: #fff;
	        border: 1px solid #ddd;
	        border-radius: 12px;
	        padding: 30px 40px;
	        box-shadow: 0 3px 10px rgba(0, 0, 0, 0.1);
	    }

	        .checkout-container h2 {
	            text-align: center;
	            margin-bottom: 25px;
	            color: #198754;
	            font-weight: 700;
	            letter-spacing: 0.5px;
	        }

	    /* ======= INPUTS ======= */
	    .form-group {
	        margin-bottom: 18px;
	    }

	    label {
	        display: block;
	        font-weight: 600;
	        color: #333;
	        margin-bottom: 6px;
	    }

	    .form-control {
	        width: 100%;
	        padding: 9px 12px;
	        font-size: 15px;
	        border: 1px solid #ccc;
	        border-radius: 6px;
	        box-sizing: border-box;
	        transition: border-color 0.2s ease, box-shadow 0.2s ease;
	    }

	        .form-control:focus {
	            border-color: #198754;
	            box-shadow: 0 0 5px rgba(25, 135, 84, 0.3);
	            outline: none;
	        }

	    /* ======= VALIDATION ======= */
	    span[style*="color:Red"] {
	        display: block;
	        font-size: 13px;
	        margin-top: 3px;
	    }

	    /* ======= SUBMIT BUTTON ======= */
	    .btn-success {
	        background-color: #198754;
	        color: #fff;
	        padding: 10px 22px;
	        border: none;
	        border-radius: 6px;
	        font-size: 16px;
	        cursor: pointer;
	        transition: all 0.2s ease;
	        box-shadow: 0 3px 6px rgba(25, 135, 84, 0.25);
	    }

	        .btn-success:hover {
	            background-color: #157347;
	            transform: translateY(-1px);
	            box-shadow: 0 4px 10px rgba(21, 115, 71, 0.3);
	        }

	    .text-center {
	        text-align: center;
	    }

	    hr {
	        margin: 25px 0;
	        border: none;
	        border-top: 1px solid #ccc;
	    }

	    /* ======= RESPONSIVE ======= */
	    @media (max-width: 576px) {
	        .checkout-container {
	            padding: 20px;
	        }

	        .btn-success {
	            width: 100%;
	            font-size: 15px;
	        }
	    }
	</style>
	<div class="container mt-5 mb-5" style="max-width: 600px; margin: 0 auto;">
		<h2 class="text-center mb-4">Thanh toán đơn hàng</h2>
		<asp:Panel ID="pnlCheckout" runat="server">

			<div class="form-group mb-3">
				<label>Tên đăng nhập:</label>
				<asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" ReadOnly="true" />
			</div>

			<div class="form-group mb-3">
				<label>Email:</label>
				<asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true" />
			</div>

			<div class="form-group mb-3">
				<label>Vai trò:</label>
				<asp:TextBox ID="txtRole" runat="server" CssClass="form-control" ReadOnly="true" />
			</div>

			<hr />

			<div class="form-group mb-3">
				<label>Địa chỉ giao hàng:</label>
				<asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
				<asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress"
					ErrorMessage="Vui lòng nhập địa chỉ" ForeColor="Red" Display="Dynamic" />
			</div>

			<div class="form-group mb-3">
				<label>Mã bưu điện:</label>
				<asp:TextBox ID="txtZipcode" runat="server" CssClass="form-control" />
				<asp:RequiredFieldValidator ID="rfvZip" runat="server" ControlToValidate="txtZipcode"
					ErrorMessage="Vui lòng nhập mã bưu điện" ForeColor="Red" Display="Dynamic" />
			</div>

			<div class="form-group mb-3">
				<label>Phương thức thanh toán:</label>
				<asp:DropDownList ID="ddlPayment" runat="server" CssClass="form-control">
					<asp:ListItem Text="-- Chọn phương thức --" Value="" />
					<asp:ListItem Text="Thẻ tín dụng" Value="Thẻ tín dụng" />
					<asp:ListItem Text="Thu COD" Value="Thu COD" />
				</asp:DropDownList>
				<asp:RequiredFieldValidator ID="rfvPayment" runat="server" ControlToValidate="ddlPayment"
					InitialValue="" ErrorMessage="Vui lòng chọn phương thức thanh toán" ForeColor="Red" Display="Dynamic" />
			</div>

			<div class="text-center mt-4">
				<asp:Button ID="btnSubmit" runat="server" Text="Xác nhận đặt hàng" CssClass="btn btn-success" OnClick="btnSubmit_Click" />
			</div>
		</asp:Panel>
	</div>

	<!-- SweetAlert2 -->
	<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>
