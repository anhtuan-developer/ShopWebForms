using System;

namespace web_ban_hang2.Models
{
    [Serializable]
    public class KhachHang
    {
        public int MaKhachHang { get; set; }

        public string HoTen { get; set; }

        public string Email { get; set; }

        public string MatKhau { get; set; }

        public string SoDienThoai { get; set; }

        public string DiaChi { get; set; }

        public DateTime NgayTao { get; set; }
    }
}