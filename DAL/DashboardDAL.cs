using System;
using System.Data;
using System.Data.SqlClient;

namespace web_ban_hang2.DAL
{
    public class DashboardDAL
    {
        private readonly Database database;

        public DashboardDAL()
        {
            database = new Database();
        }


        // =========================================
        // ĐẾM SẢN PHẨM
        // =========================================

        public int GetTotalSanPham()
        {
            string sql = @"
                SELECT COUNT(*)
                FROM SanPham
            ";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                return Convert.ToInt32(
                    cmd.ExecuteScalar()
                );
            }
        }


        // =========================================
        // ĐẾM DANH MỤC
        // =========================================

        public int GetTotalDanhMuc()
        {
            string sql = @"
                SELECT COUNT(*)
                FROM DanhMuc
            ";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                return Convert.ToInt32(
                    cmd.ExecuteScalar()
                );
            }
        }


        // =========================================
        // ĐẾM KHÁCH HÀNG
        // =========================================

        public int GetTotalKhachHang()
        {
            string sql = @"
                SELECT COUNT(*)
                FROM KhachHang
            ";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                return Convert.ToInt32(
                    cmd.ExecuteScalar()
                );
            }
        }


        // =========================================
        // ĐẾM ĐƠN HÀNG
        // =========================================

        public int GetTotalDonHang()
        {
            string sql = @"
                SELECT COUNT(*)
                FROM DonHang
            ";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                return Convert.ToInt32(
                    cmd.ExecuteScalar()
                );
            }
        }


        // =========================================
        // DOANH THU HÔM NAY
        // Chỉ tính đơn đã giao
        // =========================================

        public decimal GetDoanhThuHomNay()
        {
            string sql = @"
                SELECT ISNULL(SUM(TongTien), 0)
                FROM DonHang
                WHERE TrangThai = N'Đã giao'
                AND CAST(NgayDat AS DATE) = CAST(GETDATE() AS DATE)
            ";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                return Convert.ToDecimal(
                    cmd.ExecuteScalar()
                );
            }
        }


        // =========================================
        // DOANH THU THÁNG NÀY
        // =========================================

        public decimal GetDoanhThuThang()
        {
            string sql = @"
                SELECT ISNULL(SUM(TongTien), 0)
                FROM DonHang
                WHERE TrangThai = N'Đã giao'
                AND YEAR(NgayDat) = YEAR(GETDATE())
                AND MONTH(NgayDat) = MONTH(GETDATE())
            ";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                return Convert.ToDecimal(
                    cmd.ExecuteScalar()
                );
            }
        }


        // =========================================
        // DOANH THU NĂM NAY
        // =========================================

        public decimal GetDoanhThuNam()
        {
            string sql = @"
                SELECT ISNULL(SUM(TongTien), 0)
                FROM DonHang
                WHERE TrangThai = N'Đã giao'
                AND YEAR(NgayDat) = YEAR(GETDATE())
            ";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                return Convert.ToDecimal(
                    cmd.ExecuteScalar()
                );
            }
        }


        // =========================================
        // SỐ ĐƠN ĐÃ GIAO
        // =========================================

        public int GetSoDonDaGiao()
        {
            string sql = @"
                SELECT COUNT(*)
                FROM DonHang
                WHERE TrangThai = N'Đã giao'
            ";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                return Convert.ToInt32(
                    cmd.ExecuteScalar()
                );
            }
        }


        // =========================================
        // SỐ ĐƠN ĐANG GIAO
        // =========================================

        public int GetSoDonDangGiao()
        {
            string sql = @"
                SELECT COUNT(*)
                FROM DonHang
                WHERE TrangThai = N'Đang giao'
            ";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                return Convert.ToInt32(
                    cmd.ExecuteScalar()
                );
            }
        }


        // =========================================
        // TOP 5 SẢN PHẨM BÁN CHẠY
        // =========================================

        public DataTable GetTopSanPhamBanChay()
        {
            string sql = @"
                SELECT TOP 5
                    sp.TenSanPham,
                    SUM(ctdh.SoLuong) AS TongSoLuongBan,
                    SUM(ctdh.ThanhTien) AS DoanhThu
                FROM ChiTietDonHang ctdh
                INNER JOIN DonHang dh
                    ON ctdh.MaDonHang = dh.MaDonHang
                INNER JOIN SanPham sp
                    ON ctdh.MaSanPham = sp.MaSanPham
                WHERE dh.TrangThai = N'Đã giao'
                GROUP BY
                    sp.TenSanPham
                ORDER BY
                    SUM(ctdh.SoLuong) DESC
            ";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();

                adapter.Fill(table);

                return table;
            }
        }
    }
}
