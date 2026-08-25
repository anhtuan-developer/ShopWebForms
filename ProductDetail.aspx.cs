using System;
using System.Data;
using web_ban_hang2.DAL;
using web_ban_hang2.Models;
using web_ban_hang2.Services;

namespace web_ban_hang2
{
    public partial class ProductDetail
        : System.Web.UI.Page
    {
        private SanPhamDAL sanPhamDAL;
        private CartService cartService;

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            sanPhamDAL =
                new SanPhamDAL();

            cartService =
                new CartService();

            if (!IsPostBack)
            {
                LoadProduct();
            }
        }

        private void LoadProduct()
        {
            string id =
                Request.QueryString["id"];

            if (string.IsNullOrEmpty(id))
            {
                Response.Redirect(
                    "shop.aspx");

                return;
            }

            int maSanPham;

            if (!int.TryParse(
                id,
                out maSanPham))
            {
                Response.Redirect(
                    "shop.aspx");

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
                    Response.Redirect(
                        "shop.aspx");

                    return;
                }

                SanPham sanPham =
                    ConvertToSanPham(
                        table.Rows[0]);

                // Không hiển thị sản phẩm
                // đã ngừng bán.
                if (!sanPham.TrangThai)
                {
                    Response.Redirect(
                        "shop.aspx");

                    return;
                }

                DisplayProduct(
                    sanPham);
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Lỗi tải sản phẩm: "
                    + ex.Message;
            }
        }

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
                    ? row["TenDanhMuc"]
                        .ToString()
                    : "";

            return sanPham;
        }

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

            if (!sanPham.TrangThai)
            {
                btnAddToCart.Enabled =
                    false;

                lblStock.InnerText =
                    "Sản phẩm hiện không còn được bán";

                return;
            }

            if (sanPham.SoLuong <= 0)
            {
                btnAddToCart.Enabled =
                    false;

                lblStock.InnerText =
                    "Sản phẩm đã hết hàng";
            }
        }

        protected void btnAddToCart_Click(
            object sender,
            EventArgs e)
        {
            string id =
                Request.QueryString["id"];

            int maSanPham;

            if (!int.TryParse(
                id,
                out maSanPham))
            {
                Response.Redirect(
                    "shop.aspx");

                return;
            }

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
                // CartService tự kiểm tra:
                // - sản phẩm tồn tại
                // - trạng thái bán
                // - tồn kho
                // - số lượng hiện tại trong giỏ
                // - số lượng thêm vào
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
                        message;

                    return;
                }

                Response.Redirect(
                    "Cart.aspx");
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Có lỗi xảy ra: "
                    + ex.Message;
            }
        }
    }
}