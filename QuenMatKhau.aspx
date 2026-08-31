<%@ Page
Title="Quên mật khẩu"
Language="C#"
MasterPageFile="~/Site.Master"
AutoEventWireup="true"
CodeBehind="QuenMatKhau.aspx.cs"
Inherits="web_ban_hang2.QuenMatKhau"
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

<div class="container py-5">


    <div class="row justify-content-center">


        <div class="col-lg-5">


            <div class="card border-0 shadow-sm">


                <div class="card-body p-4">


                    <h1 class="h3 fw-bold">

                        Quên mật khẩu

                    </h1>


                    <p class="text-secondary">

                        Nhập email đã đăng ký.
                        Nếu hợp lệ, hệ thống sẽ gửi
                        liên kết đặt lại mật khẩu.

                    </p>


                    <asp:Label
                        ID="lblMessage"
                        runat="server"
                        CssClass="d-block mb-3">
                    </asp:Label>


                    <label class="form-label">

                        Email

                    </label>


                    <asp:TextBox
                        ID="txtEmail"
                        runat="server"
                        TextMode="Email"
                        CssClass="form-control mb-3">
                    </asp:TextBox>


                    <asp:Button
                        ID="btnGui"
                        runat="server"
                        Text="Gửi liên kết đặt lại"
                        CssClass="btn btn-danger w-100"
                        OnClick="btnGui_Click" />


                    <div class="text-center mt-3">

                        <a href="Dang_nhap.aspx">

                            Quay lại đăng nhập

                        </a>

                    </div>


                </div>

            </div>


        </div>


    </div>


</div>

</asp:Content>
