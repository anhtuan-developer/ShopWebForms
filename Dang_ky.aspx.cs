using System;
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


            // Kiểm tra họ tên
            if (string.IsNullOrEmpty(hoTen))
            {
                ShowMessage(
                    "Vui lòng nhập họ tên."
                );

                return;
            }


            // Kiểm tra email
            if (string.IsNullOrEmpty(email))
            {
                ShowMessage(
                    "Vui lòng nhập email."
                );

                return;
            }


            // Kiểm tra mật khẩu
            if (string.IsNullOrEmpty(matKhau))
            {
                ShowMessage(
                    "Vui lòng nhập mật khẩu."
                );

                return;
            }


            // Kiểm tra xác nhận
            if (matKhau != xacNhan)
            {
                ShowMessage(
                    "Mật khẩu xác nhận không khớp."
                );

                return;
            }


            // Kiểm tra email tồn tại
            if (khachHangDAL.EmailExists(email))
            {
                ShowMessage(
                    "Email này đã được đăng ký."
                );

                return;
            }


            KhachHang khachHang =
                new KhachHang
                {
                    HoTen = hoTen,

                    Email = email,

                    MatKhau = matKhau,

                    SoDienThoai = soDienThoai,

                    DiaChi = diaChi
                };


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
                    "Đăng ký thất bại."
                );
            }
        }


        private void ShowMessage(
            string message)
        {
            lblMessage.Text = message;
        }
    }
}