using System;

namespace web_ban_hang2.Models
{
    public class DanhGia
    {
        public int MaDanhGia { get; set; }

        public int MaSanPham { get; set; }

        public int MaKhachHang { get; set; }

        public string NoiDung { get; set; }

        public int SoSao { get; set; }

        public DateTime NgayDanhGia { get; set; }

        public bool TrangThai { get; set; }
    }
}