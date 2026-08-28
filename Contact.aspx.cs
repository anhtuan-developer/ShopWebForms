using System;
using System.Net.Mail;
using web_ban_hang2.DAL;

namespace web_ban_hang2
{
    public partial class Contact : System.Web.UI.Page
    {
        private readonly LienHeDAL lienHeDAL =
            new LienHeDAL();


        protected void Page_Load(
            object sender,
            EventArgs e)
        {
        }


        // =====================================================
        // GỬI LIÊN HỆ
        // =====================================================

        protected void btnSend_Click(
            object sender,
            EventArgs e)
        {
            string hoTen =
                txtName.Text.Trim();

            string email =
                txtEmail.Text.Trim();

            string tieuDe =
                txtSubject.Text.Trim();

            string noiDung =
                txtMessage.Text.Trim();


            // =================================================
            // KIỂM TRA RỖNG
            // =================================================

            if (string.IsNullOrWhiteSpace(hoTen) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(tieuDe) ||
                string.IsNullOrWhiteSpace(noiDung))
            {
                lblMessage.Text =
                    "Vui lòng nhập đầy đủ thông tin.";

                lblMessage.CssClass =
                    "text-danger d-block mt-3";

                return;
            }


            // =================================================
            // KIỂM TRA EMAIL
            // =================================================

            try
            {
                MailAddress mailAddress =
                    new MailAddress(email);

                if (mailAddress.Address != email)
                {
                    throw new FormatException();
                }
            }
            catch
            {
                lblMessage.Text =
                    "Email không hợp lệ.";

                lblMessage.CssClass =
                    "text-danger d-block mt-3";

                return;
            }


            // =================================================
            // GIỚI HẠN ĐỘ DÀI
            // =================================================

            if (hoTen.Length > 100)
            {
                lblMessage.Text =
                    "Họ và tên không được vượt quá 100 ký tự.";

                lblMessage.CssClass =
                    "text-danger d-block mt-3";

                return;
            }


            if (tieuDe.Length > 250)
            {
                lblMessage.Text =
                    "Chủ đề không được vượt quá 250 ký tự.";

                lblMessage.CssClass =
                    "text-danger d-block mt-3";

                return;
            }


            try
            {
                // =============================================
                // INSERT DATABASE
                // =============================================

                bool result =
                    lienHeDAL.Insert(
                        hoTen,
                        email,
                        tieuDe,
                        noiDung
                    );


                if (result)
                {
                    lblMessage.Text =
                        "Cảm ơn bạn! Tin nhắn đã được gửi thành công.";

                    lblMessage.CssClass =
                        "text-success d-block mt-3";


                    // =========================================
                    // XÓA FORM SAU KHI LƯU THÀNH CÔNG
                    // =========================================

                    txtName.Text = "";
                    txtEmail.Text = "";
                    txtSubject.Text = "";
                    txtMessage.Text = "";
                }
                else
                {
                    lblMessage.Text =
                        "Không thể gửi tin nhắn. Vui lòng thử lại.";

                    lblMessage.CssClass =
                        "text-danger d-block mt-3";
                }
            }
            catch (Exception)
            {
                lblMessage.Text =
                    "Có lỗi xảy ra khi lưu tin nhắn. Vui lòng thử lại sau.";

                lblMessage.CssClass =
                    "text-danger d-block mt-3";
            }
        }
    }
}