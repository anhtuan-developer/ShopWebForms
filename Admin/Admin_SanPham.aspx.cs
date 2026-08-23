using System;
using System.Web.UI.WebControls;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_SanPham : System.Web.UI.Page
    {
        private readonly SanPhamDAL sanPhamDAL =
            new SanPhamDAL();


        // ==========================================
        // PAGE LOAD
        // ==========================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSanPham();
            }
        }


        // ==========================================
        // LOAD SẢN PHẨM
        // ==========================================

        private void LoadSanPham()
        {
            gvSanPham.DataSource =
                sanPhamDAL.GetAll();

            gvSanPham.DataBind();
        }


        // ==========================================
        // THÊM SẢN PHẨM
        // ==========================================

        protected void btnThemSanPham_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "Admin_SanPham_Them.aspx"
            );
        }


        // ==========================================
        // GRIDVIEW ROW COMMAND
        // ==========================================

        protected void gvSanPham_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            if (e.CommandName != "DeleteProduct")
            {
                return;
            }


            int maSanPham;


            if (!int.TryParse(
                e.CommandArgument.ToString(),
                out maSanPham))
            {
                ShowMessage(
                    "Lỗi",
                    "Mã sản phẩm không hợp lệ.",
                    "❌"
                );

                return;
            }


            DeleteSanPham(maSanPham);
        }


        // ==========================================
        // XÓA SẢN PHẨM
        // ==========================================

        private void DeleteSanPham(
            int maSanPham)
        {
            try
            {
                // ----------------------------------
                // Kiểm tra sản phẩm đã có trong đơn hàng
                // ----------------------------------

                int soLuongDonHang =
                    sanPhamDAL.CountOrderDetails(
                        maSanPham
                    );


                if (soLuongDonHang > 0)
                {
                    ShowMessage(
                        "Không thể xóa",
                        "Sản phẩm này đã xuất hiện trong "
                        + soLuongDonHang
                        + " chi tiết đơn hàng. "
                        + "Không thể xóa sản phẩm.",
                        "⚠️"
                    );

                    return;
                }


                // ----------------------------------
                // Thực hiện xóa
                // ----------------------------------

                bool result =
                    sanPhamDAL.Delete(
                        maSanPham
                    );


                // ----------------------------------
                // Xóa thành công
                // ----------------------------------

                if (result)
                {
                    LoadSanPham();

                    ShowMessage(
                        "Xóa thành công",
                        "Sản phẩm đã được xóa thành công.",
                        "✅"
                    );
                }


                // ----------------------------------
                // Không tìm thấy sản phẩm
                // ----------------------------------

                else
                {
                    ShowMessage(
                        "Xóa thất bại",
                        "Không tìm thấy sản phẩm cần xóa.",
                        "❌"
                    );
                }
            }
            catch (Exception ex)
            {
                ShowMessage(
                    "Có lỗi xảy ra",
                    ex.Message,
                    "❌"
                );
            }
        }


        // ==========================================
        // HIỂN THỊ THÔNG BÁO
        // ==========================================

        private void ShowMessage(
            string title,
            string message,
            string icon)
        {
            string safeTitle =
                System.Web.HttpUtility
                .JavaScriptStringEncode(
                    title
                );


            string safeMessage =
                System.Web.HttpUtility
                .JavaScriptStringEncode(
                    message
                );


            string safeIcon =
                System.Web.HttpUtility
                .JavaScriptStringEncode(
                    icon
                );


            string script =
                "alert('"
                + safeIcon
                + " "
                + safeTitle
                + "\\n"
                + safeMessage
                + "');";


            ClientScript.RegisterStartupScript(
                GetType(),
                "deleteProductMessage",
                script,
                true
            );
        }
    }
}