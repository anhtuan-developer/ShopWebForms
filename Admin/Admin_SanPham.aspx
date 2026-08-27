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

</asp:Content>


<asp:Content
    ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">


    <div class="container-fluid py-3">


        <!-- TIÊU ĐỀ -->

        <div class="mb-4">

            <h1 class="h3 fw-bold text-dark mb-2">
                Quản lý sản phẩm
            </h1>

            <p class="text-secondary mb-0">
                Quản lý danh sách sản phẩm của cửa hàng
            </p>

        </div>


        <!-- THANH CÔNG CỤ -->

        <div class="d-flex flex-column flex-md-row
                    justify-content-between
                    align-items-md-center
                    gap-3
                    mb-3">

            <h2 class="h5 fw-semibold text-dark mb-0">
                Danh sách sản phẩm
            </h2>


            <asp:Button
                ID="btnThemSanPham"
                runat="server"
                Text="+ Thêm sản phẩm"
                CssClass="btn btn-primary px-4 py-2 fw-semibold"
                OnClick="btnThemSanPham_Click" />

        </div>


        <!-- DANH SÁCH SẢN PHẨM -->

        <div class="card border-0 shadow-sm">

            <div class="card-body p-0">

                <div class="table-responsive">


                    <asp:GridView
                        ID="gvSanPham"
                        runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-hover align-middle mb-0"
                        GridLines="None"
                        EmptyDataText="Chưa có sản phẩm nào."
                        OnRowCommand="gvSanPham_RowCommand">

                        <EmptyDataRowStyle
                            CssClass="text-center text-secondary py-5" />


                        <Columns>


                            <asp:BoundField
                                DataField="MaSanPham"
                                HeaderText="Mã"
                                HeaderStyle-CssClass="table-dark"
                                ItemStyle-CssClass="fw-semibold text-secondary" />


                            <asp:TemplateField
                                HeaderText="Hình ảnh"
                                HeaderStyle-CssClass="table-dark">

                                <ItemTemplate>

                                    <asp:Image
                                        ID="imgSanPham"
                                        runat="server"
                                        CssClass="rounded border"
                                        Style="width:70px;height:70px;object-fit:contain;"
                                        ImageUrl='<%# ResolveUrl("~/img/" + Eval("HinhAnh")) %>'
                                        AlternateText='<%# Eval("TenSanPham") %>' />

                                </ItemTemplate>

                            </asp:TemplateField>


                            <asp:BoundField
                                DataField="TenSanPham"
                                HeaderText="Tên sản phẩm"
                                HeaderStyle-CssClass="table-dark"
                                ItemStyle-CssClass="fw-semibold text-dark" />


                            <asp:BoundField
                                DataField="TenDanhMuc"
                                HeaderText="Danh mục"
                                HeaderStyle-CssClass="table-dark" />


                            <asp:BoundField
                                DataField="Gia"
                                HeaderText="Giá"
                                DataFormatString="{0:N0} ₫"
                                HtmlEncode="false"
                                HeaderStyle-CssClass="table-dark"
                                ItemStyle-CssClass="fw-bold text-danger text-nowrap" />


                            <asp:BoundField
                                DataField="SoLuong"
                                HeaderText="Số lượng"
                                HeaderStyle-CssClass="table-dark"
                                ItemStyle-CssClass="text-center fw-semibold" />


                            <asp:TemplateField
                                HeaderText="Trạng thái"
                                HeaderStyle-CssClass="table-dark">

                                <ItemTemplate>

                                    <asp:Label
                                        ID="lblTrangThai"
                                        runat="server"
                                        Text='<%# Convert.ToBoolean(Eval("TrangThai")) ? "Đang bán" : "Ngừng bán" %>'
                                        CssClass='<%# Convert.ToBoolean(Eval("TrangThai")) ? "badge text-bg-success" : "badge text-bg-secondary" %>' />

                                </ItemTemplate>

                            </asp:TemplateField>


                            <asp:TemplateField
                                HeaderText="Thao tác"
                                HeaderStyle-CssClass="table-dark">

                                <ItemTemplate>

                                    <div class="d-flex flex-wrap gap-2">


                                        <asp:HyperLink
                                            ID="lnkSua"
                                            runat="server"
                                            Text="Sửa"
                                            CssClass="btn btn-warning btn-sm fw-semibold"
                                            NavigateUrl='<%# "Admin_SanPham_Sua.aspx?id=" + Eval("MaSanPham") %>' />


                                        <asp:Button
                                            ID="btnXoa"
                                            runat="server"
                                            Text="Xóa"
                                            CssClass="btn btn-danger btn-sm fw-semibold"
                                            CommandName="DeleteProduct"
                                            CommandArgument='<%# Eval("MaSanPham") %>'
                                            CausesValidation="false"
                                            OnClientClick="return confirm('Bạn có chắc chắn muốn xóa sản phẩm này?');" />

                                    </div>

                                </ItemTemplate>

                            </asp:TemplateField>


                        </Columns>

                    </asp:GridView>


                </div>

            </div>

        </div>


    </div>


</asp:Content>

