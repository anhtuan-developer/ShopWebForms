<%@ Page
    Title="Không tìm thấy trang"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="404.aspx.cs"
    Inherits="web_ban_hang2._404"
%>

<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">
</asp:Content>

<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="error-page">

        <div class="error-content">

            <div class="error-code">
                404
            </div>

            <div class="error-icon">
                🔍
            </div>

            <h1>
                Không tìm thấy trang
            </h1>

            <p>
                Xin lỗi, trang bạn đang tìm kiếm không tồn tại
                hoặc đã được di chuyển.
            </p>

            <div class="error-actions">

                <a
                    href="shop.aspx"
                    class="error-btn error-btn-primary">
                    Xem sản phẩm
                </a>

                <a
                    href="index.aspx"
                    class="error-btn error-btn-secondary">
                    Về trang chủ
                </a>

            </div>

        </div>

    </section>

</asp:Content>