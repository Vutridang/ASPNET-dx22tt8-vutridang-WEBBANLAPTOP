<%@ Page Title="Thương hiệu nổi bật" Language="C#" MasterPageFile="~/Home/Site.Master" AutoEventWireup="true" CodeBehind="Topbrands.aspx.cs" Inherits="WebBanLapTop.Home.Topbrands" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="main">
        <div class="content">
            <asp:Repeater ID="rptTopBrands" runat="server">
                <ItemTemplate>
                    <!-- Tên thương hiệu -->
                    <div class="content_top">
                        <div class="heading">
                            <h3><%# Eval("name") %></h3>
                        </div>
                        <div class="clear"></div>
                    </div>

                    <!-- Danh sách sản phẩm -->
                    <div class="section group">
                        <asp:Repeater ID="rptProducts" runat="server" DataSource='<%# Eval("Products") %>'>
                            <ItemTemplate>
                                <div class="grid_1_of_4 images_1_of_4">
                                    <a href="#">
                                        <img src='<%# Eval("image_url") %>' alt='<%# Eval("name") %>' style="width:200px; height:150px;" />
                                    </a>
                                    <h2><%# Eval("name") %></h2>
                                    <p><span class="price"><%# String.Format("{0:N0}₫", Eval("price")) %></span></p>
                                    <div class="button"><span><a href="<%# "ProductDetail.aspx?id=" + Eval("id") %>" class="details">Chi tiết</a></span></div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

</asp:Content>
