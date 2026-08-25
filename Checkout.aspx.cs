using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using web_ban_hang2.DAL;
using web_ban_hang2.Models;
using web_ban_hang2.Services;

namespace web_ban_hang2
{
    public partial class Checkout : System.Web.UI.Page
    {
        private CartService cartService;

        protected void Page_Load(
    object sender,
    EventArgs e)
        {
            cartService =
                new CartService();


            // KIỂM TRA ĐĂNG NHẬP
            
            if (Session["UserId"] == null)
            {
                Response.Redirect(
                    "Dang_nhap.aspx?returnUrl=Checkout.aspx",
                    false
                );

                Context.ApplicationInstance
                    .CompleteRequest();

                return;
            }


            if (!IsPostBack)
            {
                LoadCheckout();
            }
        }


        // HIỂN THỊ CHECKOUT

        private void LoadCheckout()
        {
            List<CartItem> cart =
                Session["Cart"]
                as List<CartItem>;

            // Kiểm tra giỏ hàng
            
            if (cart == null ||
                cart.Count == 0)
            {
                Response.Redirect(
                    "Cart.aspx");

                return;
            }


            // Kiểm tra lại giỏ hàng với Database
            
            string validationMessage;

            bool valid =
                cartService.ValidateCart(
                    out validationMessage);

            cart =
                Session["Cart"]
                as List<CartItem>;

            if (cart == null ||
                cart.Count == 0)
            {
                Response.Redirect(
                    "Cart.aspx");

                return;
            }


            // Nếu có thay đổi tồn kho
            
            if (!valid &&
                !string.IsNullOrEmpty(
                    validationMessage))
            {
                lblMessage.Text =
                    validationMessage;
            }


            // Tính tổng tiền từ giỏ
            
            decimal tongTien =
                cart.Sum(
                    x => x.ThanhTien);


            lblTongTien.Text =
                "Tổng tiền: "
                + tongTien.ToString("N0")
                + " ₫";
        }


        // ĐẶT HÀNG

        protected void btnDatHang_Click(
    object sender,
    EventArgs e)
        {
            // KIỂM TRA ĐĂNG NHẬP LẠI
            
            if (Session["UserId"] == null)
            {
                Response.Redirect(
                    "Dang_nhap.aspx?returnUrl=Checkout.aspx",
                    false
                );

                Context.ApplicationInstance
                    .CompleteRequest();

                return;
            }


            // LẤY GIỎ HÀNG
           
            List<CartItem> cart =
                Session["Cart"]
                as List<CartItem>;


            // KIỂM TRA GIỎ HÀNG VỚI DATABASE

            string validationMessage;

            bool cartValid =
                cartService.ValidateCart(
                    out validationMessage);


            cart =
                Session["Cart"]
                as List<CartItem>;


            if (cart == null ||
                cart.Count == 0)
            {
                lblMessage.Text =
                    "Giỏ hàng không còn sản phẩm.";

                return;
            }


            // Nếu CartService phát hiện thay đổi
            
            if (!cartValid)
            {
                lblMessage.Text =
                    string.IsNullOrEmpty(
                        validationMessage)
                    ? "Giỏ hàng đã thay đổi. Vui lòng kiểm tra lại."
                    : validationMessage;

                LoadCheckout();

                return;
            }


            // LẤY THÔNG TIN NGƯỜI NHẬN
            
            string hoTen =
                txtHoTen.Text.Trim();

            string soDienThoai =
                txtSoDienThoai.Text.Trim();

            string diaChi =
                txtDiaChi.Text.Trim();


            // VALIDATION HỌ TÊN
            
            if (string.IsNullOrWhiteSpace(
                hoTen))
            {
                lblMessage.Text =
                    "Vui lòng nhập họ tên người nhận.";

                return;
            }


            if (hoTen.Length < 2)
            {
                lblMessage.Text =
                    "Họ tên phải có ít nhất 2 ký tự.";

                return;
            }


            if (hoTen.Length > 100)
            {
                lblMessage.Text =
                    "Họ tên không được vượt quá 100 ký tự.";

                return;
            }


            // VALIDATION SỐ ĐIỆN THOẠI
           
            if (string.IsNullOrWhiteSpace(
                soDienThoai))
            {
                lblMessage.Text =
                    "Vui lòng nhập số điện thoại.";

                return;
            }


            if (!IsValidPhoneNumber(
                soDienThoai))
            {
                lblMessage.Text =
                    "Số điện thoại phải gồm 10 hoặc 11 chữ số.";

                return;
            }


            // VALIDATION ĐỊA CHỈ
            
            if (string.IsNullOrWhiteSpace(
                diaChi))
            {
                lblMessage.Text =
                    "Vui lòng nhập địa chỉ giao hàng.";

                return;
            }


            if (diaChi.Length < 5)
            {
                lblMessage.Text =
                    "Địa chỉ phải có ít nhất 5 ký tự.";

                return;
            }


            if (diaChi.Length > 255)
            {
                lblMessage.Text =
                    "Địa chỉ không được vượt quá 255 ký tự.";

                return;
            }



            // TÍNH TỔNG TIỀN
           
            decimal tongTien =
                cart.Sum(
                    x => x.ThanhTien);


            if (tongTien <= 0)
            {
                lblMessage.Text =
                    "Tổng tiền đơn hàng không hợp lệ.";

                return;
            }


            //TẠO ĐƠN HÀNG
            
            DonHang donHang =
                new DonHang();


            //LẤY MÃ KHÁCH HÀNG

            int? maKhachHang =
    GetMaKhachHang();


            if (!maKhachHang.HasValue)
            {
                Response.Redirect(
                    "Dang_nhap.aspx?returnUrl=Checkout.aspx",
                    false
                );

                Context.ApplicationInstance
                    .CompleteRequest();

                return;
            }


            donHang.MaKhachHang =
                maKhachHang.Value;

            // THÔNG TIN NGƯỜI NHẬN

            donHang.HoTenNguoiNhan =
                hoTen;

            donHang.SoDienThoai =
                soDienThoai;

            donHang.DiaChiGiaoHang =
                diaChi;


            // TỔNG TIỀN
            
            donHang.TongTien =
                tongTien;


             // TRẠNG THÁI
            
            donHang.TrangThai =
                "Chờ xử lý";


           // CHI TIẾT ĐƠN HÀNG
            
            foreach (CartItem item in cart)
            {
                if (item == null)
                {
                    continue;
                }


                if (item.MaSanPham <= 0)
                {
                    lblMessage.Text =
                        "Sản phẩm trong giỏ không hợp lệ.";

                    return;
                }


                if (item.SoLuong <= 0)
                {
                    lblMessage.Text =
                        "Số lượng sản phẩm không hợp lệ.";

                    return;
                }


                ChiTietDonHang chiTiet =
                    new ChiTietDonHang();


                chiTiet.MaSanPham =
                    item.MaSanPham;


                chiTiet.SoLuong =
                    item.SoLuong;


                chiTiet.DonGia =
                    item.Gia;


                donHang.ChiTiet.Add(
                    chiTiet);
            }


            //  LƯU ĐƠN HÀNG
            
            try
            {
                DonHangDAL dal =
                    new DonHangDAL();


                int maDonHang =
                    dal.TaoDonHang(
                        donHang);


                // TẠO ĐƠN THẤT BẠI
                
                if (maDonHang <= 0)
                {
                    lblMessage.Text =
                        "Không thể tạo đơn hàng.";

                    return;
                }

                // XÓA GIỎ HÀNG
              
                Session.Remove(
                    "Cart");


               // CHUYỂN TRANG THÀNH CÔNG
                
                Response.Redirect(
                    "Dat_hang_thanh_cong.aspx?maDonHang="
                    + maDonHang,
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();
            }
            catch (InvalidOperationException ex)
            {
                // Lỗi nghiệp vụ:
                // sản phẩm hết hàng,
                // không đủ tồn kho,
                // sản phẩm ngừng bán,...

                lblMessage.Text =
                    Server.HtmlEncode(
                        ex.Message);

                // Không xóa Cart.
                // Người dùng cần quay lại
                // kiểm tra lại giỏ hàng.
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Có lỗi xảy ra khi đặt hàng: "
                    + Server.HtmlEncode(
                        ex.Message);
            }
        }


        // LẤY MÃ KHÁCH HÀNG

        private int? GetMaKhachHang()
        {
            object sessionValue =
                Session["UserId"];


            if (sessionValue == null)
            {
                return null;
            }


            int maKhachHang;


            if (!int.TryParse(
                sessionValue.ToString(),
                out maKhachHang))
            {
                return null;
            }


            if (maKhachHang <= 0)
            {
                return null;
            }


            return maKhachHang;
        }
        // KIỂM TRA SỐ ĐIỆN THOẠI

        private bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return false;
            }

            return Regex.IsMatch(
                phone,
                @"^\d{10,11}$"
            );
        }
    }
}