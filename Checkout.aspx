<%@ Page
    Title="Thanh toán"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Checkout.aspx.cs"
    Inherits="web_ban_hang2.Checkout"
%>

<asp:Content
    ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="checkout-page">

        <!-- TIÊU ĐỀ -->
        <h2>Thanh toán đơn hàng</h2>

        <hr />

        <!-- THÔNG TIN NGƯỜI NHẬN -->
        <h3>Thông tin nhận hàng</h3>

        <div class="checkout-form">

            <!-- HỌ TÊN -->
            <div class="form-group">

                <label for="txtHoTen">
                    Họ tên người nhận
                </label>

                <asp:TextBox
                    ID="txtHoTen"
                    runat="server"
                    CssClass="form-control"
                    placeholder="Nhập họ tên người nhận">
                </asp:TextBox>

            </div>


            <!-- SỐ ĐIỆN THOẠI -->
            <div class="form-group">

                <label for="txtSoDienThoai">
                    Số điện thoại
                </label>

                <asp:TextBox
                    ID="txtSoDienThoai"
                    runat="server"
                    CssClass="form-control"
                    placeholder="Nhập số điện thoại">
                </asp:TextBox>

            </div>


            <!-- ĐỊA CHỈ -->
            <div class="form-group">

                <label for="txtDiaChi">
                    Địa chỉ giao hàng
                </label>

                <asp:TextBox
                    ID="txtDiaChi"
                    runat="server"
                    CssClass="form-control"
                    TextMode="MultiLine"
                    Rows="4"
                    placeholder="Nhập địa chỉ nhận hàng">
                </asp:TextBox>

            </div>

        </div>


        <hr />


        <!-- TỔNG TIỀN -->
        <div class="checkout-total">

            <asp:Label
                ID="lblTongTien"
                runat="server"
                Text="Tổng tiền: 0 VNĐ">
            </asp:Label>

        </div>


        <br />


        <!-- NÚT ĐẶT HÀNG -->
        <div class="checkout-action">

            <asp:Button
                ID="btnDatHang"
                runat="server"
                Text="Đặt hàng"
                CssClass="btn btn-primary"
                OnClick="btnDatHang_Click">
            </asp:Button>

        </div>


        <br />


        <!-- THÔNG BÁO -->
        <div class="checkout-message">

            <asp:Label
                ID="lblMessage"
                runat="server">
            </asp:Label>

        </div>

    </div>

</asp:Content>