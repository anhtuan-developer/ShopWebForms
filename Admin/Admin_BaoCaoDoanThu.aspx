<%@ Page
    Title="Báo cáo doanh thu"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_BaoCaoDoanThu.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_BaoCaoDoanhThu"
%>

<asp:Content
    ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <!-- ==========================================
         TIÊU ĐỀ
         ========================================== -->

    <div class="admin-title">

        <h1>
            Báo cáo doanh thu
        </h1>

        <p>
            Thống kê các đơn hàng đã giao và xuất báo cáo.
        </p>

    </div>


    <!-- ==========================================
         BỘ LỌC
         ========================================== -->

    <div class="dashboard-card">

        <h3>
            Bộ lọc thời gian
        </h3>

        <div class="report-filter">

            <!-- TỪ NGÀY -->

            <div class="report-field">

                <label for="<%= txtTuNgay.ClientID %>">
                    Từ ngày
                </label>

                <asp:TextBox
                    ID="txtTuNgay"
                    runat="server"
                    CssClass="form-control"
                    TextMode="Date">
                </asp:TextBox>

            </div>


            <!-- ĐẾN NGÀY -->

            <div class="report-field">

                <label for="<%= txtDenNgay.ClientID %>">
                    Đến ngày
                </label>

                <asp:TextBox
                    ID="txtDenNgay"
                    runat="server"
                    CssClass="form-control"
                    TextMode="Date">
                </asp:TextBox>

            </div>


            <!-- NÚT -->

            <div class="report-actions">

                <asp:Button
                    ID="btnXemBaoCao"
                    runat="server"
                    Text="Xem báo cáo"
                    CssClass="btn btn-primary"
                    OnClick="btnXemBaoCao_Click" />


                <asp:Button
                    ID="btnExcel"
                    runat="server"
                    Text="Xuất Excel"
                    CssClass="btn btn-success"
                    OnClick="btnExcel_Click" />


                <asp:Button
                    ID="btnPdf"
                    runat="server"
                    Text="Xuất PDF"
                    CssClass="btn btn-danger"
                    OnClick="btnPdf_Click" />

            </div>

        </div>


        <div class="mt-3">

            <asp:Label
                ID="lblThongBao"
                runat="server"
                CssClass="text-danger">
            </asp:Label>

        </div>

    </div>


    <!-- ==========================================
         THỐNG KÊ
         ========================================== -->

    <div class="order-statistics">


        <!-- SỐ ĐƠN -->

        <div class="stat-box">

            <div class="stat-title">
                Tổng đơn đã giao
            </div>

            <div class="stat-value">

                <asp:Label
                    ID="lblSoDonHang"
                    runat="server"
                    Text="0">
                </asp:Label>

            </div>

        </div>


        <!-- DOANH THU -->

        <div class="stat-box">

            <div class="stat-title">
                Tổng doanh thu
            </div>

            <div class="stat-value">

                <asp:Label
                    ID="lblTongDoanhThu"
                    runat="server"
                    Text="0 ₫">
                </asp:Label>

            </div>

        </div>

    </div>


    <!-- ==========================================
         CHI TIẾT
         ========================================== -->

    <div class="dashboard-card">

        <h3>
            Chi tiết doanh thu
        </h3>


        <div class="table-container">

            <asp:GridView
                ID="gvBaoCao"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="admin-table"
                GridLines="None"
                EmptyDataText="Không có đơn hàng đã giao trong khoảng thời gian này.">

                <Columns>

                    <asp:BoundField
                        DataField="MaDonHang"
                        HeaderText="Mã đơn" />


                    <asp:BoundField
                        DataField="TenKhachHang"
                        HeaderText="Khách hàng" />


                    <asp:BoundField
                        DataField="HoTenNguoiNhan"
                        HeaderText="Người nhận" />


                    <asp:BoundField
                        DataField="SoDienThoai"
                        HeaderText="Số điện thoại" />


                    <asp:BoundField
                        DataField="TongTien"
                        HeaderText="Tổng tiền"
                        DataFormatString="{0:N0} ₫"
                        HtmlEncode="false" />


                    <asp:BoundField
                        DataField="TrangThai"
                        HeaderText="Trạng thái" />


                    <asp:BoundField
                        DataField="NgayDat"
                        HeaderText="Ngày đặt"
                        DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                </Columns>

            </asp:GridView>

        </div>

    </div>

</asp:Content>
