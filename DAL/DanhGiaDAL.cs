using System;
using System.Data;
using System.Data.SqlClient;

namespace web_ban_hang2.DAL
{
    public class DanhGiaDAL
    {
        private readonly Database database;

        public DanhGiaDAL()
        {
            database = new Database();
        }

        // ==========================================
        // LẤY ĐÁNH GIÁ THEO SẢN PHẨM
        // ==========================================

        public DataTable GetByProductId(int maSanPham)
        {
            string sql = @"
                SELECT
                    dg.MaDanhGia,
                    dg.MaSanPham,
                    dg.MaKhachHang,
                    dg.NoiDung,
                    dg.SoSao,
                    dg.NgayDanhGia,
                    dg.TrangThai,
                    kh.HoTen
                FROM DanhGia dg
                INNER JOIN KhachHang kh
                    ON dg.MaKhachHang = kh.MaKhachHang
                WHERE dg.MaSanPham = @MaSanPham
                    AND dg.TrangThai = 1
                ORDER BY dg.NgayDanhGia DESC
            ";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add(
                    "@MaSanPham",
                    SqlDbType.Int
                ).Value = maSanPham;

                DataTable table = new DataTable();

                adapter.Fill(table);

                return table;
            }
        }

        // ==========================================
        // TÍNH ĐIỂM TRUNG BÌNH
        // ==========================================

        public decimal GetAverageRating(int maSanPham)
        {
            string sql = @"
                SELECT ISNULL(AVG(CAST(SoSao AS DECIMAL(10,2))), 0)
                FROM DanhGia
                WHERE MaSanPham = @MaSanPham
                    AND TrangThai = 1
            ";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@MaSanPham",
                    SqlDbType.Int
                ).Value = maSanPham;

                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    return 0;
                }

                return Convert.ToDecimal(result);
            }
        }

        // ==========================================
        // ĐẾM SỐ ĐÁNH GIÁ
        // ==========================================

        public int CountByProductId(int maSanPham)
        {
            string sql = @"
                SELECT COUNT(*)
                FROM DanhGia
                WHERE MaSanPham = @MaSanPham
                    AND TrangThai = 1
            ";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@MaSanPham",
                    SqlDbType.Int
                ).Value = maSanPham;

                conn.Open();

                return Convert.ToInt32(
                    cmd.ExecuteScalar()
                );
            }
        }

        // ==========================================
        // THÊM ĐÁNH GIÁ
        // ==========================================

        public bool Insert(
            int maSanPham,
            int maKhachHang,
            string noiDung,
            int soSao)
        {
            string sql = @"
                INSERT INTO DanhGia
                (
                    MaSanPham,
                    MaKhachHang,
                    NoiDung,
                    SoSao,
                    NgayDanhGia,
                    TrangThai
                )
                VALUES
                (
                    @MaSanPham,
                    @MaKhachHang,
                    @NoiDung,
                    @SoSao,
                    GETDATE(),
                    1
                )
            ";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@MaSanPham",
                    SqlDbType.Int
                ).Value = maSanPham;

                cmd.Parameters.Add(
                    "@MaKhachHang",
                    SqlDbType.Int
                ).Value = maKhachHang;

                cmd.Parameters.Add(
                    "@NoiDung",
                    SqlDbType.NVarChar,
                    2000
                ).Value = noiDung;

                cmd.Parameters.Add(
                    "@SoSao",
                    SqlDbType.Int
                ).Value = soSao;

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ==========================================
        // LẤY TẤT CẢ ĐÁNH GIÁ - ADMIN
        // ==========================================

        public DataTable GetAll()
        {
            string sql = @"
                SELECT
                    dg.MaDanhGia,
                    dg.MaSanPham,
                    sp.TenSanPham,
                    dg.MaKhachHang,
                    kh.HoTen,
                    dg.NoiDung,
                    dg.SoSao,
                    dg.NgayDanhGia,
                    dg.TrangThai
                FROM DanhGia dg
                INNER JOIN SanPham sp
                    ON dg.MaSanPham = sp.MaSanPham
                INNER JOIN KhachHang kh
                    ON dg.MaKhachHang = kh.MaKhachHang
                ORDER BY dg.NgayDanhGia DESC
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

        // ==========================================
        // ẨN / HIỆN ĐÁNH GIÁ
        // ==========================================

        public bool UpdateStatus(
            int maDanhGia,
            bool trangThai)
        {
            string sql = @"
                UPDATE DanhGia
                SET TrangThai = @TrangThai
                WHERE MaDanhGia = @MaDanhGia
            ";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@MaDanhGia",
                    SqlDbType.Int
                ).Value = maDanhGia;

                cmd.Parameters.Add(
                    "@TrangThai",
                    SqlDbType.Bit
                ).Value = trangThai;

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}