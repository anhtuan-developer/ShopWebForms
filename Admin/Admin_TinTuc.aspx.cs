using System;
using System.Web.UI.WebControls;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_TinTuc : AdminBasePage
    {
        private readonly TinTucDAL tinTucDAL =
            new TinTucDAL();


        // =====================================================
        // PAGE LOAD
        // =====================================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTinTuc();
            }
        }


        // =====================================================
        // LOAD TIN TỨC
        // =====================================================

        private void LoadTinTuc()
        {
            gvTinTuc.DataSource =
                tinTucDAL.GetAll();

            gvTinTuc.DataBind();
        }


        // =====================================================
        // GRIDVIEW COMMAND
        // =====================================================

        protected void gvTinTuc_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            try
            {
                // =================================================
                // XÓA
                // =================================================

                if (e.CommandName == "DeleteNews")
                {
                    int maTinTuc;

                    if (!int.TryParse(
                        Convert.ToString(
                            e.CommandArgument),
                        out maTinTuc))
                    {
                        ShowMessage(
                            "Mã tin tức không hợp lệ.",
                            "danger");

                        return;
                    }


                    bool result =
                        tinTucDAL.Delete(maTinTuc);


                    if (result)
                    {
                        LoadTinTuc();

                        ShowMessage(
                            "Đã xóa bài viết thành công.",
                            "success");
                    }
                    else
                    {
                        ShowMessage(
                            "Không tìm thấy bài viết.",
                            "warning");
                    }

                    return;
                }


                // =================================================
                // ẨN / HIỆN
                // =================================================

                if (e.CommandName == "ToggleStatus")
                {
                    string argument =
                        Convert.ToString(
                            e.CommandArgument);

                    string[] parts =
                        argument.Split('|');


                    if (parts.Length != 2)
                    {
                        ShowMessage(
                            "Dữ liệu trạng thái không hợp lệ.",
                            "danger");

                        return;
                    }


                    int maTinTuc;

                    bool currentStatus;


                    if (!int.TryParse(
                        parts[0],
                        out maTinTuc))
                    {
                        ShowMessage(
                            "Mã tin tức không hợp lệ.",
                            "danger");

                        return;
                    }


                    if (!bool.TryParse(
                        parts[1],
                        out currentStatus))
                    {
                        ShowMessage(
                            "Trạng thái không hợp lệ.",
                            "danger");

                        return;
                    }


                    bool newStatus =
                        !currentStatus;


                    bool result =
                        tinTucDAL.UpdateStatus(
                            maTinTuc,
                            newStatus);


                    if (result)
                    {
                        LoadTinTuc();

                        ShowMessage(
                            newStatus
                                ? "Đã hiển thị bài viết."
                                : "Đã ẩn bài viết.",
                            "success");
                    }
                    else
                    {
                        ShowMessage(
                            "Không thể cập nhật trạng thái.",
                            "warning");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(
                    "Không thể thực hiện thao tác: "
                    + ex.Message,
                    "danger");
            }
        }


        // =====================================================
        // THÔNG BÁO
        // =====================================================

        private void ShowMessage(
            string message,
            string type)
        {
            string safeMessage =
                Server.HtmlEncode(
                    message ?? "");


            lblMessage.Text =
                "<div class=\"alert alert-"
                + type
                + " alert-dismissible fade show mt-3\" "
                + "role=\"alert\">"
                + safeMessage
                + "<button type=\"button\" "
                + "class=\"btn-close\" "
                + "data-bs-dismiss=\"alert\" "
                + "aria-label=\"Close\"></button>"
                + "</div>";
        }
    }
}