<%@ Page
    Title="Sửa sản phẩm"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_SanPham_Sua.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_SanPham_Sua"
%>

<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

    <style>

        .product-form {
            max-width: 900px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .form-label {
            display: block;
            margin-bottom: 7px;
            font-weight: 600;
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
            border-color: #0d6efd;
            box-shadow: 0 0 0 2px rgba(13,110,253,.15);
        }

        textarea.form-control-custom {
            resize: vertical;
        }

        .form-row {
            display: flex;
            gap: 20px;
        }

        .form-col {
            flex: 1;
        }

        .image-preview {
            margin-top: 10px;
            width: 150px;
            height: 150px;
            object-fit: cover;
            border: 1px solid #ddd;
            border-radius: 6px;
            display: block;
        }

        .validation-error {
            display: block;
            margin-top: 5px;
            color: #dc3545;
            font-size: 14px;
        }

        .form-actions {
            margin-top: 25px;
            padding-top: 20px;
            border-top: 1px solid #eee;
        }

        .btn-custom {
            display: inline-block;
            padding: 9px 18px;
            border: none;
            border-radius: 5px;
            text-decoration: none;
            cursor: pointer;
            font-size: 15px;
            margin-right: 8px;
        }

        .btn-save {
            background-color: #0d6efd;
            color: white;
        }

        .btn-save:hover {
            background-color: #0b5ed7;
        }

        .btn-cancel {
            background-color: #6c757d;
            color: white;
        }

        .btn-cancel:hover {
            background-color: #5c636a;
        }

        .status-box {
            display: flex;
            align-items: center;
            gap: 8px;
        }

    </style>

</asp:Content>


<asp:Content
    ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">


    <!-- TIÊU ĐỀ -->

    <div class="admin-title">

        <h1>
            Sửa sản phẩm
        </h1>

        <p>
            Cập nhật thông tin sản phẩm
        </p>

    </div>


    <!-- FORM -->

    <div class="dashboard-card product-form">


        <!-- TÊN SẢN PHẨM -->

        <div class="form-group">

            <label
                class="form-label"
                for="<%= txtTenSanPham.ClientID %>">

                Tên sản phẩm

            </label>

            <asp:TextBox
                ID="txtTenSanPham"
                runat="server"
                CssClass="form-control-custom">
            </asp:TextBox>

            <asp:RequiredFieldValidator
                ID="rfvTenSanPham"
                runat="server"
                ControlToValidate="txtTenSanPham"
                ErrorMessage="Vui lòng nhập tên sản phẩm."
                CssClass="validation-error"
                Display="Dynamic">
            </asp:RequiredFieldValidator>

        </div>


        <!-- DANH MỤC -->

        <div class="form-group">

            <label
                class="form-label"
                for="<%= ddlDanhMuc.ClientID %>">

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
                InitialValue="0"
                ErrorMessage="Vui lòng chọn danh mục."
                CssClass="validation-error"
                Display="Dynamic">
            </asp:RequiredFieldValidator>

        </div>


        <!-- MÔ TẢ -->

        <div class="form-group">

            <label
                class="form-label"
                for="<%= txtMoTa.ClientID %>">

                Mô tả

            </label>

            <asp:TextBox
                ID="txtMoTa"
                runat="server"
                CssClass="form-control-custom"
                TextMode="MultiLine"
                Rows="6">
            </asp:TextBox>

        </div>


        <!-- GIÁ + SỐ LƯỢNG -->

        <div class="form-row">


            <div class="form-col">

                <div class="form-group">

                    <label
                        class="form-label"
                        for="<%= txtGia.ClientID %>">

                        Giá

                    </label>

                    <asp:TextBox
                        ID="txtGia"
                        runat="server"
                        CssClass="form-control-custom">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvGia"
                        runat="server"
                        ControlToValidate="txtGia"
                        ErrorMessage="Vui lòng nhập giá."
                        CssClass="validation-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                </div>

            </div>


            <div class="form-col">

                <div class="form-group">

                    <label
                        class="form-label"
                        for="<%= txtSoLuong.ClientID %>">

                        Số lượng

                    </label>

                    <asp:TextBox
                        ID="txtSoLuong"
                        runat="server"
                        CssClass="form-control-custom">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvSoLuong"
                        runat="server"
                        ControlToValidate="txtSoLuong"
                        ErrorMessage="Vui lòng nhập số lượng."
                        CssClass="validation-error"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                </div>

            </div>


        </div>


        <!-- HÌNH ẢNH -->

        <div class="form-group">

            <label
                class="form-label"
                for="<%= txtHinhAnh.ClientID %>">

                Hình ảnh

            </label>

            <asp:TextBox
                ID="txtHinhAnh"
                runat="server"
                CssClass="form-control-custom">
            </asp:TextBox>

            <asp:Image
                ID="imgSanPham"
                runat="server"
                CssClass="image-preview"
                AlternateText="Hình ảnh sản phẩm" />

        </div>


        <!-- TRẠNG THÁI -->

        <div class="form-group">

            <div class="status-box">

            <asp:CheckBox
                ID="chkTrangThai"
                runat="server" />
        
            <label
                for="<%= chkTrangThai.ClientID %>">
        
                Đang bán
        
            </label>
        
        
            <asp:CheckBox
                ID="chkNoiBat"
                runat="server"
                Text=" ⭐ Sản phẩm nổi bật"
                CssClass="ms-3" />
        
        </div>

        </div>


        <!-- BUTTON -->

        <div class="form-actions">

            <asp:Button
                ID="btnCapNhat"
                runat="server"
                Text="Cập nhật sản phẩm"
                CssClass="btn-custom btn-save"
                OnClick="btnCapNhat_Click" />

            <asp:Button
                ID="btnHuy"
                runat="server"
                Text="Hủy"
                CssClass="btn-custom btn-cancel"
                CausesValidation="false"
                OnClick="btnHuy_Click" />

        </div>


    </div>


</asp:Content>