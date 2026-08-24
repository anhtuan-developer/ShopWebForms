<%@ Page
    Title="Quản lý khách hàng"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_KhachHang.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_KhachHang"
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


    <!-- ========================================== -->
    <!-- TIÊU ĐỀ -->
    <!-- ========================================== -->

    <div class="admin-title">

        <h1>
            Quản lý khách hàng
        </h1>

        <p>
            Quản lý danh sách khách hàng của cửa hàng
        </p>

    </div>


    <!-- ========================================== -->
    <!-- THỐNG KÊ -->
    <!-- ========================================== -->

    <div class="order-statistics">

        <div class="stat-box">

            <div class="stat-title">
                Tổng khách hàng
            </div>

            <div class="stat-value">

                <asp:Label
                    ID="lblTongKhachHang"
                    runat="server"
                    Text="0">
                </asp:Label>

            </div>

        </div>

    </div>


    <!-- ========================================== -->
    <!-- DANH SÁCH KHÁCH HÀNG -->
    <!-- ========================================== -->

    <div class="dashboard-card">


        <h3>
            Danh sách khách hàng
        </h3>


        <div class="table-container">


            <asp:GridView
                ID="gvKhachHang"
                runat="server"

                AutoGenerateColumns="False"

                CssClass="admin-table"

                GridLines="None"

                EmptyDataText="Chưa có khách hàng nào."

                OnRowCommand="gvKhachHang_RowCommand">


                <Columns>



                    <asp:BoundField
                        DataField="MaKhachHang"
                        HeaderText="Mã"
                    />



                    <asp:BoundField
                        DataField="HoTen"
                        HeaderText="Họ tên"
                    />



                    <asp:BoundField
                        DataField="Email"
                        HeaderText="Email"
                    />


                    <asp:BoundField
                        DataField="SoDienThoai"
                        HeaderText="Số điện thoại"
                    />



                    <asp:BoundField
                        DataField="DiaChi"
                        HeaderText="Địa chỉ"
                    />


                    <asp:BoundField
                        DataField="NgayTao"
                        HeaderText="Ngày đăng ký"
                        DataFormatString="{0:dd/MM/yyyy HH:mm}"
                    />



                    <asp:TemplateField
                        HeaderText="Thao tác">

                        <ItemTemplate>


                            <asp:Button
                                ID="btnXoa"
                                runat="server"

                                Text="Xóa"

                                CssClass="btn-delete"

                                CommandName="DeleteCustomer"

                                CommandArgument='<%#
                                    Eval("MaKhachHang")
                                %>'

                                CausesValidation="false"

                                OnClientClick="
                                    return confirm(
                                        'Bạn có chắc chắn muốn xóa khách hàng này?'
                                    );
                                "
                            />


                        </ItemTemplate>

                    </asp:TemplateField>


                </Columns>


            </asp:GridView>


        </div>


    </div>


</asp:Content>