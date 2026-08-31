using System;
using web_ban_hang2.DAL;

namespace web_ban_hang2
{
    public partial class DoiMatKhau : System.Web.UI.Page
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
                    "Dang_nhap.aspx?returnUrl=DoiMatKhau.aspx",
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();
            }
        }


        protected void btnDoiMatKhau_Click(
            object sender,
            EventArgs e)
        {
            string oldPassword =
                txtMatKhauCu.Text;

            string newPassword =
                txtMatKhauMoi.Text;

            string confirmPassword =
                txtXacNhan.Text;


            if (string.IsNullOrEmpty(oldPassword) ||
                newPassword.Length < 6 ||
                newPassword.Length > 100 ||
                newPassword != confirmPassword)
            {
                Show(
                    "Vui lòng kiểm tra mật khẩu hiện tại, " +
                    "mật khẩu mới và xác nhận mật khẩu.",
                    false);

                return;
            }


            if (dal.ChangePassword(
                UserId,
                oldPassword,
                newPassword))
            {
                Show(
                    "Đổi mật khẩu thành công. " +
                    "Vui lòng sử dụng mật khẩu mới " +
                    "ở lần đăng nhập tiếp theo.",
                    true);


                txtMatKhauCu.Text = "";
                txtMatKhauMoi.Text = "";
                txtXacNhan.Text = "";
            }
            else
            {
                Show(
                    "Mật khẩu hiện tại không chính xác.",
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
