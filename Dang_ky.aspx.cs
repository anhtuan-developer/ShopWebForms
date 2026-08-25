using System;
using System.Text.RegularExpressions;
using web_ban_hang2.DAL;
using web_ban_hang2.Models;

namespace web_ban_hang2
{
    public partial class Dang_ky : System.Web.UI.Page
    {
        private KhachHangDAL khachHangDAL;


        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            khachHangDAL =
                new KhachHangDAL();
        }


        // ==========================================
        // ĐĂNG KÝ
        // ==========================================

        protected void btnDangKy_Click(
            object sender,
            EventArgs e)
        {
            string hoTen =
                txtHoTen.Text.Trim();

            string email =
                txtEmail.Text.Trim();

            string matKhau =
                txtMatKhau.Text;

            string xacNhan =
                txtXacNhanMatKhau.Text;

            string soDienThoai =
                txtSoDienThoai.Text.Trim();

            string diaChi =
                txtDiaChi.Text.Trim();


            // ==========================================
            // 1. HỌ TÊN
            // ==========================================

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                ShowMessage(
                    "Vui lòng nhập họ tên.");

                return;
            }


            if (hoTen.Length < 2)
            {
                ShowMessage(
                    "Họ tên phải có ít nhất 2 ký tự.");

                return;
            }


            if (hoTen.Length > 100)
            {
                ShowMessage(
                    "Họ tên không được vượt quá 100 ký tự.");

                return;
            }


            // ==========================================
            // 2. EMAIL
            // ==========================================

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowMessage(
                    "Vui lòng nhập email.");

                return;
            }


            if (email.Length > 150)
            {
                ShowMessage(
                    "Email không được vượt quá 150 ký tự.");

                return;
            }


            if (!IsValidEmail(email))
            {
                ShowMessage(
                    "Email không hợp lệ. Ví dụ: example@gmail.com.");

                return;
            }


            // ==========================================
            // 3. MẬT KHẨU
            // ==========================================

            if (string.IsNullOrEmpty(matKhau))
            {
                ShowMessage(
                    "Vui lòng nhập mật khẩu.");

                return;
            }


            if (matKhau.Length < 6)
            {
                ShowMessage(
                    "Mật khẩu phải có ít nhất 6 ký tự.");

                return;
            }


            if (matKhau.Length > 100)
            {
                ShowMessage(
                    "Mật khẩu không được vượt quá 100 ký tự.");

                return;
            }


            // ==========================================
            // 4. XÁC NHẬN MẬT KHẨU
            // ==========================================

            if (string.IsNullOrEmpty(xacNhan))
            {
                ShowMessage(
                    "Vui lòng nhập lại mật khẩu.");

                return;
            }


            if (matKhau != xacNhan)
            {
                ShowMessage(
                    "Mật khẩu xác nhận không khớp.");

                return;
            }


            // ==========================================
            // 5. SỐ ĐIỆN THOẠI
            // ==========================================

            if (string.IsNullOrWhiteSpace(soDienThoai))
            {
                ShowMessage(
                    "Vui lòng nhập số điện thoại.");

                return;
            }


            if (!IsValidPhoneNumber(soDienThoai))
            {
                ShowMessage(
                    "Số điện thoại phải gồm 10 hoặc 11 chữ số.");

                return;
            }


            // ==========================================
            // 6. ĐỊA CHỈ
            // ==========================================

            if (string.IsNullOrWhiteSpace(diaChi))
            {
                ShowMessage(
                    "Vui lòng nhập địa chỉ.");

                return;
            }


            if (diaChi.Length < 5)
            {
                ShowMessage(
                    "Địa chỉ phải có ít nhất 5 ký tự.");

                return;
            }


            if (diaChi.Length > 255)
            {
                ShowMessage(
                    "Địa chỉ không được vượt quá 255 ký tự.");

                return;
            }


            // ==========================================
            // 7. KIỂM TRA EMAIL ĐÃ TỒN TẠI
            // ==========================================

            if (khachHangDAL.EmailExists(email))
            {
                ShowMessage(
                    "Email này đã được đăng ký.");

                return;
            }


            // ==========================================
            // 8. TẠO KHÁCH HÀNG
            // ==========================================

            KhachHang khachHang =
                new KhachHang
                {
                    HoTen = hoTen,

                    Email = email,

                    MatKhau = matKhau,

                    SoDienThoai = soDienThoai,

                    DiaChi = diaChi
                };


            // ==========================================
            // 9. LƯU DATABASE
            // ==========================================

            bool result =
                khachHangDAL.Register(
                    khachHang
                );


            if (result)
            {
                Response.Redirect(
                    "Dang_nhap.aspx?register=success"
                );
            }
            else
            {
                ShowMessage(
                    "Đăng ký thất bại.");
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
                // Kiểm tra email cơ bản:
                // abc@gmail.com
                // example.user@domain.vn
                //
                // Không chấp nhận:
                // abc
                // abc@
                // abc@gmail
                // @gmail.com

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


        // ==========================================
        // KIỂM TRA SỐ ĐIỆN THOẠI
        // ==========================================

        private bool IsValidPhoneNumber(
            string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return false;
            }


            // Chỉ cho phép chữ số.
            //
            // 10 chữ số:
            // 0987654321
            //
            // 11 chữ số:
            // 01234567890

            return Regex.IsMatch(
                phone,
                @"^\d{10,11}$"
            );
        }


        // ==========================================
        // HIỂN THỊ THÔNG BÁO
        // ==========================================

        private void ShowMessage(
            string message)
        {
            lblMessage.Text =
                Server.HtmlEncode(
                    message);
        }
    }
}