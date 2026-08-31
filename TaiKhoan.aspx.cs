using System;
using System.Text.RegularExpressions;
using web_ban_hang2.DAL;
using web_ban_hang2.Models;

namespace web_ban_hang2
{
    public partial class TaiKhoan : System.Web.UI.Page
    {
        private readonly KhachHangDAL dal =
        new KhachHangDAL();

    private int UserId
        {
            get
            {
                int id;

                return Session["UserId"] != null &&
                       int.TryParse(
                           Session["UserId"].ToString(),
                           out id)
                    ? id
                    : 0;
            }
        }


        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (UserId <= 0)
            {
                Response.Redirect(
                    "Dang_nhap.aspx?returnUrl=TaiKhoan.aspx",
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();

                return;
            }


            if (!IsPostBack)
            {
                LoadProfile();
            }
        }


        private void LoadProfile()
        {
            KhachHang kh =
                dal.GetById(UserId);


            if (kh == null)
            {
                Response.Redirect(
                    "Dang_xuat.aspx",
                    false);

                return;
            }


            txtHoTen.Text =
                kh.HoTen;

            txtEmail.Text =
                kh.Email;

            txtSoDienThoai.Text =
                kh.SoDienThoai;

            txtDiaChi.Text =
                kh.DiaChi;
        }


        protected void btnLuu_Click(
            object sender,
            EventArgs e)
        {
            string hoTen =
                txtHoTen.Text.Trim();

            string phone =
                txtSoDienThoai.Text.Trim();

            string diaChi =
                txtDiaChi.Text.Trim();


            if (hoTen.Length < 2 ||
                hoTen.Length > 100)
            {
                Show(
                    "Họ tên phải từ 2 đến 100 ký tự.",
                    false);

                return;
            }


            if (!string.IsNullOrEmpty(phone) &&
                !Regex.IsMatch(
                    phone,
                    @"^\d{10,11}$"))
            {
                Show(
                    "Số điện thoại phải gồm 10 hoặc 11 chữ số.",
                    false);

                return;
            }


            if (diaChi.Length > 255)
            {
                Show(
                    "Địa chỉ không được vượt quá 255 ký tự.",
                    false);

                return;
            }


            if (dal.UpdateProfile(
                UserId,
                hoTen,
                phone,
                diaChi))
            {
                Session["UserName"] =
                    hoTen;

                Show(
                    "Cập nhật thông tin thành công.",
                    true);
            }
            else
            {
                Show(
                    "Không thể cập nhật thông tin.",
                    false);
            }
        }


        private void Show(
            string message,
            bool success)
        {
            lblMessage.Text =
                message;


            lblMessage.CssClass =
                success
                    ? "alert alert-success d-block"
                    : "alert alert-danger d-block";
        }
    }

}
