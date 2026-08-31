<%@ Page
Title="Tài khoản"
Language="C#"
MasterPageFile="~/Site.Master"
AutoEventWireup="true"
CodeBehind="TaiKhoan.aspx.cs"
Inherits="web_ban_hang2.TaiKhoan"
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


    <div class="row g-4">


        <!-- MENU TÀI KHOẢN -->

        <div class="col-lg-4">

            <div class="list-group shadow-sm">

                <a
                    class="list-group-item
                           list-group-item-action
                           active"
                    href="TaiKhoan.aspx">

                    👤 Thông tin cá nhân

                </a>


                <a
                    class="list-group-item
                           list-group-item-action"
                    href="DoiMatKhau.aspx">

                    🔐 Đổi mật khẩu

                </a>


                <a
                    class="list-group-item
                           list-group-item-action"
                    href="DonHangCuaToi.aspx">

                    📦 Đơn hàng

                </a>


                <a
                    class="list-group-item
                           list-group-item-action
                           text-danger"
                    href="Dang_xuat.aspx">

                    🚪 Đăng xuất

                </a>

            </div>

        </div>


        <!-- THÔNG TIN -->

        <div class="col-lg-8">

            <div class="card border-0 shadow-sm">


                <div class="card-body p-4">


                    <h1 class="h3 fw-bold">

                        Thông tin tài khoản

                    </h1>


                    <p class="text-secondary">

                        Cập nhật thông tin giao hàng
                        của bạn.

                    </p>


                    <asp:Label
                        ID="lblMessage"
                        runat="server"
                        CssClass="d-block mb-3">
                    </asp:Label>


                    <div class="row g-3">


                        <!-- HỌ TÊN -->

                        <div class="col-md-6">

                            <label class="form-label">

                                Họ tên

                            </label>


                            <asp:TextBox
                                ID="txtHoTen"
                                runat="server"
                                CssClass="form-control">
                            </asp:TextBox>

                        </div>


                        <!-- EMAIL -->

                        <div class="col-md-6">

                            <label class="form-label">

                                Email

                            </label>


                            <asp:TextBox
                                ID="txtEmail"
                                runat="server"
                                CssClass="form-control"
                                ReadOnly="true">
                            </asp:TextBox>

                        </div>


                        <!-- SỐ ĐIỆN THOẠI -->

                        <div class="col-md-6">

                            <label class="form-label">

                                Số điện thoại

                            </label>


                            <asp:TextBox
                                ID="txtSoDienThoai"
                                runat="server"
                                CssClass="form-control">
                            </asp:TextBox>

                        </div>


                        <!-- ĐỊA CHỈ -->

                        <div class="col-12">

                            <label class="form-label">

                                Địa chỉ

                            </label>


                            <asp:TextBox
                                ID="txtDiaChi"
                                runat="server"
                                CssClass="form-control"
                                TextMode="MultiLine"
                                Rows="3">
                            </asp:TextBox>

                        </div>


                        <!-- BUTTON -->

                        <div class="col-12">

                            <asp:Button
                                ID="btnLuu"
                                runat="server"
                                Text="Lưu thay đổi"
                                CssClass="btn btn-danger"
                                OnClick="btnLuu_Click" />

                        </div>

                    </div>


                </div>

            </div>

        </div>


    </div>

</div>

</asp:Content>
