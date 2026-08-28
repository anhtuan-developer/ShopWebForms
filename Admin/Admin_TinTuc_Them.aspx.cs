using System;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_TinTuc_Them : AdminBasePage
    {
        private readonly TinTucDAL tinTucDAL =
            new TinTucDAL();


        protected void Page_Load(
            object sender,
            EventArgs e)
        {
        }


        protected void btnLuu_Click(
            object sender,
            EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }


            string tieuDe =
                txtTieuDe.Text.Trim();


            string noiDung =
                txtNoiDung.Text.Trim();


            string hinhAnh =
                txtHinhAnh.Text.Trim();


            // =================================================
            // VALIDATION
            // =================================================

            if (string.IsNullOrWhiteSpace(tieuDe))
            {
                ShowMessage(
                    "Vui lòng nhập tiêu đề.");

                return;
            }


            if (tieuDe.Length > 250)
            {
                ShowMessage(
                    "Tiêu đề không được vượt quá 250 ký tự.");

                return;
            }


            if (string.IsNullOrWhiteSpace(noiDung))
            {
                ShowMessage(
                    "Vui lòng nhập nội dung.");

                return;
            }


            // =================================================
            // INSERT
            // =================================================

            try
            {
                bool result =
                    tinTucDAL.Insert(
                        tieuDe,
                        noiDung,
                        hinhAnh,
                        chkTrangThai.Checked);


                if (result)
                {
                    Response.Redirect(
                        "Admin_TinTuc.aspx",
                        false);

                    Context.ApplicationInstance
                        .CompleteRequest();

                    return;
                }


                ShowMessage(
                    "Không thể thêm bài viết.");
            }
            catch (Exception ex)
            {
                ShowMessage(
                    "Có lỗi xảy ra: "
                    + ex.Message);
            }
        }


        // =====================================================
        // THÔNG BÁO
        // =====================================================

        private void ShowMessage(
            string message)
        {
            string safe =
                System.Web.HttpUtility
                    .JavaScriptStringEncode(
                        message ?? "");


            ClientScript.RegisterStartupScript(
                GetType(),
                "newsError",
                "alert('" + safe + "');",
                true);
        }
    }
}