using System;
using System.Data;
using System.Data.SqlClient;

namespace web_ban_hang2.DAL
{
    public class DanhMucDAL
    {
        private readonly Database database;

        public DanhMucDAL()
        {
            database = new Database();
        }


        // ==========================================
        // LẤY TẤT CẢ DANH MỤC
        // ==========================================

        public DataTable GetAll()
        {
            string sql = @"
                SELECT
                    MaDanhMuc,
                    TenDanhMuc,
                    MoTa,
                    TrangThai,
                    NgayTao
                FROM DanhMuc
                ORDER BY MaDanhMuc DESC
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
        // THÊM DANH MỤC
        // ==========================================

        public bool Insert(
            string tenDanhMuc,
            string moTa,
            bool trangThai)
        {
            string sql = @"
                INSERT INTO DanhMuc
                (
                    TenDanhMuc,
                    MoTa,
                    TrangThai
                )
                VALUES
                (
                    @TenDanhMuc,
                    @MoTa,
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
                        "@TenDanhMuc",
                        SqlDbType.NVarChar,
                        100
                    ).Value = tenDanhMuc;

                    cmd.Parameters.Add(
                        "@MoTa",
                        SqlDbType.NVarChar,
                        500
                    ).Value =
                        string.IsNullOrWhiteSpace(moTa)
                            ? (object)DBNull.Value
                            : moTa;

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
        // LẤY 1 DANH MỤC THEO MÃ
        // ==========================================

        public DataTable GetById(int maDanhMuc)
        {
            string sql = @"
                SELECT
                    MaDanhMuc,
                    TenDanhMuc,
                    MoTa,
                    TrangThai,
                    NgayTao
                FROM DanhMuc
                WHERE MaDanhMuc = @MaDanhMuc
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
        // CẬP NHẬT DANH MỤC
        // ==========================================

        public bool Update(
            int maDanhMuc,
            string tenDanhMuc,
            string moTa,
            bool trangThai)
        {
            string sql = @"
                UPDATE DanhMuc
                SET
                    TenDanhMuc = @TenDanhMuc,
                    MoTa = @MoTa,
                    TrangThai = @TrangThai
                WHERE
                    MaDanhMuc = @MaDanhMuc
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
                        "@TenDanhMuc",
                        SqlDbType.NVarChar,
                        100
                    ).Value = tenDanhMuc;

                    cmd.Parameters.Add(
                        "@MoTa",
                        SqlDbType.NVarChar,
                        500
                    ).Value =
                        string.IsNullOrWhiteSpace(moTa)
                            ? (object)DBNull.Value
                            : moTa;

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
        public int CountProducts(int maDanhMuc)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM SanPham
        WHERE MaDanhMuc = @MaDanhMuc
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

                    conn.Open();

                    return Convert.ToInt32(
                        cmd.ExecuteScalar()
                    );
                }
            }
        }
        public bool Delete(int maDanhMuc)
        {
            string sql = @"
        DELETE FROM DanhMuc
        WHERE MaDanhMuc = @MaDanhMuc
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

                    conn.Open();

                    int result =
                        cmd.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }

    }

}