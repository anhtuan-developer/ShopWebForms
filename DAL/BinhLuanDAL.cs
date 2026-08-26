using System.Data;
using System.Data.SqlClient;

namespace web_ban_hang2.DAL
{
    public class BinhLuanDAL
    {
        private readonly Database database;

        public BinhLuanDAL()
        {
            database = new Database();
        }

        // ==========================================
        // LẤY BÌNH LUẬN CỦA MỘT BÀI VIẾT
        // ==========================================

        public DataTable GetByTinTuc(int maTinTuc)
        {
            string sql = @"
                SELECT
                    bl.MaBinhLuan,
                    bl.MaTinTuc,
                    bl.MaKhachHang,
                    kh.HoTen,
                    bl.NoiDung,
                    bl.TrangThai,
                    bl.NgayBinhLuan

                FROM BinhLuan bl

                INNER JOIN KhachHang kh
                    ON bl.MaKhachHang = kh.MaKhachHang

                WHERE
                    bl.MaTinTuc = @MaTinTuc

                    AND bl.TrangThai = 1

                ORDER BY
                    bl.NgayBinhLuan DESC,
                    bl.MaBinhLuan DESC
            ";

            using (SqlConnection conn =
                database.GetConnection())
            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            using (SqlDataAdapter adapter =
                new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add(
                    "@MaTinTuc",
                    SqlDbType.Int
                ).Value = maTinTuc;

                DataTable table =
                    new DataTable();

                adapter.Fill(table);

                return table;
            }
        }


        // ==========================================
        // THÊM BÌNH LUẬN
        // ==========================================

        public bool Insert(
            int maTinTuc,
            int maKhachHang,
            string noiDung)
        {
            string sql = @"
                INSERT INTO BinhLuan
                (
                    MaTinTuc,
                    MaKhachHang,
                    NoiDung,
                    TrangThai,
                    NgayBinhLuan
                )

                VALUES
                (
                    @MaTinTuc,
                    @MaKhachHang,
                    @NoiDung,
                    1,
                    GETDATE()
                )
            ";

            using (SqlConnection conn =
                database.GetConnection())
            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@MaTinTuc",
                    SqlDbType.Int
                ).Value = maTinTuc;

                cmd.Parameters.Add(
                    "@MaKhachHang",
                    SqlDbType.Int
                ).Value = maKhachHang;

                cmd.Parameters.Add(
                    "@NoiDung",
                    SqlDbType.NVarChar,
                    -1
                ).Value = noiDung;

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }


        // ==========================================
        // ADMIN - LẤY TẤT CẢ BÌNH LUẬN
        // ==========================================

        public DataTable GetAllForAdmin()
        {
            string sql = @"
                SELECT
                    bl.MaBinhLuan,
                    bl.MaTinTuc,
                    tt.TieuDe,
                    bl.MaKhachHang,
                    kh.HoTen,
                    kh.Email,
                    bl.NoiDung,
                    bl.TrangThai,
                    bl.NgayBinhLuan

                FROM BinhLuan bl

                INNER JOIN TinTuc tt
                    ON bl.MaTinTuc = tt.MaTinTuc

                INNER JOIN KhachHang kh
                    ON bl.MaKhachHang = kh.MaKhachHang

                ORDER BY
                    bl.NgayBinhLuan DESC,
                    bl.MaBinhLuan DESC
            ";

            using (SqlConnection conn =
                database.GetConnection())
            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            using (SqlDataAdapter adapter =
                new SqlDataAdapter(cmd))
            {
                DataTable table =
                    new DataTable();

                adapter.Fill(table);

                return table;
            }
        }


        // ==========================================
        // ADMIN - ĐỔI TRẠNG THÁI
        // ==========================================

        public bool SetStatus(
            int maBinhLuan,
            bool trangThai)
        {
            const string sql = @"
                UPDATE BinhLuan

                SET
                    TrangThai = @TrangThai

                WHERE
                    MaBinhLuan = @MaBinhLuan
            ";

            using (SqlConnection conn =
                database.GetConnection())
            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@MaBinhLuan",
                    SqlDbType.Int
                ).Value = maBinhLuan;

                cmd.Parameters.Add(
                    "@TrangThai",
                    SqlDbType.Bit
                ).Value = trangThai;

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }


        // ==========================================
        // ADMIN - XÓA
        // ==========================================

        public bool Delete(
            int maBinhLuan)
        {
            const string sql = @"
                DELETE FROM BinhLuan

                WHERE
                    MaBinhLuan = @MaBinhLuan
            ";

            using (SqlConnection conn =
                database.GetConnection())
            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@MaBinhLuan",
                    SqlDbType.Int
                ).Value = maBinhLuan;

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}