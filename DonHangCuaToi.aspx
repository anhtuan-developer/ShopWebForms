<%@ Page
    Title="Đơn hàng của tôi"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="DonHangCuaToi.aspx.cs"
    Inherits="web_ban_hang2.DonHangCuaToi"
%>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .customer-orders {
            padding: 20px 0 40px;
        }

        .customer-orders h1 {
            margin-bottom: 8px;
        }

        .customer-orders .subtitle {
            color: #666;
            margin-bottom: 25px;
        }

        .order-table-wrap {
            overflow-x: auto;
        }

        .order-table {
            width: 100%;
            border-collapse: collapse;
            background: #fff;
        }

        .order-table th,
        .order-table td {
            padding: 13px 12px;
            border-bottom: 1px solid #eee;
            text-align: left;
            vertical-align: middle;
        }

        .order-table th {
            background: #f7f7f7;
        }

        .order-status {
            font-weight: 600;
        }

        .btn-order-detail {
            display: inline-block;
            padding: 7px 12px;
            background: #e53935;
            color: #fff !important;
            text-decoration: none;
            border-radius: 4px;
        }

        .empty-orders {
            padding: 35px;
            text-align: center;
            background: #fff;
            border: 1px solid #eee;
        }

        .message {
            margin-bottom: 18px;
            padding: 12px 15px;
            background: #fff3cd;
            border: 1px solid #ffe69c;
            color: #664d03;
            border-radius: 4px;
        }
    </style>
</asp:Content>

<asp:Content ID="MainContent"
             ContentPlaceHolderID="MainContent"
             runat="server">

    <section class="customer-orders">

        <h1>Đơn hàng của tôi</h1>

        <p class="subtitle">
            Theo dõi các đơn hàng bạn đã đặt.
        </p>


        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="message"
            Visible="false" />


        <asp:Panel
            ID="pnlOrders"
            runat="server">

            <div class="order-table-wrap">

                <asp:GridView
                    ID="gvDonHang"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="order-table"
                    GridLines="None"
                    EmptyDataText="Bạn chưa có đơn hàng nào."
                    OnRowDataBound="gvDonHang_RowDataBound">

                    <Columns>

                        <asp:BoundField
                            DataField="MaDonHang"
                            HeaderText="Mã đơn" />

                        <asp:BoundField
                            DataField="NgayDat"
                            HeaderText="Ngày đặt"
                            DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                        <asp:BoundField
                            DataField="TongTien"
                            HeaderText="Tổng tiền"
                            DataFormatString="{0:N0} ₫"
                            HtmlEncode="false" />


                        <asp:TemplateField
                            HeaderText="Trạng thái">

                            <ItemTemplate>

                                <asp:Label
                                    ID="lblTrangThai"
                                    runat="server"
                                    CssClass="order-status"
                                    Text='<%# Eval("TrangThai") %>' />

                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField
                            HeaderText="Chi tiết">

                            <ItemTemplate>

                                <asp:HyperLink
                                    ID="lnkChiTiet"
                                    runat="server"
                                    CssClass="btn-order-detail"
                                    Text="Xem chi tiết" />

                            </ItemTemplate>

                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

        </asp:Panel>

    </section>

</asp:Content>