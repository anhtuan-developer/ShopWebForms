using System;
using System.Data;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_TinTuc_Sua : AdminBasePage
    {
        private readonly TinTucDAL tinTucDAL =
            new TinTucDAL();


        // =====================================================
        // MÃ TIN TỨC
        // =====================================================

        private int MaTinTuc
        {
            get
            {
                if (ViewState["MaTinTuc"] == null)
                {
                    return 0;
                }

                return Convert.ToInt32(
                    ViewState["MaTinTuc"]);
            }

            set
            {
                ViewState["MaTinTuc"] = value;
            }
        }


        // =====================================================
        // PAGE LOAD
        // =====================================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                int id;


                if (!int.TryParse(
                    Request.QueryString["id"],
                    out id)
                    || id <= 0)
                {
                    ShowError(
                        "Mã tin tức không hợp lệ.");

                    return;
                }


                MaTinTuc = id;

                LoadTinTuc(id);
            }
        }


        // =====================================================
        // LOAD TIN TỨC
        // =====================================================

        private void LoadTinTuc(int id)
        {
            try
            {
                DataTable table =
                    tinTucDAL.GetByIdForAdmin(id);


                if (table == null ||
                    table.Rows.Count == 0)
                {
                    ShowError(
                        "Không tìm thấy bài viết.");

                    return;
                }


                DataRow row =
                    table.Rows[0];


                txtTieuDe.Text =
                    Convert.ToString(
                        row["TieuDe"]);


                txtNoiDung.Text =
                    Convert.ToString(
                        row["NoiDung"]);


                if (row["HinhAnh"] ==
                    DBNull.Value)
                {
                    txtHinhAnh.Text = "";
                }
                else
                {
                    txtHinhAnh.Text =
                        Convert.ToString(
                            row["HinhAnh"]);
                }


                chkTrangThai.Checked =
                    Convert.ToBoolean(
                        row["TrangThai"]);
            }
            catch (Exception ex)
            {
                ShowError(
                    "Không thể tải bài viết: "
                    + ex.Message);
            }
        }


        // =====================================================
        // LƯU THAY ĐỔI
        // =====================================================

        protected void btnLuu_Click(
            object sender,
            EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }


            if (MaTinTuc <= 0)
            {
                ShowError(
                    "Mã tin tức không hợp lệ.");

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
                ShowError(
                    "Vui lòng nhập tiêu đề.");

                return;
            }


            if (tieuDe.Length > 250)
            {
                ShowError(
                    "Tiêu đề không được vượt quá 250 ký tự.");

                return;
            }


            if (string.IsNullOrWhiteSpace(noiDung))
            {
                ShowError(
                    "Vui lòng nhập nội dung.");

                return;
            }


            // =================================================
            // UPDATE
            // =================================================

            try
            {
                bool result =
                    tinTucDAL.Update(
                        MaTinTuc,
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


                ShowError(
                    "Không thể cập nhật bài viết.");
            }
            catch (Exception ex)
            {
                ShowError(
                    "Có lỗi xảy ra: "
                    + ex.Message);
            }
        }


        // =====================================================
        // HIỂN THỊ LỖI
        // =====================================================

        private void ShowError(
            string message)
        {
            pnlError.Visible = true;

            lblError.Text =
                Server.HtmlEncode(
                    message ?? "");
        }
    }
}