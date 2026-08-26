using System;
using System.Data;
using web_ban_hang2.DAL;

namespace web_ban_hang2
{
    public partial class DonHangCuaToi_ChiTiet :
        System.Web.UI.Page
    {
        private readonly DonHangDAL donHangDAL =
            new DonHangDAL();


        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            // ==========================================
            // KIỂM TRA ĐĂNG NHẬP
            // ==========================================

            if (!IsLoggedIn())
            {
                Response.Redirect(
                    "Dang_nhap.aspx?returnUrl=DonHangCuaToi_ChiTiet.aspx",
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();

                return;
            }


            if (!IsPostBack)
            {
                LoadChiTiet();
            }
        }


        // ==========================================
        // KIỂM TRA ĐĂNG NHẬP
        // ==========================================

        private bool IsLoggedIn()
        {
            int maKhachHang;


            return Session["UserId"] != null
                && int.TryParse(
                    Session["UserId"].ToString(),
                    out maKhachHang)
                && maKhachHang > 0;
        }


        // ==========================================
        // LẤY MÃ KHÁCH HÀNG
        // ==========================================

        private int GetMaKhachHang()
        {
            int maKhachHang;


            if (!int.TryParse(
                Session["UserId"].ToString(),
                out maKhachHang)
                || maKhachHang <= 0)
            {
                throw new InvalidOperationException(
                    "Phiên đăng nhập không hợp lệ.");
            }


            return maKhachHang;
        }


        // ==========================================
        // LẤY MÃ ĐƠN HÀNG
        // ==========================================

        private int? GetMaDonHang()
        {
            int maDonHang;


            if (!int.TryParse(
                Request.QueryString["id"],
                out maDonHang)
                || maDonHang <= 0)
            {
                return null;
            }


            return maDonHang;
        }


        // ==========================================
        // LOAD CHI TIẾT
        // ==========================================

        private void LoadChiTiet()
        {
            try
            {
                int? maDonHang =
                    GetMaDonHang();


                if (!maDonHang.HasValue)
                {
                    ShowError(
                        "Mã đơn hàng không hợp lệ.");

                    return;
                }


                DataTable table =
                    donHangDAL.GetChiTietByCustomerId(
                        maDonHang.Value,
                        GetMaKhachHang());


                // ======================================
                // KIỂM TRA QUYỀN XEM ĐƠN
                // ======================================

                if (table == null ||
                    table.Rows.Count == 0)
                {
                    ShowError(
                        "Không tìm thấy đơn hàng "
                        + "hoặc bạn không có quyền "
                        + "xem đơn hàng này.");

                    return;
                }


                DataRow row =
                    table.Rows[0];


                lblMaDonHang.Text =
                    row["MaDonHang"].ToString();


                lblNgayDat.Text =
                    row["NgayDat"] == DBNull.Value
                        ? ""
                        : Convert
                            .ToDateTime(
                                row["NgayDat"])
                            .ToString(
                                "dd/MM/yyyy HH:mm");


                lblTrangThai.Text =
                    row["TrangThai"].ToString();


                lblHoTenNguoiNhan.Text =
                    row["HoTenNguoiNhan"]
                        .ToString();


                lblSoDienThoai.Text =
                    row["SoDienThoai"]
                        .ToString();


                lblDiaChiGiaoHang.Text =
                    row["DiaChiGiaoHang"]
                        .ToString();


                lblTongTien.Text =
                    Convert
                        .ToDecimal(
                            row["TongTien"])
                        .ToString("N0")
                    + " ₫";


                gvChiTiet.DataSource =
                    table;

                gvChiTiet.DataBind();


                pnlDetail.Visible =
                    true;

                lblMessage.Visible =
                    false;
            }
            catch (Exception)
            {
                ShowError(
                    "Không thể tải chi tiết đơn hàng. "
                    + "Vui lòng thử lại sau.");
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


            lblMessage.Visible =
                true;
        }
    }
}