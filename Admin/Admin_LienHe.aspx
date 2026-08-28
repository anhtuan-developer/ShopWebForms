<%@ Page
    Title="Quản lý liên hệ"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_LienHe.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_LienHe"
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

        <div class="d-flex
                    flex-column
                    flex-md-row
                    justify-content-between
                    align-items-md-center
                    gap-3
                    mb-4">

            <div>

                <h1 class="h3 fw-bold mb-1">
                    Quản lý liên hệ
                </h1>

                <p class="text-secondary mb-0">
                    Xem và quản lý tin nhắn khách hàng gửi đến cửa hàng.
                </p>

            </div>

        </div>


        <asp:Label
            ID="lblMessage"
            runat="server"
            EnableViewState="false">
        </asp:Label>


        <div class="card border-0 shadow-sm">

            <div class="card-header
                        bg-white
                        border-0
                        py-3">

                <h5 class="mb-0 fw-semibold">
                    Danh sách liên hệ
                </h5>

            </div>


            <div class="card-body p-0">

                <div class="table-responsive">

                    <asp:GridView
                        ID="gvLienHe"
                        runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-hover align-middle mb-0"
                        GridLines="None"
                        EmptyDataText="Chưa có tin nhắn liên hệ nào."
                        OnRowCommand="gvLienHe_RowCommand">

                        <Columns>

                            <asp:BoundField
                                DataField="MaLienHe"
                                HeaderText="Mã"
                                HeaderStyle-CssClass="table-light"
                                ItemStyle-CssClass="fw-semibold text-center"
                            />


                            <asp:BoundField
                                DataField="HoTen"
                                HeaderText="Họ tên"
                                HeaderStyle-CssClass="table-light"
                            />


                            <asp:BoundField
                                DataField="Email"
                                HeaderText="Email"
                                HeaderStyle-CssClass="table-light"
                            />


                            <asp:BoundField
                                DataField="TieuDe"
                                HeaderText="Chủ đề"
                                HeaderStyle-CssClass="table-light"
                            />


                            <asp:TemplateField
                                HeaderText="Nội dung">

                                <HeaderStyle
                                    CssClass="table-light" />

                                <ItemTemplate>

                                    <div
                                        class="text-truncate"
                                        style="max-width: 300px;"
                                        title='<%# Server.HtmlEncode(Convert.ToString(Eval("NoiDung"))) %>'>

                                        <%# Server.HtmlEncode(
                                            Convert.ToString(
                                                Eval("NoiDung"))) %>

                                    </div>

                                </ItemTemplate>

                            </asp:TemplateField>


                            <asp:BoundField
                                DataField="NgayGui"
                                HeaderText="Ngày gửi"
                                DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                HeaderStyle-CssClass="table-light"
                            />


                            <asp:TemplateField
                                HeaderText="Trạng thái">

                                <HeaderStyle
                                    CssClass="table-light" />

                                <ItemTemplate>

                                    <span
                                        class='<%#
                                            Convert.ToBoolean(
                                                Eval("TrangThai"))
                                                ? "badge text-bg-success"
                                                : "badge text-bg-warning"
                                        %>'>

                                        <%#
                                            Convert.ToBoolean(
                                                Eval("TrangThai"))
                                                ? "Đã xử lý"
                                                : "Chưa xử lý"
                                        %>

                                    </span>

                                </ItemTemplate>

                            </asp:TemplateField>


                            <asp:TemplateField
                                HeaderText="Thao tác">

                                <HeaderStyle
                                    CssClass="table-light" />

                                <ItemTemplate>

                                    <div class="d-flex flex-wrap gap-1">

                                        <asp:Button
                                            ID="btnStatus"
                                            runat="server"
                                            Text='<%#
                                                Convert.ToBoolean(
                                                    Eval("TrangThai"))
                                                    ? "Chưa xử lý"
                                                    : "Đã xử lý"
                                            %>'
                                            CssClass="btn btn-sm btn-outline-primary"
                                            CommandName="ToggleStatus"
                                            CommandArgument='<%#
                                                Eval("MaLienHe")
                                                + "|"
                                                + Eval("TrangThai")
                                            %>'
                                            CausesValidation="false" />


                                        <asp:Button
                                            ID="btnDelete"
                                            runat="server"
                                            Text="Xóa"
                                            CssClass="btn btn-sm btn-outline-danger"
                                            CommandName="DeleteContact"
                                            CommandArgument='<%#
                                                Eval("MaLienHe")
                                            %>'
                                            CausesValidation="false"
                                            OnClientClick="return confirm('Bạn có chắc chắn muốn xóa liên hệ này?');" />

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