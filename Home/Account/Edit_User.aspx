<%@ Page Title="Chỉnh sửa thông tin người dùng" Language="C#" MasterPageFile="~/Home/Site.Master"
	AutoEventWireup="true" CodeBehind="Edit_User.aspx.cs"
	Inherits="WebBanLapTop.Home.Account.Edit_User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="main">
		<div class="content">
			<h2 style="text-align: center; margin-bottom: 20px;">Thông tin tài khoản</h2>

			<!-- FORM SỬA USER -->
			<div class="user-form" style="max-width: 500px; margin: 0 auto;">
				<div class="form-group" style="margin-bottom: 15px;">
					<label>Tên đăng nhập:</label>
					<asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Style="width: 100%; padding: 8px;" />
				</div>

				<div class="form-group" style="margin-bottom: 15px;">
					<label>Email:</label>
					<asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Style="width: 100%; padding: 8px;" />
				</div>

				<div style="text-align: center;">
					<asp:Button ID="btnSave" runat="server" Text="Lưu thay đổi"
						CssClass="btn-save"
						OnClick="btnSave_Click"
						Style="background: #198754; color: white; border: none; padding: 10px 20px; border-radius: 5px; font-weight: 600;" />
				</div>
			</div>

			<hr style="margin: 30px 0;" />

			<!-- LỊCH SỬ ĐƠN HÀNG -->
			<h2 style="text-align: center;">Lịch sử đơn hàng</h2>
			<asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false"
				CssClass="order-table" GridLines="None" Width="100%"
				Style="margin-top: 20px; border-collapse: collapse;">
				<Columns>
					<asp:BoundField DataField="id" HeaderText="Mã đơn" />
					<asp:BoundField DataField="total_amount" HeaderText="Tổng tiền" DataFormatString="{0:N0}₫" />
					<asp:BoundField DataField="status" HeaderText="Trạng thái" />
					<asp:BoundField DataField="created_at" HeaderText="Ngày tạo" DataFormatString="{0:dd/MM/yyyy}" />
					<asp:TemplateField HeaderText="Thao tác">
						<ItemTemplate>
							<asp:Button ID="btnViewDetail" runat="server" Text="Xem chi tiết"
								CssClass="btn-detail"
								CommandArgument='<%# Eval("id") %>'
								OnClick="btnViewDetail_Click" />
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
			</asp:GridView>
			<!-- ✅ Phân trang -->
			<div class="pagination-wrapper text-center mt-4">
				<asp:LinkButton ID="btnPrev" runat="server"
					CssClass="btn-pagination" OnClick="btnPrev_Click">« Trước</asp:LinkButton>
				<asp:Label ID="lblPageInfo" runat="server" CssClass="page-info" />
				<asp:LinkButton ID="btnNext" runat="server"
					CssClass="btn-pagination" OnClick="btnNext_Click">Sau »</asp:LinkButton>
			</div>

			<!-- POPUP CHI TIẾT ĐƠN HÀNG -->
			<div id="orderModal" runat="server" class="modal" style="display: none;">
				<div class="modal-content"
					style="background: #fff; width: 700px; margin: 80px auto; border-radius: 10px; padding: 20px; box-shadow: 0 0 15px rgba(0,0,0,0.3);">
					<h3 style="text-align: center;">Chi tiết đơn hàng</h3>
					<table style="width: 100%; border-collapse: collapse; margin-top: 15px;">
						<thead>
							<tr style="background: #f5f5f5;">
								<th style="padding: 8px; border: 1px solid #ddd;">Hình ảnh</th>
								<th style="padding: 8px; border: 1px solid #ddd;">Tên sản phẩm</th>
								<th style="padding: 8px; border: 1px solid #ddd;">Thương hiệu</th>
								<th style="padding: 8px; border: 1px solid #ddd;">Số lượng</th>
								<th style="padding: 8px; border: 1px solid #ddd;">Giá</th>
							</tr>
						</thead>
						<tbody id="tblOrderItems" runat="server"></tbody>
					</table>

					<div style="text-align: center; margin-top: 20px;">
						<asp:Button ID="btnCloseModal" runat="server" Text="Đóng"
							OnClick="btnCloseModal_Click"
							Style="background: #dc3545; color: #fff; border: none; padding: 10px 20px; border-radius: 5px;" />
					</div>
				</div>
			</div>
		</div>
	</div>

	<!-- CSS -->
	<style>
		.order-table th {
			background: #198754;
			color: #fff;
			padding: 10px;
			text-align: center;
		}

		.order-table td {
			border: 1px solid #ddd;
			padding: 8px;
			text-align: center;
		}

		.order-table tr:nth-child(even) {
			background: #f9f9f9;
		}

		.order-table tr:hover {
			background: #f1f1f1;
		}

		/* Pagination styling (đồng bộ ProductByBrand) */
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

		.page-info {
			display: inline-block;
			font-weight: 600;
			color: #333;
			font-size: 15px;
			min-width: 100px;
			text-align: center;
		}
	</style>
</asp:Content>
