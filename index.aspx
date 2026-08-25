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

    <!-- BANNER -->
    <section class="home-banner">

        <div class="banner-content">

            <span class="banner-label">
                SẢN PHẨM MỚI
            </span>

            <h1>
                Mua sắm dễ dàng<br />
                Giá tốt mỗi ngày
            </h1>

            <p>
                Khám phá hàng nghìn sản phẩm chất lượng
                với mức giá hấp dẫn.
            </p>

            <a href="shop.aspx" class="banner-button">
                Mua sắm ngay
            </a>

        </div>

        <div class="banner-image">
            <div class="banner-product">
                🛍️
            </div>
        </div>

    </section>


    <!-- DANH MỤC -->
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

            <a href="shop.aspx" class="category-card">

                <div class="category-icon">
                    📱
                </div>

                <h3>
                    Điện thoại
                </h3>

                <p>
                    Sản phẩm điện thoại
                </p>

            </a>


            <a href="shop.aspx" class="category-card">

                <div class="category-icon">
                    💻
                </div>

                <h3>
                    Laptop
                </h3>

                <p>
                    Laptop chính hãng
                </p>

            </a>


            <a href="shop.aspx" class="category-card">

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


            <a href="shop.aspx" class="category-card">

                <div class="category-icon">
                    ⌚
                </div>

                <h3>
                    Đồng hồ
                </h3>

                <p>
                    Đồng hồ thời trang
                </p>

            </a>

        </div>

    </section>


    <!-- SẢN PHẨM NỔI BẬT -->
    <section class="product-section">

        <div class="section-header">

            <h2>
                Sản phẩm nổi bật
            </h2>

            <a href="shop.aspx">
                Xem tất cả →
            </a>

        </div>


        <div class="product-grid">


            <!-- PRODUCT 1 -->
            <div class="product-card">

                <div class="product-image">
                    📱
                </div>

                <div class="product-info">

                    <span class="product-category">
                        Điện thoại
                    </span>

                    <h3>
                        Smartphone Pro
                    </h3>

                    <div class="product-price">
                        12.990.000 ₫
                    </div>

                    <a href="shop.aspx"
                       class="product-button">
                        Xem sản phẩm
                    </a>

                </div>

            </div>


            <!-- PRODUCT 2 -->
            <div class="product-card">

                <div class="product-image">
                    💻
                </div>

                <div class="product-info">

                    <span class="product-category">
                        Laptop
                    </span>

                    <h3>
                        Laptop Pro 15
                    </h3>

                    <div class="product-price">
                        18.990.000 ₫
                    </div>

                    <a href="shop.aspx"
                       class="product-button">
                        Xem sản phẩm
                    </a>

                </div>

            </div>


            <!-- PRODUCT 3 -->
            <div class="product-card">

                <div class="product-image">
                    🎧
                </div>

                <div class="product-info">

                    <span class="product-category">
                        Phụ kiện
                    </span>

                    <h3>
                        Tai nghe Bluetooth
                    </h3>

                    <div class="product-price">
                        1.290.000 ₫
                    </div>

                    <a href="shop.aspx"
                       class="product-button">
                        Xem sản phẩm
                    </a>

                </div>

            </div>


            <!-- PRODUCT 4 -->
            <div class="product-card">

                <div class="product-image">
                    ⌚
                </div>

                <div class="product-info">

                    <span class="product-category">
                        Đồng hồ
                    </span>

                    <h3>
                        Smart Watch
                    </h3>

                    <div class="product-price">
                        2.490.000 ₫
                    </div>

                    <a href="shop.aspx"
                       class="product-button">
                        Xem sản phẩm
                    </a>

                </div>

            </div>

        </div>

    </section>


    <!-- CAM KẾT -->
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
                    Đặt hàng an toàn
                </h3>

                <p>
                    Bảo mật thông tin đặt hàng
                </p>
            </div>

        </div>


        <div class="service-item">

            <div class="service-icon">
                ⭐
            </div>

            <div>
                <h3>
                    Sản phẩm chất lượng
                </h3>

                <p>
                    Cam kết chính hãng
                </p>
            </div>

        </div>


        <div class="service-item">

            <div class="service-icon">
                📞
            </div>

            <div>
                <h3>
                    Hỗ trợ 24/7
                </h3>

                <p>
                    Luôn sẵn sàng hỗ trợ
                </p>
            </div>

        </div>

    </section>

</asp:Content>