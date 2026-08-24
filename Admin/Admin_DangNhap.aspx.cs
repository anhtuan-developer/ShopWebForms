using System;
using System.Web.UI;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_DangNhap : Page
    {
        private readonly AdminAccountDAL adminAccountDAL =
            new AdminAccountDAL();


        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            // Nếu đã đăng nhập Admin
            // thì chuyển về Dashboard

            if (Session["AdminMa"] != null)
            {
                Response.Redirect(
                    "Admin_Default.aspx"
                );

                return;
            }
        }


        protected void btnDangNhap_Click(
            object sender,
            EventArgs e)
        {
            string email =
                txtEmail.Text.Trim();


            string matKhau =
                txtMatKhau.Text.Trim();


            // Xóa thông báo cũ

            lblThongBao.Text = "";


            // =================================
            // KIỂM TRA EMAIL
            // =================================

            if (string.IsNullOrEmpty(email))
            {
                lblThongBao.Text =
                    "Vui lòng nhập email.";

                return;
            }


            // =================================
            // KIỂM TRA MẬT KHẨU
            // =================================

            if (string.IsNullOrEmpty(matKhau))
            {
                lblThongBao.Text =
                    "Vui lòng nhập mật khẩu.";

                return;
            }


            try
            {
                AdminLoginResult result =
                    adminAccountDAL.Login(
                        email,
                        matKhau
                    );


                // =================================
                // ĐĂNG NHẬP THÀNH CÔNG
                // =================================

                if (result.Success)
                {
                    Session["AdminMa"] =
                        result.MaAdmin;


                    Session["AdminEmail"] =
                        result.Email;


                    Session["AdminHoTen"] =
                        result.HoTen;


                    Response.Redirect(
                        "Admin_Default.aspx"
                    );

                    return;
                }


                // =================================
                // ĐĂNG NHẬP THẤT BẠI
                // =================================

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