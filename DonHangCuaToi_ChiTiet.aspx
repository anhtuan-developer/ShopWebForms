<%@ Page
    Title="Chi tiết đơn hàng"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="DonHangCuaToi_ChiTiet.aspx.cs"
    Inherits="web_ban_hang2.DonHangCuaToi_ChiTiet"
%>

<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

    <style>

        .customer-order-detail {
            padding: 20px 0 40px;
        }

        .detail-card {
            background: #fff;
            border: 1px solid #eee;
            padding: 22px;
            margin-bottom: 20px;
        }

        .detail-card h2 {
            margin-top: 0;
            margin-bottom: 18px;
        }

        .detail-row {
            display: flex;
            gap: 12px;
            padding: 8px 0;
            border-bottom: 1px solid #f1f1f1;
        }

        .detail-row strong {
            min-width: 160px;
        }

        .detail-table-wrap {
            overflow-x: auto;
        }

        .detail-table {
            width: 100%;
            border-collapse: collapse;
        }

        .detail-table th,
        .detail-table td {
            padding: 12px;
            border-bottom: 1px solid #eee;
            text-align: left;
        }

        .detail-table th {
            background: #f7f7f7;
        }

        .total-row {
            text-align: right;
            font-size: 20px;
            font-weight: 700;
            margin-top: 18px;
        }

        .message {
            padding: 12px 15px;
            background: #fff3cd;
            border: 1px solid #ffe69c;
            color: #664d03;
            border-radius: 4px;
        }

        .back-link {
            display: inline-block;
            margin-top: 10px;
            text-decoration: none;
        }

    </style>

</asp:Content>


<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <section class="customer-order-detail">

        <h1>Chi tiết đơn hàng</h1>


        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="message"
            Visible="false" />


        <asp:Panel
            ID="pnlDetail"
            runat="server">

            <!-- THÔNG TIN ĐƠN HÀNG -->

            <div class="detail-card">

                <h2>
                    Thông tin đơn hàng
                </h2>


                <div class="detail-row">

                    <strong>
                        Mã đơn hàng:
                    </strong>

                    <asp:Label
                        ID="lblMaDonHang"
                        runat="server" />

                </div>


                <div class="detail-row">

                    <strong>
                        Ngày đặt:
                    </strong>

                    <asp:Label
                        ID="lblNgayDat"
                        runat="server" />

                </div>


                <div class="detail-row">

                    <strong>
                        Trạng thái:
                    </strong>

                    <asp:Label
                        ID="lblTrangThai"
                        runat="server" />

                </div>


                <div class="detail-row">

                    <strong>
                        Người nhận:
                    </strong>

                    <asp:Label
                        ID="lblHoTenNguoiNhan"
                        runat="server" />

                </div>


                <div class="detail-row">

                    <strong>
                        Số điện thoại:
                    </strong>

                    <asp:Label
                        ID="lblSoDienThoai"
                        runat="server" />

                </div>


                <div class="detail-row">

                    <strong>
                        Địa chỉ giao hàng:
                    </strong>

                    <asp:Label
                        ID="lblDiaChiGiaoHang"
                        runat="server" />

                </div>

            </div>


            <!-- CHI TIẾT SẢN PHẨM -->

            <div class="detail-card">

                <h2>
                    Sản phẩm
                </h2>


                <div class="detail-table-wrap">

                    <asp:GridView
                        ID="gvChiTiet"
                        runat="server"
                        AutoGenerateColumns="False"
                        CssClass="detail-table"
                        GridLines="None"
                        EmptyDataText="Đơn hàng chưa có sản phẩm.">

                        <Columns>

                            <asp:BoundField
                                DataField="MaSanPham"
                                HeaderText="Mã SP" />


                            <asp:BoundField
                                DataField="TenSanPham"
                                HeaderText="Sản phẩm" />


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


                <div class="total-row">

                    Tổng tiền:

                    <asp:Label
                        ID="lblTongTien"
                        runat="server" />

                </div>

            </div>

            <asp:Button
                ID="btnHuy"
                runat="server"
            
                Text="Hủy đơn hàng"
            
                CssClass="btn btn-danger"
            
                Visible="false"
            
                OnClick="btnHuy_Click"
            
                OnClientClick="
                    return confirm(
                        'Bạn có chắc chắn muốn hủy đơn hàng này không?'
                    );
                " />


            <asp:HyperLink
                ID="lnkQuayLai"
                runat="server"
                NavigateUrl="~/DonHangCuaToi.aspx"
                Text="← Quay lại đơn hàng của tôi"
                CssClass="back-link" />

        </asp:Panel>

    </section>

</asp:Content>