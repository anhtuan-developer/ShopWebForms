using System;
using web_ban_hang2.DAL;
using web_ban_hang2.Models;

namespace web_ban_hang2
{
    public partial class Dang_nhap : System.Web.UI.Page
    {
        private KhachHangDAL khachHangDAL;


        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            khachHangDAL =
                new KhachHangDAL();


            if (!IsPostBack)
            {
                string register =
                    Request.QueryString["register"];

                if (register == "success")
                {
                    lblMessage.Text =
                        "Đăng ký thành công. Vui lòng đăng nhập.";
                }
            }
        }


        protected void btnDangNhap_Click(
            object sender,
            EventArgs e)
        {
            string email =
                txtEmail.Text.Trim();

            string matKhau =
                txtMatKhau.Text;


            if (string.IsNullOrEmpty(email))
            {
                lblMessage.Text =
                    "Vui lòng nhập email.";

                return;
            }


            if (string.IsNullOrEmpty(matKhau))
            {
                lblMessage.Text =
                    "Vui lòng nhập mật khẩu.";

                return;
            }


            KhachHang khachHang =
                khachHangDAL.Login(
                    email,
                    matKhau
                );


            if (khachHang == null)
            {
                lblMessage.Text =
                    "Email hoặc mật khẩu không chính xác.";

                return;
            }


            // Lưu thông tin người dùng vào Session

            Session["User"] =
                khachHang;


            // Lưu ID riêng để sử dụng thuận tiện

            Session["UserId"] =
                khachHang.MaKhachHang;


            // Lưu tên

            Session["UserName"] =
                khachHang.HoTen;


            Response.Redirect(
                "index.aspx"
            );
        }
    }
}