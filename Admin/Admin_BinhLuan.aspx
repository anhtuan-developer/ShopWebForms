<%@ Page
    Title="Quản lý bình luận"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_BinhLuan.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_BinhLuan" %>


<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">
</asp:Content>


<asp:Content
    ID="MainContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="admin-title">

        <h1>
            Quản lý bình luận
        </h1>

        <p>
            Kiểm duyệt bình luận của khách hàng
            trên các bài viết.
        </p>

    </div>


    <div class="dashboard-card">

        <div class="table-container">

            <asp:GridView
                ID="gvBinhLuan"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="admin-table"
                GridLines="None"
                EmptyDataText="Chưa có bình luận nào."
                OnRowCommand="gvBinhLuan_RowCommand">

                <Columns>

                    <asp:BoundField
                        DataField="MaBinhLuan"
                        HeaderText="Mã" />


                    <asp:BoundField
                        DataField="TieuDe"
                        HeaderText="Bài viết" />


                    <asp:BoundField
                        DataField="HoTen"
                        HeaderText="Khách hàng" />


                    <asp:BoundField
                        DataField="Email"
                        HeaderText="Email" />


                    <asp:BoundField
                        DataField="NoiDung"
                        HeaderText="Nội dung" />


                    <asp:BoundField
                        DataField="NgayBinhLuan"
                        HeaderText="Ngày"
                        DataFormatString="{0:dd/MM/yyyy HH:mm}" />


                    <asp:TemplateField
                        HeaderText="Trạng thái">

                        <ItemTemplate>

                            <%#
                                Convert.ToBoolean(
                                    Eval("TrangThai")
                                )
                                ? "Hiển thị"
                                : "Ẩn"
                            %>

                        </ItemTemplate>

                    </asp:TemplateField>


                    <asp:TemplateField
                        HeaderText="Thao tác">

                        <ItemTemplate>

                            <asp:Button
                                ID="btnTrangThai"
                                runat="server"
                                Text='<%#
                                    Convert.ToBoolean(
                                        Eval("TrangThai")
                                    )
                                    ? "Ẩn"
                                    : "Hiện"
                                %>'
                                CssClass="btn btn-sm btn-warning"
                                CommandName="ToggleStatus"
                                CommandArgument='<%#
                                    Eval("MaBinhLuan")
                                    + "|"
                                    + Eval("TrangThai")
                                %>'
                                CausesValidation="false" />


                            <asp:Button
                                ID="btnXoa"
                                runat="server"
                                Text="Xóa"
                                CssClass="btn-delete"
                                CommandName="DeleteComment"
                                CommandArgument='<%#
                                    Eval("MaBinhLuan")
                                %>'
                                CausesValidation="false"
                                OnClientClick="return confirm('Bạn có chắc chắn muốn xóa bình luận này?');" />

                        </ItemTemplate>

                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

        </div>

    </div>

</asp:Content>