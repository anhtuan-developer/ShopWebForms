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
</asp:Content>


<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="shop-page">


        <!-- ======================================
             TIÊU ĐỀ
             ====================================== -->

        <div class="shop-header">

            <div>

                <h1>
                    Sản phẩm
                </h1>

                <p>
                    Khám phá các sản phẩm của chúng tôi
                </p>

            </div>

        </div>


        <!-- ======================================
             KẾT QUẢ TÌM KIẾM
             ====================================== -->

        <div class="shop-toolbar">

            <div class="shop-result-info">

                <asp:Label
                    ID="lblSearchResult"
                    runat="server">
                </asp:Label>

            </div>


            <div class="shop-page-size">

                <span>
                    Hiển thị 12 sản phẩm / trang
                </span>

            </div>

        </div>


        <!-- ======================================
             DANH SÁCH SẢN PHẨM
             ====================================== -->

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

                                <%# String.Format(
                                    "{0:N0} ₫",
                                    Eval("Gia")) %>

                            </div>


                            <div class="product-stock">

                                Còn lại:

                                <%# Eval("SoLuong") %>

                                sản phẩm

                            </div>


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


        <!-- ======================================
             PHÂN TRANG
             ====================================== -->

        <asp:Panel
            ID="pnlPager"
            runat="server"
            CssClass="shop-pagination">


            <asp:LinkButton
                ID="btnPrevious"
                runat="server"
                CssClass="page-link page-prev"
                OnClick="btnPrevious_Click">

                ← Trước

            </asp:LinkButton>


            <asp:Repeater
                ID="rptPager"
                runat="server"
                OnItemCommand="rptPager_ItemCommand">

                <ItemTemplate>

                    <asp:LinkButton
                        ID="btnPage"
                        runat="server"
                        CommandName="PageNumber"
                        CommandArgument='<%# Eval("Page") %>'
                        CssClass='<%#
                            Convert.ToBoolean(
                                Eval("IsCurrent"))
                            ? "page-link active"
                            : "page-link"
                        %>'>

                        <%# Eval("Page") %>

                    </asp:LinkButton>

                </ItemTemplate>

            </asp:Repeater>


            <asp:LinkButton
                ID="btnNext"
                runat="server"
                CssClass="page-link page-next"
                OnClick="btnNext_Click">

                Sau →

            </asp:LinkButton>

        </asp:Panel>


        <!-- ======================================
             KHÔNG CÓ SẢN PHẨM
             ====================================== -->

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