<%@ Page
    Title="Tin tức"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="TinTuc.aspx.cs"
    Inherits="web_ban_hang2.TinTucPage" %>


<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

</asp:Content>


<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="news-page">

        <!-- HEADER -->

        <div class="news-header">

            <h1>Tin tức</h1>

            <p>
                Cập nhật những thông tin mới nhất
                từ SHOP 5 ANH EM.
            </p>

        </div>


        <!-- ERROR -->

        <asp:Panel
            ID="pnlError"
            runat="server"
            Visible="false"
            CssClass="alert alert-danger">

            <asp:Label
                ID="lblMessage"
                runat="server" />

        </asp:Panel>


        <!-- DANH SÁCH TIN -->

        <asp:Repeater
            ID="rptTinTuc"
            runat="server">

            <HeaderTemplate>

                <div class="news-grid">

            </HeaderTemplate>


            <ItemTemplate>

                <article class="news-card">

                    <!-- HÌNH ẢNH -->

                    <a
                        href='TinTuc_ChiTiet.aspx?id=<%# Eval("MaTinTuc") %>'
                        class="news-image-link">

                        <img
                            src='<%# GetImageUrl(Eval("HinhAnh")) %>'
                            alt='<%# Server.HtmlEncode(Eval("TieuDe").ToString()) %>'
                            class="news-image" />

                    </a>


                    <!-- NỘI DUNG -->

                    <div class="news-card-body">

                        <div class="news-date">

                            <%#
                                Eval(
                                    "NgayTao",
                                    "{0:dd/MM/yyyy}"
                                )
                            %>

                        </div>


                        <h2>

                            <a
                                href='TinTuc_ChiTiet.aspx?id=<%# Eval("MaTinTuc") %>'>

                                <%#
                                    Server.HtmlEncode(
                                        Eval("TieuDe").ToString()
                                    )
                                %>

                            </a>

                        </h2>


                        <p>

                            <%#
                                GetSummary(
                                    Eval("NoiDung")
                                )
                            %>

                        </p>


                        <a
                            href='TinTuc_ChiTiet.aspx?id=<%# Eval("MaTinTuc") %>'
                            class="news-read-more">

                            Đọc chi tiết →

                        </a>

                    </div>

                </article>

            </ItemTemplate>


            <FooterTemplate>

                </div>

            </FooterTemplate>

        </asp:Repeater>


        <!-- KHÔNG CÓ TIN -->

        <asp:Panel
            ID="pnlEmpty"
            runat="server"
            Visible="false"
            CssClass="empty-state">

            <h3>
                Chưa có tin tức
            </h3>

            <p>
                Hiện tại chưa có bài viết nào được đăng.
            </p>

        </asp:Panel>

    </section>

</asp:Content>