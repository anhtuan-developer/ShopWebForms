using System;

namespace web_ban_hang2.Models
{
    [Serializable]
    public class AdminAccount
    {
        public int MaAdmin { get; set; }

        public string HoTen { get; set; }

        public string Email { get; set; }

        public string MatKhau { get; set; }

        public DateTime NgayTao { get; set; }

        public bool TrangThai { get; set; }
    }
}