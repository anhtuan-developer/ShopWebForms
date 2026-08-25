<%@ Page Title="Giỏ hàng"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Cart.aspx.cs"
    Inherits="web_ban_hang2.Cart" %>


<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

</asp:Content>


<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="cart-page">

        <div class="cart-header">

            <h1>
                Giỏ hàng
            </h1>

            <a href="shop.aspx">
                ← Tiếp tục mua hàng
            </a>

        </div>


        <asp:Panel
            ID="pnlCart"
            runat="server">

            <div class="cart-container">

                <!-- DANH SÁCH -->

                <div class="cart-items">

                    <asp:Repeater
                        ID="rptCart"
                        runat="server"
                        OnItemCommand="rptCart_ItemCommand">

                        <HeaderTemplate>

                            <div class="cart-table-header">

                                <div>
                                    Sản phẩm
                                </div>

                                <div>
                                    Đơn giá
                                </div>

                                <div>
                                    Số lượng
                                </div>

                                <div>
                                    Thành tiền
                                </div>

                                <div>
                                </div>

                            </div>

                        </HeaderTemplate>


                        <ItemTemplate>

                            <div class="cart-item">

                                <!-- PRODUCT -->

                                <div class="cart-product">

                                    <img
                                        src='<%# ResolveUrl("~/img/" + Eval("HinhAnh")) %>'
                                        alt='<%# Eval("TenSanPham") %>' />


                                    <div>

                                        <h3>

                                            <%# Eval("TenSanPham") %>

                                        </h3>

                                    </div>

                                </div>


                                <!-- PRICE -->

                                <div class="cart-price">

                                    <%#
                                        String.Format(
                                            "{0:N0} ₫",
                                            Eval("Gia")
                                        )
                                    %>

                                </div>


                                <!-- QUANTITY -->

                                <div class="cart-quantity">

                                    <asp:TextBox
                                        ID="txtQuantity"
                                        runat="server"
                                        Text='<%# Eval("SoLuong") %>'
                                        CssClass="cart-quantity-input">
                                    </asp:TextBox>


                                    <asp:Button
                                        ID="btnUpdate"
                                        runat="server"
                                        Text="Cập nhật"
                                        CommandName="UpdateCart"
                                        CommandArgument='<%# Eval("MaSanPham") %>'
                                        CssClass="update-button" />

                                </div>


                                <!-- TOTAL -->

                                <div class="cart-item-total">

                                    <%#
                                        String.Format(
                                            "{0:N0} ₫",
                                            Eval("ThanhTien")
                                        )
                                    %>

                                </div>


                                <!-- REMOVE -->

                                <div>

                                    <asp:Button
                                        ID="btnRemove"
                                        runat="server"
                                        Text="Xóa"
                                        CommandName="RemoveCart"
                                        CommandArgument='<%# Eval("MaSanPham") %>'
                                        CssClass="remove-button" />

                                </div>

                            </div>

                        </ItemTemplate>

                    </asp:Repeater>

                </div>


                <!-- SUMMARY -->

                <div class="cart-summary">

                    <h2>
                        Tổng đơn hàng
                    </h2>


                    <div class="summary-row">

                        <span>
                            Số sản phẩm
                        </span>

                        <strong>

                            <asp:Label
                                ID="lblTotalQuantity"
                                runat="server">
                            </asp:Label>

                        </strong>

                    </div>


                    <div class="summary-row total-row">

                        <span>
                            Tổng tiền
                        </span>

                        <strong>

                            <asp:Label
                                ID="lblTotal"
                                runat="server">
                            </asp:Label>

                        </strong>

                    </div>


                    <asp:Button
                         ID="btnCheckout"
                         runat="server"
                         Text="Tiến hành đặt hàng"
                         CssClass="btn btn-primary"
                         OnClick="btnCheckout_Click" />

                </div>

            </div>

        </asp:Panel>


        <!-- GIỎ HÀNG TRỐNG -->

        <asp:Panel
            ID="pnlEmpty"
            runat="server"
            Visible="false"
            CssClass="empty-cart">

            <div class="empty-cart-icon">
                🛒
            </div>

            <h2>
                Giỏ hàng đang trống
            </h2>

            <p>
                Bạn chưa có sản phẩm nào trong giỏ hàng.
            </p>

            <a
                href="shop.aspx"
                class="continue-button">

                Mua sắm ngay

            </a>

        </asp:Panel>

    </section>

</asp:Content>