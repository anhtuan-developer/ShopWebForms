<%@ Page
    Title="Quản lý đánh giá"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_DanhGia.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_DanhGia"
%>

<asp:Content
    ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="admin-page">

        <h1>Quản lý đánh giá</h1>

        <asp:GridView
            ID="gvDanhGia"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-bordered table-hover"
            OnRowCommand="gvDanhGia_RowCommand">

            <Columns>

                <asp:BoundField
                    DataField="MaDanhGia"
                    HeaderText="Mã" />

                <asp:BoundField
                    DataField="TenSanPham"
                    HeaderText="Sản phẩm" />

                <asp:BoundField
                    DataField="HoTen"
                    HeaderText="Khách hàng" />

                <asp:BoundField
                    DataField="SoSao"
                    HeaderText="Số sao" />

                <asp:BoundField
                    DataField="NoiDung"
                    HeaderText="Nội dung" />

                <asp:BoundField
                    DataField="NgayDanhGia"
                    HeaderText="Ngày đánh giá"
                    DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                <asp:CheckBoxField
                    DataField="TrangThai"
                    HeaderText="Hiển thị" />

                <asp:TemplateField
                    HeaderText="Thao tác">

                    <ItemTemplate>

                        <asp:LinkButton
                            ID="btnToggle"
                            runat="server"
                            CommandName="ToggleStatus"
                            CommandArgument='<%# Eval("MaDanhGia") %>'
                            CssClass="btn btn-sm btn-warning">

                            Ẩn/Hiện

                        </asp:LinkButton>

                    </ItemTemplate>

                </asp:TemplateField>

            </Columns>

        </asp:GridView>

    </div>

</asp:Content>