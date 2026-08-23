using System;
using System.Collections.Generic;
using System.Linq;
using web_ban_hang2.DAL;
using web_ban_hang2.Models;

namespace web_ban_hang2
{
    public partial class Checkout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

            // Nếu giỏ hàng không tồn tại hoặc rỗng
            if (cart == null || cart.Count == 0)
            {
                Response.Redirect("Cart.aspx");
                return;
            }

            // Tính tổng tiền
            decimal tongTien =
                cart.Sum(x => x.ThanhTien);

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
            // Lấy giỏ hàng từ Session
            List<CartItem> cart =
                Session["Cart"] as List<CartItem>;

            // Kiểm tra giỏ hàng
            if (cart == null || cart.Count == 0)
            {
                lblMessage.Text =
                    "Giỏ hàng đang trống.";

                return;
            }


            // ======================================
            // KIỂM TRA THÔNG TIN NGƯỜI NHẬN
            // ======================================

            string hoTen =
                txtHoTen.Text.Trim();

            string soDienThoai =
                txtSoDienThoai.Text.Trim();

            string diaChi =
                txtDiaChi.Text.Trim();


            if (string.IsNullOrWhiteSpace(hoTen))
            {
                lblMessage.Text =
                    "Vui lòng nhập họ tên người nhận.";

                return;
            }


            if (string.IsNullOrWhiteSpace(soDienThoai))
            {
                lblMessage.Text =
                    "Vui lòng nhập số điện thoại.";

                return;
            }


            if (string.IsNullOrWhiteSpace(diaChi))
            {
                lblMessage.Text =
                    "Vui lòng nhập địa chỉ giao hàng.";

                return;
            }


            // ======================================
            // TÍNH TỔNG TIỀN
            // ======================================

            decimal tongTien =
                cart.Sum(x => x.ThanhTien);


            // ======================================
            // TẠO ĐƠN HÀNG
            // ======================================

            DonHang donHang =
                new DonHang();

            donHang.HoTenNguoiNhan =
                hoTen;

            donHang.SoDienThoai =
                soDienThoai;

            donHang.DiaChiGiaoHang =
                diaChi;

            donHang.TongTien =
                tongTien;

            donHang.TrangThai =
                "Chờ xử lý";


            // ======================================
            // CHUYỂN CARTITEM → CHITIETDONHANG
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

                donHang.ChiTiet.Add(chiTiet);
            }


            // ======================================
            // LƯU DATABASE
            // ======================================

            DonHangDAL dal =
                new DonHangDAL();

            int maDonHang =
                dal.TaoDonHang(donHang);


            // ======================================
            // XÓA GIỎ HÀNG
            // ======================================

            Session.Remove("Cart");


            // ======================================
            // CHUYỂN SANG TRANG THÀNH CÔNG
            // ======================================

            Response.Redirect(
                "Dat_hang_thanh_cong.aspx?maDonHang="
                + maDonHang
            );
        }
    }
}