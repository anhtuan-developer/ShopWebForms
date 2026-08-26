using System;

namespace web_ban_hang2.Models
{
    [Serializable]
    public class BinhLuan
    {
        public int MaBinhLuan { get; set; }

        public int MaTinTuc { get; set; }

        public int MaKhachHang { get; set; }

        public string HoTen { get; set; }

        public string NoiDung { get; set; }

        public bool TrangThai { get; set; }

        public DateTime NgayBinhLuan { get; set; }
    }
}