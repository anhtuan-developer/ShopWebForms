<%@ Page
    Title="Đặt hàng thành công"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Dat_hang_thanh_cong.aspx.cs"
    Inherits="web_ban_hang2.Dat_hang_thanh_cong"
%>

<asp:Content
    ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="text-center">

        <h2>Đặt hàng thành công!</h2>

        <asp:Label
            ID="lblMaDonHang"
            runat="server">
        </asp:Label>

        <br /><br />

        <a href="shop.aspx">
            Tiếp tục mua hàng
        </a>

    </div>

</asp:Content>