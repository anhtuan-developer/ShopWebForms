<%@ Page
    Title="Sửa danh mục"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_DanhMuc_Sua.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_DanhMuc_Sua"
%>


<asp:Content
    ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">


    <div class="admin-title">

        <h1>
            Sửa danh mục
        </h1>

    </div>


    <div class="dashboard-card">


        <div class="form-group">

            <label>
                Mã danh mục
            </label>

            <asp:TextBox
                ID="txtMaDanhMuc"
                runat="server"
                CssClass="form-control"
                ReadOnly="true">
            </asp:TextBox>

        </div>


        <br />


        <div class="form-group">

            <label>
                Tên danh mục
            </label>

            <asp:TextBox
                ID="txtTenDanhMuc"
                runat="server"
                CssClass="form-control"
                MaxLength="100">
            </asp:TextBox>

            <asp:RequiredFieldValidator
                ID="rfvTenDanhMuc"
                runat="server"
                ControlToValidate="txtTenDanhMuc"
                ErrorMessage="Vui lòng nhập tên danh mục."
                ForeColor="Red"
                Display="Dynamic">
            </asp:RequiredFieldValidator>

        </div>


        <br />


        <div class="form-group">

            <label>
                Mô tả
            </label>

            <asp:TextBox
                ID="txtMoTa"
                runat="server"
                CssClass="form-control"
                TextMode="MultiLine"
                Rows="5"
                MaxLength="500">
            </asp:TextBox>

        </div>


        <br />


        <div class="form-group">

            <label>
                Trạng thái
            </label>

            <br />

            <asp:CheckBox
                ID="chkTrangThai"
                runat="server"
                Text=" Đang hoạt động">
            </asp:CheckBox>

        </div>


        <br />


        <div>

            <asp:Button
                ID="btnCapNhat"
                runat="server"
                Text="Cập nhật"
                CssClass="btn btn-primary"
                OnClick="btnCapNhat_Click" />


            <asp:Button
                ID="btnHuy"
                runat="server"
                Text="Hủy"
                CssClass="btn btn-secondary"
                CausesValidation="false"
                OnClick="btnHuy_Click" />

        </div>


        <br />


        <asp:Label
            ID="lblMessage"
            runat="server">
        </asp:Label>


    </div>


</asp:Content>