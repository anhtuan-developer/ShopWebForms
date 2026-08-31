using System;
using System.Configuration;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using web_ban_hang2.DAL;
using web_ban_hang2.Models;

namespace web_ban_hang2
{
    public partial class QuenMatKhau : System.Web.UI.Page
    {
        private readonly KhachHangDAL dal =
        new KhachHangDAL();

    protected void Page_Load(
        object sender,
        EventArgs e)
        {
        }


        // =========================================================
        // GỬI YÊU CẦU QUÊN MẬT KHẨU
        // =========================================================

        protected void btnGui_Click(
            object sender,
            EventArgs e)
        {
            string email =
                txtEmail.Text.Trim();


            // =====================================================
            // KIỂM TRA EMAIL
            // =====================================================

            if (string.IsNullOrWhiteSpace(email))
            {
                Show(
                    "Vui lòng nhập địa chỉ email.",
                    false);

                return;
            }


            if (!IsValidEmail(email))
            {
                Show(
                    "Địa chỉ email không hợp lệ.",
                    false);

                return;
            }


            // =====================================================
            // TÌM KHÁCH HÀNG
            //
            // Không tiết lộ email có tồn tại hay không.
            // =====================================================

            KhachHang kh =
                dal.GetByEmail(email);


            if (kh == null)
            {
                Show(
                    "Nếu email tồn tại, liên kết đặt lại " +
                    "mật khẩu sẽ được gửi.",
                    true);

                return;
            }


            // =====================================================
            // TẠO TOKEN NGẪU NHIÊN 32 BYTE
            // =====================================================

            byte[] raw =
                new byte[32];


            using (RandomNumberGenerator rng =
                RandomNumberGenerator.Create())
            {
                rng.GetBytes(raw);
            }


            // Token URL-safe

            string token =
                Convert.ToBase64String(raw)
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');


            // =====================================================
            // HASH TOKEN
            // =====================================================

            string tokenHash =
                Sha256(token);


            // =====================================================
            // TOKEN CÓ HIỆU LỰC 30 PHÚT
            // =====================================================

            DateTime expiresAt =
                DateTime.Now.AddMinutes(30);


            // =====================================================
            // VÔ HIỆU HÓA TOKEN CŨ + TẠO TOKEN MỚI
            // =====================================================

            bool created =
                dal.CreateResetToken(
                    kh.MaKhachHang,
                    tokenHash,
                    expiresAt);


            if (!created)
            {
                Show(
                    "Không thể tạo yêu cầu đặt lại mật khẩu. " +
                    "Vui lòng thử lại sau.",
                    false);

                return;
            }


            // =====================================================
            // TẠO LINK ĐẶT LẠI MẬT KHẨU
            // =====================================================

            string path =
                ResolveUrl(
                    "~/DatLaiMatKhau.aspx?token=" +
                    Server.UrlEncode(token));


            string link =
                GetAbsoluteUrl(path);


            // =====================================================
            // GỬI EMAIL
            // =====================================================

            try
            {
                SendMail(
                    kh.Email,
                    kh.HoTen,
                    link);


                Show(
                    "Nếu email tồn tại, liên kết đặt lại " +
                    "mật khẩu đã được gửi. " +
                    "Liên kết có hiệu lực trong 30 phút.",
                    true);
            }
            catch (SmtpException)
            {
                // =================================================
                // SMTP THẤT BẠI
                //
                // Vô hiệu hóa token vừa tạo.
                // Không để token mồ côi còn hiệu lực.
                // =================================================

                dal.InvalidateResetToken(tokenHash);


                Show(
                    "Không thể gửi email đặt lại mật khẩu. " +
                    "Vui lòng kiểm tra cấu hình SMTP " +
                    "trong Web.config và thử lại.",
                    false);
            }
            catch (Exception)
            {
                // =================================================
                // LỖI KHÁC KHI GỬI EMAIL
                // =================================================

                dal.InvalidateResetToken(tokenHash);


                Show(
                    "Đã xảy ra lỗi khi gửi email. " +
                    "Vui lòng thử lại sau.",
                    false);
            }
        }


        // =========================================================
        // KIỂM TRA EMAIL
        // =========================================================

        private bool IsValidEmail(
            string email)
        {
            try
            {
                MailAddress address =
                    new MailAddress(email);

                return
                    string.Equals(
                        address.Address,
                        email,
                        StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }


        // =========================================================
        // TẠO ABSOLUTE URL
        // =========================================================

        private string GetAbsoluteUrl(
            string path)
        {
            Uri requestUrl =
                Request.Url;


            if (requestUrl == null)
            {
                throw new InvalidOperationException(
                    "Không thể xác định URL hiện tại.");
            }


            string authority =
                requestUrl.GetLeftPart(
                    UriPartial.Authority);


            return
                authority +
                ResolveUrl(path);
        }


        // =========================================================
        // GỬI EMAIL
        // =========================================================

        private void SendMail(
            string email,
            string name,
            string link)
        {
            string from =
                ConfigurationManager
                    .AppSettings["SmtpFrom"];


            if (string.IsNullOrWhiteSpace(from))
            {
                throw new InvalidOperationException(
                    "Thiếu cấu hình SmtpFrom trong Web.config.");
            }


            using (MailMessage mail =
                new MailMessage())
            {
                mail.From =
                    new MailAddress(
                        from,
                        "SHOP 5 ANH EM",
                        Encoding.UTF8);


                mail.To.Add(
                    new MailAddress(email));


                mail.Subject =
                    "Đặt lại mật khẩu - SHOP 5 ANH EM";


                mail.SubjectEncoding =
                    Encoding.UTF8;


                mail.Body =
                    "Xin chào " +
                    (string.IsNullOrWhiteSpace(name)
                        ? "bạn"
                        : name) +
                    ",\n\n" +

                    "Bạn vừa yêu cầu đặt lại mật khẩu " +
                    "cho tài khoản SHOP 5 ANH EM.\n\n" +

                    "Vui lòng mở liên kết sau trong vòng 30 phút:\n\n" +

                    link +

                    "\n\n" +

                    "Nếu bạn không thực hiện yêu cầu này, " +
                    "vui lòng bỏ qua email này.\n\n" +

                    "Trân trọng,\n" +
                    "SHOP 5 ANH EM";


                mail.BodyEncoding =
                    Encoding.UTF8;


                mail.IsBodyHtml =
                    false;


                using (SmtpClient smtp =
                    new SmtpClient())
                {
                    // Host / Port / SSL / Credentials
                    // được đọc từ Web.config.

                    smtp.Send(mail);
                }
            }
        }


        // =========================================================
        // SHA-256 TOKEN
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
