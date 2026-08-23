using System;
using System.Collections.Generic;

namespace web_ban_hang2.Models
{
    public class DonHang
    {
        public int MaDonHang { get; set; }

        public int? MaKhachHang { get; set; }

        public string HoTenNguoiNhan { get; set; }

        public string SoDienThoai { get; set; }

        public string DiaChiGiaoHang { get; set; }

        public decimal TongTien { get; set; }

        public string TrangThai { get; set; }

        public DateTime NgayDat { get; set; }

        public List<ChiTietDonHang> ChiTiet { get; set; }

        public DonHang()
        {
            ChiTiet = new List<ChiTietDonHang>();
        }
    }
}