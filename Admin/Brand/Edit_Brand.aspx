<%@ Page Title="Chỉnh Sửa Thương Hiệu" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Edit_Brand.aspx.cs" Inherits="WebBanLapTop.Admin.Brand.Edit_Brand" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Chỉnh Sửa Thương Hiệu</h2>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

    <asp:Panel ID="pnlForm" runat="server">
        <div class="form-group">
            <label>Tên Thương Hiệu</label>
            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label>Mô Tả</label>
            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
        </div>

        <div class="form-group">
            <label>Logo Hiện Tại</label><br />
            <div style="width:100px; height:100px; overflow:hidden; border:1px solid #ccc; display:flex; align-items:center; justify-content:center;">
                <asp:Image ID="imgCurrent" runat="server" Width="100" />
            </div>
            <asp:FileUpload ID="fileUpload" runat="server" CssClass="form-control" onchange="previewImage(this);" />
            <small>Upload hình mới sẽ thay thế hình cũ.</small>

            <script type="text/javascript">
                function previewImage(input) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            var img = document.getElementById('<%= imgCurrent.ClientID %>');
                            img.src = e.target.result;
                        }
                        reader.readAsDataURL(input.files[0]);
                    }
                }
			</script>
        </div>

        <div class="form-group form-check">
            <asp:CheckBox ID="chkIsTop" runat="server" CssClass="form-check-input" />
            <label class="form-check-label" for="chkIsTop">Là thương hiệu nổi bật (Top Brand)</label>
        </div>

        <asp:Button ID="btnUpdate" runat="server" Text="Cập Nhật" CssClass="btn btn-primary" OnClick="btnUpdate_Click" />
        <a href="Brand.aspx" class="btn btn-secondary">Hủy</a>
    </asp:Panel>
</asp:Content>
