<%@ Page Title="Bảng điều khiển" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="WebBanLapTop.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    Bảng điều khiển
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12 mb-4">
            <div class="card">
                <div class="card-header">
                    <h5>Tổng hợp</h5>
                </div>
                <div class="card-body">
                    <asp:GridView ID="gvSummary" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped">
                        <Columns>
                            <asp:BoundField DataField="STT" HeaderText="STT" />
                            <asp:BoundField DataField="Name" HeaderText="Tên" />
                            <asp:BoundField DataField="Quantity" HeaderText="Số lượng" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
