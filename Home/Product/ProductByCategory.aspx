<%@ Page Title="Sản phẩm theo danh mục" Language="C#" MasterPageFile="~/Home/Site.Master"
    AutoEventWireup="true" CodeBehind="ProductByCategory.aspx.cs"
    Inherits="WebBanLapTop.Home.ProductByCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div class="content">
            <!-- Tiêu đề danh mục -->
            <div class="content_top">
                <div class="heading">
                    <h3>
                        <asp:Label ID="lblCategoryName" runat="server" Text="Danh mục sản phẩm"></asp:Label>
                    </h3>
                </div>
                <div class="clear"></div>
            </div>

            <!-- Danh sách sản phẩm -->
            <asp:Repeater ID="rptProductsByCategory" runat="server">
                <ItemTemplate>
                    <div class="grid_1_of_4 images_1_of_4">
                        <a href='<%# "ProductDetail.aspx?id=" + Eval("id") %>'>
                            <img src='<%# Eval("image_url") %>' alt='<%# Eval("name") %>' class="product-img" />
                        </a>
                        <h2 class="product-name"><%# Eval("name") %></h2>
                        <p>
                            <span class="price"><%# string.Format("{0:N0} VNĐ", Eval("price")) %></span>
                        </p>
                        <div class="button">
                            <span>
                                <a href='<%# "ProductDetail.aspx?id=" + Eval("id") %>' class="details">Chi tiết</a>
                            </span>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <!-- Thông báo khi không có sản phẩm -->
            <asp:Label ID="lblNoResults" runat="server"
                Text="Không có sản phẩm nào trong danh mục này."
                Visible="false"
                CssClass="no-results" />

            <!-- ✅ Phân trang -->
            <div class="pagination-wrapper text-center mt-4">
                <asp:LinkButton ID="btnPrev" runat="server"
                    CssClass="btn-pagination" OnClick="btnPrev_Click">« Trước</asp:LinkButton>
                <asp:Label ID="lblPageInfo" runat="server" CssClass="page-info" />
                <asp:LinkButton ID="btnNext" runat="server"
                    CssClass="btn-pagination" OnClick="btnNext_Click">Sau »</asp:LinkButton>
            </div>
        </div>
    </div>

    <!-- ✅ CSS -->
    <style>
        /* --- Grid layout --- */
        .grid_1_of_4 {
            float: left;
            width: 23%;
            margin: 1%;
            box-sizing: border-box;
            background: #fff;
            border: 1px solid #ddd;
            border-radius: 8px;
            padding: 10px;
            text-align: center;
            transition: all 0.2s ease;
        }

        .grid_1_of_4:hover {
            transform: translateY(-3px);
            box-shadow: 0 3px 10px rgba(0, 0, 0, 0.15);
        }

        .product-img {
            width: 100%;
            height: 200px;
            object-fit: cover;
            border-radius: 6px;
        }

        .product-name {
            font-size: 15px;
            font-weight: 600;
            margin: 10px 0;
            color: #333;
            min-height: 38px;
        }

        .price {
            font-size: 14px;
            color: #c00;
            font-weight: bold;
        }

/*        .button .details {
            display: inline-block;
            background-color: #007bff;
            color: #fff !important;
            padding: 5px 14px;
            border-radius: 4px;
            font-size: 13px;
            text-transform: uppercase;
            transition: 0.2s;
        }*/

        .button .details:hover {
            background-color: #0056b3;
            text-decoration: none;
        }

        .no-results {
            display: block;
            margin-top: 25px;
            text-align: center;
            color: #666;
            font-style: italic;
        }

        /* --- Pagination styling --- */
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

        /* Responsive */
        @media (max-width: 992px) {
            .grid_1_of_4 {
                width: 48%;
                margin: 1%;
            }
        }

        @media (max-width: 576px) {
            .grid_1_of_4 {
                width: 98%;
                margin: 1%;
            }
        }
    </style>
</asp:Content>
