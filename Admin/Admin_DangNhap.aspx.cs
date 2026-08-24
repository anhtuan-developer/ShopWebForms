using System;
using System.Web.UI;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_DangNhap : Page
    {
        private readonly AdminAccountDAL adminAccountDAL =
            new AdminAccountDAL();


        // =====================================================
        // PAGE LOAD
        // =====================================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            // Nếu Admin đã đăng nhập
            // thì chuyển thẳng về Dashboard

            if (Session["AdminMa"] != null)
            {
                Response.Redirect(
                    "Admin_Default.aspx"
                );

                return;
            }
        }


        // =====================================================
        // ĐĂNG NHẬP
        // =====================================================

        protected void btnDangNhap_Click(
            object sender,
            EventArgs e)
        {
            // ================================================
            // LẤY EMAIL
            // ================================================

            string email =
                txtEmail.Text.Trim();


            // ================================================
            // LẤY MẬT KHẨU
            // ================================================

            string matKhau =
                txtMatKhau.Text.Trim();


            // Xóa thông báo cũ

            lblThongBao.Text = "";


            // ================================================
            // KIỂM TRA EMAIL
            // ================================================

            if (string.IsNullOrEmpty(email))
            {
                lblThongBao.Text =
                    "Vui lòng nhập email.";

                return;
            }


            // ================================================
            // KIỂM TRA MẬT KHẨU
            // ================================================

            if (string.IsNullOrEmpty(matKhau))
            {
                lblThongBao.Text =
                    "Vui lòng nhập mật khẩu.";

                return;
            }


            // ================================================
            // GỌI DAL
            // ================================================

            try
            {
                AdminLoginResult result =
                    adminAccountDAL.Login(
                        email,
                        matKhau
                    );


                // ============================================
                // ĐĂNG NHẬP THÀNH CÔNG
                // ============================================

                if (result.Success)
                {
                    // Mã Admin

                    Session["AdminMa"] =
                        result.MaAdmin;


                    // Email Admin

                    Session["AdminEmail"] =
                        result.Email;


                    // Họ tên Admin

                    Session["AdminHoTen"] =
                        result.HoTen;


                    // ========================================
                    // CHUYỂN ĐẾN DASHBOARD
                    // ========================================

                    Response.Redirect(
                        "Admin_Default.aspx"
                    );

                    return;
                }


                // ============================================
                // ĐĂNG NHẬP THẤT BẠI
                // ============================================

                lblThongBao.Text =
                    result.Message;
            }
            catch (Exception ex)
            {
                lblThongBao.Text =
                    "Có lỗi xảy ra: "
                    + ex.Message;
            }
        }
    }
}