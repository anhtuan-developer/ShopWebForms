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
                    "Mã sản phẩm không hợp lệ."
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
                bool result =
                    sanPhamDAL.Delete(
                        maSanPham
                    );


                if (result)
                {
                    LoadSanPham();

                    ShowMessage(
                        "Xóa sản phẩm thành công."
                    );
                }
                else
                {
                    ShowMessage(
                        "Không tìm thấy sản phẩm cần xóa."
                    );
                }
            }
            catch (Exception ex)
            {
                ShowMessage(
                    ex.Message
                );
            }
        }


        // ==========================================
        // HIỂN THỊ THÔNG BÁO
        // ==========================================

        private void ShowMessage(
            string message)
        {
            string safeMessage =
                System.Web.HttpUtility
                .JavaScriptStringEncode(
                    message
                );


            ClientScript.RegisterStartupScript(
                GetType(),
                "message",
                "alert('" + safeMessage + "');",
                true
            );
        }
    }
}