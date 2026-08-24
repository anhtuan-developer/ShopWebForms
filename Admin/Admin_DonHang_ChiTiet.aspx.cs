using System;
using System.Data;
using System.Web.UI;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_DonHang_ChiTiet : AdminBasePage
    {
        private readonly DonHangDAL donHangDAL =
            new DonHangDAL();


        // ==========================================
        // PAGE LOAD
        // ==========================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadChiTietDonHang();
            }
        }


        // ==========================================
        // LOAD CHI TIẾT ĐƠN HÀNG
        // ==========================================

        private void LoadChiTietDonHang()
        {
            string id =
                Request.QueryString["id"];


            int maDonHang;


            if (!int.TryParse(
                id,
                out maDonHang))
            {
                ShowMessage(
                    "Mã đơn hàng không hợp lệ."
                );

                return;
            }


            // ======================================
            // LẤY THÔNG TIN ĐƠN HÀNG
            // ======================================

            DataTable donHang =
                donHangDAL.GetById(
                    maDonHang
                );


            if (donHang == null ||
                donHang.Rows.Count == 0)
            {
                ShowMessage(
                    "Không tìm thấy đơn hàng."
                );

                return;
            }


            DataRow row =
                donHang.Rows[0];


            lblMaDonHang.Text =
                row["MaDonHang"].ToString();


            lblTenKhachHang.Text =
                row["TenKhachHang"] == DBNull.Value
                    ? "Khách vãng lai"
                    : row["TenKhachHang"].ToString();


            lblHoTenNguoiNhan.Text =
                row["HoTenNguoiNhan"].ToString();


            lblSoDienThoai.Text =
                row["SoDienThoai"].ToString();


            lblDiaChiGiaoHang.Text =
                row["DiaChiGiaoHang"].ToString();


            lblTrangThai.Text =
                row["TrangThai"].ToString();


            if (row["NgayDat"] != DBNull.Value)
            {
                lblNgayDat.Text =
                    Convert.ToDateTime(
                        row["NgayDat"]
                    ).ToString(
                        "dd/MM/yyyy HH:mm"
                    );
            }


            if (row["TongTien"] != DBNull.Value)
            {
                lblTongTien.Text =
                    Convert.ToDecimal(
                        row["TongTien"]
                    ).ToString(
                        "N0"
                    ) + " ₫";
            }


            // ======================================
            // LẤY CHI TIẾT SẢN PHẨM
            // ======================================

            DataTable chiTiet =
                donHangDAL.GetChiTietByDonHang(
                    maDonHang
                );


            gvChiTiet.DataSource =
                chiTiet;

            gvChiTiet.DataBind();
        }


        // ==========================================
        // HIỂN THỊ LỖI
        // ==========================================

        private void ShowMessage(
            string message)
        {
            lblMaDonHang.Text = "";
            lblTenKhachHang.Text = "";
            lblHoTenNguoiNhan.Text = "";
            lblSoDienThoai.Text = "";
            lblDiaChiGiaoHang.Text = "";
            lblTrangThai.Text = "";
            lblNgayDat.Text = "";
            lblTongTien.Text = "";


            string safeMessage =
                System.Web.HttpUtility
                .JavaScriptStringEncode(
                    message
                );


            ClientScript.RegisterStartupScript(
                GetType(),
                "orderDetailError",
                "alert('" +
                safeMessage +
                "');",
                true
            );
        }
    }
}