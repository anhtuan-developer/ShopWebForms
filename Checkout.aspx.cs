using System;
using System.Collections.Generic;
using System.Linq;
using web_ban_hang2.DAL;
using web_ban_hang2.Models;

namespace web_ban_hang2
{
    public partial class Checkout : System.Web.UI.Page
    {
        // ==========================================
        // PAGE LOAD
        // ==========================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCheckout();
            }
        }


        // ==========================================
        // HIỂN THỊ THÔNG TIN THANH TOÁN
        // ==========================================

        private void LoadCheckout()
        {
            List<CartItem> cart =
                Session["Cart"] as List<CartItem>;


            // Kiểm tra giỏ hàng
            if (cart == null ||
                cart.Count == 0)
            {
                Response.Redirect(
                    "Cart.aspx"
                );

                return;
            }


            // Tính tổng tiền
            decimal tongTien =
                cart.Sum(
                    x => x.ThanhTien
                );


            // Hiển thị tổng tiền
            lblTongTien.Text =
                "Tổng tiền: " +
                tongTien.ToString("N0") +
                " ₫";
        }


        // ==========================================
        // NÚT ĐẶT HÀNG
        // ==========================================

        protected void btnDatHang_Click(
            object sender,
            EventArgs e)
        {
            // ======================================
            // LẤY GIỎ HÀNG
            // ======================================

            List<CartItem> cart =
                Session["Cart"] as List<CartItem>;


            // Kiểm tra giỏ hàng
            if (cart == null ||
                cart.Count == 0)
            {
                lblMessage.Text =
                    "Giỏ hàng đang trống.";

                return;
            }


            // ======================================
            // LẤY THÔNG TIN NGƯỜI NHẬN
            // ======================================

            string hoTen =
                txtHoTen.Text.Trim();


            string soDienThoai =
                txtSoDienThoai.Text.Trim();


            string diaChi =
                txtDiaChi.Text.Trim();


            // ======================================
            // KIỂM TRA HỌ TÊN
            // ======================================

            if (string.IsNullOrWhiteSpace(
                hoTen))
            {
                lblMessage.Text =
                    "Vui lòng nhập họ tên người nhận.";

                return;
            }


            // ======================================
            // KIỂM TRA SỐ ĐIỆN THOẠI
            // ======================================

            if (string.IsNullOrWhiteSpace(
                soDienThoai))
            {
                lblMessage.Text =
                    "Vui lòng nhập số điện thoại.";

                return;
            }


            // ======================================
            // KIỂM TRA ĐỊA CHỈ
            // ======================================

            if (string.IsNullOrWhiteSpace(
                diaChi))
            {
                lblMessage.Text =
                    "Vui lòng nhập địa chỉ giao hàng.";

                return;
            }


            // ======================================
            // TÍNH TỔNG TIỀN
            // ======================================

            decimal tongTien =
                cart.Sum(
                    x => x.ThanhTien
                );


            // ======================================
            // TẠO ĐỐI TƯỢNG ĐƠN HÀNG
            // ======================================

            DonHang donHang =
                new DonHang();


            // ======================================
            // LẤY MÃ KHÁCH HÀNG TỪ SESSION
            // ======================================

            donHang.MaKhachHang =
                GetMaKhachHang();


            // ======================================
            // THÔNG TIN NGƯỜI NHẬN
            // ======================================

            donHang.HoTenNguoiNhan =
                hoTen;


            donHang.SoDienThoai =
                soDienThoai;


            donHang.DiaChiGiaoHang =
                diaChi;


            // ======================================
            // TỔNG TIỀN
            // ======================================

            donHang.TongTien =
                tongTien;


            // ======================================
            // TRẠNG THÁI BAN ĐẦU
            // ======================================

            donHang.TrangThai =
                "Chờ xử lý";


            // ======================================
            // TẠO CHI TIẾT ĐƠN HÀNG
            // ======================================

            foreach (CartItem item in cart)
            {
                ChiTietDonHang chiTiet =
                    new ChiTietDonHang();


                chiTiet.MaSanPham =
                    item.MaSanPham;


                chiTiet.SoLuong =
                    item.SoLuong;


                chiTiet.DonGia =
                    item.Gia;


                donHang.ChiTiet.Add(
                    chiTiet
                );
            }


            // ======================================
            // LƯU ĐƠN HÀNG
            // ======================================

            try
            {
                DonHangDAL dal =
                    new DonHangDAL();


                int maDonHang =
                    dal.TaoDonHang(
                        donHang
                    );


                // Kiểm tra kết quả
                if (maDonHang <= 0)
                {
                    lblMessage.Text =
                        "Không thể tạo đơn hàng.";

                    return;
                }


                // ==================================
                // XÓA GIỎ HÀNG
                // ==================================

                Session.Remove(
                    "Cart"
                );


                // ==================================
                // CHUYỂN TRANG THÀNH CÔNG
                // ==================================

                Response.Redirect(
                    "Dat_hang_thanh_cong.aspx?maDonHang="
                    + maDonHang
                );
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi trên trang
                lblMessage.Text =
                    "Có lỗi xảy ra khi đặt hàng: "
                    + Server.HtmlEncode(
                        ex.Message
                    );
            }
        }


        // ==========================================
        // LẤY MÃ KHÁCH HÀNG
        // ==========================================

        private int? GetMaKhachHang()
        {
            // ======================================
            // KIỂM TRA SESSION
            // ======================================

            object sessionValue =
                Session["MaKhachHang"];


            if (sessionValue == null)
            {
                return null;
            }


            // ======================================
            // NẾU SESSION LÀ INT
            // ======================================

            if (sessionValue is int)
            {
                return (int)sessionValue;
            }


            // ======================================
            // NẾU SESSION LÀ CHUỖI
            // ======================================

            int maKhachHang;


            if (int.TryParse(
                sessionValue.ToString(),
                out maKhachHang))
            {
                return maKhachHang;
            }


            // Không xác định được
            return null;
        }
    }
}