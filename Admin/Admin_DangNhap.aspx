<%@ Page
    Title="Đăng nhập Admin"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_DangNhap.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_DangNhap"
%>

<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

</asp:Content>


<asp:Content
    ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="admin-login-wrapper">

        <div class="admin-login">

            <!-- TIÊU ĐỀ -->

            <h1>
                Đăng nhập Admin
            </h1>


            <p class="admin-login-description">
                Vui lòng đăng nhập để truy cập trang quản trị.
            </p>


            <!-- EMAIL -->

            <div class="form-group">

                <label for="<%= txtEmail.ClientID %>">
                    Email
                </label>

                <asp:TextBox
                    ID="txtEmail"
                    runat="server"
                    CssClass="admin-input"
                    TextMode="Email"
                    placeholder="Nhập email">
                </asp:TextBox>

            </div>


            <!-- MẬT KHẨU -->

            <div class="form-group">

                <label for="<%= txtMatKhau.ClientID %>">
                    Mật khẩu
                </label>

                <asp:TextBox
                    ID="txtMatKhau"
                    runat="server"
                    CssClass="admin-input"
                    TextMode="Password"
                    placeholder="Nhập mật khẩu">
                </asp:TextBox>

            </div>


            <!-- NÚT ĐĂNG NHẬP -->

            <asp:Button
                ID="btnDangNhap"
                runat="server"
                Text="Đăng nhập"
                CssClass="btn-admin-login"
                OnClick="btnDangNhap_Click" />


            <!-- THÔNG BÁO -->

            <asp:Label
                ID="lblThongBao"
                runat="server"
                CssClass="error-message">
            </asp:Label>

        </div>

    </div>

</asp:Content>