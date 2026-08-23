using System;
using System.Collections.Generic;
using web_ban_hang2.DAL;
using web_ban_hang2.Models;
using web_ban_hang2.Services;

namespace web_ban_hang2
{
    public partial class ProductDetail : System.Web.UI.Page
    {
        private SanPhamDAL sanPhamDAL;

        private CartService cartService;


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
                out maSanPham))
            {
                Response.Redirect("shop.aspx");

                return;
            }


            List<SanPham> danhSach =
                sanPhamDAL.GetAll();


            SanPham sanPham =
                danhSach.Find(
                    x => x.MaSanPham == maSanPham
                );


            if (sanPham == null)
            {
                Response.Redirect("shop.aspx");

                return;
            }


            DisplayProduct(sanPham);
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
                    sanPham.Gia
                );


            lblStock.InnerText =
                "Còn lại: " +
                sanPham.SoLuong +
                " sản phẩm";


            lblDescription.InnerText =
                sanPham.MoTa;


            imgProduct.ImageUrl =
                ResolveUrl(
                    "~/img/" +
                    sanPham.HinhAnh
                );


            imgProduct.AlternateText =
                sanPham.TenSanPham;


            if (sanPham.SoLuong <= 0)
            {
                btnAddToCart.Enabled = false;

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
                Response.Redirect("shop.aspx");

                return;
            }


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


            List<SanPham> danhSach =
                sanPhamDAL.GetAll();


            SanPham sanPham =
                danhSach.Find(
                    x => x.MaSanPham == maSanPham
                );


            if (sanPham == null)
            {
                lblMessage.Text =
                    "Sản phẩm không tồn tại.";

                return;
            }


            if (sanPham.SoLuong <= 0)
            {
                lblMessage.Text =
                    "Sản phẩm đã hết hàng.";

                return;
            }


            if (soLuong > sanPham.SoLuong)
            {
                lblMessage.Text =
                    "Số lượng mua vượt quá số lượng tồn kho.";

                return;
            }


            cartService.Add(
                sanPham.MaSanPham,
                sanPham.TenSanPham,
                sanPham.HinhAnh,
                sanPham.Gia,
                soLuong
            );


            Response.Redirect("Cart.aspx");
        }
    }
}