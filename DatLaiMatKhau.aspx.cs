using System;
using System.Security.Cryptography;
using System.Text;
using web_ban_hang2.DAL;

namespace web_ban_hang2
{
    public partial class DatLaiMatKhau : System.Web.UI.Page
    {
        private readonly KhachHangDAL dal =
        new KhachHangDAL();

    // =========================================================
    // TOKEN TỪ URL
    // =========================================================

    private string Token
        {
            get
            {
                return
                    Request.QueryString["token"]
                    ?? "";
            }
        }


        // =========================================================
        // PAGE LOAD
        // =========================================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                ValidateTokenOnLoad();
            }
        }


        // =========================================================
        // KIỂM TRA TOKEN NGAY KHI MỞ TRANG
        // =========================================================

        private void ValidateTokenOnLoad()
        {
            string token =
                Token.Trim();


            // -----------------------------------------------------
            // Không có token
            // -----------------------------------------------------

            if (string.IsNullOrWhiteSpace(token))
            {
                DisableResetForm(
                    "Liên kết đặt lại mật khẩu " +
                    "không hợp lệ.");

                return;
            }


            // -----------------------------------------------------
            // Token quá dài bất thường
            // -----------------------------------------------------

            if (token.Length > 200)
            {
                DisableResetForm(
                    "Liên kết đặt lại mật khẩu " +
                    "không hợp lệ.");

                return;
            }


            // -----------------------------------------------------
            // Hash token
            // -----------------------------------------------------

            string tokenHash =
                Sha256(token);


            // -----------------------------------------------------
            // Kiểm tra token trong database
            //
            // Phải:
            // Used = 0
            // ExpiresAt > hiện tại
            // -----------------------------------------------------

            bool valid =
                dal.IsResetTokenValid(
                    tokenHash);


            if (!valid)
            {
                DisableResetForm(
                    "Liên kết đặt lại mật khẩu " +
                    "đã hết hạn, đã được sử dụng " +
                    "hoặc không hợp lệ.");

                return;
            }


            // -----------------------------------------------------
            // Token hợp lệ
            // -----------------------------------------------------

            pnlReset.Visible =
                true;


            lblMessage.Text =
                "";


            lblMessage.CssClass =
                "d-none";
        }


        // =========================================================
        // ĐẶT LẠI MẬT KHẨU
        // =========================================================

        protected void btnDatLai_Click(
            object sender,
            EventArgs e)
        {
            string token =
                Token.Trim();


            // -----------------------------------------------------
            // Kiểm tra token
            // -----------------------------------------------------

            if (string.IsNullOrWhiteSpace(token))
            {
                DisableResetForm(
                    "Liên kết đặt lại mật khẩu " +
                    "không hợp lệ.");

                return;
            }


            // -----------------------------------------------------
            // Mật khẩu mới
            // -----------------------------------------------------

            string password =
                txtMatKhauMoi.Text;


            string confirm =
                txtXacNhan.Text;


            // -----------------------------------------------------
            // Kiểm tra mật khẩu
            // -----------------------------------------------------

            if (string.IsNullOrEmpty(password))
            {
                Show(
                    "Vui lòng nhập mật khẩu mới.",
                    false);

                return;
            }


            if (password.Length < 6 ||
                password.Length > 100)
            {
                Show(
                    "Mật khẩu mới phải từ 6 đến 100 " +
                    "ký tự.",
                    false);

                return;
            }


            if (password != confirm)
            {
                Show(
                    "Mật khẩu xác nhận không trùng khớp.",
                    false);

                return;
            }


            // -----------------------------------------------------
            // Hash token
            // -----------------------------------------------------

            string tokenHash =
                Sha256(token);


            // -----------------------------------------------------
            // ĐẶT LẠI MẬT KHẨU
            //
            // DAL sẽ:
            // 1. Kiểm tra token
            // 2. Kiểm tra hết hạn
            // 3. Kiểm tra Used
            // 4. Hash mật khẩu
            // 5. UPDATE KhachHang
            // 6. Used = 1
            // 7. Commit transaction
            // -----------------------------------------------------

            bool success =
                dal.ResetPassword(
                    tokenHash,
                    password);


            if (success)
            {
                pnlReset.Visible =
                    false;


                txtMatKhauMoi.Text =
                    "";


                txtXacNhan.Text =
                    "";


                Show(
                    "Đặt lại mật khẩu thành công. " +
                    "Bạn có thể đăng nhập bằng mật khẩu mới.",
                    true);
            }
            else
            {
                DisableResetForm(
                    "Liên kết đã hết hạn " +
                    "hoặc đã được sử dụng.");
            }
        }


        // =========================================================
        // VÔ HIỆU HÓA FORM RESET
        // =========================================================

        private void DisableResetForm(
            string message)
        {
            pnlReset.Visible =
                false;


            Show(
                message,
                false);
        }


        // =========================================================
        // SHA-256
        // =========================================================

        private string Sha256(
            string value)
        {
            using (SHA256 sha =
                SHA256.Create())
            {
                byte[] bytes =
                    sha.ComputeHash(
                        Encoding.UTF8.GetBytes(
                            value));


                StringBuilder sb =
                    new StringBuilder(
                        bytes.Length * 2);


                foreach (byte b in bytes)
                {
                    sb.Append(
                        b.ToString("x2"));
                }


                return sb.ToString();
            }
        }


        // =========================================================
        // HIỂN THỊ THÔNG BÁO
        // =========================================================

        private void Show(
            string message,
            bool success)
        {
            lblMessage.Text =
                Server.HtmlEncode(message);


            lblMessage.CssClass =
                success
                    ? "alert alert-success d-block"
                    : "alert alert-danger d-block";
        }
    }

}
