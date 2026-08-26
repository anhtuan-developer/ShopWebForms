<%@ Page
    Title="Chi tiết tin tức"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="TinTuc_ChiTiet.aspx.cs"
    Inherits="web_ban_hang2.TinTuc_ChiTiet" %>

<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

</asp:Content>


<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="news-detail-page">

        <!-- ==============================
             THÔNG BÁO LỖI
             ============================== -->

        <asp:Panel
            ID="pnlError"
            runat="server"
            Visible="false"
            CssClass="alert alert-danger">

            <asp:Label
                ID="lblMessage"
                runat="server" />

        </asp:Panel>


        <!-- ==============================
             CHI TIẾT BÀI VIẾT
             ============================== -->

        <asp:Panel
            ID="pnlDetail"
            runat="server"
            Visible="false">

            <article class="news-detail-card">

                <div class="news-date">

                    <asp:Label
                        ID="lblNgayTao"
                        runat="server" />

                </div>


                <h1>

                    <asp:Label
                        ID="lblTieuDe"
                        runat="server" />

                </h1>


                <asp:Image
                    ID="imgTinTuc"
                    runat="server"
                    CssClass="news-detail-image" />


                <div class="news-detail-content">

                    <asp:Literal
                        ID="litNoiDung"
                        runat="server"
                        Mode="Encode" />

                </div>


                <hr />


                <!-- ==============================
                     BÌNH LUẬN
                     ============================== -->

                <section class="news-comments">

                    <h3>
                        Bình luận
                    </h3>


                    <!-- FORM BÌNH LUẬN -->

                    <asp:Panel
                        ID="pnlCommentForm"
                        runat="server"
                        CssClass="comment-form">

                        <asp:TextBox
                            ID="txtBinhLuan"
                            runat="server"
                            TextMode="MultiLine"
                            Rows="4"
                            MaxLength="1000"
                            CssClass="form-control"
                            placeholder="Viết bình luận của bạn...">
                        </asp:TextBox>


                        <asp:Button
                            ID="btnBinhLuan"
                            runat="server"
                            Text="Gửi bình luận"
                            CssClass="btn btn-primary mt-2"
                            CausesValidation="false"
                            OnClick="btnBinhLuan_Click" />

                    </asp:Panel>


                    <!-- CHƯA ĐĂNG NHẬP -->

                    <asp:Panel
                        ID="pnlCommentLogin"
                        runat="server"
                        Visible="false"
                        CssClass="alert alert-info">

                        Vui lòng

                        <a href="Dang_nhap.aspx">

                            đăng nhập

                        </a>

                        để bình luận.

                    </asp:Panel>


                    <!-- THÔNG BÁO -->

                    <asp:Label
                        ID="lblCommentMessage"
                        runat="server"
                        CssClass="d-block mt-2">
                    </asp:Label>


                    <!-- DANH SÁCH BÌNH LUẬN -->

                    <asp:Repeater
                        ID="rptBinhLuan"
                        runat="server">

                        <ItemTemplate>

                            <div class="comment-item">

                                <div class="comment-header">

                                    <strong>

                                        <%#
                                            Server.HtmlEncode(
                                                Eval("HoTen")
                                                    .ToString()
                                            )
                                        %>

                                    </strong>


                                    <span>

                                        <%#
                                            Eval(
                                                "NgayBinhLuan",
                                                "{0:dd/MM/yyyy HH:mm}"
                                            )
                                        %>

                                    </span>

                                </div>


                                <div class="comment-content">

                                    <%#
                                        Server.HtmlEncode(
                                            Eval("NoiDung")
                                                .ToString()
                                        )
                                    %>

                                </div>

                            </div>

                        </ItemTemplate>

                    </asp:Repeater>


                    <!-- CHƯA CÓ BÌNH LUẬN -->

                    <asp:Panel
                        ID="pnlNoComment"
                        runat="server"
                        Visible="false"
                        CssClass="empty-state">

                        Chưa có bình luận nào.

                        Hãy là người đầu tiên
                        bình luận.

                    </asp:Panel>

                </section>

            </article>

        </asp:Panel>

    </section>

</asp:Content>