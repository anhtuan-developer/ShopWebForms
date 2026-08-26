using System;
using System.Data;
using web_ban_hang2.DAL;
using web_ban_hang2.Models;
using web_ban_hang2.Services;

namespace web_ban_hang2
{
    public partial class ProductDetail : System.Web.UI.Page
    {
        private SanPhamDAL sanPhamDAL;
        private CartService cartService;

        private readonly DanhGiaDAL danhGiaDAL =
            new DanhGiaDAL();


        // =====================================================
        // PAGE LOAD
        // =====================================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            sanPhamDAL = new SanPhamDAL();

            cartService = new CartService();

            if (!IsPostBack)
            {
                LoadProduct();
            }
        }


        // =====================================================
        // LOAD SẢN PHẨM
        // =====================================================

        private void LoadProduct()
        {
            string id =
                Request.QueryString["id"];


            if (string.IsNullOrEmpty(id))
            {
                Response.Redirect("shop.aspx");
                return;
            }


            int maSanPham;

            if (!int.TryParse(
                id,
                out maSanPham)
                || maSanPham <= 0)
            {
                Response.Redirect("shop.aspx");
                return;
            }


            try
            {
                DataTable table =
                    sanPhamDAL.GetById(
                        maSanPham);


                if (table == null ||
                    table.Rows.Count == 0)
                {
                    Response.Redirect("shop.aspx");
                    return;
                }


                SanPham sanPham =
                    ConvertToSanPham(
                        table.Rows[0]);


                // ==========================================
                // SẢN PHẨM NGỪNG BÁN
                // ==========================================

                if (!sanPham.TrangThai)
                {
                    Response.Redirect("shop.aspx");
                    return;
                }


                DisplayProduct(
                    sanPham);


                // ==========================================
                // LOAD ĐÁNH GIÁ
                // ==========================================

                LoadDanhGia(
                    maSanPham);
            }
            catch (Exception)
            {
                lblMessage.Text =
                    "Không thể tải thông tin sản phẩm. " +
                    "Vui lòng thử lại sau.";
            }
        }


        // =====================================================
        // CHUYỂN DATAROW → SANPHAM
        // =====================================================

        private SanPham ConvertToSanPham(
            DataRow row)
        {
            SanPham sanPham =
                new SanPham();


            sanPham.MaSanPham =
                Convert.ToInt32(
                    row["MaSanPham"]);


            sanPham.MaDanhMuc =
                Convert.ToInt32(
                    row["MaDanhMuc"]);


            sanPham.TenSanPham =
                row["TenSanPham"]
                    .ToString();


            sanPham.MoTa =
                row["MoTa"] == DBNull.Value
                    ? ""
                    : row["MoTa"].ToString();


            sanPham.Gia =
                Convert.ToDecimal(
                    row["Gia"]);


            sanPham.SoLuong =
                Convert.ToInt32(
                    row["SoLuong"]);


            sanPham.HinhAnh =
                row["HinhAnh"] == DBNull.Value
                    ? ""
                    : row["HinhAnh"].ToString();


            sanPham.TrangThai =
                Convert.ToBoolean(
                    row["TrangThai"]);


            sanPham.NgayTao =
                Convert.ToDateTime(
                    row["NgayTao"]);


            sanPham.TenDanhMuc =
                row.Table.Columns.Contains(
                    "TenDanhMuc")
                &&
                row["TenDanhMuc"] != DBNull.Value
                    ? row["TenDanhMuc"].ToString()
                    : "";


            return sanPham;
        }


        // =====================================================
        // HIỂN THỊ SẢN PHẨM
        // =====================================================

        private void DisplayProduct(
            SanPham sanPham)
        {
            lblProductName.InnerText =
                sanPham.TenSanPham;


            lblCategory.InnerText =
                sanPham.TenDanhMuc;


            lblPrice.InnerText =
                String.Format(
                    "{0:N0} ₫",
                    sanPham.Gia);


            lblDescription.InnerText =
                sanPham.MoTa;


            lblStock.InnerText =
                "Còn lại: "
                + sanPham.SoLuong
                + " sản phẩm";


            imgProduct.ImageUrl =
                ResolveUrl(
                    "~/img/"
                    + sanPham.HinhAnh);


            imgProduct.AlternateText =
                sanPham.TenSanPham;


            // ==========================================
            // SẢN PHẨM NGỪNG BÁN
            // ==========================================

            if (!sanPham.TrangThai)
            {
                btnAddToCart.Enabled =
                    false;

                lblStock.InnerText =
                    "Sản phẩm hiện không còn được bán";

                return;
            }


            // ==========================================
            // HẾT HÀNG
            // ==========================================

            if (sanPham.SoLuong <= 0)
            {
                btnAddToCart.Enabled =
                    false;

                lblStock.InnerText =
                    "Sản phẩm đã hết hàng";
            }
        }


        // =====================================================
        // LOAD ĐÁNH GIÁ
        // =====================================================

        private void LoadDanhGia(
            int maSanPham)
        {
            try
            {
                DataTable table =
                    danhGiaDAL.GetByProductId(
                        maSanPham);


                // ==========================================
                // DANH SÁCH ĐÁNH GIÁ
                // ==========================================

                if (rptDanhGia != null)
                {
                    rptDanhGia.DataSource =
                        table;

                    rptDanhGia.DataBind();
                }


                // ==========================================
                // ĐIỂM TRUNG BÌNH
                // ==========================================

                decimal averageRating =
                    danhGiaDAL.GetAverageRating(
                        maSanPham);


                // ==========================================
                // SỐ LƯỢNG ĐÁNH GIÁ
                // ==========================================

                int reviewCount =
                    danhGiaDAL.CountByProductId(
                        maSanPham);


                if (lblAverageRating != null)
                {
                    lblAverageRating.Text =
                        averageRating.ToString("0.0")
                        + " / 5 ★";
                }


                if (lblReviewCount != null)
                {
                    lblReviewCount.Text =
                        "("
                        + reviewCount
                        + " đánh giá)";
                }
            }
            catch (Exception)
            {
                // Không để lỗi đánh giá
                // làm hỏng trang sản phẩm.

                if (lblAverageRating != null)
                {
                    lblAverageRating.Text =
                        "0.0 / 5 ★";
                }


                if (lblReviewCount != null)
                {
                    lblReviewCount.Text =
                        "(0 đánh giá)";
                }
            }
        }


        // =====================================================
        // KIỂM TRA KHÁCH HÀNG ĐĂNG NHẬP
        // =====================================================

        private bool IsLoggedIn()
        {
            int maKhachHang;


            return Session["UserId"] != null
                &&
                int.TryParse(
                    Session["UserId"].ToString(),
                    out maKhachHang)
                &&
                maKhachHang > 0;
        }


        // =====================================================
        // GỬI ĐÁNH GIÁ
        // =====================================================

        protected void btnGuiDanhGia_Click(
            object sender,
            EventArgs e)
        {
            // ==========================================
            // KIỂM TRA ĐĂNG NHẬP
            // ==========================================

            if (!IsLoggedIn())
            {
                string returnUrl =
                    Request.RawUrl;


                Response.Redirect(
                    "Dang_nhap.aspx?returnUrl="
                    + Server.UrlEncode(
                        returnUrl));

                return;
            }


            // ==========================================
            // LẤY MÃ SẢN PHẨM
            // ==========================================

            int maSanPham;


            if (!int.TryParse(
                Request.QueryString["id"],
                out maSanPham)
                || maSanPham <= 0)
            {
                ShowReviewMessage(
                    "Mã sản phẩm không hợp lệ.");

                return;
            }


            // ==========================================
            // KIỂM TRA SỐ SAO
            // ==========================================

            int soSao;


            if (!int.TryParse(
                ddlSoSao.SelectedValue,
                out soSao)
                ||
                soSao < 1
                ||
                soSao > 5)
            {
                ShowReviewMessage(
                    "Số sao phải từ 1 đến 5.");

                return;
            }


            // ==========================================
            // LẤY NỘI DUNG
            // ==========================================

            string noiDung =
                txtNoiDungDanhGia.Text.Trim();


            if (string.IsNullOrWhiteSpace(
                noiDung))
            {
                ShowReviewMessage(
                    "Vui lòng nhập nội dung đánh giá.");

                return;
            }


            // ==========================================
            // KIỂM TRA ĐỘ DÀI
            // ==========================================

            if (noiDung.Length < 5)
            {
                ShowReviewMessage(
                    "Nội dung đánh giá phải có ít nhất 5 ký tự.");

                return;
            }


            if (noiDung.Length > 2000)
            {
                ShowReviewMessage(
                    "Nội dung đánh giá không được vượt quá 2000 ký tự.");

                return;
            }


            // ==========================================
            // LẤY MÃ KHÁCH HÀNG
            // ==========================================

            int maKhachHang;


            if (!int.TryParse(
                Session["UserId"].ToString(),
                out maKhachHang)
                ||
                maKhachHang <= 0)
            {
                ShowReviewMessage(
                    "Phiên đăng nhập không hợp lệ.");

                return;
            }


            // ==========================================
            // INSERT ĐÁNH GIÁ
            // ==========================================

            try
            {
                bool success =
                    danhGiaDAL.Insert(
                        maSanPham,
                        maKhachHang,
                        noiDung,
                        soSao);


                if (!success)
                {
                    ShowReviewMessage(
                        "Không thể gửi đánh giá. " +
                        "Vui lòng thử lại.");

                    return;
                }


                // ======================================
                // THÀNH CÔNG
                // ======================================

                txtNoiDungDanhGia.Text = "";


                if (ddlSoSao.Items.Count > 0)
                {
                    ddlSoSao.SelectedValue = "5";
                }


                ShowReviewMessage(
                    "Đánh giá của bạn đã được gửi thành công.");


                // Load lại danh sách đánh giá
                LoadDanhGia(
                    maSanPham);
            }
            catch (Exception)
            {
                ShowReviewMessage(
                    "Không thể gửi đánh giá. " +
                    "Vui lòng thử lại sau.");
            }
        }


        // =====================================================
        // HIỂN THỊ THÔNG BÁO ĐÁNH GIÁ
        // =====================================================

        private void ShowReviewMessage(
            string message)
        {
            if (lblReviewMessage != null)
            {
                lblReviewMessage.Text =
                    Server.HtmlEncode(
                        message);
            }
        }


        // =====================================================
        // THÊM VÀO GIỎ HÀNG
        // =====================================================

        protected void btnAddToCart_Click(
            object sender,
            EventArgs e)
        {
            string id =
                Request.QueryString["id"];


            int maSanPham;


            if (!int.TryParse(
                id,
                out maSanPham)
                ||
                maSanPham <= 0)
            {
                Response.Redirect(
                    "shop.aspx");

                return;
            }


            // ==========================================
            // KIỂM TRA SỐ LƯỢNG
            // ==========================================

            int soLuong;


            if (!int.TryParse(
                txtQuantity.Text,
                out soLuong)
                ||
                soLuong <= 0)
            {
                lblMessage.Text =
                    "Số lượng phải lớn hơn 0.";

                return;
            }


            try
            {
                // ======================================
                // KIỂM TRA SẢN PHẨM
                // ======================================

                DataTable table =
                    sanPhamDAL.GetById(
                        maSanPham);


                if (table == null ||
                    table.Rows.Count == 0)
                {
                    lblMessage.Text =
                        "Sản phẩm không tồn tại.";

                    return;
                }


                DataRow row =
                    table.Rows[0];


                bool trangThai =
                    Convert.ToBoolean(
                        row["TrangThai"]);


                int tonKho =
                    Convert.ToInt32(
                        row["SoLuong"]);


                // ======================================
                // KIỂM TRA ĐANG BÁN
                // ======================================

                if (!trangThai)
                {
                    lblMessage.Text =
                        "Sản phẩm hiện không còn được bán.";

                    btnAddToCart.Enabled =
                        false;

                    return;
                }


                // ======================================
                // KIỂM TRA TỒN KHO
                // ======================================

                if (tonKho <= 0)
                {
                    lblMessage.Text =
                        "Sản phẩm đã hết hàng.";

                    btnAddToCart.Enabled =
                        false;

                    return;
                }


                if (soLuong > tonKho)
                {
                    lblMessage.Text =
                        "Số lượng mua không được vượt quá "
                        + tonKho
                        + " sản phẩm tồn kho.";

                    return;
                }


                // ======================================
                // THÊM VÀO CART
                // ======================================

                string message;


                bool success =
                    cartService.Add(
                        maSanPham,
                        "",
                        "",
                        0,
                        soLuong,
                        out message);


                if (!success)
                {
                    lblMessage.Text =
                        Server.HtmlEncode(
                            message);

                    return;
                }


                Response.Redirect(
                    "Cart.aspx");
            }
            catch (Exception)
            {
                lblMessage.Text =
                    "Không thể thêm sản phẩm vào giỏ hàng. " +
                    "Vui lòng thử lại.";
            }
        }
    }
}