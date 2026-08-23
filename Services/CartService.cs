using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_ban_hang2.Models;

namespace web_ban_hang2.Services
{
    public class CartService
    {
        private const string CartSessionKey = "Cart";

        private List<CartItem> GetCart()
        {
            var cart =
                HttpContext.Current.Session[CartSessionKey]
                as List<CartItem>;

            if (cart == null)
            {
                cart = new List<CartItem>();

                HttpContext.Current.Session[CartSessionKey] = cart;
            }

            return cart;
        }


        // Lấy toàn bộ giỏ hàng
        public List<CartItem> GetItems()
        {
            return GetCart();
        }


        // Thêm sản phẩm
        public void Add(
            int maSanPham,
            string tenSanPham,
            string hinhAnh,
            decimal gia,
            int soLuong)
        {
            List<CartItem> cart = GetCart();

            CartItem item =
                cart.FirstOrDefault(
                    x => x.MaSanPham == maSanPham
                );

            if (item == null)
            {
                item = new CartItem
                {
                    MaSanPham = maSanPham,
                    TenSanPham = tenSanPham,
                    HinhAnh = hinhAnh,
                    Gia = gia,
                    SoLuong = soLuong
                };

                cart.Add(item);
            }
            else
            {
                item.SoLuong += soLuong;
            }

            SaveCart(cart);
        }


        // Cập nhật số lượng
        public void UpdateQuantity(
            int maSanPham,
            int soLuong)
        {
            List<CartItem> cart = GetCart();

            CartItem item =
                cart.FirstOrDefault(
                    x => x.MaSanPham == maSanPham
                );

            if (item == null)
            {
                return;
            }

            if (soLuong <= 0)
            {
                cart.Remove(item);
            }
            else
            {
                item.SoLuong = soLuong;
            }

            SaveCart(cart);
        }


        // Xóa sản phẩm
        public void Remove(int maSanPham)
        {
            List<CartItem> cart = GetCart();

            CartItem item =
                cart.FirstOrDefault(
                    x => x.MaSanPham == maSanPham
                );

            if (item != null)
            {
                cart.Remove(item);
            }

            SaveCart(cart);
        }


        // Xóa toàn bộ giỏ hàng
        public void Clear()
        {
            HttpContext.Current.Session.Remove(
                CartSessionKey
            );
        }


        // Tổng tiền
        public decimal GetTotal()
        {
            return GetCart()
                .Sum(x => x.ThanhTien);
        }


        // Tổng số lượng
        public int GetTotalQuantity()
        {
            return GetCart()
                .Sum(x => x.SoLuong);
        }


        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Current.Session[
                CartSessionKey
            ] = cart;
        }
    }
}