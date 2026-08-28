<%@ Page
    Title="Sửa tin tức"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_TinTuc_Sua.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_TinTuc_Sua"
%>


<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

</asp:Content>


<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container-fluid px-0">

        <!-- TIÊU ĐỀ -->

        <div class="mb-4">

            <h1 class="h3 fw-bold mb-1">
                Sửa tin tức
            </h1>

            <p class="text-secondary mb-0">
                Cập nhật nội dung bài viết.
            </p>

        </div>


        <!-- LỖI -->

        <asp:Panel
            ID="pnlError"
            runat="server"
            Visible="false"
            CssClass="alert alert-danger">

            <asp:Label
                ID="lblError"
                runat="server">
            </asp:Label>

        </asp:Panel>


        <!-- FORM -->

        <div class="card border-0 shadow-sm">

            <div class="card-header
                        bg-white
                        py-3">

                <h5 class="mb-0 fw-semibold">
                    Thông tin bài viết
                </h5>

            </div>


            <div class="card-body">

                <!-- TIÊU ĐỀ -->

                <div class="mb-4">

                    <label
                        for="<%= txtTieuDe.ClientID %>"
                        class="form-label fw-semibold">

                        Tiêu đề

                    </label>


                    <asp:TextBox
                        ID="txtTieuDe"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="250">
                    </asp:TextBox>


                    <asp:RequiredFieldValidator
                        ID="rfvTieuDe"
                        runat="server"
                        ControlToValidate="txtTieuDe"
                        ErrorMessage="Vui lòng nhập tiêu đề."
                        CssClass="text-danger small"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                </div>


                <!-- NỘI DUNG -->

                <div class="mb-4">

                    <label
                        for="<%= txtNoiDung.ClientID %>"
                        class="form-label fw-semibold">

                        Nội dung

                    </label>


                    <asp:TextBox
                        ID="txtNoiDung"
                        runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="14">
                    </asp:TextBox>


                    <asp:RequiredFieldValidator
                        ID="rfvNoiDung"
                        runat="server"
                        ControlToValidate="txtNoiDung"
                        ErrorMessage="Vui lòng nhập nội dung."
                        CssClass="text-danger small"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                </div>


                <!-- HÌNH ẢNH -->

                <div class="mb-4">

                    <label
                        for="<%= txtHinhAnh.ClientID %>"
                        class="form-label fw-semibold">

                        Hình ảnh

                    </label>


                    <asp:TextBox
                        ID="txtHinhAnh"
                        runat="server"
                        CssClass="form-control"
                        MaxLength="500">
                    </asp:TextBox>

                </div>


                <!-- TRẠNG THÁI -->

                <div class="mb-4">

                    <div class="form-check form-switch">

                        <asp:CheckBox
                            ID="chkTrangThai"
                            runat="server"
                            CssClass="form-check-input" />


                        <label
                            class="form-check-label"
                            for="<%= chkTrangThai.ClientID %>">

                            Hiển thị bài viết

                        </label>

                    </div>

                </div>


                <!-- BUTTON -->

                <div class="d-flex
                            flex-wrap
                            gap-2">

                    <asp:Button
                        ID="btnLuu"
                        runat="server"
                        Text="Lưu thay đổi"
                        CssClass="btn btn-primary px-4"
                        OnClick="btnLuu_Click" />


                    <asp:HyperLink
                        ID="lnkHuy"
                        runat="server"
                        NavigateUrl="~/Admin/Admin_TinTuc.aspx"
                        CssClass="btn btn-outline-secondary px-4">

                        Hủy

                    </asp:HyperLink>

                </div>

            </div>

        </div>

    </div>

</asp:Content>