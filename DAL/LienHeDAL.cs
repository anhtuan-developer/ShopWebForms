using System;
using System.Data;
using System.Data.SqlClient;

namespace web_ban_hang2.DAL
{
    public class LienHeDAL
    {
        private readonly Database database;

        public LienHeDAL()
        {
            database = new Database();
        }


        // =====================================================
        // THÊM LIÊN HỆ
        // =====================================================

        public bool Insert(
            string hoTen,
            string email,
            string tieuDe,
            string noiDung)
        {
            const string sql = @"
                INSERT INTO LienHe
                (
                    HoTen,
                    Email,
                    TieuDe,
                    NoiDung,
                    NgayGui,
                    TrangThai
                )
                VALUES
                (
                    @HoTen,
                    @Email,
                    @TieuDe,
                    @NoiDung,
                    GETDATE(),
                    0
                )";

            using (SqlConnection conn =
                database.GetConnection())
            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@HoTen",
                    SqlDbType.NVarChar,
                    100
                ).Value = hoTen.Trim();

                cmd.Parameters.Add(
                    "@Email",
                    SqlDbType.VarChar,
                    150
                ).Value = email.Trim();

                cmd.Parameters.Add(
                    "@TieuDe",
                    SqlDbType.NVarChar,
                    250
                ).Value = tieuDe.Trim();

                cmd.Parameters.Add(
                    "@NoiDung",
                    SqlDbType.NVarChar,
                    -1
                ).Value = noiDung.Trim();

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }


        // =====================================================
        // LẤY TẤT CẢ LIÊN HỆ - ADMIN
        // =====================================================

        public DataTable GetAll()
        {
            const string sql = @"
                SELECT
                    MaLienHe,
                    HoTen,
                    Email,
                    TieuDe,
                    NoiDung,
                    NgayGui,
                    TrangThai
                FROM LienHe
                ORDER BY
                    NgayGui DESC,
                    MaLienHe DESC";

            using (SqlConnection conn =
                database.GetConnection())
            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            using (SqlDataAdapter adapter =
                new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();

                adapter.Fill(table);

                return table;
            }
        }


        // =====================================================
        // LẤY LIÊN HỆ THEO MÃ
        // =====================================================

        public DataTable GetById(int maLienHe)
        {
            const string sql = @"
                SELECT
                    MaLienHe,
                    HoTen,
                    Email,
                    TieuDe,
                    NoiDung,
                    NgayGui,
                    TrangThai
                FROM LienHe
                WHERE MaLienHe = @MaLienHe";

            using (SqlConnection conn =
                database.GetConnection())
            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            using (SqlDataAdapter adapter =
                new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add(
                    "@MaLienHe",
                    SqlDbType.Int
                ).Value = maLienHe;

                DataTable table = new DataTable();

                adapter.Fill(table);

                return table;
            }
        }


        // =====================================================
        // CẬP NHẬT TRẠNG THÁI
        // =====================================================

        public bool UpdateStatus(
            int maLienHe,
            bool trangThai)
        {
            const string sql = @"
                UPDATE LienHe
                SET TrangThai = @TrangThai
                WHERE MaLienHe = @MaLienHe";

            using (SqlConnection conn =
                database.GetConnection())
            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@MaLienHe",
                    SqlDbType.Int
                ).Value = maLienHe;

                cmd.Parameters.Add(
                    "@TrangThai",
                    SqlDbType.Bit
                ).Value = trangThai;

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }


        // =====================================================
        // XÓA LIÊN HỆ
        // =====================================================

        public bool Delete(int maLienHe)
        {
            const string sql = @"
                DELETE FROM LienHe
                WHERE MaLienHe = @MaLienHe";

            using (SqlConnection conn =
                database.GetConnection())
            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@MaLienHe",
                    SqlDbType.Int
                ).Value = maLienHe;

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}