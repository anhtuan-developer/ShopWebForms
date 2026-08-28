using System;
using System.Web.UI.WebControls;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_LienHe : AdminBasePage
    {
        private readonly LienHeDAL lienHeDAL =
            new LienHeDAL();


        // =====================================================
        // PAGE LOAD
        // =====================================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLienHe();
            }
        }


        // =====================================================
        // LOAD DANH SÁCH
        // =====================================================

        private void LoadLienHe()
        {
            gvLienHe.DataSource =
                lienHeDAL.GetAll();

            gvLienHe.DataBind();
        }


        // =====================================================
        // GRIDVIEW COMMAND
        // =====================================================

        protected void gvLienHe_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            try
            {
                // =================================================
                // XÓA
                // =================================================

                if (e.CommandName == "DeleteContact")
                {
                    int maLienHe;

                    if (!int.TryParse(
                        Convert.ToString(
                            e.CommandArgument),
                        out maLienHe))
                    {
                        ShowMessage(
                            "Mã liên hệ không hợp lệ.",
                            "danger");

                        return;
                    }


                    bool result =
                        lienHeDAL.Delete(
                            maLienHe);


                    if (result)
                    {
                        LoadLienHe();

                        ShowMessage(
                            "Đã xóa liên hệ thành công.",
                            "success");
                    }
                    else
                    {
                        ShowMessage(
                            "Không tìm thấy liên hệ.",
                            "warning");
                    }

                    return;
                }


                // =================================================
                // CẬP NHẬT TRẠNG THÁI
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


                    int maLienHe;

                    bool currentStatus;


                    if (!int.TryParse(
                        parts[0],
                        out maLienHe))
                    {
                        ShowMessage(
                            "Mã liên hệ không hợp lệ.",
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
                        lienHeDAL.UpdateStatus(
                            maLienHe,
                            newStatus);


                    if (result)
                    {
                        LoadLienHe();

                        ShowMessage(
                            newStatus
                                ? "Đã đánh dấu liên hệ là đã xử lý."
                                : "Đã chuyển liên hệ về chưa xử lý.",
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
            catch (Exception)
            {
                ShowMessage(
                    "Không thể thực hiện thao tác.",
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