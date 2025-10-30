<%@ Page Title="Bảng điều khiển" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="WebBanLapTop.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    Bảng điều khiển
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col">
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title mb-4 d-inline">Admins</h5>
                    <a href="CreateAdmin.aspx" class="btn btn-primary mb-4 float-right">Create Admins</a>

                    <asp:GridView ID="gvAdmins" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="#" />
                            <asp:BoundField DataField="username" HeaderText="Username" />
                            <asp:BoundField DataField="email" HeaderText="Email" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
