<%@ Page
    Title="Chi tiết đơn hàng"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_DonHang_ChiTiet.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_DonHang_ChiTiet"
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

    <div class="admin-title">

        <h1>
            Chi tiết đơn hàng
        </h1>

        <p>
            Thông tin chi tiết của đơn hàng
        </p>

    </div>


    <!-- THÔNG TIN ĐƠN HÀNG -->

    <div class="dashboard-card">

        <h3>
            Thông tin đơn hàng
        </h3>

        <div class="order-detail-info">

            <div class="detail-row">

                <strong>
                    Mã đơn hàng:
                </strong>

                <asp:Label
                    ID="lblMaDonHang"
                    runat="server">
                </asp:Label>

            </div>


            <div class="detail-row">

                <strong>
                    Khách hàng:
                </strong>

                <asp:Label
                    ID="lblTenKhachHang"
                    runat="server">
                </asp:Label>

            </div>


            <div class="detail-row">

                <strong>
                    Người nhận:
                </strong>

                <asp:Label
                    ID="lblHoTenNguoiNhan"
                    runat="server">
                </asp:Label>

            </div>


            <div class="detail-row">

                <strong>
                    Số điện thoại:
                </strong>

                <asp:Label
                    ID="lblSoDienThoai"
                    runat="server">
                </asp:Label>

            </div>


            <div class="detail-row">

                <strong>
                    Địa chỉ giao hàng:
                </strong>

                <asp:Label
                    ID="lblDiaChiGiaoHang"
                    runat="server">
                </asp:Label>

            </div>


            <div class="detail-row">

                <strong>
                    Trạng thái:
                </strong>

                <asp:Label
                    ID="lblTrangThai"
                    runat="server">
                </asp:Label>

            </div>


            <div class="detail-row">

                <strong>
                    Ngày đặt:
                </strong>

                <asp:Label
                    ID="lblNgayDat"
                    runat="server">
                </asp:Label>

            </div>


            <div class="detail-row">

                <strong>
                    Tổng tiền:
                </strong>

                <asp:Label
                    ID="lblTongTien"
                    runat="server">
                </asp:Label>

            </div>

        </div>

    </div>


    <!-- CHI TIẾT SẢN PHẨM -->

    <div class="dashboard-card">

        <h3>
            Sản phẩm trong đơn hàng
        </h3>


        <div class="table-container">

            <asp:GridView
                ID="gvChiTiet"
                runat="server"

                AutoGenerateColumns="False"

                CssClass="admin-table"

                GridLines="None"

                EmptyDataText="Đơn hàng chưa có sản phẩm.">

                <Columns>

                    <asp:BoundField
                        DataField="MaSanPham"
                        HeaderText="Mã SP" />


                    <asp:BoundField
                        DataField="TenSanPham"
                        HeaderText="Tên sản phẩm" />


                    <asp:BoundField
                        DataField="SoLuong"
                        HeaderText="Số lượng" />


                    <asp:BoundField
                        DataField="DonGia"
                        HeaderText="Đơn giá"
                        DataFormatString="{0:N0} ₫"
                        HtmlEncode="false" />


                    <asp:BoundField
                        DataField="ThanhTien"
                        HeaderText="Thành tiền"
                        DataFormatString="{0:N0} ₫"
                        HtmlEncode="false" />

                </Columns>

            </asp:GridView>

        </div>

    </div>


    <div style="margin-top:20px;">

        <asp:HyperLink
            ID="lnkQuayLai"
            runat="server"
            NavigateUrl="~/Admin/Admin_DonHang.aspx"
            Text="← Quay lại danh sách đơn hàng"
            CssClass="btn-detail">
        </asp:HyperLink>

    </div>


</asp:Content>