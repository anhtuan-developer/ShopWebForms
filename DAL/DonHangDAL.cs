using System;
using System.Data;
using System.Data.SqlClient;
using web_ban_hang2.Models;

namespace web_ban_hang2.DAL
{
    public class DonHangDAL
    {
        private readonly Database database;

        public DonHangDAL()
        {
            database = new Database();
        }


        // ==========================================
        // LẤY TẤT CẢ ĐƠN HÀNG
        // ==========================================

        public DataTable GetAll()
        {
            string sql = @"
                SELECT
                    dh.MaDonHang,
                    dh.MaKhachHang,
                    kh.HoTen AS TenKhachHang,
                    dh.HoTenNguoiNhan,
                    dh.SoDienThoai,
                    dh.DiaChiGiaoHang,
                    dh.TongTien,
                    dh.TrangThai,
                    dh.NgayDat

                FROM DonHang dh

                LEFT JOIN KhachHang kh
                    ON dh.MaKhachHang = kh.MaKhachHang

                ORDER BY dh.MaDonHang DESC
            ";

            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(cmd))
                    {
                        DataTable table =
                            new DataTable();

                        adapter.Fill(table);

                        return table;
                    }
                }
            }
        }


        // ==========================================
        // LẤY ĐƠN HÀNG THEO MÃ
        // ==========================================

        public DataTable GetById(
            int maDonHang)
        {
            string sql = @"
                SELECT
                    dh.MaDonHang,
                    dh.MaKhachHang,
                    kh.HoTen AS TenKhachHang,
                    dh.HoTenNguoiNhan,
                    dh.SoDienThoai,
                    dh.DiaChiGiaoHang,
                    dh.TongTien,
                    dh.TrangThai,
                    dh.NgayDat

                FROM DonHang dh

                LEFT JOIN KhachHang kh
                    ON dh.MaKhachHang = kh.MaKhachHang

                WHERE dh.MaDonHang = @MaDonHang
            ";

            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaDonHang",
                        SqlDbType.Int
                    ).Value = maDonHang;

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(cmd))
                    {
                        DataTable table =
                            new DataTable();

                        adapter.Fill(table);

                        return table;
                    }
                }
            }
        }


        // ==========================================
        // TẠO ĐƠN HÀNG
        // ==========================================

        public int TaoDonHang(
            DonHang donHang)
        {
            string sql = @"
                INSERT INTO DonHang
                (
                    MaKhachHang,
                    HoTenNguoiNhan,
                    SoDienThoai,
                    DiaChiGiaoHang,
                    TongTien,
                    TrangThai,
                    NgayDat
                )
                VALUES
                (
                    @MaKhachHang,
                    @HoTenNguoiNhan,
                    @SoDienThoai,
                    @DiaChiGiaoHang,
                    @TongTien,
                    @TrangThai,
                    GETDATE()
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    // ======================================
                    // MÃ KHÁCH HÀNG
                    // ======================================

                    SqlParameter pMaKhachHang =
                        cmd.Parameters.Add(
                            "@MaKhachHang",
                            SqlDbType.Int
                        );

                    if (donHang.MaKhachHang.HasValue)
                    {
                        pMaKhachHang.Value =
                            donHang.MaKhachHang.Value;
                    }
                    else
                    {
                        pMaKhachHang.Value =
                            DBNull.Value;
                    }


                    // ======================================
                    // HỌ TÊN NGƯỜI NHẬN
                    // ======================================

                    cmd.Parameters.Add(
                        "@HoTenNguoiNhan",
                        SqlDbType.NVarChar,
                        100
                    ).Value =
                        string.IsNullOrWhiteSpace(
                            donHang.HoTenNguoiNhan)
                            ? (object)DBNull.Value
                            : donHang.HoTenNguoiNhan;


                    // ======================================
                    // SỐ ĐIỆN THOẠI
                    // ======================================

                    cmd.Parameters.Add(
                        "@SoDienThoai",
                        SqlDbType.VarChar,
                        20
                    ).Value =
                        string.IsNullOrWhiteSpace(
                            donHang.SoDienThoai)
                            ? (object)DBNull.Value
                            : donHang.SoDienThoai;


                    // ======================================
                    // ĐỊA CHỈ
                    // ======================================

                    cmd.Parameters.Add(
                        "@DiaChiGiaoHang",
                        SqlDbType.NVarChar,
                        255
                    ).Value =
                        string.IsNullOrWhiteSpace(
                            donHang.DiaChiGiaoHang)
                            ? (object)DBNull.Value
                            : donHang.DiaChiGiaoHang;


                    // ======================================
                    // TỔNG TIỀN
                    // ======================================

                    SqlParameter pTongTien =
                        cmd.Parameters.Add(
                            "@TongTien",
                            SqlDbType.Decimal
                        );

                    pTongTien.Precision = 18;
                    pTongTien.Scale = 2;
                    pTongTien.Value =
                        donHang.TongTien;


                    // ======================================
                    // TRẠNG THÁI
                    // ======================================

                    cmd.Parameters.Add(
                        "@TrangThai",
                        SqlDbType.NVarChar,
                        50
                    ).Value =
                        string.IsNullOrWhiteSpace(
                            donHang.TrangThai)
                            ? "Chờ xử lý"
                            : donHang.TrangThai;


                    // ======================================
                    // THỰC THI
                    // ======================================

                    conn.Open();

                    object result =
                        cmd.ExecuteScalar();


                    if (result == null ||
                        result == DBNull.Value)
                    {
                        return 0;
                    }


                    return Convert.ToInt32(result);
                }
            }
        }


        // ==========================================
        // ĐẾM TỔNG SỐ ĐƠN HÀNG
        // ==========================================

        public int CountAll()
        {
            string sql = @"
                SELECT COUNT(*)
                FROM DonHang
            ";

            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    conn.Open();

                    return Convert.ToInt32(
                        cmd.ExecuteScalar()
                    );
                }
            }
        }


        // ==========================================
        // ĐẾM ĐƠN HÀNG THEO TRẠNG THÁI
        // ==========================================

        public int CountByStatus(
            string trangThai)
        {
            string sql = @"
                SELECT COUNT(*)
                FROM DonHang
                WHERE TrangThai = @TrangThai
            ";

            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@TrangThai",
                        SqlDbType.NVarChar,
                        50
                    ).Value =
                        trangThai;

                    conn.Open();

                    return Convert.ToInt32(
                        cmd.ExecuteScalar()
                    );
                }
            }
        }


        // ==========================================
        // CẬP NHẬT TRẠNG THÁI ĐƠN HÀNG
        // ==========================================

        public bool UpdateTrangThai(
            int maDonHang,
            string trangThai)
        {
            string sql = @"
                UPDATE DonHang
                SET TrangThai = @TrangThai
                WHERE MaDonHang = @MaDonHang
            ";

            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaDonHang",
                        SqlDbType.Int
                    ).Value =
                        maDonHang;


                    cmd.Parameters.Add(
                        "@TrangThai",
                        SqlDbType.NVarChar,
                        50
                    ).Value =
                        trangThai;


                    conn.Open();


                    int result =
                        cmd.ExecuteNonQuery();


                    return result > 0;
                }
            }
        }
    }
}