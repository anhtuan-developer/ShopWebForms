<%@ Page
Title="Đổi mật khẩu"
Language="C#"
MasterPageFile="~/Site.Master"
AutoEventWireup="true"
CodeBehind="DoiMatKhau.aspx.cs"
Inherits="web_ban_hang2.DoiMatKhau"
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

<div class="container py-4">


    <div class="row justify-content-center">


        <div class="col-lg-6">


            <div class="card border-0 shadow-sm">


                <div class="card-body p-4">


                    <h1 class="h3 fw-bold">

                        Đổi mật khẩu

                    </h1>


                    <p class="text-secondary">

                        Sử dụng mật khẩu mới có ít nhất
                        6 ký tự.

                    </p>


                    <asp:Label
                        ID="lblMessage"
                        runat="server"
                        CssClass="d-block mb-3">
                    </asp:Label>


                    <!-- MẬT KHẨU CŨ -->

                    <label class="form-label">

                        Mật khẩu hiện tại

                    </label>


                    <asp:TextBox
                        ID="txtMatKhauCu"
                        runat="server"
                        TextMode="Password"
                        CssClass="form-control mb-3">
                    </asp:TextBox>


                    <!-- MẬT KHẨU MỚI -->

                    <label class="form-label">

                        Mật khẩu mới

                    </label>


                    <asp:TextBox
                        ID="txtMatKhauMoi"
                        runat="server"
                        TextMode="Password"
                        CssClass="form-control mb-3">
                    </asp:TextBox>


                    <!-- XÁC NHẬN -->

                    <label class="form-label">

                        Xác nhận mật khẩu mới

                    </label>


                    <asp:TextBox
                        ID="txtXacNhan"
                        runat="server"
                        TextMode="Password"
                        CssClass="form-control mb-4">
                    </asp:TextBox>


                    <!-- BUTTON -->

                    <asp:Button
                        ID="btnDoiMatKhau"
                        runat="server"
                        Text="Đổi mật khẩu"
                        CssClass="btn btn-danger w-100"
                        OnClick="btnDoiMatKhau_Click" />


                </div>

            </div>


        </div>


    </div>


</div>
</asp:Content>
