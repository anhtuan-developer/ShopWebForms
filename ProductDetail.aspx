<%@ Page Title="Chi tiết sản phẩm"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="ProductDetail.aspx.cs"
    Inherits="web_ban_hang2.ProductDetail" %>

<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

</asp:Content>


<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="product-detail-page">

        <!-- ĐƯỜNG DẪN -->

        <div class="breadcrumb">

            <a href="index.aspx">
                Trang chủ
            </a>

            <span> / </span>

            <a href="shop.aspx">
                Sản phẩm
            </a>

            <span> / Chi tiết</span>

        </div>


        <!-- CHI TIẾT -->

        <div class="product-detail">

            <!-- HÌNH ẢNH -->

            <div class="detail-image">

                <asp:Image
                    ID="imgProduct"
                    runat="server"
                    CssClass="product-detail-image"
                    AlternateText="Sản phẩm" />

            </div>


            <!-- THÔNG TIN -->

            <div class="detail-info">

                <span
                    class="detail-category"
                    runat="server"
                    id="lblCategory">
                </span>


                <h1
                    runat="server"
                    id="lblProductName">
                </h1>


                <div
                    class="detail-price"
                    runat="server"
                    id="lblPrice">
                </div>


                <div
                    class="detail-stock"
                    runat="server"
                    id="lblStock">
                </div>


                <div class="detail-description">

                    <h3>
                        Mô tả sản phẩm
                    </h3>

                    <p
                        runat="server"
                        id="lblDescription">
                    </p>

                </div>


                <!-- SỐ LƯỢNG -->

                <div class="quantity-box">

                    <label for="txtQuantity">
                        Số lượng:
                    </label>

                    <asp:TextBox
                        ID="txtQuantity"
                        runat="server"
                        Text="1"
                        TextMode="Number"
                        CssClass="quantity-input">
                    </asp:TextBox>

                </div>


                <!-- BUTTON -->

                <div class="detail-actions">

                    <asp:Button
                        ID="btnAddToCart"
                        runat="server"
                        Text="Thêm vào giỏ hàng"
                        CssClass="add-cart-button"
                        OnClick="btnAddToCart_Click" />

                    <a
                        href="shop.aspx"
                        class="continue-button">

                        Tiếp tục mua hàng

                    </a>

                </div>


                <!-- THÔNG BÁO -->

                <asp:Label
                    ID="lblMessage"
                    runat="server"
                    CssClass="detail-message">
                </asp:Label>

            </div>

        </div>

    </section>

</asp:Content>