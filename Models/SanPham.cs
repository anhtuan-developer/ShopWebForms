using System;

namespace web_ban_hang2.Models
{
    public class SanPham
    {
        public int MaSanPham { get; set; }

        public int MaDanhMuc { get; set; }

        public string TenSanPham { get; set; }

        public string MoTa { get; set; }

        public decimal Gia { get; set; }

        public int SoLuong { get; set; }

        public string HinhAnh { get; set; }
        public bool NoiBat { get; set; }

        public bool TrangThai { get; set; }

        public DateTime NgayTao { get; set; }

        public string TenDanhMuc { get; set; }
    }
}