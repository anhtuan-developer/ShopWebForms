<%@ Page
    Title="Dashboard"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_Default.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_Default"
%>


<asp:Content
    ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <h1 class="admin-title">
        Dashboard
    </h1>


    <div class="row">

        <!-- SẢN PHẨM -->

        <div class="col-md-3">

            <div class="dashboard-card">

                <h4>
                    Sản phẩm
                </h4>

                <h2>

                    <asp:Label
                        ID="lblSanPham"
                        runat="server"
                        Text="0">
                    </asp:Label>

                </h2>

                <p>
                    Tổng số sản phẩm
                </p>

            </div>

        </div>


        <!-- DANH MỤC -->

        <div class="col-md-3">

            <div class="dashboard-card">

                <h4>
                    Danh mục
                </h4>

                <h2>

                    <asp:Label
                        ID="lblDanhMuc"
                        runat="server"
                        Text="0">
                    </asp:Label>

                </h2>

                <p>
                    Tổng số danh mục
                </p>

            </div>

        </div>


        <!-- ĐƠN HÀNG -->

        <div class="col-md-3">

            <div class="dashboard-card">

                <h4>
                    Đơn hàng
                </h4>

                <h2>

                    <asp:Label
                        ID="lblDonHang"
                        runat="server"
                        Text="0">
                    </asp:Label>

                </h2>

                <p>
                    Tổng số đơn hàng
                </p>

            </div>

        </div>


        <!-- KHÁCH HÀNG -->

        <div class="col-md-3">

            <div class="dashboard-card">

                <h4>
                    Khách hàng
                </h4>

                <h2>

                    <asp:Label
                        ID="lblKhachHang"
                        runat="server"
                        Text="0">
                    </asp:Label>

                </h2>

                <p>
                    Tổng số khách hàng
                </p>

            </div>

        </div>

    </div>


    <div class="dashboard-card">

        <h3>
            Chào mừng đến trang quản trị
        </h3>

        <p>

            Tại đây bạn có thể quản lý sản phẩm,
            danh mục, đơn hàng và khách hàng.

        </p>

    </div>

</asp:Content>