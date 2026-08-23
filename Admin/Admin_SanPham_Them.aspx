<%@ Page
    Title="Thêm sản phẩm"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_SanPham_Them.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_SanPham_Them"
%>


<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

    <style>

        .product-form-title {
            margin-bottom: 25px;
        }

        .product-form-title h1 {
            margin: 0;
            font-size: 28px;
        }

        .product-form-title p {
            margin-top: 8px;
            color: #777;
        }

        .product-form-card {
            background-color: #ffffff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,.08);
            max-width: 900px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .form-group label {
            display: block;
            font-weight: 600;
            margin-bottom: 7px;
            color: #333;
        }

        .form-control-custom {
            width: 100%;
            padding: 10px 12px;
            border: 1px solid #ced4da;
            border-radius: 5px;
            font-size: 15px;
            box-sizing: border-box;
        }

        .form-control-custom:focus {
            outline: none;
            border-color: #80bdff;
            box-shadow: 0 0 0 2px rgba(0,123,255,.15);
        }

        textarea.form-control-custom {
            min-height: 120px;
            resize: vertical;
        }

        .validation-message {
            display: block;
            margin-top: 5px;
            color: #dc3545;
            font-size: 14px;
        }

        .form-row {
            display: flex;
            gap: 20px;
        }

        .form-column {
            flex: 1;
        }

        .form-actions {
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid #ddd;
        }

        .admin-button {
            display: inline-block;
            padding: 9px 16px;
            border-radius: 5px;
            border: none;
            text-decoration: none;
            font-size: 15px;
            cursor: pointer;
        }

        .button-save {
            background-color: #007bff;
            color: white;
        }

        .button-save:hover {
            background-color: #0056b3;
            color: white;
        }

        .button-cancel {
            background-color: #6c757d;
            color: white;
            margin-left: 8px;
        }

        .button-cancel:hover {
            background-color: #545b62;
            color: white;
        }

        .image-note {
            color: #777;
            font-size: 13px;
            margin-top: 5px;
        }

        @media (max-width: 700px) {

            .form-row {
                flex-direction: column;
                gap: 0;
            }

            .product-form-card {
                padding: 20px;
            }

        }

    </style>

</asp:Content>


<asp:Content
    ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">


    <!-- TIÊU ĐỀ -->

    <div class="product-form-title">

        <h1>
            Thêm sản phẩm
        </h1>

        <p>
            Thêm sản phẩm mới vào cửa hàng
        </p>

    </div>


    <!-- FORM -->

    <div class="product-form-card">


        <!-- TÊN SẢN PHẨM -->

        <div class="form-group">

            <label for="<%= txtTenSanPham.ClientID %>">
                Tên sản phẩm
            </label>

            <asp:TextBox
                ID="txtTenSanPham"
                runat="server"
                CssClass="form-control-custom"
                MaxLength="200"
                placeholder="Nhập tên sản phẩm..." />

            <asp:RequiredFieldValidator
                ID="rfvTenSanPham"
                runat="server"
                ControlToValidate="txtTenSanPham"
                ErrorMessage="Vui lòng nhập tên sản phẩm."
                CssClass="validation-message"
                Display="Dynamic" />

        </div>


        <!-- DANH MỤC -->

        <div class="form-group">

            <label for="<%= ddlDanhMuc.ClientID %>">
                Danh mục
            </label>

            <asp:DropDownList
                ID="ddlDanhMuc"
                runat="server"
                CssClass="form-control-custom">

            </asp:DropDownList>

            <asp:RequiredFieldValidator
                ID="rfvDanhMuc"
                runat="server"
                ControlToValidate="ddlDanhMuc"
                InitialValue=""
                ErrorMessage="Vui lòng chọn danh mục."
                CssClass="validation-message"
                Display="Dynamic" />

        </div>


        <!-- MÔ TẢ -->

        <div class="form-group">

            <label for="<%= txtMoTa.ClientID %>">
                Mô tả
            </label>

            <asp:TextBox
                ID="txtMoTa"
                runat="server"
                CssClass="form-control-custom"
                TextMode="MultiLine"
                MaxLength="500"
                placeholder="Nhập mô tả sản phẩm..." />

        </div>


        <!-- GIÁ + SỐ LƯỢNG -->

        <div class="form-row">


            <!-- GIÁ -->

            <div class="form-column">

                <div class="form-group">

                    <label for="<%= txtGia.ClientID %>">
                        Giá
                    </label>

                    <asp:TextBox
                        ID="txtGia"
                        runat="server"
                        CssClass="form-control-custom"
                        placeholder="Ví dụ: 24990000" />

                    <asp:RequiredFieldValidator
                        ID="rfvGia"
                        runat="server"
                        ControlToValidate="txtGia"
                        ErrorMessage="Vui lòng nhập giá."
                        CssClass="validation-message"
                        Display="Dynamic" />

                    <asp:RegularExpressionValidator
                        ID="revGia"
                        runat="server"
                        ControlToValidate="txtGia"
                        ValidationExpression="^\d+([.,]\d{1,2})?$"
                        ErrorMessage="Giá không hợp lệ."
                        CssClass="validation-message"
                        Display="Dynamic" />

                </div>

            </div>


            <!-- SỐ LƯỢNG -->

            <div class="form-column">

                <div class="form-group">

                    <label for="<%= txtSoLuong.ClientID %>">
                        Số lượng
                    </label>

                    <asp:TextBox
                        ID="txtSoLuong"
                        runat="server"
                        CssClass="form-control-custom"
                        placeholder="Ví dụ: 20" />

                    <asp:RequiredFieldValidator
                        ID="rfvSoLuong"
                        runat="server"
                        ControlToValidate="txtSoLuong"
                        ErrorMessage="Vui lòng nhập số lượng."
                        CssClass="validation-message"
                        Display="Dynamic" />

                    <asp:RegularExpressionValidator
                        ID="revSoLuong"
                        runat="server"
                        ControlToValidate="txtSoLuong"
                        ValidationExpression="^\d+$"
                        ErrorMessage="Số lượng phải là số nguyên."
                        CssClass="validation-message"
                        Display="Dynamic" />

                </div>

            </div>


        </div>


        <!-- HÌNH ẢNH -->

        <div class="form-group">

            <label for="<%= txtHinhAnh.ClientID %>">
                Hình ảnh
            </label>

            <asp:TextBox
                ID="txtHinhAnh"
                runat="server"
                CssClass="form-control-custom"
                MaxLength="500"
                placeholder="Ví dụ: iphone-15-pro.jpg" />

            <div class="image-note">

                Nhập tên file hình ảnh nằm trong thư mục img.

            </div>

        </div>


        <!-- TRẠNG THÁI -->

        <div class="form-group">

            <label for="<%= ddlTrangThai.ClientID %>">
                Trạng thái
            </label>

            <asp:DropDownList
                ID="ddlTrangThai"
                runat="server"
                CssClass="form-control-custom">

                <asp:ListItem
                    Text="Đang bán"
                    Value="true" />

                <asp:ListItem
                    Text="Ngừng bán"
                    Value="false" />

            </asp:DropDownList>

        </div>


        <!-- BUTTON -->

        <div class="form-actions">

            <asp:Button
                ID="btnLuu"
                runat="server"
                Text="Lưu sản phẩm"
                CssClass="admin-button button-save"
                OnClick="btnLuu_Click" />


            <asp:Button
                ID="btnHuy"
                runat="server"
                Text="Hủy"
                CssClass="admin-button button-cancel"
                CausesValidation="false"
                OnClick="btnHuy_Click" />

        </div>


    </div>


</asp:Content>