<%@ Page Title="Trang chủ" 
    Language="C#" 
    MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" 
    CodeBehind="index.aspx.cs" 
    Inherits="web_ban_hang2.index" %>

<asp:Content 
    ID="MainContent" 
    ContentPlaceHolderID="MainContent" 
    runat="server">


    <!-- ================================================= -->
    <!-- HERO -->
    <!-- ================================================= -->

    <section class="home-banner">

        <div class="banner-content">

            <span class="banner-label">
                🔥 SẢN PHẨM MỚI
            </span>

            <h1>
                Mua sắm dễ dàng
                <br />
                Giá tốt mỗi ngày
            </h1>

            <p>
                Khám phá các sản phẩm công nghệ,
                điện thoại, laptop và phụ kiện
                chất lượng với mức giá hấp dẫn.
            </p>

            <a 
                href="shop.aspx"
                class="banner-button">

                Khám phá sản phẩm

            </a>

        </div>


        <div class="banner-image">

            <div class="banner-product">
        
                <img
                    src="<%= ResolveUrl("~/img/logo.png") %>"
                    alt="SHOP 5 ANH EM"
                    class="banner-logo"
                    style=" width: 210px;  height: 210px;  object-fit: contain; display: block;
            "
                />
        
            </div>
        </div>

    </section>


    <!-- ================================================= -->
    <!-- CATEGORY -->
    <!-- ================================================= -->

    <section class="category-section">

        <div class="section-header">

            <h2>
                Danh mục sản phẩm
            </h2>

            <a href="shop.aspx">
                Xem tất cả →
            </a>

        </div>


        <div class="category-grid">

            <a 
                href="shop.aspx"
                class="category-card">

                <div class="category-icon">
                    📱
                </div>

                <h3>
                    Điện thoại
                </h3>

                <p>
                    Smartphone chính hãng
                </p>

            </a>


            <a 
                href="shop.aspx"
                class="category-card">

                <div class="category-icon">
                    💻
                </div>

                <h3>
                    Laptop
                </h3>

                <p>
                    Laptop học tập và làm việc
                </p>

            </a>


            <a 
                href="shop.aspx"
                class="category-card">

                <div class="category-icon">
                    🎧
                </div>

                <h3>
                    Phụ kiện
                </h3>

                <p>
                    Phụ kiện công nghệ
                </p>

            </a>


            <a 
                href="shop.aspx"
                class="category-card">

                <div class="category-icon">
                    ⌚
                </div>

                <h3>
                    Đồng hồ
                </h3>

                <p>
                    Đồng hồ thông minh
                </p>

            </a>

        </div>

    </section>


    <!-- ================================================= -->
    <!-- FEATURED PRODUCTS -->
    <!-- ================================================= -->

    <section class="product-section">

        <div class="section-header">

            <div>

                <h2>
                    Sản phẩm nổi bật
                </h2>

            </div>

            <a href="shop.aspx">
                Xem tất cả →
            </a>

        </div>


        <div class="product-grid">

            <asp:Repeater
                ID="rptFeaturedProducts"
                runat="server">

                <ItemTemplate>

                    <div class="product-card">


                        <!-- ================================= -->
                        <!-- HÌNH ẢNH SẢN PHẨM -->
                        <!-- ================================= -->

                        <div class="product-image">

                            <asp:Image
                                ID="imgProduct"
                                runat="server"

                                ImageUrl='<%# GetProductImageUrl(Eval("HinhAnh")) %>'

                                AlternateText='<%#
                                    Convert.ToString(
                                        Eval("TenSanPham")
                                    )
                                %>'

                                Style="
                                    max-width:100%;
                                    max-height:100%;
                                    object-fit:contain;
                                "
                            />

                        </div>


                        <!-- ================================= -->
                        <!-- THÔNG TIN SẢN PHẨM -->
                        <!-- ================================= -->

                        <div class="product-info">


                            <!-- DANH MỤC -->

                            <span class="product-category">

                                <%#
                                    Server.HtmlEncode(
                                        Convert.ToString(
                                            Eval("TenDanhMuc")
                                        )
                                    )
                                %>

                            </span>


                            <!-- TÊN SẢN PHẨM -->

                            <h3>

                                <%#
                                    Server.HtmlEncode(
                                        Convert.ToString(
                                            Eval("TenSanPham")
                                        )
                                    )
                                %>

                            </h3>


                            <!-- GIÁ -->

                            <div class="product-price">

                                <%#
                                    Convert.ToDecimal(
                                        Eval("Gia")
                                    ).ToString("N0")
                                %>

                                ₫

                            </div>


                            <!-- XEM SẢN PHẨM -->

                            <a
                                href='<%#
                                    "ProductDetail.aspx?id="
                                    + Eval("MaSanPham")
                                %>'

                                class="product-button">

                                Xem sản phẩm

                            </a>

                        </div>

                    </div>

                </ItemTemplate>

            </asp:Repeater>

        </div>

    </section>


    <!-- ================================================= -->
    <!-- SERVICE -->
    <!-- ================================================= -->

    <section class="service-section">


        <div class="service-item">

            <div class="service-icon">
                🚚
            </div>

            <div>

                <h3>
                    Giao hàng nhanh
                </h3>

                <p>
                    Giao hàng toàn quốc
                </p>

            </div>

        </div>


        <div class="service-item">

            <div class="service-icon">
                🔒
            </div>

            <div>

                <h3>
                    Mua hàng an toàn
                </h3>

                <p>
                    Bảo mật thông tin
                </p>

            </div>

        </div>


        <div class="service-item">

            <div class="service-icon">
                ⭐
            </div>

            <div>

                <h3>
                    Hàng chính hãng
                </h3>

                <p>
                    Chất lượng đảm bảo
                </p>

            </div>

        </div>


        <div class="service-item">

            <div class="service-icon">
                📞
            </div>

            <div>

                <h3>
                    Hỗ trợ khách hàng
                </h3>

                <p>
                    Luôn sẵn sàng hỗ trợ
                </p>

            </div>

        </div>


    </section>


</asp:Content>