<%@ Page
    Title="Thêm danh mục"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_DanhMuc_Them.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_DanhMuc_Them"
%>


<asp:Content
    ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">


    <div class="admin-title">

        <h1>
            Thêm danh mục
        </h1>

    </div>


    <div class="dashboard-card">

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
                Text=" Đang hoạt động"
                Checked="true">
            </asp:CheckBox>

        </div>


        <br />


        <div>

            <asp:Button
                ID="btnLuu"
                runat="server"
                Text="Lưu danh mục"
                CssClass="btn btn-primary"
                OnClick="btnLuu_Click" />


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