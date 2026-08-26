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

                <!-- ĐÁNH GIÁ SẢN PHẨM -->

                <section class="product-reviews">
                
                    <div class="reviews-header">
                
                        <h2>Đánh giá sản phẩm</h2>
                
                        <asp:Label
                            ID="lblAverageRating"
                            runat="server"
                            CssClass="average-rating">
                        </asp:Label>
                
                        <asp:Label
                            ID="lblReviewCount"
                            runat="server"
                            CssClass="review-count">
                        </asp:Label>
                
                    </div>
                
                
                    <!-- FORM ĐÁNH GIÁ -->
                
                    <asp:Panel
                        ID="pnlReviewForm"
                        runat="server"
                        CssClass="review-form">
                
                        <h3>Viết đánh giá</h3>
                
                        <div class="form-group">
                
                            <label>Số sao</label>
                
                            <asp:DropDownList
                                ID="ddlSoSao"
                                runat="server"
                                CssClass="form-control">
                
                                <asp:ListItem Value="5">
                                    5 sao
                                </asp:ListItem>
                
                                <asp:ListItem Value="4">
                                    4 sao
                                </asp:ListItem>
                
                                <asp:ListItem Value="3">
                                    3 sao
                                </asp:ListItem>
                
                                <asp:ListItem Value="2">
                                    2 sao
                                </asp:ListItem>
                
                                <asp:ListItem Value="1">
                                    1 sao
                                </asp:ListItem>
                
                            </asp:DropDownList>
                
                        </div>
                
                
                        <div class="form-group">
                
                            <label>Nội dung</label>
                
                            <asp:TextBox
                                ID="txtNoiDungDanhGia"
                                runat="server"
                                TextMode="MultiLine"
                                Rows="5"
                                MaxLength="2000"
                                CssClass="form-control">
                            </asp:TextBox>
                
                        </div>
                
                
                        <asp:Button
                            ID="btnGuiDanhGia"
                            runat="server"
                            Text="Gửi đánh giá"
                            CssClass="review-submit-button"
                            OnClick="btnGuiDanhGia_Click" />
                
                
                        <asp:Label
                            ID="lblReviewMessage"
                            runat="server"
                            CssClass="review-message">
                        </asp:Label>
                
                    </asp:Panel>
                
                
                    <!-- DANH SÁCH ĐÁNH GIÁ -->
                
                    <asp:Repeater
                        ID="rptDanhGia"
                        runat="server">
                
                        <ItemTemplate>
                
                            <div class="review-item">
                
                                <div class="review-item-header">
                
                                    <strong>
                                        <%# Eval("HoTen") %>
                                    </strong>
                
                                    <span class="review-stars">
                                        <%# Eval("SoSao") %> ★
                                    </span>
                
                                </div>
                
                                <div class="review-date">
                                    <%# Eval(
                                        "NgayDanhGia",
                                        "{0:dd/MM/yyyy HH:mm}") %>
                                </div>
                
                                <div class="review-content">
                                    <%# Server.HtmlEncode(
                                        Eval("NoiDung").ToString()) %>
                                </div>
                
                            </div>
                
                        </ItemTemplate>
                
                    </asp:Repeater>
                
                </section>


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