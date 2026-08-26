using System;

namespace web_ban_hang2.Models
{
    public class TinTuc
    {
        public int MaTinTuc { get; set; }

        public string TieuDe { get; set; }

        public string NoiDung { get; set; }

        public string HinhAnh { get; set; }

        public bool TrangThai { get; set; }

        public DateTime NgayTao { get; set; }
    }
}