using System;
using System.Data;
using System.Data.SqlClient;

namespace web_ban_hang2.DAL
{
    public class SanPhamDAL
    {
        private readonly Database database;

        public SanPhamDAL()
        {
            database = new Database();
        }


        // ==========================================
        // LẤY TẤT CẢ SẢN PHẨM
        // ==========================================

        public DataTable GetAll()
        {
            string sql = @"
                SELECT
                    sp.MaSanPham,
                    sp.MaDanhMuc,
                    sp.TenSanPham,
                    sp.MoTa,
                    sp.Gia,
                    sp.SoLuong,
                    sp.HinhAnh,
                    sp.TrangThai,
                    sp.NgayTao,

                    dm.TenDanhMuc

                FROM SanPham sp

                INNER JOIN DanhMuc dm
                    ON sp.MaDanhMuc = dm.MaDanhMuc

                ORDER BY sp.MaSanPham DESC
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
        // LẤY SẢN PHẨM THEO MÃ
        // ==========================================

        public DataTable GetById(int maSanPham)
        {
            string sql = @"
                SELECT
                    MaSanPham,
                    MaDanhMuc,
                    TenSanPham,
                    MoTa,
                    Gia,
                    SoLuong,
                    HinhAnh,
                    TrangThai,
                    NgayTao

                FROM SanPham

                WHERE MaSanPham = @MaSanPham
            ";

            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaSanPham",
                        SqlDbType.Int
                    ).Value = maSanPham;

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
        // THÊM SẢN PHẨM
        // ==========================================

        public bool Insert(
            int maDanhMuc,
            string tenSanPham,
            string moTa,
            decimal gia,
            int soLuong,
            string hinhAnh,
            bool trangThai)
        {
            string sql = @"
                INSERT INTO SanPham
                (
                    MaDanhMuc,
                    TenSanPham,
                    MoTa,
                    Gia,
                    SoLuong,
                    HinhAnh,
                    TrangThai
                )

                VALUES
                (
                    @MaDanhMuc,
                    @TenSanPham,
                    @MoTa,
                    @Gia,
                    @SoLuong,
                    @HinhAnh,
                    @TrangThai
                )
            ";

            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaDanhMuc",
                        SqlDbType.Int
                    ).Value = maDanhMuc;

                    cmd.Parameters.Add(
                        "@TenSanPham",
                        SqlDbType.NVarChar,
                        200
                    ).Value = tenSanPham;

                    cmd.Parameters.Add(
                        "@MoTa",
                        SqlDbType.NVarChar
                    ).Value =
                        string.IsNullOrWhiteSpace(moTa)
                            ? (object)DBNull.Value
                            : moTa;

                    cmd.Parameters.Add(
                        "@Gia",
                        SqlDbType.Decimal
                    ).Value = gia;

                    cmd.Parameters.Add(
                        "@SoLuong",
                        SqlDbType.Int
                    ).Value = soLuong;

                    cmd.Parameters.Add(
                        "@HinhAnh",
                        SqlDbType.NVarChar,
                        500
                    ).Value =
                        string.IsNullOrWhiteSpace(hinhAnh)
                            ? (object)DBNull.Value
                            : hinhAnh;

                    cmd.Parameters.Add(
                        "@TrangThai",
                        SqlDbType.Bit
                    ).Value = trangThai;

                    conn.Open();

                    int result =
                        cmd.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }


        // ==========================================
        // CẬP NHẬT SẢN PHẨM
        // ==========================================

        public bool Update(
            int maSanPham,
            int maDanhMuc,
            string tenSanPham,
            string moTa,
            decimal gia,
            int soLuong,
            string hinhAnh,
            bool trangThai)
        {
            string sql = @"
                UPDATE SanPham

                SET
                    MaDanhMuc = @MaDanhMuc,
                    TenSanPham = @TenSanPham,
                    MoTa = @MoTa,
                    Gia = @Gia,
                    SoLuong = @SoLuong,
                    HinhAnh = @HinhAnh,
                    TrangThai = @TrangThai

                WHERE
                    MaSanPham = @MaSanPham
            ";

            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaSanPham",
                        SqlDbType.Int
                    ).Value = maSanPham;

                    cmd.Parameters.Add(
                        "@MaDanhMuc",
                        SqlDbType.Int
                    ).Value = maDanhMuc;

                    cmd.Parameters.Add(
                        "@TenSanPham",
                        SqlDbType.NVarChar,
                        200
                    ).Value = tenSanPham;

                    cmd.Parameters.Add(
                        "@MoTa",
                        SqlDbType.NVarChar
                    ).Value =
                        string.IsNullOrWhiteSpace(moTa)
                            ? (object)DBNull.Value
                            : moTa;

                    cmd.Parameters.Add(
                        "@Gia",
                        SqlDbType.Decimal
                    ).Value = gia;

                    cmd.Parameters.Add(
                        "@SoLuong",
                        SqlDbType.Int
                    ).Value = soLuong;

                    cmd.Parameters.Add(
                        "@HinhAnh",
                        SqlDbType.NVarChar,
                        500
                    ).Value =
                        string.IsNullOrWhiteSpace(hinhAnh)
                            ? (object)DBNull.Value
                            : hinhAnh;

                    cmd.Parameters.Add(
                        "@TrangThai",
                        SqlDbType.Bit
                    ).Value = trangThai;

                    conn.Open();

                    int result =
                        cmd.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }


        // ==========================================
        // XÓA SẢN PHẨM
        // ==========================================

        public bool Delete(int maSanPham)
        {
            string sql = @"
                DELETE FROM SanPham

                WHERE
                    MaSanPham = @MaSanPham
            ";

            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaSanPham",
                        SqlDbType.Int
                    ).Value = maSanPham;

                    conn.Open();

                    int result =
                        cmd.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }
        public int CountOrderDetails(int maSanPham)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM ChiTietDonHang
        WHERE MaSanPham = @MaSanPham
    ";

            using (SqlConnection conn = database.GetConnection())
            {
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
        }
    }
}