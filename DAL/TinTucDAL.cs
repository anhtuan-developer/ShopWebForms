using System;
using System.Data;
using System.Data.SqlClient;

namespace web_ban_hang2.DAL
{
    public class TinTucDAL
    {
        private readonly Database database;

        public TinTucDAL()
        {
            database = new Database();
        }

        // =====================================================
        // LẤY TẤT CẢ TIN ĐANG HIỂN THỊ - PHÍA KHÁCH HÀNG
        // =====================================================

        public DataTable GetAllActive()
        {
            const string sql = @"
                SELECT
                    MaTinTuc,
                    TieuDe,
                    NoiDung,
                    HinhAnh,
                    TrangThai,
                    NgayTao
                FROM TinTuc
                WHERE TrangThai = 1
                ORDER BY NgayTao DESC, MaTinTuc DESC";

            return ExecuteTable(sql);
        }

        // =====================================================
        // LẤY TẤT CẢ TIN - ADMIN
        // =====================================================

        public DataTable GetAll()
        {
            const string sql = @"
                SELECT
                    MaTinTuc,
                    TieuDe,
                    NoiDung,
                    HinhAnh,
                    TrangThai,
                    NgayTao
                FROM TinTuc
                ORDER BY NgayTao DESC, MaTinTuc DESC";

            return ExecuteTable(sql);
        }

        // =====================================================
        // LẤY CHI TIẾT TIN - PHÍA KHÁCH HÀNG
        // =====================================================

        public DataTable GetById(int maTinTuc)
        {
            const string sql = @"
                SELECT
                    MaTinTuc,
                    TieuDe,
                    NoiDung,
                    HinhAnh,
                    TrangThai,
                    NgayTao
                FROM TinTuc
                WHERE MaTinTuc = @MaTinTuc
                  AND TrangThai = 1";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@MaTinTuc", SqlDbType.Int).Value =
                    maTinTuc;

                DataTable table = new DataTable();

                adapter.Fill(table);

                return table;
            }
        }

        // =====================================================
        // LẤY CHI TIẾT TIN - ADMIN
        // =====================================================

        public DataTable GetByIdForAdmin(int maTinTuc)
        {
            const string sql = @"
                SELECT
                    MaTinTuc,
                    TieuDe,
                    NoiDung,
                    HinhAnh,
                    TrangThai,
                    NgayTao
                FROM TinTuc
                WHERE MaTinTuc = @MaTinTuc";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@MaTinTuc", SqlDbType.Int).Value =
                    maTinTuc;

                DataTable table = new DataTable();

                adapter.Fill(table);

                return table;
            }
        }

        // =====================================================
        // THÊM TIN TỨC
        // =====================================================

        public bool Insert(
            string tieuDe,
            string noiDung,
            string hinhAnh,
            bool trangThai)
        {
            const string sql = @"
                INSERT INTO TinTuc
                (
                    TieuDe,
                    NoiDung,
                    HinhAnh,
                    TrangThai,
                    NgayTao
                )
                VALUES
                (
                    @TieuDe,
                    @NoiDung,
                    @HinhAnh,
                    @TrangThai,
                    GETDATE()
                )";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                AddParameters(
                    cmd,
                    tieuDe,
                    noiDung,
                    hinhAnh,
                    trangThai
                );

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // =====================================================
        // SỬA TIN TỨC
        // =====================================================

        public bool Update(
            int maTinTuc,
            string tieuDe,
            string noiDung,
            string hinhAnh,
            bool trangThai)
        {
            const string sql = @"
                UPDATE TinTuc
                SET
                    TieuDe = @TieuDe,
                    NoiDung = @NoiDung,
                    HinhAnh = @HinhAnh,
                    TrangThai = @TrangThai
                WHERE MaTinTuc = @MaTinTuc";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@MaTinTuc",
                    SqlDbType.Int
                ).Value = maTinTuc;

                AddParameters(
                    cmd,
                    tieuDe,
                    noiDung,
                    hinhAnh,
                    trangThai
                );

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // =====================================================
        // XÓA TIN TỨC
        // =====================================================

        public bool Delete(int maTinTuc)
        {
            using (SqlConnection conn = database.GetConnection())
            {
                conn.Open();

                SqlTransaction transaction =
                    conn.BeginTransaction();

                try
                {
                    // Xóa bình luận trước
                    using (SqlCommand cmdComment =
                        new SqlCommand(
                            @"DELETE FROM BinhLuan
                              WHERE MaTinTuc = @MaTinTuc",
                            conn,
                            transaction))
                    {
                        cmdComment.Parameters.Add(
                            "@MaTinTuc",
                            SqlDbType.Int
                        ).Value = maTinTuc;

                        cmdComment.ExecuteNonQuery();
                    }

                    // Sau đó xóa tin tức
                    int affected;

                    using (SqlCommand cmdNews =
                        new SqlCommand(
                            @"DELETE FROM TinTuc
                              WHERE MaTinTuc = @MaTinTuc",
                            conn,
                            transaction))
                    {
                        cmdNews.Parameters.Add(
                            "@MaTinTuc",
                            SqlDbType.Int
                        ).Value = maTinTuc;

                        affected =
                            cmdNews.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    return affected > 0;
                }
                catch
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                    }

                    throw;
                }
            }
        }

        // =====================================================
        // CẬP NHẬT TRẠNG THÁI
        // =====================================================

        public bool UpdateStatus(
            int maTinTuc,
            bool trangThai)
        {
            const string sql = @"
                UPDATE TinTuc
                SET TrangThai = @TrangThai
                WHERE MaTinTuc = @MaTinTuc";

            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@MaTinTuc",
                    SqlDbType.Int
                ).Value = maTinTuc;

                cmd.Parameters.Add(
                    "@TrangThai",
                    SqlDbType.Bit
                ).Value = trangThai;

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // =====================================================
        // HÀM DÙNG CHUNG
        // =====================================================

        private DataTable ExecuteTable(string sql)
        {
            using (SqlConnection conn = database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataAdapter adapter =
                new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();

                adapter.Fill(table);

                return table;
            }
        }

        private void AddParameters(
            SqlCommand cmd,
            string tieuDe,
            string noiDung,
            string hinhAnh,
            bool trangThai)
        {
            cmd.Parameters.Add(
                "@TieuDe",
                SqlDbType.NVarChar,
                250
            ).Value =
                string.IsNullOrWhiteSpace(tieuDe)
                    ? (object)DBNull.Value
                    : tieuDe.Trim();

            cmd.Parameters.Add(
                "@NoiDung",
                SqlDbType.NVarChar,
                -1
            ).Value =
                string.IsNullOrWhiteSpace(noiDung)
                    ? (object)DBNull.Value
                    : noiDung.Trim();

            cmd.Parameters.Add(
                "@HinhAnh",
                SqlDbType.NVarChar,
                500
            ).Value =
                string.IsNullOrWhiteSpace(hinhAnh)
                    ? (object)DBNull.Value
                    : hinhAnh.Trim();

            cmd.Parameters.Add(
                "@TrangThai",
                SqlDbType.Bit
            ).Value = trangThai;
        }
    }
}