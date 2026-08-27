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

    <div class="admin-title">

        <h1>
            Dashboard
        </h1>

        <p>
            Tổng quan hoạt động của cửa hàng
        </p>

    </div>


    <!-- ====================================== -->
    <!-- THỐNG KÊ TỔNG QUAN -->
    <!-- ====================================== -->

    <div class="dashboard-grid">

        <!-- SẢN PHẨM -->

        <div class="dashboard-card">

            <div class="dashboard-card-title">
                Tổng sản phẩm
            </div>

            <div class="dashboard-card-value">

                <asp:Label
                    ID="lblSanPham"
                    runat="server"
                    Text="0">
                </asp:Label>

            </div>

        </div>


        <!-- DANH MỤC -->

        <div class="dashboard-card">

            <div class="dashboard-card-title">
                Tổng danh mục
            </div>

            <div class="dashboard-card-value">

                <asp:Label
                    ID="lblDanhMuc"
                    runat="server"
                    Text="0">
                </asp:Label>

            </div>

        </div>


        <!-- KHÁCH HÀNG -->

        <div class="dashboard-card">

            <div class="dashboard-card-title">
                Tổng khách hàng
            </div>

            <div class="dashboard-card-value">

                <asp:Label
                    ID="lblKhachHang"
                    runat="server"
                    Text="0">
                </asp:Label>

            </div>

        </div>


        <!-- ĐƠN HÀNG -->

        <div class="dashboard-card">

            <div class="dashboard-card-title">
                Tổng đơn hàng
            </div>

            <div class="dashboard-card-value">

                <asp:Label
                    ID="lblDonHang"
                    runat="server"
                    Text="0">
                </asp:Label>

            </div>

        </div>

    </div>


    <!-- ====================================== -->
    <!-- BÁO CÁO DOANH THU -->
    <!-- ====================================== -->

    <div class="admin-card">

        <div class="admin-card-header">

            <h3>
                Báo cáo doanh thu
            </h3>

        </div>


        <div class="admin-card-body">

            <div class="dashboard-grid">

                <!-- DOANH THU HÔM NAY -->

                <div class="dashboard-card">

                    <div class="dashboard-card-title">
                        Doanh thu hôm nay
                    </div>

                    <div class="dashboard-card-value">

                        <asp:Label
                            ID="lblDoanhThuHomNay"
                            runat="server"
                            Text="0 ₫">
                        </asp:Label>

                    </div>

                </div>


                <!-- DOANH THU THÁNG -->

                <div class="dashboard-card">

                    <div class="dashboard-card-title">
                        Doanh thu tháng này
                    </div>

                    <div class="dashboard-card-value">

                        <asp:Label
                            ID="lblDoanhThuThang"
                            runat="server"
                            Text="0 ₫">
                        </asp:Label>

                    </div>

                </div>


                <!-- DOANH THU NĂM -->

                <div class="dashboard-card">

                    <div class="dashboard-card-title">
                        Doanh thu năm nay
                    </div>

                    <div class="dashboard-card-value">

                        <asp:Label
                            ID="lblDoanhThuNam"
                            runat="server"
                            Text="0 ₫">
                        </asp:Label>

                    </div>

                </div>


                <!-- ĐÃ GIAO -->

                <div class="dashboard-card">

                    <div class="dashboard-card-title">
                        Số đơn đã giao
                    </div>

                    <div class="dashboard-card-value">

                        <asp:Label
                            ID="lblSoDonDaGiao"
                            runat="server"
                            Text="0">
                        </asp:Label>

                    </div>

                </div>


                <!-- ĐANG GIAO -->

                <div class="dashboard-card">

                    <div class="dashboard-card-title">
                        Số đơn đang giao
                    </div>

                    <div class="dashboard-card-value">

                        <asp:Label
                            ID="lblSoDonDangGiao"
                            runat="server"
                            Text="0">
                        </asp:Label>

                    </div>

                </div>

            </div>

        </div>

    </div>


    <!-- ====================================== -->
    <!-- TOP SẢN PHẨM BÁN CHẠY -->
    <!-- ====================================== -->

    <div class="admin-card">

        <div class="admin-card-header">

            <h3>
                Top 5 sản phẩm bán chạy
            </h3>

        </div>


        <div class="admin-card-body">

            <div class="table-container">

                <asp:GridView
                    ID="gvTopSanPham"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="admin-table"
                    EmptyDataText="Chưa có dữ liệu bán hàng.">

                    <Columns>

                        <asp:BoundField
                            DataField="TenSanPham"
                            HeaderText="Sản phẩm" />

                        <asp:BoundField
                            DataField="TongSoLuongBan"
                            HeaderText="Số lượng bán" />

                        <asp:BoundField
                            DataField="DoanhThu"
                            HeaderText="Doanh thu"
                            DataFormatString="{0:N0} ₫"
                            HtmlEncode="false" />

                    </Columns>

                </asp:GridView>

            </div>

        </div>

    </div>


    <!-- ====================================== -->
    <!-- GIỚI THIỆU -->
    <!-- ====================================== -->

    <div class="admin-card">

        <div class="admin-card-body">

            <h3>
                Chào mừng đến trang quản trị
            </h3>

            <p>
                Tại đây bạn có thể quản lý sản phẩm,
                danh mục, đơn hàng, khách hàng
                và theo dõi doanh thu của cửa hàng.
            </p>

        </div>

    </div>

</asp:Content>
