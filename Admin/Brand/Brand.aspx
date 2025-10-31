<%@ Page Title="Quản Lý Thương Hiệu" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Brand.aspx.cs" Inherits="WebBanLapTop.Admin.Brand.Brand" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Quản Lý Thương Hiệu</h2>
    <p>
        <a href="Create_Brand.aspx" class="btn btn-success">Thêm Thương Hiệu Mới</a>
    </p>

    <asp:GridView ID="gvBrands" runat="server" AutoGenerateColumns="False"
        CssClass="table table-bordered table-striped" DataKeyNames="id">
        <Columns>
            <asp:TemplateField HeaderText="STT">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="name" HeaderText="Tên Thương Hiệu" />

            <asp:TemplateField HeaderText="Logo">
                <ItemTemplate>
                    <asp:Image ID="imgLogo" runat="server" Width="100px" Height="100px" 
                        ImageUrl='<%# Eval("logo_url") %>' AlternateText="Logo" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="description" HeaderText="Mô Tả" />

            <asp:TemplateField HeaderText="Top Brand">
                <ItemTemplate>
                    <asp:Label ID="lblTopBrand" runat="server"
                        Text='<%# (bool)Eval("is_top") ? "✔️ Có" : "❌ Không" %>'
                        ForeColor='<%# (bool)Eval("is_top") ? System.Drawing.Color.Green : System.Drawing.Color.Gray %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Hành Động">
                <ItemTemplate>
                    <a href='Edit_Brand.aspx?id=<%# Eval("id") %>' class="btn btn-primary btn-sm">Sửa</a>
                    <a href="javascript:void(0);" 
                       class="btn btn-danger btn-sm btn-delete"
                       data-id='<%# Eval("id") %>' 
                       data-type="brand"
                       data-name='<%# Eval("name") %>'>Xóa</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
