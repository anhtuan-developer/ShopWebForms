<%@ Page Title="Sản phẩm"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="shop.aspx.cs"
    Inherits="web_ban_hang2.shop" %>

<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

</asp:Content>


<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="shop-page">

        <!-- TIÊU ĐỀ -->
        <div class="shop-header">

            <div>
                <h1>Sản phẩm</h1>

                <p>
                    Khám phá các sản phẩm của chúng tôi
                </p>
            </div>

        </div>


        <!-- DANH SÁCH SẢN PHẨM -->

        <div class="product-grid">

            <asp:Repeater
                ID="rptProducts"
                runat="server">

                <ItemTemplate>

                    <div class="product-card">

                        <!-- HÌNH ẢNH -->

                        <div class="product-image">

                            <img
                                src='<%# ResolveUrl("~/img/" + Eval("HinhAnh")) %>'
                                alt='<%# Eval("TenSanPham") %>' />

                        </div>


                        <!-- THÔNG TIN -->

                        <div class="product-info">

                            <span class="product-category">

                                <%# Eval("TenDanhMuc") %>

                            </span>


                            <h3>

                                <%# Eval("TenSanPham") %>

                            </h3>


                            <p class="product-description">

                                <%# Eval("MoTa") %>

                            </p>


                            <div class="product-price">

                                <%# String.Format("{0:N0} ₫", Eval("Gia")) %>

                            </div>


                            <div class="product-stock">

                                Còn lại:
                                <%# Eval("SoLuong") %>
                                sản phẩm

                            </div>


                            <a
                                href='<%# "ProductDetail.aspx?id=" + Eval("MaSanPham") %>'
                                class="product-button">

                                Xem sản phẩm

                            </a>

                        </div>

                    </div>

                </ItemTemplate>

            </asp:Repeater>

        </div>


        <!-- KHÔNG CÓ SẢN PHẨM -->

        <asp:Panel
            ID="pnlNoProduct"
            runat="server"
            Visible="false"
            CssClass="no-product">

            <h2>
                Không có sản phẩm
            </h2>

            <p>
                Hiện tại chưa có sản phẩm nào.
            </p>

        </asp:Panel>

    </section>

</asp:Content>