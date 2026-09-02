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
            transition:
                transform .2s ease,
                box-shadow .2s ease;
        }

        .shop-product-card:hover {
            transform: translateY(-4px);
            box-shadow:
                0 .5rem 1rem rgba(0,0,0,.12) !important;
        }

        .shop-pagination .page-link {
            min-width: 42px;
            text-align: center;
        }

        .shop-filter-card {
            border-radius: 14px;
        }

        .shop-filter-card .form-label {
            margin-bottom: 6px;
        }

        .shop-result {
            font-size: 15px;
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
            class="
                d-flex
                flex-column
                flex-md-row
                justify-content-between
                align-items-md-center
                gap-3
                mb-4
            ">

            <div>

                <h1 class="fw-bold mb-2">
                    Sản phẩm
                </h1>

                <p class="text-secondary mb-0">
                    Khám phá các sản phẩm của chúng tôi
                </p>

            </div>


            <a
                href="<%= ResolveUrl("~/shop.aspx") %>"
                class="btn btn-outline-danger">

                🛍️ Tất cả sản phẩm

            </a>

        </div>


        <!-- =====================================================
             BỘ LỌC
             ===================================================== -->

        <div
            class="
                card
                border-0
                shadow-sm
                mb-4
                shop-filter-card
            ">

            <div class="card-body">


                <!-- =================================================
                     HÀNG 1
                     ================================================= -->

                <div class="row g-3 align-items-end">


                    <!-- DANH MỤC -->

                    <div class="col-12 col-md-6 col-lg-3">

                        <label class="form-label fw-semibold">
                            Danh mục
                        </label>

                        <asp:DropDownList
                            ID="ddlCategory"
                            runat="server"
                            CssClass="form-select">

                        </asp:DropDownList>

                    </div>


                    <!-- KHOẢNG GIÁ -->

                    <div class="col-12 col-md-6 col-lg-3">

                        <label class="form-label fw-semibold">
                            Khoảng giá
                        </label>

                        <div class="input-group">

                            <asp:TextBox
                                ID="txtMinPrice"
                                runat="server"
                                CssClass="form-control"
                                placeholder="Từ"
                                inputmode="numeric">
                            </asp:TextBox>

                            <span class="input-group-text">
                                -
                            </span>

                            <asp:TextBox
                                ID="txtMaxPrice"
                                runat="server"
                                CssClass="form-control"
                                placeholder="Đến"
                                inputmode="numeric">
                            </asp:TextBox>

                        </div>

                    </div>


                    <!-- TRẠNG THÁI -->

                    <div class="col-12 col-md-6 col-lg-2">

                        <label class="form-label fw-semibold">
                            Trạng thái
                        </label>

                        <asp:DropDownList
                            ID="ddlStatus"
                            runat="server"
                            CssClass="form-select">

                            <asp:ListItem
                                Value="0">
                                Tất cả
                            </asp:ListItem>

                            <asp:ListItem
                                Value="1">
                                Còn hàng
                            </asp:ListItem>

                            <asp:ListItem
                                Value="2">
                                Hết hàng
                            </asp:ListItem>

                        </asp:DropDownList>

                    </div>


                    <!-- SẮP XẾP -->

                    <div class="col-12 col-md-6 col-lg-2">

                        <label class="form-label fw-semibold">
                            Sắp xếp
                        </label>

                        <asp:DropDownList
                            ID="ddlSort"
                            runat="server"
                            CssClass="form-select">

                            <asp:ListItem
                                Value="newest">
                                Mới nhất
                            </asp:ListItem>

                            <asp:ListItem
                                Value="price_asc">
                                Giá thấp → cao
                            </asp:ListItem>

                            <asp:ListItem
                                Value="price_desc">
                                Giá cao → thấp
                            </asp:ListItem>

                            <asp:ListItem
                                Value="bestseller">
                                Bán chạy
                            </asp:ListItem>

                        </asp:DropDownList>

                    </div>


                    <!-- BUTTON -->

                    <div
                        class="
                            col-12
                            col-lg-2
                            d-flex
                            gap-2
                        ">

                        <asp:Button
                            ID="btnFilter"
                            runat="server"
                            Text="🔎 Lọc"
                            CssClass="
                                btn
                                btn-danger
                                flex-grow-1
                            "
                            OnClick="btnFilter_Click" />

                        <asp:Button
                            ID="btnClearFilter"
                            runat="server"
                            Text="↺"
                            CssClass="
                                btn
                                btn-outline-secondary
                            "
                            ToolTip="Xóa bộ lọc"
                            OnClick="btnClearFilter_Click" />

                    </div>

                </div>


                <!-- =================================================
                     GHI CHÚ TÌM KIẾM
                     ================================================= -->

                <div
                    class="
                        mt-3
                        small
                        text-secondary
                    ">

                    Tìm kiếm theo

                    <strong>
                        tên sản phẩm
                    </strong>,

                    <strong>
                        mô tả
                    </strong>

                    và

                    <strong>
                        danh mục
                    </strong>.

                </div>

            </div>

        </div>


        <!-- =====================================================
             KẾT QUẢ
             ===================================================== -->

        <div
            class="
                card
                border-0
                shadow-sm
                mb-4
            ">

            <div
                class="
                    card-body
                    d-flex
                    flex-column
                    flex-md-row
                    justify-content-between
                    align-items-md-center
                    gap-2
                ">


                <div class="shop-result">

                    <asp:Label
                        ID="lblSearchResult"
                        runat="server"
                        CssClass="fw-semibold text-dark">
                    </asp:Label>

                </div>


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

        <div
            class="
                row
                row-cols-1
                row-cols-sm-2
                row-cols-lg-3
                row-cols-xl-4
                g-4
            ">


            <asp:Repeater
                ID="rptProducts"
                runat="server">


                <ItemTemplate>


                    <div class="col">


                        <div
                            class="
                                card
                                h-100
                                border-0
                                shadow-sm
                                shop-product-card
                                overflow-hidden
                            ">


                            <!-- =============================
                                 HÌNH ẢNH
                                 ============================= -->

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
                                    alt='<%#
                                        Server.HtmlEncode(
                                            Convert.ToString(
                                                Eval("TenSanPham")
                                            )
                                        )
                                    %>'
                                    class="
                                        card-img-top
                                        shop-product-image
                                    "
                                    loading="lazy" />

                            </a>


                            <!-- =============================
                                 THÔNG TIN
                                 ============================= -->

                            <div
                                class="
                                    card-body
                                    d-flex
                                    flex-column
                                ">


                                <!-- DANH MỤC -->

                                <div class="mb-2">

                                    <span
                                        class="
                                            badge
                                            text-bg-light
                                            border
                                        ">

                                        📁

                                        <%#
                                            Server.HtmlEncode(
                                                Convert.ToString(
                                                    Eval("TenDanhMuc")
                                                )
                                            )
                                        %>

                                    </span>

                                </div>


                                <!-- TÊN -->

                                <h5
                                    class="
                                        card-title
                                        fw-bold
                                        shop-product-title
                                    ">

                                    <a
                                        href='<%#
                                            ResolveUrl(
                                                "~/ProductDetail.aspx?id="
                                                + Eval("MaSanPham")
                                            )
                                        %>'
                                        class="
                                            text-dark
                                            text-decoration-none
                                        ">

                                        <%#
                                            Server.HtmlEncode(
                                                Convert.ToString(
                                                    Eval("TenSanPham")
                                                )
                                            )
                                        %>

                                    </a>

                                </h5>


                                <!-- MÔ TẢ -->

                                <p
                                    class="
                                        card-text
                                        text-secondary
                                        small
                                        shop-product-description
                                    ">

                                    <%#
                                        Server.HtmlEncode(
                                            Convert.ToString(
                                                Eval("MoTa")
                                            )
                                        )
                                    %>

                                </p>


                                <!-- GIÁ -->

                                <div
                                    class="
                                        shop-product-price
                                        text-danger
                                        mt-auto
                                        mb-2
                                    ">

                                    <%#
                                        String.Format(
                                            "{0:N0} ₫",
                                            Eval("Gia")
                                        )
                                    %>

                                </div>


                                <!-- TỒN KHO -->

                                <div
                                    class="
                                        shop-product-stock
                                        mb-3
                                    ">

                                    <%#
                                        Convert.ToInt32(
                                            Eval("SoLuong")
                                        ) > 0

                                        ? "<span class='text-success'>✓ Còn hàng</span>"

                                        : "<span class='text-danger'>✕ Hết hàng</span>"
                                    %>

                                </div>


                                <!-- XEM CHI TIẾT -->

                                <a
                                    href='<%#
                                        ResolveUrl(
                                            "~/ProductDetail.aspx?id="
                                            + Eval("MaSanPham")
                                        )
                                    %>'
                                    class="
                                        btn
                                        btn-outline-danger
                                        w-100
                                    ">

                                    Xem chi tiết

                                </a>

                            </div>

                        </div>

                    </div>


                </ItemTemplate>

            </asp:Repeater>

        </div>


        <!-- =====================================================
             KHÔNG CÓ SẢN PHẨM
             ===================================================== -->

        <asp:Panel
            ID="pnlNoProduct"
            runat="server"
            Visible="false"
            CssClass="text-center py-5">


            <div
                class="
                    display-4
                    mb-3
                ">

                🔍

            </div>


            <h3 class="fw-bold">
                Không tìm thấy sản phẩm
            </h3>


            <p class="text-secondary">

                Hãy thử thay đổi từ khóa
                hoặc bộ lọc tìm kiếm.

            </p>


            <a
                href="<%= ResolveUrl("~/shop.aspx") %>"
                class="
                    btn
                    btn-danger
                ">

                Xem tất cả sản phẩm

            </a>

        </asp:Panel>


        <!-- =====================================================
             PHÂN TRANG
             ===================================================== -->

        <asp:Panel
            ID="pnlPager"
            runat="server"
            Visible="false"
            CssClass="mt-5">


            <nav
                aria-label="Phân trang sản phẩm">


                <ul
                    class="
                        pagination
                        justify-content-center
                        shop-pagination
                    ">


                    <!-- TRANG TRƯỚC -->

                    <li
                        class="page-item">

                        <asp:LinkButton
                            ID="btnPrevious"
                            runat="server"
                            CssClass="page-link"
                            OnClick="btnPrevious_Click">

                            ←

                        </asp:LinkButton>

                    </li>


                    <!-- CÁC TRANG -->

                    <asp:Repeater
                        ID="rptPager"
                        runat="server"
                        OnItemCommand="rptPager_ItemCommand">


                        <ItemTemplate>

                            <li
                                class='<%#
                                    Convert.ToBoolean(
                                        Eval("IsCurrent")
                                    )
                                    ? "page-item active"
                                    : "page-item"
                                %>'>


                                <asp:LinkButton
                                    runat="server"
                                    CommandName="PageNumber"
                                    CommandArgument='<%#
                                        Eval("Page")
                                    %>'
                                    CssClass="page-link">

                                    <%#
                                        Eval("Page")
                                    %>

                                </asp:LinkButton>


                            </li>

                        </ItemTemplate>


                    </asp:Repeater>


                    <!-- TRANG SAU -->

                    <li
                        class="page-item">

                        <asp:LinkButton
                            ID="btnNext"
                            runat="server"
                            CssClass="page-link"
                            OnClick="btnNext_Click">

                            →

                        </asp:LinkButton>

                    </li>


                </ul>

            </nav>

        </asp:Panel>


    </section>

</asp:Content>