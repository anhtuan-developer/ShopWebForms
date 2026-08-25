using System;
using System.Text.RegularExpressions;
using web_ban_hang2.DAL;
using web_ban_hang2.Models;

namespace web_ban_hang2
{
    public partial class Dang_nhap : System.Web.UI.Page
    {
        private KhachHangDAL khachHangDAL;


        // ==========================================
        // PAGE LOAD
        // ==========================================

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


        // ==========================================
        // ĐĂNG NHẬP
        // ==========================================

        protected void btnDangNhap_Click(
            object sender,
            EventArgs e)
        {
            string email =
                txtEmail.Text.Trim();

            string matKhau =
                txtMatKhau.Text;


            // ======================================
            // 1. KIỂM TRA EMAIL RỖNG
            // ======================================

            if (string.IsNullOrWhiteSpace(email))
            {
                lblMessage.Text =
                    "Vui lòng nhập email.";

                return;
            }


            // ======================================
            // 2. KIỂM TRA EMAIL HỢP LỆ
            // ======================================

            if (!IsValidEmail(email))
            {
                lblMessage.Text =
                    "Email không hợp lệ. Ví dụ: example@gmail.com.";

                return;
            }


            // ======================================
            // 3. KIỂM TRA MẬT KHẨU RỖNG
            // ======================================

            if (string.IsNullOrEmpty(matKhau))
            {
                lblMessage.Text =
                    "Vui lòng nhập mật khẩu.";

                return;
            }


            // ======================================
            // 4. KIỂM TRA ĐỘ DÀI MẬT KHẨU
            // ======================================

            if (matKhau.Length < 6)
            {
                lblMessage.Text =
                    "Mật khẩu phải có ít nhất 6 ký tự.";

                return;
            }


            // ======================================
            // 5. KIỂM TRA ĐỘ DÀI EMAIL
            // ======================================

            if (email.Length > 200)
            {
                lblMessage.Text =
                    "Email không được vượt quá 200 ký tự.";

                return;
            }


            // ======================================
            // 6. ĐĂNG NHẬP DATABASE
            // ======================================

            try
            {
                KhachHang khachHang =
                    khachHangDAL.Login(
                        email,
                        matKhau
                    );


                // ==================================
                // 7. SAI EMAIL / MẬT KHẨU
                // ==================================

                if (khachHang == null)
                {
                    lblMessage.Text =
                        "Email hoặc mật khẩu không chính xác.";

                    return;
                }


                // ==================================
                // 8. XÓA SESSION ĐĂNG NHẬP CŨ
                // ==================================

                Session.Remove("User");
                Session.Remove("UserId");
                Session.Remove("UserName");


                // ==================================
                // 9. LƯU SESSION NGƯỜI DÙNG
                // ==================================

                Session["User"] =
                    khachHang;


                // ==================================
                // 10. LƯU MÃ KHÁCH HÀNG
                // ==================================

                Session["UserId"] =
                    khachHang.MaKhachHang;


                // ==================================
                // 11. LƯU TÊN KHÁCH HÀNG
                // ==================================

                Session["UserName"] =
                    khachHang.HoTen;


                // ==================================
                // 12. CHUYỂN VỀ TRANG CHỦ
                // ==================================

                string returnUrl =
    Request.QueryString["returnUrl"];


                if (!string.IsNullOrWhiteSpace(returnUrl)
                    && returnUrl == "Checkout.aspx")
                {
                    Response.Redirect(
                        "Checkout.aspx",
                        false
                    );
                }
                else
                {
                    Response.Redirect(
                        "index.aspx",
                        false
                    );
                }


                Context.ApplicationInstance
                    .CompleteRequest();
            }
            catch (System.Data.SqlClient.SqlException)
            {
                // Không hiển thị lỗi SQL
                // trực tiếp cho khách hàng.

                lblMessage.Text =
                    "Không thể kết nối đến hệ thống. "
                    + "Vui lòng thử lại sau.";

                return;
            }
            catch (Exception)
            {
                // Không hiển thị Exception
                // trực tiếp cho người dùng.

                lblMessage.Text =
                    "Đã xảy ra lỗi khi đăng nhập. "
                    + "Vui lòng thử lại sau.";

                return;
            }
        }


        // ==========================================
        // KIỂM TRA EMAIL
        // ==========================================

        private bool IsValidEmail(
            string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }


            try
            {
                return Regex.IsMatch(
                    email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase
                );
            }
            catch
            {
                return false;
            }
        }
    }
}