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


        // =========================================================
        // PAGE LOAD
        // =========================================================

        protected void Page_Load(object sender, EventArgs e)
        {
            sanPhamDAL = new SanPhamDAL();
            cartService = new CartService();

            if (!IsPostBack)
            {
                LoadProduct();
            }
        }


        // =========================================================
        // LẤY SẢN PHẨM
        // =========================================================

        private void LoadProduct()
        {
            string id = Request.QueryString["id"];

            // Không có id
            if (string.IsNullOrEmpty(id))
            {
                Response.Redirect("shop.aspx");
                return;
            }


            // Kiểm tra id có phải số nguyên không
            int maSanPham;

            if (!int.TryParse(id, out maSanPham))
            {
                Response.Redirect("shop.aspx");
                return;
            }


            try
            {
                // GetAll() trong SanPhamDAL trả về DataTable
                DataTable table = sanPhamDAL.GetAll();


                // Tìm sản phẩm theo MaSanPham
                DataRow productRow = null;

                foreach (DataRow row in table.Rows)
                {
                    if (Convert.ToInt32(row["MaSanPham"]) == maSanPham)
                    {
                        productRow = row;
                        break;
                    }
                }


                // Không tìm thấy sản phẩm
                if (productRow == null)
                {
                    Response.Redirect("shop.aspx");
                    return;
                }


                // Chuyển DataRow thành đối tượng SanPham
                SanPham sanPham = ConvertToSanPham(productRow);


                // Hiển thị sản phẩm
                DisplayProduct(sanPham);
            }
            catch (Exception ex)
            {
                Response.Write(
                    "<h3>Lỗi tải sản phẩm:</h3>" +
                    "<p>" +
                    ex.Message +
                    "</p>"
                );
            }
        }


        // =========================================================
        // CHUYỂN DATAROW → SANPHAM
        // =========================================================

        private SanPham ConvertToSanPham(DataRow row)
        {
            SanPham sanPham = new SanPham();

            sanPham.MaSanPham =
                Convert.ToInt32(row["MaSanPham"]);

            sanPham.MaDanhMuc =
                Convert.ToInt32(row["MaDanhMuc"]);

            sanPham.TenSanPham =
                row["TenSanPham"].ToString();

            sanPham.MoTa =
                row["MoTa"] == DBNull.Value
                    ? ""
                    : row["MoTa"].ToString();

            sanPham.Gia =
                Convert.ToDecimal(row["Gia"]);

            sanPham.SoLuong =
                Convert.ToInt32(row["SoLuong"]);

            sanPham.HinhAnh =
                row["HinhAnh"] == DBNull.Value
                    ? ""
                    : row["HinhAnh"].ToString();

            sanPham.TrangThai =
                Convert.ToBoolean(row["TrangThai"]);

            sanPham.NgayTao =
                Convert.ToDateTime(row["NgayTao"]);

            // GetAll() của SanPhamDAL có TenDanhMuc
            sanPham.TenDanhMuc =
                row["TenDanhMuc"] == DBNull.Value
                    ? ""
                    : row["TenDanhMuc"].ToString();

            return sanPham;
        }


        // =========================================================
        // HIỂN THỊ CHI TIẾT SẢN PHẨM
        // =========================================================

        private void DisplayProduct(SanPham sanPham)
        {
            // Tên sản phẩm
            lblProductName.InnerText =
                sanPham.TenSanPham;


            // Danh mục
            lblCategory.InnerText =
                sanPham.TenDanhMuc;


            // Giá
            lblPrice.InnerText =
                String.Format(
                    "{0:N0} ₫",
                    sanPham.Gia
                );


            // Số lượng tồn kho
            lblStock.InnerText =
                "Còn lại: " +
                sanPham.SoLuong +
                " sản phẩm";


            // Mô tả
            lblDescription.InnerText =
                sanPham.MoTa;


            // Hình ảnh
            imgProduct.ImageUrl =
                ResolveUrl(
                    "~/img/" +
                    sanPham.HinhAnh
                );


            // Alt hình ảnh
            imgProduct.AlternateText =
                sanPham.TenSanPham;


            // Nếu hết hàng
            if (sanPham.SoLuong <= 0)
            {
                btnAddToCart.Enabled = false;

                lblStock.InnerText =
                    "Sản phẩm đã hết hàng";
            }
        }


        // =========================================================
        // THÊM SẢN PHẨM VÀO GIỎ HÀNG
        // =========================================================

        protected void btnAddToCart_Click(
            object sender,
            EventArgs e)
        {
            string id =
                Request.QueryString["id"];


            // Kiểm tra id
            int maSanPham;

            if (!int.TryParse(
                id,
                out maSanPham))
            {
                Response.Redirect("shop.aspx");
                return;
            }


            // Kiểm tra số lượng người dùng nhập
            int soLuong;

            if (!int.TryParse(
                txtQuantity.Text,
                out soLuong) ||
                soLuong <= 0)
            {
                lblMessage.Text =
                    "Số lượng không hợp lệ.";

                return;
            }


            try
            {
                // Lấy danh sách sản phẩm
                DataTable table =
                    sanPhamDAL.GetAll();


                // Tìm sản phẩm
                DataRow productRow = null;

                foreach (DataRow row in table.Rows)
                {
                    if (Convert.ToInt32(row["MaSanPham"]) == maSanPham)
                    {
                        productRow = row;
                        break;
                    }
                }


                // Không tìm thấy sản phẩm
                if (productRow == null)
                {
                    lblMessage.Text =
                        "Sản phẩm không tồn tại.";

                    return;
                }


                // Chuyển DataRow thành SanPham
                SanPham sanPham =
                    ConvertToSanPham(productRow);


                // Kiểm tra hết hàng
                if (sanPham.SoLuong <= 0)
                {
                    lblMessage.Text =
                        "Sản phẩm đã hết hàng.";

                    return;
                }


                // Kiểm tra số lượng mua
                if (soLuong > sanPham.SoLuong)
                {
                    lblMessage.Text =
                        "Số lượng mua vượt quá số lượng tồn kho.";

                    return;
                }


                // Thêm sản phẩm vào giỏ hàng
                cartService.Add(
                    sanPham.MaSanPham,
                    sanPham.TenSanPham,
                    sanPham.HinhAnh,
                    sanPham.Gia,
                    soLuong
                );


                // Chuyển sang giỏ hàng
                Response.Redirect("Cart.aspx");
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Có lỗi xảy ra: " +
                    ex.Message;
            }
        }
    }
}