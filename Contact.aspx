<%@ Page Title="Liên hệ"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Contact.aspx.cs"
    Inherits="web_ban_hang2.Contact" %>

<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">
</asp:Content>

<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="contact-page">

        <!-- ==========================================
             TIÊU ĐỀ
             ========================================== -->

        <div class="contact-header">

            <span class="contact-label">
                LIÊN HỆ
            </span>

            <h1>
                Liên hệ với chúng tôi
            </h1>

            <p>
                Nếu bạn có câu hỏi, góp ý hoặc cần hỗ trợ,
                hãy gửi tin nhắn cho chúng tôi.
            </p>

        </div>


        <!-- ==========================================
             NỘI DUNG
             ========================================== -->

        <div class="contact-grid">


            <!-- ======================================
                 THÔNG TIN LIÊN HỆ
                 ====================================== -->

            <div class="contact-info">

                <!-- ĐỊA CHỈ -->

                <div class="contact-card">

                    <div class="contact-icon">
                        📍
                    </div>

                    <div>

                        <h3>
                            Địa chỉ
                        </h3>

                        <p>
                            Hà Nội, Việt Nam
                        </p>

                    </div>

                </div>


                <!-- ĐIỆN THOẠI -->

                <div class="contact-card">

                    <div class="contact-icon">
                        📞
                    </div>

                    <div>

                        <h3>
                            Điện thoại
                        </h3>

                        <p>
                            0123 456 789
                        </p>

                    </div>

                </div>


                <!-- EMAIL -->

                <div class="contact-card">

                    <div class="contact-icon">
                        ✉️
                    </div>

                    <div>

                        <h3>
                            Email
                        </h3>

                        <p>
                            shop@gmail.com
                        </p>

                    </div>

                </div>


                <!-- THỜI GIAN -->

                <div class="contact-card">

                    <div class="contact-icon">
                        🕒
                    </div>

                    <div>

                        <h3>
                            Thời gian hỗ trợ
                        </h3>

                        <p>
                            Thứ 2 - Chủ nhật: 08:00 - 22:00
                        </p>

                    </div>

                </div>

            </div>


            <!-- ======================================
                 FORM LIÊN HỆ
                 ====================================== -->

            <div class="contact-form-card">

                <h2>
                    Gửi tin nhắn
                </h2>

                <p class="contact-form-description">
                    Vui lòng điền đầy đủ thông tin bên dưới.
                </p>


                <!-- HỌ TÊN + EMAIL -->

                <div class="contact-form-grid">

                    <!-- HỌ TÊN -->

                    <div class="form-group">

                        <label for="txtName">
                            Họ và tên
                        </label>

                        <asp:TextBox
                            ID="txtName"
                            runat="server"
                            CssClass="form-control"
                            placeholder="Nhập họ và tên">
                        </asp:TextBox>

                    </div>


                    <!-- EMAIL -->

                    <div class="form-group">

                        <label for="txtEmail">
                            Email
                        </label>

                        <asp:TextBox
                            ID="txtEmail"
                            runat="server"
                            CssClass="form-control"
                            TextMode="Email"
                            placeholder="example@gmail.com">
                        </asp:TextBox>

                    </div>

                </div>


                <!-- CHỦ ĐỀ -->

                <div class="form-group">

                    <label for="txtSubject">
                        Chủ đề
                    </label>

                    <asp:TextBox
                        ID="txtSubject"
                        runat="server"
                        CssClass="form-control"
                        placeholder="Bạn muốn liên hệ về vấn đề gì?">
                    </asp:TextBox>

                </div>


                <!-- NỘI DUNG -->

                <div class="form-group">

                    <label for="txtMessage">
                        Nội dung
                    </label>

                    <asp:TextBox
                        ID="txtMessage"
                        runat="server"
                        CssClass="form-control contact-message"
                        TextMode="MultiLine"
                        Rows="6"
                        placeholder="Nhập nội dung tin nhắn...">
                    </asp:TextBox>

                </div>


                <!-- NÚT GỬI -->

                <asp:Button
                    ID="btnSend"
                    runat="server"
                    Text="Gửi tin nhắn"
                    CssClass="contact-button"
                    OnClick="btnSend_Click" />


                <!-- THÔNG BÁO -->

                <asp:Label
                    ID="lblMessage"
                    runat="server"
                    CssClass="contact-message-result">
                </asp:Label>

            </div>

        </div>

    </section>

</asp:Content>