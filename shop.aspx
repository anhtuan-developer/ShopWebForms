<%@ Page
Title="Sản phẩm"
Language="C#"
MasterPageFile="~/Site.Master"
AutoEventWireup="true"
CodeBehind="shop.aspx.cs"
Inherits="web_ban_hang2.shop"
%>

<asp:Content
ID="HeadContent"
ContentPlaceHolderID="HeadContent"
runat="server">

<style>

    /* =====================================================
       SHOP PAGE
       ===================================================== */

    .shop-product-image {
        height: 240px;
        object-fit: cover;
    }

    .shop-product-title {
        min-height: 48px;
    }

    .shop-product-description {
        min-height: 48px;
    }

    .shop-product-price {
        font-size: 20px;
        font-weight: 700;
    }

    .shop-product-stock {
        font-size: 14px;
    }

    .shop-product-card {
        transition: transform .2s ease,
                    box-shadow .2s ease;
    }

    .shop-product-card:hover {
        transform: translateY(-4px);
        box-shadow: 0 .5rem 1rem rgba(0,0,0,.12) !important;
    }

    .shop-pagination .page-link {
        min-width: 42px;
        text-align: center;
    }

    @media (max-width: 767.98px) {

        .shop-product-image {
            height: 200px;
        }

    }

</style>

</asp:Content>

<asp:Content
ID="MainContent"
ContentPlaceHolderID="MainContent"
runat="server">

<!-- =========================================================
     TRANG SẢN PHẨM
     ========================================================= -->

<section class="py-2">


    <!-- =====================================================
         TIÊU ĐỀ
         ===================================================== -->

    <div
        class="d-flex
               flex-column
               flex-md-row
               justify-content-between
               align-items-md-center
               gap-3
               mb-4">


        <div>

            <h1 class="fw-bold mb-2">

                Sản phẩm

            </h1>

            <p class="text-secondary mb-0">

                Khám phá các sản phẩm của chúng tôi

            </p>

        </div>


        <!-- NÚT VỀ TẤT CẢ SẢN PHẨM -->

        <a
            href="<%= ResolveUrl("~/shop.aspx") %>"
            class="btn btn-outline-danger">

            🛍️ Tất cả sản phẩm

        </a>

    </div>


    <!-- =====================================================
         TOOLBAR
         ===================================================== -->

    <div
        class="card
               border-0
               shadow-sm
               mb-4">


        <div
            class="card-body
                   d-flex
                   flex-column
                   flex-md-row
                   justify-content-between
                   align-items-md-center
                   gap-2">


            <!-- KẾT QUẢ -->

            <div>

                <asp:Label
                    ID="lblSearchResult"
                    runat="server"
                    CssClass="fw-semibold text-dark">
                </asp:Label>

            </div>


            <!-- PAGE SIZE -->

            <div>

                <span class="text-secondary small">

                    Hiển thị 12 sản phẩm / trang

                </span>

            </div>

        </div>

    </div>


    <!-- =====================================================
         DANH SÁCH SẢN PHẨM
         ===================================================== -->

    <div class="row row-cols-1 row-cols-sm-2 row-cols-lg-3 row-cols-xl-4 g-4">


        <asp:Repeater
            ID="rptProducts"
            runat="server">


            <ItemTemplate>


                <!-- =================================================
                     PRODUCT
                     ================================================= -->

                <div class="col">


                    <div
                        class="card
                               h-100
                               border-0
                               shadow-sm
                               shop-product-card
                               overflow-hidden">


                        <!-- ================================
                             HÌNH ẢNH
                             ================================ -->

                        <a
                            href='<%#
                                ResolveUrl(
                                    "~/ProductDetail.aspx?id="
                                    + Eval("MaSanPham")
                                )
                            %>'
                            class="text-decoration-none">


                            <img
                                src='<%#
                                    ResolveUrl(
                                        "~/img/"
                                        + Eval("HinhAnh")
                                    )
                                %>'
                                alt='<%# Eval("TenSanPham") %>'
                                class="card-img-top
                                       shop-product-image"
                                loading="lazy" />

                        </a>


                        <!-- ================================
                             THÔNG TIN
                             ================================ -->

                        <div class="card-body d-flex flex-column">


                            <!-- DANH MỤC -->

                            <div class="mb-2">

                                <span
                                    class="badge
                                           text-bg-light
                                           border">

                                    📁
                                    <%# Eval("TenDanhMuc") %>

                                </span>

                            </div>


                            <!-- TÊN -->

                            <h5
                                class="card-title
                                       fw-bold
                                       shop-product-title">

                                <%# Eval("TenSanPham") %>

                            </h5>


                            <!-- MÔ TẢ -->

                            <p
                                class="card-text
                                       text-secondary
                                       small
                                       shop-product-description">


                                <%# Eval("MoTa") %>


                            </p>


                            <!-- GIÁ -->

                            <div
                                class="shop-product-price
                                       text-danger
                                       mb-2">


                                <%#
                                    String.Format(
                                        "{0:N0} ₫",
                                        Eval("Gia")
                                    )
                                %>


                            </div>


                            <!-- TỒN KHO -->

                            <div
                                class="shop-product-stock
                                       text-secondary
                                       mb-3">


                                <span>

                                    📦 Còn lại:

                                </span>


                                <strong>

                                    <%# Eval("SoLuong") %>

                                </strong>


                                <span>

                                    sản phẩm

                                </span>

                            </div>


                            <!-- BUTTON -->

                            <div class="mt-auto">


                                <a
                                    href='<%#
                                        ResolveUrl(
                                            "~/ProductDetail.aspx?id="
                                            + Eval("MaSanPham")
                                        )
                                    %>'
                                    class="btn
                                           btn-danger
                                           w-100">

                                    Xem sản phẩm

                                </a>


                            </div>


                        </div>

                    </div>


                </div>


            </ItemTemplate>


        </asp:Repeater>


    </div>


    <!-- =====================================================
         PHÂN TRANG
         ===================================================== -->

    <asp:Panel
        ID="pnlPager"
        runat="server"
        CssClass="shop-pagination mt-5">


        <nav
            aria-label="Phân trang sản phẩm">


            <ul
                class="pagination
                       justify-content-center
                       flex-wrap
                       gap-1">


                <!-- TRANG TRƯỚC -->

                <li class="page-item">


                    <asp:LinkButton
                        ID="btnPrevious"
                        runat="server"
                        CssClass="page-link rounded"
                        OnClick="btnPrevious_Click">


                        ← Trước


                    </asp:LinkButton>


                </li>


                <!-- CÁC TRANG -->

                <asp:Repeater
                    ID="rptPager"
                    runat="server"
                    OnItemCommand="rptPager_ItemCommand">


                    <ItemTemplate>


                        <li class='<%#
                            Convert.ToBoolean(
                                Eval("IsCurrent")
                            )
                            ? "page-item active"
                            : "page-item"
                        %>'>


                            <asp:LinkButton
                                ID="btnPage"
                                runat="server"
                                CommandName="PageNumber"
                                CommandArgument='<%# Eval("Page") %>'
                                CssClass="page-link rounded">


                                <%# Eval("Page") %>


                            </asp:LinkButton>


                        </li>


                    </ItemTemplate>


                </asp:Repeater>


                <!-- TRANG SAU -->

                <li class="page-item">


                    <asp:LinkButton
                        ID="btnNext"
                        runat="server"
                        CssClass="page-link rounded"
                        OnClick="btnNext_Click">


                        Sau →


                    </asp:LinkButton>


                </li>


            </ul>


        </nav>


    </asp:Panel>


    <!-- =====================================================
         KHÔNG CÓ SẢN PHẨM
         ===================================================== -->

    <asp:Panel
        ID="pnlNoProduct"
        runat="server"
        Visible="false"
        CssClass="mt-5">


        <div
            class="card
                   border-0
                   shadow-sm
                   text-center">


            <div class="card-body py-5">


                <div
                    class="display-4
                           mb-3">

                    🛍️

                </div>


                <h2
                    class="h4
                           fw-bold
                           mb-2">

                    Không có sản phẩm

                </h2>


                <p
                    class="text-secondary
                           mb-4">

                    Hiện tại chưa có sản phẩm nào
                    phù hợp với yêu cầu của bạn.

                </p>


                <a
                    href="<%= ResolveUrl("~/shop.aspx") %>"
                    class="btn btn-danger">

                    Xem tất cả sản phẩm

                </a>


            </div>

        </div>


    </asp:Panel>


</section>

</asp:Content>
