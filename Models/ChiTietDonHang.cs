namespace web_ban_hang2.Models
{
    public class ChiTietDonHang
    {
        public int MaChiTiet { get; set; }

        public int MaDonHang { get; set; }

        public int MaSanPham { get; set; }

        public int SoLuong { get; set; }

        public decimal DonGia { get; set; }

        public decimal ThanhTien
        {
            get
            {
                return SoLuong * DonGia;
            }
        }
    }
}