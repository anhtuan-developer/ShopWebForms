<%@ Page
    Title="Quản lý sản phẩm"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_SanPham.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_SanPham"
%>


<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

    <style>

        .product-page-title {
            margin-bottom: 25px;
        }

        .product-page-title h1 {
            margin: 0;
            font-size: 28px;
        }

        .product-page-title p {
            margin-top: 8px;
            color: #777;
        }

        .product-toolbar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
        }

        .product-table-wrapper {
            background: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,.08);
            overflow-x: auto;
        }

        .product-table {
            width: 100%;
            border-collapse: collapse;
        }

        .product-table th {
            background: #343a40;
            color: #ffffff;
            padding: 12px;
            text-align: left;
            white-space: nowrap;
        }

        .product-table td {
            padding: 12px;
            border-bottom: 1px solid #ddd;
            vertical-align: middle;
        }

        .product-table tr:hover {
            background-color: #f8f9fa;
        }

        .product-image {
            width: 70px;
            height: 70px;
            object-fit: cover;
            border-radius: 6px;
            border: 1px solid #ddd;
        }

        .product-button {
            display: inline-block;
            padding: 7px 12px;
            border-radius: 4px;
            text-decoration: none;
            font-size: 14px;
            border: none;
            cursor: pointer;
        }

        .button-add {
            background: #007bff;
            color: white;
        }

        .button-add:hover {
            background: #0056b3;
            color: white;
        }

        .button-edit {
            background: #ffc107;
            color: #212529;
        }

        .button-edit:hover {
            background: #e0a800;
            color: #212529;
        }

        .button-delete {
            background: #dc3545;
            color: white;
        }

        .button-delete:hover {
            background: #bd2130;
            color: white;
        }

        .status-active {
            display: inline-block;
            background: #28a745;
            color: white;
            padding: 5px 9px;
            border-radius: 4px;
            font-size: 13px;
        }

        .status-inactive {
            display: inline-block;
            background: #6c757d;
            color: white;
            padding: 5px 9px;
            border-radius: 4px;
            font-size: 13px;
        }

        .empty-message {
            padding: 30px;
            text-align: center;
            color: #777;
        }
        .btn-xoa {
            background-color: #dc3545;
            color: white;
            border: none;
            padding: 6px 12px;
            border-radius: 4px;
            cursor: pointer;
        }
        
        .btn-xoa:hover {
            background-color: #bb2d3b;
        }

    </style>

</asp:Content>


<asp:Content
    ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">


    <!-- TIÊU ĐỀ -->

    <div class="product-page-title">

        <h1>
            Quản lý sản phẩm
        </h1>

        <p>
            Quản lý danh sách sản phẩm của cửa hàng
        </p>

    </div>


    <!-- TOOLBAR -->

    <div class="product-toolbar">

        <h2>
            Danh sách sản phẩm
        </h2>


        <asp:Button
            ID="btnThemSanPham"
            runat="server"
            Text="+ Thêm sản phẩm"
            CssClass="product-button button-add"
            OnClick="btnThemSanPham_Click" />

    </div>


    <!-- DANH SÁCH -->

    <div class="product-table-wrapper">


        <asp:GridView
            ID="gvSanPham"
            runat="server"

            AutoGenerateColumns="False"

            CssClass="product-table"

            GridLines="None"

            EmptyDataText="Chưa có sản phẩm nào."

            OnRowCommand="gvSanPham_RowCommand">


            <EmptyDataRowStyle
                CssClass="empty-message" />


            <Columns>



                <asp:BoundField
                    DataField="MaSanPham"
                    HeaderText="Mã" />



                <asp:TemplateField
                    HeaderText="Hình ảnh">

                    <ItemTemplate>

                        <asp:Image
                            ID="imgSanPham"
                            runat="server"

                            CssClass="product-image"

                            ImageUrl='<%# ResolveUrl("~/img/" + Eval("HinhAnh")) %>'

                            AlternateText='<%# Eval("TenSanPham") %>' />

                    </ItemTemplate>

                </asp:TemplateField>


                <asp:BoundField
                    DataField="TenSanPham"
                    HeaderText="Tên sản phẩm" />



                <asp:BoundField
                    DataField="TenDanhMuc"
                    HeaderText="Danh mục" />



                <asp:BoundField
                    DataField="Gia"
                    HeaderText="Giá"
                    DataFormatString="{0:N0} ₫"
                    HtmlEncode="false" />


                <asp:BoundField
                    DataField="SoLuong"
                    HeaderText="Số lượng" />



                <asp:TemplateField
                    HeaderText="Trạng thái">

                    <ItemTemplate>

                        <asp:Label
                            ID="lblTrangThai"
                            runat="server"

                            Text='<%# Convert.ToBoolean(Eval("TrangThai")) ? "Đang bán" : "Ngừng bán" %>'

                            CssClass='<%# Convert.ToBoolean(Eval("TrangThai")) ? "status-active" : "status-inactive" %>' />

                    </ItemTemplate>

                </asp:TemplateField>


                <asp:TemplateField
                    HeaderText="Thao tác">

                    <ItemTemplate>


                        <asp:HyperLink
                            ID="lnkSua"
                            runat="server"

                            Text="Sửa"

                            CssClass="product-button button-edit"

                            NavigateUrl='<%# "Admin_SanPham_Sua.aspx?id=" + Eval("MaSanPham") %>' />



                        <asp:Button
                            ID="btnXoa"
                            runat="server"
                            Text="Xóa"
                            CssClass="btn-xoa"
                            CommandName="DeleteProduct"
                            CommandArgument='<%# Eval("MaSanPham") %>'
                            CausesValidation="false"
                            OnClientClick="return confirm('Bạn có chắc chắn muốn xóa sản phẩm này?');" />


                    </ItemTemplate>

                </asp:TemplateField>


            </Columns>


        </asp:GridView>


    </div>


</asp:Content>