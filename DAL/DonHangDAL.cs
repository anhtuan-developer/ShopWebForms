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
        // LẤY CHI TIẾT ĐƠN HÀNG
        // ==========================================

        public DataTable GetChiTietByDonHang(
            int maDonHang)
        {
            string sql = @"
                SELECT
                    ctdh.MaChiTiet,
                    ctdh.MaDonHang,
                    ctdh.MaSanPham,
                    sp.TenSanPham,
                    ctdh.SoLuong,
                    ctdh.DonGia,
                    ctdh.ThanhTien
                FROM ChiTietDonHang ctdh
                INNER JOIN SanPham sp
                    ON ctdh.MaSanPham = sp.MaSanPham
                WHERE ctdh.MaDonHang = @MaDonHang
                ORDER BY ctdh.MaChiTiet ASC
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
            string insertOrderSql = @"
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


            string insertDetailSql = @"
                INSERT INTO ChiTietDonHang
                (
                    MaDonHang,
                    MaSanPham,
                    SoLuong,
                    DonGia
                )
                VALUES
                (
                    @MaDonHang,
                    @MaSanPham,
                    @SoLuong,
                    @DonGia
                );
            ";


            using (SqlConnection conn =
                database.GetConnection())
            {
                conn.Open();


                using (SqlTransaction transaction =
                    conn.BeginTransaction())
                {
                    try
                    {
                        int maDonHang;


                        // ==================================
                        // TẠO ĐƠN HÀNG
                        // ==================================

                        using (SqlCommand cmd =
                            new SqlCommand(
                                insertOrderSql,
                                conn,
                                transaction))
                        {
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


                            cmd.Parameters.Add(
                                "@HoTenNguoiNhan",
                                SqlDbType.NVarChar,
                                100
                            ).Value =
                                donHang.HoTenNguoiNhan;


                            cmd.Parameters.Add(
                                "@SoDienThoai",
                                SqlDbType.VarChar,
                                20
                            ).Value =
                                donHang.SoDienThoai;


                            cmd.Parameters.Add(
                                "@DiaChiGiaoHang",
                                SqlDbType.NVarChar,
                                255
                            ).Value =
                                donHang.DiaChiGiaoHang;


                            SqlParameter pTongTien =
                                cmd.Parameters.Add(
                                    "@TongTien",
                                    SqlDbType.Decimal
                                );

                            pTongTien.Precision = 18;
                            pTongTien.Scale = 2;
                            pTongTien.Value =
                                donHang.TongTien;


                            cmd.Parameters.Add(
                                "@TrangThai",
                                SqlDbType.NVarChar,
                                50
                            ).Value =
                                string.IsNullOrWhiteSpace(
                                    donHang.TrangThai)
                                    ? "Chờ xử lý"
                                    : donHang.TrangThai;


                            object result =
                                cmd.ExecuteScalar();


                            if (result == null ||
                                result == DBNull.Value)
                            {
                                transaction.Rollback();

                                return 0;
                            }


                            maDonHang =
                                Convert.ToInt32(
                                    result
                                );
                        }


                        // ==================================
                        // LƯU CHI TIẾT ĐƠN HÀNG
                        // ==================================

                        if (donHang.ChiTiet != null)
                        {
                            foreach (
                                ChiTietDonHang chiTiet
                                in donHang.ChiTiet)
                            {
                                using (SqlCommand cmd =
                                    new SqlCommand(
                                        insertDetailSql,
                                        conn,
                                        transaction))
                                {
                                    cmd.Parameters.Add(
                                        "@MaDonHang",
                                        SqlDbType.Int
                                    ).Value =
                                        maDonHang;


                                    cmd.Parameters.Add(
                                        "@MaSanPham",
                                        SqlDbType.Int
                                    ).Value =
                                        chiTiet.MaSanPham;


                                    cmd.Parameters.Add(
                                        "@SoLuong",
                                        SqlDbType.Int
                                    ).Value =
                                        chiTiet.SoLuong;


                                    SqlParameter pDonGia =
                                        cmd.Parameters.Add(
                                            "@DonGia",
                                            SqlDbType.Decimal
                                        );

                                    pDonGia.Precision = 18;
                                    pDonGia.Scale = 2;
                                    pDonGia.Value =
                                        chiTiet.DonGia;


                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }


                        // ==================================
                        // HOÀN TẤT TRANSACTION
                        // ==================================

                        transaction.Commit();


                        return maDonHang;
                    }
                    catch
                    {
                        transaction.Rollback();

                        throw;
                    }
                }
            }
        }


        // ==========================================
        // ĐẾM TỔNG ĐƠN HÀNG
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
        // ĐẾM THEO TRẠNG THÁI
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
                    ).Value = trangThai;


                    conn.Open();


                    return Convert.ToInt32(
                        cmd.ExecuteScalar()
                    );
                }
            }
        }


        // ==========================================
        // CẬP NHẬT TRẠNG THÁI
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
                    ).Value = maDonHang;


                    cmd.Parameters.Add(
                        "@TrangThai",
                        SqlDbType.NVarChar,
                        50
                    ).Value = trangThai;


                    conn.Open();


                    return
                        cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}