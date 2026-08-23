<%@ Page Title="Đăng nhập"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Dang_nhap.aspx.cs"
    Inherits="web_ban_hang2.Dang_nhap" %>


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
                Đăng nhập
            </h1>

            <p class="auth-description">
                Đăng nhập vào tài khoản của bạn
            </p>


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


            <asp:Button
                ID="btnDangNhap"
                runat="server"
                Text="Đăng nhập"
                CssClass="auth-button"
                OnClick="btnDangNhap_Click" />


            <asp:Label
                ID="lblMessage"
                runat="server"
                CssClass="auth-message">
            </asp:Label>


            <div class="auth-footer">

                Chưa có tài khoản?

                <a href="Dang_ky.aspx">
                    Đăng ký ngay
                </a>

            </div>

        </div>

    </section>

</asp:Content>