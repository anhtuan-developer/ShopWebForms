using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using web_ban_hang2.DAL;
using web_ban_hang2.Models;

namespace web_ban_hang2.Services
{
    public class CartService
    {
        private const string CartSessionKey = "Cart";
        private readonly SanPhamDAL sanPhamDAL;

        public CartService()
        {
            sanPhamDAL = new SanPhamDAL();
        }

        private List<CartItem> GetCart()
        {
            List<CartItem> cart =
                HttpContext.Current.Session[CartSessionKey]
                as List<CartItem>;

            if (cart == null)
            {
                cart = new List<CartItem>();
                HttpContext.Current.Session[CartSessionKey] = cart;
            }

            return cart;
        }

        public List<CartItem> GetItems()
        {
            return GetCart();
        }

        // Kiểm tra lại toàn bộ giỏ hàng với Database.
        // Dùng trước khi hiển thị giỏ và trước Checkout.
        public bool ValidateCart(out string message)
        {
            message = "";
            List<CartItem> cart = GetCart();
            bool valid = true;

            for (int i = cart.Count - 1; i >= 0; i--)
            {
                CartItem item = cart[i];
                DataTable table = sanPhamDAL.GetById(item.MaSanPham);

                if (table == null || table.Rows.Count == 0)
                {
                    cart.RemoveAt(i);
                    message = "Một sản phẩm trong giỏ không còn tồn tại.";
                    valid = false;
                    continue;
                }

                DataRow row = table.Rows[0];
                bool trangThai = Convert.ToBoolean(row["TrangThai"]);
                int tonKho = Convert.ToInt32(row["SoLuong"]);

                if (!trangThai)
                {
                    cart.RemoveAt(i);
                    message = "Một sản phẩm trong giỏ hiện không còn được bán.";
                    valid = false;
                    continue;
                }

                if (tonKho <= 0)
                {
                    cart.RemoveAt(i);
                    message = "Một sản phẩm trong giỏ đã hết hàng.";
                    valid = false;
                    continue;
                }

                if (item.SoLuong > tonKho)
                {
                    item.SoLuong = tonKho;
                    message = "Một sản phẩm trong giỏ đã được giảm về số lượng tồn kho hiện tại.";
                }

                // Đồng bộ dữ liệu có thể thay đổi trong Database.
                item.TenSanPham = row["TenSanPham"].ToString();
                item.Gia = Convert.ToDecimal(row["Gia"]);
                item.HinhAnh = row["HinhAnh"] == DBNull.Value
                    ? ""
                    : row["HinhAnh"].ToString();
            }

            SaveCart(cart);
            return valid;
        }

        // Thêm sản phẩm. Không cho thêm sản phẩm không tồn tại,
        // ngừng bán, hết hàng hoặc vượt tồn kho.
        public bool Add(
            int maSanPham,
            string tenSanPham,
            string hinhAnh,
            decimal gia,
            int soLuong,
            out string message)
        {
            message = "";

            if (soLuong <= 0)
            {
                message = "Số lượng phải lớn hơn 0.";
                return false;
            }

            DataTable table = sanPhamDAL.GetById(maSanPham);

            if (table == null || table.Rows.Count == 0)
            {
                message = "Sản phẩm không tồn tại.";
                return false;
            }

            DataRow row = table.Rows[0];
            bool trangThai = Convert.ToBoolean(row["TrangThai"]);
            int tonKho = Convert.ToInt32(row["SoLuong"]);

            if (!trangThai)
            {
                message = "Sản phẩm hiện không còn được bán.";
                return false;
            }

            if (tonKho <= 0)
            {
                message = "Sản phẩm đã hết hàng.";
                return false;
            }

            List<CartItem> cart = GetCart();
            CartItem item = cart.FirstOrDefault(x => x.MaSanPham == maSanPham);
            int soLuongHienTai = item == null ? 0 : item.SoLuong;

            if (soLuongHienTai > tonKho - soLuong)
            {
                message = "Số lượng trong giỏ vượt quá tồn kho. Hiện chỉ còn " + tonKho + " sản phẩm.";
                return false;
            }

            // Luôn lấy dữ liệu mới nhất từ DB thay vì tin giá/tên gửi từ client.
            tenSanPham = row["TenSanPham"].ToString();
            gia = Convert.ToDecimal(row["Gia"]);
            hinhAnh = row["HinhAnh"] == DBNull.Value ? "" : row["HinhAnh"].ToString();

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
                item.TenSanPham = tenSanPham;
                item.HinhAnh = hinhAnh;
                item.Gia = gia;
            }

            SaveCart(cart);
            return true;
        }

        // Giữ API cũ để các nơi khác không bị lỗi compile.
        public void Add(
            int maSanPham,
            string tenSanPham,
            string hinhAnh,
            decimal gia,
            int soLuong)
        {
            string message;
            Add(
                maSanPham,
                tenSanPham,
                hinhAnh,
                gia,
                soLuong,
                out message);
        }

        public bool UpdateQuantity(
            int maSanPham,
            int soLuong,
            out string message)
        {
            message = "";

            List<CartItem> cart = GetCart();

            CartItem item = cart.FirstOrDefault(
                x => x.MaSanPham == maSanPham);

            if (item == null)
            {
                message = "Sản phẩm không có trong giỏ hàng.";
                return false;
            }

            if (soLuong <= 0)
            {
                cart.Remove(item);
                SaveCart(cart);
                return true;
            }

            DataTable table =
                sanPhamDAL.GetById(maSanPham);

            if (table == null || table.Rows.Count == 0)
            {
                cart.Remove(item);
                SaveCart(cart);

                message =
                    "Sản phẩm không còn tồn tại và đã được xóa khỏi giỏ hàng.";

                return false;
            }

            DataRow row = table.Rows[0];

            bool trangThai =
                Convert.ToBoolean(row["TrangThai"]);

            int tonKho =
                Convert.ToInt32(row["SoLuong"]);

            if (!trangThai)
            {
                cart.Remove(item);
                SaveCart(cart);

                message =
                    "Sản phẩm hiện không còn được bán và đã được xóa khỏi giỏ hàng.";

                return false;
            }

            if (tonKho <= 0)
            {
                cart.Remove(item);
                SaveCart(cart);

                message =
                    "Sản phẩm đã hết hàng và đã được xóa khỏi giỏ hàng.";

                return false;
            }

            if (soLuong > tonKho)
            {
                message =
                    "Số lượng vượt quá tồn kho. Hiện chỉ còn "
                    + tonKho
                    + " sản phẩm.";

                return false;
            }

            item.SoLuong = soLuong;

            item.TenSanPham =
                row["TenSanPham"].ToString();

            item.Gia =
                Convert.ToDecimal(row["Gia"]);

            item.HinhAnh =
                row["HinhAnh"] == DBNull.Value
                    ? ""
                    : row["HinhAnh"].ToString();

            SaveCart(cart);

            return true;
        }

        public void UpdateQuantity(
            int maSanPham,
            int soLuong)
        {
            string message;

            UpdateQuantity(
                maSanPham,
                soLuong,
                out message);
        }

        public void Remove(int maSanPham)
        {
            List<CartItem> cart = GetCart();

            CartItem item =
                cart.FirstOrDefault(
                    x => x.MaSanPham == maSanPham);

            if (item != null)
            {
                cart.Remove(item);
            }

            SaveCart(cart);
        }

        public void Clear()
        {
            HttpContext.Current.Session.Remove(
                CartSessionKey);
        }

        public decimal GetTotal()
        {
            return GetCart()
                .Sum(x => x.ThanhTien);
        }

        public int GetTotalQuantity()
        {
            return GetCart()
                .Sum(x => x.SoLuong);
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Current.Session[
                CartSessionKey] = cart;
        }
    }
}