<%@ Page
    Title="Quản lý tin tức"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_TinTuc.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_TinTuc"
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

        <div class="d-flex
                    flex-column
                    flex-md-row
                    justify-content-between
                    align-items-md-center
                    gap-3
                    mb-4">

            <div>

                <h1 class="h3 fw-bold mb-1">
                    Quản lý tin tức
                </h1>

                <p class="text-secondary mb-0">
                    Quản lý các bài viết trên website.
                </p>

            </div>


            <asp:HyperLink
                ID="lnkThem"
                runat="server"
                NavigateUrl="~/Admin/Admin_TinTuc_Them.aspx"
                CssClass="btn btn-primary">

                <span class="me-1">+</span>
                Thêm tin tức

            </asp:HyperLink>

        </div>


        <!-- THÔNG BÁO -->

        <asp:Label
            ID="lblMessage"
            runat="server"
            EnableViewState="false">
        </asp:Label>


        <!-- DANH SÁCH -->

        <div class="card border-0 shadow-sm">

            <div class="card-header
                        bg-white
                        border-0
                        py-3">

                <h5 class="mb-0 fw-semibold">
                    Danh sách bài viết
                </h5>

            </div>


            <div class="card-body p-0">

                <div class="table-responsive">

                    <asp:GridView
                        ID="gvTinTuc"
                        runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table
                                  table-hover
                                  align-middle
                                  mb-0"
                        GridLines="None"
                        EmptyDataText="Chưa có bài viết nào."
                        OnRowCommand="gvTinTuc_RowCommand">

                        <Columns>

                            <asp:BoundField
                                DataField="MaTinTuc"
                                HeaderText="Mã"
                                HeaderStyle-CssClass="table-light"
                                ItemStyle-CssClass="fw-semibold text-center"
                            />
                        
                            <asp:BoundField
                                DataField="TieuDe"
                                HeaderText="Tiêu đề"
                                HeaderStyle-CssClass="table-light"
                            />
                        
                            <asp:BoundField
                                DataField="NgayTao"
                                HeaderText="Ngày tạo"
                                DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                HeaderStyle-CssClass="table-light"
                            />
                        
                            <asp:TemplateField
                                HeaderText="Trạng thái">
                        
                                <HeaderStyle
                                    CssClass="table-light" />
                        
                                <ItemTemplate>
                        
                                    <span
                                        class='<%# Convert.ToBoolean(Eval("TrangThai"))
                                            ? "badge text-bg-success"
                                            : "badge text-bg-secondary" %>'>
                        
                                        <%# Convert.ToBoolean(Eval("TrangThai"))
                                            ? "Đang hiển thị"
                                            : "Đang ẩn" %>
                        
                                    </span>
                        
                                </ItemTemplate>
                        
                            </asp:TemplateField>
                        
                        
                            <asp:TemplateField
                                HeaderText="Thao tác">
                        
                                <HeaderStyle
                                    CssClass="table-light" />
                        
                                <ItemTemplate>
                        
                                    <div class="d-flex flex-wrap gap-1">
                        
                                        <a
                                            href='<%#
                                                "Admin_TinTuc_Sua.aspx?id="
                                                + Eval("MaTinTuc")
                                            %>'
                                            class="btn btn-sm btn-outline-primary">
                        
                                            Sửa
                        
                                        </a>
                        
                        
                                        <asp:Button
                                            ID="btnStatus"
                                            runat="server"
                                            Text='<%#
                                                Convert.ToBoolean(Eval("TrangThai"))
                                                    ? "Ẩn"
                                                    : "Hiện"
                                            %>'
                                            CssClass="btn btn-sm btn-outline-warning"
                                            CommandName="ToggleStatus"
                                            CommandArgument='<%#
                                                Eval("MaTinTuc")
                                                + "|"
                                                + Eval("TrangThai")
                                            %>'
                                            CausesValidation="false" />
                        
                        
                                        <asp:Button
                                            ID="btnDelete"
                                            runat="server"
                                            Text="Xóa"
                                            CssClass="btn btn-sm btn-outline-danger"
                                            CommandName="DeleteNews"
                                            CommandArgument='<%#
                                                Eval("MaTinTuc")
                                            %>'
                                            CausesValidation="false"
                                            OnClientClick="return confirm('Bạn có chắc chắn muốn xóa bài viết này? Bình luận của bài viết cũng sẽ bị xóa.');" />
                        
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