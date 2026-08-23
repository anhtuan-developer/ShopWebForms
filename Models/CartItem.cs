using System;

namespace web_ban_hang2.Models
{
    [Serializable]
    public class CartItem
    {
        public int MaSanPham { get; set; }

        public string TenSanPham { get; set; }

        public string HinhAnh { get; set; }

        public decimal Gia { get; set; }

        public int SoLuong { get; set; }

        public decimal ThanhTien
        {
            get
            {
                return Gia * SoLuong;
            }
        }
    }
}