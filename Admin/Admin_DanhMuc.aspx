<%@ Page
    Title="Quản lý danh mục"
    Language="C#"
    MasterPageFile="~/Admin/Admin_Master.master"
    AutoEventWireup="true"
    CodeBehind="Admin_DanhMuc.aspx.cs"
    Inherits="web_ban_hang2.Admin.Admin_DanhMuc"
%>


<asp:Content
    ID="HeadContent"
    ContentPlaceHolderID="HeadContent"
    runat="server">

    <style>

        /* =========================================
           MODAL OVERLAY
        ========================================= */

        .admin-modal-overlay {

            position: fixed;

            top: 0;
            left: 0;

            width: 100%;
            height: 100%;

            background: rgba(0, 0, 0, 0.55);

            display: flex;

            align-items: center;
            justify-content: center;

            z-index: 9999;

        }


        /* =========================================
           MODAL
        ========================================= */

        .admin-modal {

            width: 420px;

            max-width: 90%;

            background: white;

            border-radius: 12px;

            padding: 30px;

            text-align: center;

            box-shadow:
                0 10px 40px rgba(0, 0, 0, 0.25);

            animation: modalShow 0.2s ease;

        }


        /* =========================================
           ICON
        ========================================= */

        .admin-modal-icon {

            font-size: 45px;

            margin-bottom: 15px;

        }


        /* =========================================
           TITLE
        ========================================= */

        .admin-modal-title {

            font-size: 22px;

            font-weight: 600;

            margin-bottom: 12px;

        }


        /* =========================================
           MESSAGE
        ========================================= */

        .admin-modal-message {

            color: #555;

            font-size: 16px;

            line-height: 1.6;

            margin-bottom: 25px;

        }


        /* =========================================
           BUTTON
        ========================================= */
        .admin-modal-button {
           border: none;

           padding: 10px 25px;

           border-radius: 6px;

           cursor: pointer;

           font-size: 15px;

           background: #6c757d;

           color: white;

           transition: 0.2s;
        }


        .admin-modal-button:hover {
            background: #007bff;
        }


        /* =========================================
           ANIMATION
        ========================================= */

        @keyframes modalShow {

            from {

                opacity: 0;

                transform: scale(0.9);

            }

            to {

                opacity: 1;

                transform: scale(1);

            }

        }

    </style>

</asp:Content>


<asp:Content
    ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">


    <!-- =========================================
         TIÊU ĐỀ
    ========================================= -->

    <div class="admin-title">

        <h1>
            Quản lý danh mục
        </h1>

    </div>


    <!-- =========================================
         DANH SÁCH DANH MỤC
    ========================================= -->

    <div class="dashboard-card">


        <div style="
            display:flex;
            justify-content:space-between;
            align-items:center;
            margin-bottom:20px;">


            <h3>
                Danh sách danh mục
            </h3>


            <asp:Button
                ID="btnThemDanhMuc"
                runat="server"
                Text="+ Thêm danh mục"
                CssClass="btn btn-primary"
                OnClick="btnThemDanhMuc_Click" />

        </div>


        <!-- =========================================
             GRIDVIEW
        ========================================= -->

        <asp:GridView
            ID="gvDanhMuc"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-bordered table-striped"
            EmptyDataText="Chưa có danh mục nào."
            OnRowCommand="gvDanhMuc_RowCommand">


            <Columns>


                
                <asp:BoundField
                    DataField="MaDanhMuc"
                    HeaderText="Mã" />


                <asp:BoundField
                    DataField="TenDanhMuc"
                    HeaderText="Tên danh mục" />

                <asp:BoundField
                    DataField="MoTa"
                    HeaderText="Mô tả" />


                <asp:CheckBoxField
                    DataField="TrangThai"
                    HeaderText="Trạng thái" />


                <asp:BoundField
                    DataField="NgayTao"
                    HeaderText="Ngày tạo"
                    DataFormatString="{0:dd/MM/yyyy HH:mm}" />


                <asp:TemplateField
                    HeaderText="Thao tác">


                    <ItemTemplate>



                        <asp:HyperLink
                            ID="lnkSua"
                            runat="server"
                            Text="Sửa"
                            CssClass="btn btn-warning btn-sm"
                            NavigateUrl='<%# "Admin_DanhMuc_Sua.aspx?id=" + Eval("MaDanhMuc") %>'>
                        </asp:HyperLink>



                        <asp:Button
                            ID="btnXoa"
                            runat="server"
                            Text="Xóa"
                            CssClass="btn btn-danger btn-sm"
                            CommandName="DeleteCategory"
                            CommandArgument='<%# Eval("MaDanhMuc") %>' />

                    </ItemTemplate>


                </asp:TemplateField>


            </Columns>


        </asp:GridView>


    </div>


   
    <asp:Panel
        ID="pnlModal"
        runat="server"
        CssClass="admin-modal-overlay"
        Visible="false">


        <div class="admin-modal">



            <div class="admin-modal-icon">

                ⚠️

            </div>



            <div class="admin-modal-title">

                <asp:Label
                    ID="lblModalTitle"
                    runat="server">
                </asp:Label>

            </div>


            <div class="admin-modal-message">

                <asp:Label
                    ID="lblModalMessage"
                    runat="server">
                </asp:Label>

            </div>



            <asp:Button
                ID="btnModalClose"
                runat="server"
                Text="Đóng"
                CssClass="admin-modal-button"
                CausesValidation="false"
                OnClick="btnModalClose_Click" />


        </div>


    </asp:Panel>


</asp:Content>