using System;
using System.Web.UI.WebControls;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_BinhLuan :
        AdminBasePage
    {
        private readonly BinhLuanDAL binhLuanDAL =
            new BinhLuanDAL();


        // ==========================================
        // PAGE LOAD
        // ==========================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBinhLuan();
            }
        }


        // ==========================================
        // LOAD BÌNH LUẬN
        // ==========================================

        private void LoadBinhLuan()
        {
            gvBinhLuan.DataSource =
                binhLuanDAL.GetAllForAdmin();

            gvBinhLuan.DataBind();
        }


        // ==========================================
        // GRIDVIEW COMMAND
        // ==========================================

        protected void gvBinhLuan_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            int maBinhLuan;


            // ======================================
            // XÓA
            // ======================================

            if (e.CommandName ==
                "DeleteComment")
            {
                if (!int.TryParse(
                    e.CommandArgument.ToString(),
                    out maBinhLuan))
                {
                    ShowMessage(
                        "Mã bình luận không hợp lệ.");

                    return;
                }


                if (binhLuanDAL.Delete(
                    maBinhLuan))
                {
                    LoadBinhLuan();

                    ShowMessage(
                        "Đã xóa bình luận.");
                }
                else
                {
                    ShowMessage(
                        "Không tìm thấy bình luận.");
                }
            }


            // ======================================
            // ẨN / HIỆN
            // ======================================

            else if (
                e.CommandName ==
                "ToggleStatus")
            {
                string[] parts =
                    e.CommandArgument
                        .ToString()
                        .Split('|');


                bool currentStatus;


                if (parts.Length != 2
                    ||
                    !int.TryParse(
                        parts[0],
                        out maBinhLuan)
                    ||
                    !bool.TryParse(
                        parts[1],
                        out currentStatus))
                {
                    ShowMessage(
                        "Dữ liệu bình luận không hợp lệ.");

                    return;
                }


                bool newStatus =
                    !currentStatus;


                if (binhLuanDAL.SetStatus(
                    maBinhLuan,
                    newStatus))
                {
                    LoadBinhLuan();


                    ShowMessage(
                        newStatus
                            ? "Đã hiển thị bình luận."
                            : "Đã ẩn bình luận.");
                }
                else
                {
                    ShowMessage(
                        "Không thể cập nhật trạng thái bình luận.");
                }
            }
        }


        // ==========================================
        // HIỂN THỊ THÔNG BÁO
        // ==========================================

        private void ShowMessage(
            string message)
        {
            string safe =
                System.Web.HttpUtility
                    .JavaScriptStringEncode(
                        message ?? "");


            ClientScript.RegisterStartupScript(
                GetType(),
                "commentMessage",
                "alert('" + safe + "');",
                true);
        }
    }
}