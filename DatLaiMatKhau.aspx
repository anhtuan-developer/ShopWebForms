<%@ Page
Title="Đặt lại mật khẩu"
Language="C#"
MasterPageFile="~/Site.Master"
AutoEventWireup="true"
CodeBehind="DatLaiMatKhau.aspx.cs"
Inherits="web_ban_hang2.DatLaiMatKhau"
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

                        Đặt lại mật khẩu

                    </h1>


                    <p class="text-secondary">

                        Nhập mật khẩu mới của bạn.

                    </p>


                    <asp:Label
                        ID="lblMessage"
                        runat="server"
                        CssClass="d-block mb-3">
                    </asp:Label>


                    <asp:Panel
                        ID="pnlReset"
                        runat="server">


                        <label class="form-label">

                            Mật khẩu mới

                        </label>


                        <asp:TextBox
                            ID="txtMatKhauMoi"
                            runat="server"
                            TextMode="Password"
                            CssClass="form-control mb-3">
                        </asp:TextBox>


                        <label class="form-label">

                            Xác nhận mật khẩu mới

                        </label>


                        <asp:TextBox
                            ID="txtXacNhan"
                            runat="server"
                            TextMode="Password"
                            CssClass="form-control mb-4">
                        </asp:TextBox>


                        <asp:Button
                            ID="btnDatLai"
                            runat="server"
                            Text="Đặt lại mật khẩu"
                            CssClass="btn btn-danger w-100"
                            OnClick="btnDatLai_Click" />


                    </asp:Panel>


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
