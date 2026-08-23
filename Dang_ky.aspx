<%@ Page Title="Đăng ký"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Dang_ky.aspx.cs"
    Inherits="web_ban_hang2.Dang_ky" %>


<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">
</asp:Content>


<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="auth-page">

        <div class="auth-box">

            <h1>
                Tạo tài khoản
            </h1>

            <p class="auth-description">
                Đăng ký tài khoản để mua hàng
            </p>


            <!-- HỌ TÊN -->

            <div class="form-group">

                <label>
                    Họ và tên
                </label>

                <asp:TextBox
                    ID="txtHoTen"
                    runat="server"
                    CssClass="form-control"
                    placeholder="Nhập họ và tên">
                </asp:TextBox>

            </div>


            <!-- EMAIL -->

            <div class="form-group">

                <label>
                    Email
                </label>

                <asp:TextBox
                    ID="txtEmail"
                    runat="server"
                    CssClass="form-control"
                    TextMode="Email"
                    placeholder="Nhập email">
                </asp:TextBox>

            </div>


            <!-- MẬT KHẨU -->

            <div class="form-group">

                <label>
                    Mật khẩu
                </label>

                <asp:TextBox
                    ID="txtMatKhau"
                    runat="server"
                    CssClass="form-control"
                    TextMode="Password"
                    placeholder="Nhập mật khẩu">
                </asp:TextBox>

            </div>


            <!-- XÁC NHẬN -->

            <div class="form-group">

                <label>
                    Xác nhận mật khẩu
                </label>

                <asp:TextBox
                    ID="txtXacNhanMatKhau"
                    runat="server"
                    CssClass="form-control"
                    TextMode="Password"
                    placeholder="Nhập lại mật khẩu">
                </asp:TextBox>

            </div>


            <!-- ĐIỆN THOẠI -->

            <div class="form-group">

                <label>
                    Số điện thoại
                </label>

                <asp:TextBox
                    ID="txtSoDienThoai"
                    runat="server"
                    CssClass="form-control"
                    placeholder="Nhập số điện thoại">
                </asp:TextBox>

            </div>


            <!-- ĐỊA CHỈ -->

            <div class="form-group">

                <label>
                    Địa chỉ
                </label>

                <asp:TextBox
                    ID="txtDiaChi"
                    runat="server"
                    CssClass="form-control"
                    placeholder="Nhập địa chỉ">
                </asp:TextBox>

            </div>


            <!-- BUTTON -->

            <asp:Button
                ID="btnDangKy"
                runat="server"
                Text="Đăng ký"
                CssClass="auth-button"
                OnClick="btnDangKy_Click" />


            <!-- MESSAGE -->

            <asp:Label
                ID="lblMessage"
                runat="server"
                CssClass="auth-message">
            </asp:Label>


            <div class="auth-footer">

                Đã có tài khoản?

                <a href="Dang_nhap.aspx">
                    Đăng nhập
                </a>

            </div>

        </div>

    </section>

</asp:Content>