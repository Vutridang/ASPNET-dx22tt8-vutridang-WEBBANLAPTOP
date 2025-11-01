<%@ Page Title="Sản phẩm theo danh mục" Language="C#" MasterPageFile="~/Home/Site.Master"
    AutoEventWireup="true" CodeBehind="ProductByCategory.aspx.cs" Inherits="WebBanLapTop.Home.ProductByCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div class="content">
            <div class="content_top">
                <div class="heading">
                    <h3>
                        <asp:Label ID="lblCategoryName" runat="server" Text="Danh mục sản phẩm"></asp:Label>
                    </h3>
                </div>
                <div class="clear"></div>
            </div>

            <asp:Repeater ID="rptProductsByCategory" runat="server">
                <ItemTemplate>
                    <div class="grid_1_of_4 images_1_of_4">
                        <a href='<%# "ProductDetail.aspx?id=" + Eval("id") %>'>
                            <img src='<%# Eval("image_url") %>' alt='<%# Eval("name") %>' />
                        </a>
                        <h2><%# Eval("name") %></h2>
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

            <asp:Label ID="lblNoResults" runat="server"
                       Text="Không có sản phẩm nào trong danh mục này."
                       Visible="false"
                       ForeColor="Gray" Font-Italic="true"
                       Style="display:block;margin-top:20px;text-align:center;" />
        </div>
    </div>
</asp:Content>
