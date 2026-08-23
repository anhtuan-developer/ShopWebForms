<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="web_ban_hang2.Contact" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <meta charset="utf-8" />

    <title>MultiShop - Contact</title>

    <meta name="viewport"
          content="width=device-width, initial-scale=1.0" />

    <link rel="preconnect"
          href="https://fonts.gstatic.com" />

    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;500;700&display=swap"
          rel="stylesheet" />

    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.10.0/css/all.min.css"
          rel="stylesheet" />

    <link href="<%= ResolveUrl("~/lib/animate/animate.min.css") %>"
          rel="stylesheet" />

    <link href="<%= ResolveUrl("~/lib/owlcarousel/assets/owl.carousel.min.css") %>"
          rel="stylesheet" />

    <link href="<%= ResolveUrl("~/css/style.min.css") %>"
          rel="stylesheet" />

</head>

<body>

<form id="form1" runat="server">

    <!-- TOPBAR -->

    <div class="container-fluid">

        <div class="row bg-secondary py-1 px-xl-5">

            <div class="col-lg-6 d-none d-lg-block">

                <div class="d-inline-flex align-items-center h-100">

                    <a class="text-body mr-3" href="#">
                        About
                    </a>

                    <a class="text-body mr-3"
                       href="Contact.aspx">
                        Contact
                    </a>

                    <a class="text-body mr-3" href="#">
                        Help
                    </a>

                    <a class="text-body mr-3" href="#">
                        FAQs
                    </a>

                </div>

            </div>


            <div class="col-lg-6 text-center text-lg-right">

                <div class="d-inline-flex align-items-center">

                    <div class="btn-group">

                        <button type="button"
                                class="btn btn-sm btn-light dropdown-toggle"
                                data-toggle="dropdown">

                            My Account

                        </button>

                        <div class="dropdown-menu dropdown-menu-right">

                            <a class="dropdown-item" href="#">
                                Sign in
                            </a>

                            <a class="dropdown-item" href="#">
                                Sign up
                            </a>

                        </div>

                    </div>


                    <div class="btn-group mx-2">

                        <button type="button"
                                class="btn btn-sm btn-light dropdown-toggle"
                                data-toggle="dropdown">

                            USD

                        </button>

                        <div class="dropdown-menu dropdown-menu-right">

                            <button class="dropdown-item"
                                    type="button">
                                EUR
                            </button>

                            <button class="dropdown-item"
                                    type="button">
                                GBP
                            </button>

                            <button class="dropdown-item"
                                    type="button">
                                CAD
                            </button>

                        </div>

                    </div>


                    <div class="btn-group">

                        <button type="button"
                                class="btn btn-sm btn-light dropdown-toggle"
                                data-toggle="dropdown">

                            EN

                        </button>

                        <div class="dropdown-menu dropdown-menu-right">

                            <button class="dropdown-item"
                                    type="button">
                                FR
                            </button>

                            <button class="dropdown-item"
                                    type="button">
                                AR
                            </button>

                            <button class="dropdown-item"
                                    type="button">
                                RU
                            </button>

                        </div>

                    </div>

                </div>

            </div>

        </div>


        <!-- LOGO / SEARCH -->

        <div class="row align-items-center bg-light py-3 px-xl-5 d-none d-lg-flex">

            <div class="col-lg-4">

                <a href="shop.aspx"
                   class="text-decoration-none">

                    <span class="h1 text-uppercase text-primary bg-dark px-2">
                        Multi
                    </span>

                    <span class="h1 text-uppercase text-dark bg-primary px-2 ml-n1">
                        Shop
                    </span>

                </a>

            </div>


            <div class="col-lg-4 col-6 text-left">

                <div class="input-group">

                    <asp:TextBox
                        ID="txtSearch"
                        runat="server"
                        CssClass="form-control"
                        placeholder="Search for products">
                    </asp:TextBox>

                    <div class="input-group-append">

                        <span class="input-group-text bg-transparent text-primary">

                            <i class="fa fa-search"></i>

                        </span>

                    </div>

                </div>

            </div>


            <div class="col-lg-4 col-6 text-right">

                <p class="m-0">
                    Customer Service
                </p>

                <h5 class="m-0">
                    +012 345 6789
                </h5>

            </div>

        </div>

    </div>


    <!-- NAVBAR -->

    <div class="container-fluid bg-dark mb-30">

        <div class="row px-xl-5">

            <div class="col-lg-3 d-none d-lg-block">

                <a class="btn d-flex align-items-center justify-content-between bg-primary w-100"
                   data-toggle="collapse"
                   href="#navbar-vertical"
                   style="height:65px;padding:0 30px;">

                    <h6 class="text-dark m-0">

                        <i class="fa fa-bars mr-2"></i>

                        Categories

                    </h6>

                    <i class="fa fa-angle-down text-dark"></i>

                </a>


                <nav class="collapse position-absolute navbar navbar-vertical navbar-light align-items-start p-0 bg-light"
                     id="navbar-vertical"
                     style="width:calc(100% - 30px);z-index:999;">

                    <div class="navbar-nav w-100">

                        <a href="#" class="nav-item nav-link">
                            Dresses
                        </a>

                        <a href="#" class="nav-item nav-link">
                            Shirts
                        </a>

                        <a href="#" class="nav-item nav-link">
                            Jeans
                        </a>

                        <a href="#" class="nav-item nav-link">
                            Swimwear
                        </a>

                        <a href="#" class="nav-item nav-link">
                            Sleepwear
                        </a>

                        <a href="#" class="nav-item nav-link">
                            Sportswear
                        </a>

                        <a href="#" class="nav-item nav-link">
                            Jumpsuits
                        </a>

                        <a href="#" class="nav-item nav-link">
                            Blazers
                        </a>

                        <a href="#" class="nav-item nav-link">
                            Jackets
                        </a>

                        <a href="#" class="nav-item nav-link">
                            Shoes
                        </a>

                    </div>

                </nav>

            </div>


            <div class="col-lg-9">

                <nav class="navbar navbar-expand-lg bg-dark navbar-dark py-3 py-lg-0 px-0">

                    <a href="shop.aspx"
                       class="text-decoration-none d-block d-lg-none">

                        <span class="h1 text-uppercase text-dark bg-light px-2">
                            Multi
                        </span>

                        <span class="h1 text-uppercase text-light bg-primary px-2 ml-n1">
                            Shop
                        </span>

                    </a>


                    <button type="button"
                            class="navbar-toggler"
                            data-toggle="collapse"
                            data-target="#navbarCollapse">

                        <span class="navbar-toggler-icon"></span>

                    </button>


                    <div class="collapse navbar-collapse justify-content-between"
                         id="navbarCollapse">

                        <div class="navbar-nav mr-auto py-0">

                            <a href="Shop.aspx"
                               class="nav-item nav-link">
                                Home
                            </a>

                            <a href="Cart.aspx"
                               class="nav-item nav-link">
                                Shop Cart
                            </a>

                            <a href="Checkout.aspx"
                               class="nav-item nav-link">
                                Checkout
                            </a>

                            <a href="Contact.aspx"
                               class="nav-item nav-link active">
                                Contact
                            </a>

                        </div>


                        <div class="navbar-nav ml-auto py-0 d-none d-lg-block">

                            <a href="#"
                               class="btn px-0">

                                <i class="fas fa-heart text-primary"></i>

                                <span class="badge text-secondary border border-secondary rounded-circle">
                                    0
                                </span>

                            </a>


                            <a href="Cart.aspx"
                               class="btn px-0 ml-3">

                                <i class="fas fa-shopping-cart text-primary"></i>

                                <span class="badge text-secondary border border-secondary rounded-circle">
                                    0
                                </span>

                            </a>

                        </div>

                    </div>

                </nav>

            </div>

        </div>

    </div>


    <!-- BREADCRUMB -->

    <div class="container-fluid">

        <div class="row px-xl-5">

            <div class="col-12">

                <nav class="breadcrumb bg-light mb-30">

                    <a class="breadcrumb-item text-dark"
                       href="shop.aspx">
                        Home
                    </a>

                    <span class="breadcrumb-item active">
                        Contact
                    </span>

                </nav>

            </div>

        </div>

    </div>


    <!-- CONTACT -->

    <div class="container-fluid">

        <h2 class="section-title position-relative text-uppercase mx-xl-5 mb-4">

            <span class="bg-secondary pr-3">
                Contact Us
            </span>

        </h2>


        <div class="row px-xl-5">

            <div class="col-lg-7 mb-5">

                <div class="bg-light p-30">

                    <h4 class="mb-4">
                        Send us a message
                    </h4>


                    <div class="control-group mb-3">

                        <asp:TextBox
                            ID="txtName"
                            runat="server"
                            CssClass="form-control"
                            placeholder="Your Name">
                        </asp:TextBox>

                    </div>


                    <div class="control-group mb-3">

                        <asp:TextBox
                            ID="txtEmail"
                            runat="server"
                            CssClass="form-control"
                            TextMode="Email"
                            placeholder="Your Email">
                        </asp:TextBox>

                    </div>


                    <div class="control-group mb-3">

                        <asp:TextBox
                            ID="txtSubject"
                            runat="server"
                            CssClass="form-control"
                            placeholder="Subject">
                        </asp:TextBox>

                    </div>


                    <div class="control-group mb-3">

                        <asp:TextBox
                            ID="txtMessage"
                            runat="server"
                            CssClass="form-control"
                            TextMode="MultiLine"
                            Rows="8"
                            placeholder="Message">
                        </asp:TextBox>

                    </div>


                    <asp:Button
                        ID="btnSend"
                        runat="server"
                        Text="Send Message"
                        CssClass="btn btn-primary py-2 px-4"
                        OnClick="btnSend_Click" />


                    <asp:Label
                        ID="lblMessage"
                        runat="server"
                        CssClass="d-block mt-3">
                    </asp:Label>

                </div>

            </div>


            <div class="col-lg-5 mb-5">

                <div class="bg-light p-30 mb-30">

                    <iframe
                        title="Google Maps"
                        style="width:100%;height:315px;border:0;"
                        src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3001156.4288297426!2d-78.01371936852176!3d42.72876761954724"
                        allowfullscreen>
                    </iframe>

                </div>


                <div class="bg-light p-30 mb-3">

                    <p class="mb-2">
                        <i class="fa fa-map-marker-alt text-primary mr-3"></i>
                        123 Street, New York, USA
                    </p>

                    <p class="mb-2">
                        <i class="fa fa-envelope text-primary mr-3"></i>
                        info@example.com
                    </p>

                    <p class="mb-2">
                        <i class="fa fa-phone-alt text-primary mr-3"></i>
                        +012 345 67890
                    </p>

                </div>

            </div>

        </div>

    </div>


    <!-- FOOTER -->

    <div class="container-fluid bg-dark text-secondary mt-5 pt-5">

        <div class="row px-xl-5 pt-5">

            <div class="col-lg-4 col-md-12 mb-5 pr-3 pr-xl-5">

                <h5 class="text-secondary text-uppercase mb-4">
                    Get In Touch
                </h5>

                <p class="mb-4">
                    No dolore ipsum accusam no lorem.
                    Invidunt sed clita kasd clita et et dolor.
                </p>

                <p class="mb-2">
                    <i class="fa fa-map-marker-alt text-primary mr-3"></i>
                    123 Street, New York, USA
                </p>

                <p class="mb-2">
                    <i class="fa fa-envelope text-primary mr-3"></i>
                    info@example.com
                </p>

                <p class="mb-0">
                    <i class="fa fa-phone-alt text-primary mr-3"></i>
                    +012 345 67890
                </p>

            </div>


            <div class="col-lg-8">

                <div class="row">

                    <div class="col-md-4 mb-5">

                        <h5 class="text-secondary text-uppercase mb-4">
                            Quick Shop
                        </h5>

                        <div class="d-flex flex-column">

                            <a class="text-secondary mb-2"
                               href="shop.aspx">
                                <i class="fa fa-angle-right mr-2"></i>
                                Home
                            </a>

                            <a class="text-secondary mb-2"
                               href="Shop.aspx">
                                <i class="fa fa-angle-right mr-2"></i>
                                Our Shop
                            </a>

                            <a class="text-secondary mb-2"
                               href="Cart.aspx">
                                <i class="fa fa-angle-right mr-2"></i>
                                Shopping Cart
                            </a>

                            <a class="text-secondary mb-2"
                               href="Checkout.aspx">
                                <i class="fa fa-angle-right mr-2"></i>
                                Checkout
                            </a>

                            <a class="text-secondary"
                               href="Contact.aspx">
                                <i class="fa fa-angle-right mr-2"></i>
                                Contact Us
                            </a>

                        </div>

                    </div>


                    <div class="col-md-4 mb-5">

                        <h5 class="text-secondary text-uppercase mb-4">
                            My Account
                        </h5>

                        <div class="d-flex flex-column">

                            <a class="text-secondary mb-2" href="#">
                                <i class="fa fa-angle-right mr-2"></i>
                                Home
                            </a>

                            <a class="text-secondary mb-2" href="#">
                                <i class="fa fa-angle-right mr-2"></i>
                                Our Shop
                            </a>

                            <a class="text-secondary mb-2" href="Cart.aspx">
                                <i class="fa fa-angle-right mr-2"></i>
                                Shopping Cart
                            </a>

                            <a class="text-secondary mb-2" href="Checkout.aspx">
                                <i class="fa fa-angle-right mr-2"></i>
                                Checkout
                            </a>

                            <a class="text-secondary" href="Contact.aspx">
                                <i class="fa fa-angle-right mr-2"></i>
                                Contact Us
                            </a>

                        </div>

                    </div>


                    <div class="col-md-4 mb-5">

                        <h5 class="text-secondary text-uppercase mb-4">
                            Newsletter
                        </h5>

                        <p>
                            Duo stet tempor ipsum sit amet magna ipsum tempor est
                        </p>

                        <div class="input-group">

                            <asp:TextBox
                                ID="txtNewsletter"
                                runat="server"
                                CssClass="form-control"
                                placeholder="Your Email Address">
                            </asp:TextBox>

                            <div class="input-group-append">

                                <asp:Button
                                    ID="btnNewsletter"
                                    runat="server"
                                    Text="Sign Up"
                                    CssClass="btn btn-primary" />

                            </div>

                        </div>

                    </div>

                </div>

            </div>

        </div>


        <div class="row border-top mx-xl-5 py-4">

            <div class="col-md-6">

                <p class="mb-md-0 text-center text-md-left text-secondary">

                    &copy;

                    <a class="text-primary" href="#">
                        Domain
                    </a>

                    . All Rights Reserved.

                    Designed by

                    <a class="text-primary"
                       href="https://htmlcodex.com">
                        HTML Codex
                    </a>

                </p>

            </div>


            <div class="col-md-6 text-center text-md-right">

                <img class="img-fluid"
                     src="<%= ResolveUrl("~/img/payments.png") %>"
                     alt="Payments" />

            </div>

        </div>

    </div>


    <!-- JS -->

    <a href="#"
       class="btn btn-primary back-to-top">

        <i class="fa fa-angle-double-up"></i>

    </a>


    <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>

    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.bundle.min.js"></script>

    <script src="<%= ResolveUrl("~/lib/easing/easing.min.js") %>"></script>

    <script src="<%= ResolveUrl("~/lib/owlcarousel/owl.carousel.min.js") %>"></script>

    <script src="<%= ResolveUrl("~/js/main.js") %>"></script>

</form>

</body>
</html>