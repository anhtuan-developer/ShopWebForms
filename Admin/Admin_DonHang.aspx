<%@ Page
    Title="Quản lý đơn hàng"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_DonHang.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_DonHang"
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


    <!-- ====================================== -->
    <!-- TIÊU ĐỀ -->
    <!-- ====================================== -->

    <div class="admin-title">

        <h1>
            Quản lý đơn hàng
        </h1>

        <p>
            Danh sách các đơn hàng của cửa hàng
        </p>

    </div>


    <!-- ====================================== -->
    <!-- THỐNG KÊ -->
    <!-- ====================================== -->

    <div class="order-statistics">


        <div class="stat-box">

            <div class="stat-title">
                Tổng đơn hàng
            </div>

            <div class="stat-value">

                <asp:Label
                    ID="lblTongDonHang"
                    runat="server"
                    Text="0">
                </asp:Label>

            </div>

        </div>


        <div class="stat-box">

            <div class="stat-title">
                Chờ xử lý
            </div>

            <div class="stat-value">

                <asp:Label
                    ID="lblChoXuLy"
                    runat="server"
                    Text="0">
                </asp:Label>

            </div>

        </div>


        <div class="stat-box">

            <div class="stat-title">
                Đang giao
            </div>

            <div class="stat-value">

                <asp:Label
                    ID="lblDangGiao"
                    runat="server"
                    Text="0">
                </asp:Label>

            </div>

        </div>


        <div class="stat-box">

            <div class="stat-title">
                Đã giao
            </div>

            <div class="stat-value">

                <asp:Label
                    ID="lblDaGiao"
                    runat="server"
                    Text="0">
                </asp:Label>

            </div>

        </div>


    </div>


    <!-- ====================================== -->
    <!-- DANH SÁCH ĐƠN HÀNG -->
    <!-- ====================================== -->

    <div class="dashboard-card">


        <h3>
            Danh sách đơn hàng
        </h3>


        <div class="table-container">


            <asp:GridView
                ID="gvDonHang"
                runat="server"

                AutoGenerateColumns="False"

                CssClass="admin-table"

                GridLines="None"

                EmptyDataText="Chưa có đơn hàng nào."

                OnRowDataBound="gvDonHang_RowDataBound">


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
                        DataField="DiaChiGiaoHang"
                        HeaderText="Địa chỉ" />



                    <asp:BoundField
                        DataField="TongTien"
                        HeaderText="Tổng tiền"
                        DataFormatString="{0:N0} ₫"
                        HtmlEncode="false" />



                    <asp:TemplateField
                        HeaderText="Trạng thái">

                        <ItemTemplate>

                            <asp:DropDownList
                                ID="ddlTrangThai"
                                runat="server"

                                CssClass="form-control"

                                AutoPostBack="true"

                                OnSelectedIndexChanged="ddlTrangThai_SelectedIndexChanged">

                                <asp:ListItem
                                    Text="Chờ xử lý"
                                    Value="Chờ xử lý">
                                </asp:ListItem>

                                <asp:ListItem
                                    Text="Đã xác nhận"
                                    Value="Đã xác nhận">
                                </asp:ListItem>

                                <asp:ListItem
                                    Text="Đang giao"
                                    Value="Đang giao">
                                </asp:ListItem>

                                <asp:ListItem
                                    Text="Đã giao"
                                    Value="Đã giao">
                                </asp:ListItem>

                                <asp:ListItem
                                    Text="Đã hủy"
                                    Value="Đã hủy">
                                </asp:ListItem>

                            </asp:DropDownList>

                        </ItemTemplate>

                    </asp:TemplateField>



                    <asp:BoundField
                        DataField="NgayDat"
                        HeaderText="Ngày đặt"
                        DataFormatString="{0:dd/MM/yyyy HH:mm}" />



                    <asp:TemplateField
                        HeaderText="Thao tác">

                        <ItemTemplate>

                            <asp:HyperLink
                                ID="lnkChiTiet"
                                runat="server"

                                Text="Chi tiết"

                                CssClass="btn-detail"

                                NavigateUrl='<%#
                                    "Admin_DonHang_ChiTiet.aspx?id="
                                    + Eval("MaDonHang")
                                %>'>

                            </asp:HyperLink>

                        </ItemTemplate>

                    </asp:TemplateField>


                </Columns>


            </asp:GridView>


        </div>


    </div>


</asp:Content>