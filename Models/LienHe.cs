using System;

namespace web_ban_hang2.Models
{
    public class LienHe
    {
        public int MaLienHe { get; set; }

        public string HoTen { get; set; }

        public string Email { get; set; }

        public string TieuDe { get; set; }

        public string NoiDung { get; set; }

        public DateTime NgayGui { get; set; }

        public bool TrangThai { get; set; }
    }
}