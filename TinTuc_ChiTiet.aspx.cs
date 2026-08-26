using System;
using System.Data;
using web_ban_hang2.DAL;

namespace web_ban_hang2
{
    public partial class TinTuc_ChiTiet :
        System.Web.UI.Page
    {
        private readonly TinTucDAL tinTucDAL =
            new TinTucDAL();

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
                LoadChiTiet();
            }
        }


        // ==========================================
        // KIỂM TRA ĐĂNG NHẬP
        // ==========================================

        private bool IsLoggedIn(
            out int maKhachHang)
        {
            maKhachHang = 0;

            return Session["UserId"] != null

                && int.TryParse(
                    Session["UserId"].ToString(),
                    out maKhachHang)

                && maKhachHang > 0;
        }


        // ==========================================
        // LẤY MÃ TIN TỨC
        // ==========================================

        private int? GetMaTinTuc()
        {
            int maTinTuc;

            if (!int.TryParse(
                Request.QueryString["id"],
                out maTinTuc)
                ||
                maTinTuc <= 0)
            {
                return null;
            }

            return maTinTuc;
        }


        // ==========================================
        // LOAD BÀI VIẾT
        // ==========================================

        private void LoadChiTiet()
        {
            int? maTinTuc =
                GetMaTinTuc();


            if (!maTinTuc.HasValue)
            {
                ShowError(
                    "Mã tin tức không hợp lệ.");

                return;
            }


            try
            {
                DataTable table =
                    tinTucDAL.GetById(
                        maTinTuc.Value);


                if (table == null ||
                    table.Rows.Count == 0)
                {
                    ShowError(
                        "Không tìm thấy bài viết " +
                        "hoặc bài viết đã ngừng hiển thị.");

                    return;
                }


                DataRow row =
                    table.Rows[0];


                // ======================================
                // TIÊU ĐỀ
                // ======================================

                lblTieuDe.Text =
                    Server.HtmlEncode(
                        row["TieuDe"].ToString());


                // ======================================
                // NGÀY TẠO
                // ======================================

                lblNgayTao.Text =
                    row["NgayTao"] == DBNull.Value
                        ? ""
                        : Convert
                            .ToDateTime(
                                row["NgayTao"])
                            .ToString(
                                "dd/MM/yyyy HH:mm");


                // ======================================
                // HÌNH ẢNH
                // ======================================

                string image =
                    row["HinhAnh"] == DBNull.Value
                        ? ""
                        : row["HinhAnh"]
                            .ToString()
                            .Trim();


                imgTinTuc.ImageUrl =
                    string.IsNullOrWhiteSpace(
                        image)

                        ? ResolveUrl(
                            "~/img/about.jpg")

                        : ResolveUrl(
                            "~/img/" + image);


                // ======================================
                // NỘI DUNG
                // ======================================

                litNoiDung.Text =
                    Server.HtmlEncode(
                        row["NoiDung"] == DBNull.Value
                            ? ""
                            : row["NoiDung"]
                                .ToString());


                pnlDetail.Visible =
                    true;


                // ======================================
                // LOAD BÌNH LUẬN
                // ======================================

                LoadComments(
                    maTinTuc.Value);
            }
            catch (Exception)
            {
                ShowError(
                    "Không thể tải bài viết. " +
                    "Vui lòng thử lại sau.");
            }
        }


        // ==========================================
        // LOAD BÌNH LUẬN
        // ==========================================

        private void LoadComments(
            int maTinTuc)
        {
            try
            {
                int maKhachHang;


                bool loggedIn =
                    IsLoggedIn(
                        out maKhachHang);


                // Người đã đăng nhập
                pnlCommentForm.Visible =
                    loggedIn;


                // Người chưa đăng nhập
                pnlCommentLogin.Visible =
                    !loggedIn;


                DataTable comments =
                    binhLuanDAL.GetByTinTuc(
                        maTinTuc);


                rptBinhLuan.DataSource =
                    comments;

                rptBinhLuan.DataBind();


                pnlNoComment.Visible =
                    comments == null
                    ||
                    comments.Rows.Count == 0;
            }
            catch (Exception)
            {
                lblCommentMessage.Text =
                    "Không thể tải bình luận. " +
                    "Vui lòng thử lại sau.";
            }
        }


        // ==========================================
        // GỬI BÌNH LUẬN
        // ==========================================

        protected void btnBinhLuan_Click(
            object sender,
            EventArgs e)
        {
            int maKhachHang;


            // --------------------------------------
            // KIỂM TRA ĐĂNG NHẬP
            // --------------------------------------

            if (!IsLoggedIn(
                out maKhachHang))
            {
                Response.Redirect(
                    "Dang_nhap.aspx?returnUrl="
                    +
                    Server.UrlEncode(
                        Request.RawUrl),
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();

                return;
            }


            // --------------------------------------
            // KIỂM TRA MÃ TIN
            // --------------------------------------

            int? maTinTuc =
                GetMaTinTuc();


            if (!maTinTuc.HasValue)
            {
                lblCommentMessage.Text =
                    "Mã tin tức không hợp lệ.";

                return;
            }


            // --------------------------------------
            // LẤY NỘI DUNG
            // --------------------------------------

            string noiDung =
                txtBinhLuan.Text.Trim();


            // --------------------------------------
            // KHÔNG ĐƯỢC RỖNG
            // --------------------------------------

            if (string.IsNullOrWhiteSpace(
                noiDung))
            {
                lblCommentMessage.Text =
                    "Vui lòng nhập nội dung bình luận.";

                return;
            }


            // --------------------------------------
            // GIỚI HẠN 1000 KÝ TỰ
            // --------------------------------------

            if (noiDung.Length > 1000)
            {
                lblCommentMessage.Text =
                    "Bình luận không được vượt quá " +
                    "1000 ký tự.";

                return;
            }


            try
            {
                bool result =
                    binhLuanDAL.Insert(
                        maTinTuc.Value,
                        maKhachHang,
                        noiDung);


                if (result)
                {
                    txtBinhLuan.Text = "";


                    lblCommentMessage.Text =
                        "Đã gửi bình luận thành công.";


                    LoadComments(
                        maTinTuc.Value);
                }
                else
                {
                    lblCommentMessage.Text =
                        "Không thể gửi bình luận. " +
                        "Vui lòng thử lại.";
                }
            }
            catch (Exception)
            {
                lblCommentMessage.Text =
                    "Không thể gửi bình luận. " +
                    "Vui lòng thử lại sau.";
            }
        }


        // ==========================================
        // HIỂN THỊ LỖI
        // ==========================================

        private void ShowError(
            string message)
        {
            pnlDetail.Visible =
                false;


            lblMessage.Text =
                Server.HtmlEncode(
                    message);


            pnlError.Visible =
                true;
        }
    }
}