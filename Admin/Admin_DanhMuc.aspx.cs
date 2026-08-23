using System;
using System.Web.UI.WebControls;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_DanhMuc : System.Web.UI.Page
    {
        private readonly DanhMucDAL danhMucDAL =
            new DanhMucDAL();


        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDanhMuc();
            }
        }


        // ==========================================
        // LOAD DANH MỤC
        // ==========================================

        private void LoadDanhMuc()
        {
            gvDanhMuc.DataSource =
                danhMucDAL.GetAll();

            gvDanhMuc.DataBind();
        }


        // ==========================================
        // THÊM DANH MỤC
        // ==========================================

        protected void btnThemDanhMuc_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "Admin_DanhMuc_Them.aspx"
            );
        }


        // ==========================================
        // XỬ LÝ XÓA DANH MỤC
        // ==========================================

        protected void gvDanhMuc_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            // Chỉ xử lý nút Xóa
            if (e.CommandName != "DeleteCategory")
            {
                return;
            }


            // ======================================
            // LẤY MÃ DANH MỤC
            // ======================================

            int maDanhMuc;

            if (!int.TryParse(
                e.CommandArgument.ToString(),
                out maDanhMuc))
            {
                ShowModal(
                    "Lỗi",
                    "Mã danh mục không hợp lệ."
                );

                return;
            }


            try
            {
                // ==================================
                // KIỂM TRA SẢN PHẨM
                // ==================================

                int soLuongSanPham =
                    danhMucDAL.CountProducts(
                        maDanhMuc
                    );


                // ==================================
                // NẾU CÒN SẢN PHẨM
                // ==================================

                if (soLuongSanPham > 0)
                {
                    ShowModal(
                        "Không thể xóa",
                        "Danh mục này đang có "
                        + soLuongSanPham
                        + " sản phẩm thuộc danh mục. "
                        + "Bạn cần xóa hoặc chuyển "
                        + "các sản phẩm sang danh mục khác trước."
                    );

                    return;
                }


                // ==================================
                // KHÔNG CÓ SẢN PHẨM → XÓA
                // ==================================

                bool result =
                    danhMucDAL.Delete(
                        maDanhMuc
                    );


                // ==================================
                // XÓA THÀNH CÔNG
                // ==================================

                if (result)
                {
                    LoadDanhMuc();

                    ShowModal(
                        "Xóa thành công",
                        "Danh mục đã được xóa thành công."
                    );
                }


                // ==================================
                // XÓA THẤT BẠI
                // ==================================

                else
                {
                    ShowModal(
                        "Xóa thất bại",
                        "Không tìm thấy danh mục cần xóa."
                    );
                }
            }
            catch (Exception ex)
            {
                // ==================================
                // LỖI
                // ==================================

                ShowModal(
                    "Có lỗi xảy ra",
                    ex.Message
                );
            }
        }


        // ==========================================
        // HIỂN THỊ MODAL
        // ==========================================

        private void ShowModal(
            string title,
            string message)
        {
            lblModalTitle.Text =
                title;

            lblModalMessage.Text =
                message;

            pnlModal.Visible =
                true;
        }


        // ==========================================
        // ĐÓNG MODAL
        // ==========================================

        protected void btnModalClose_Click(
            object sender,
            EventArgs e)
        {
            pnlModal.Visible =
                false;
        }
    }
}