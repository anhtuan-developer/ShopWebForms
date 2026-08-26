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


        // ==========================================
        // LẤY TẤT CẢ TIN TỨC ĐANG HIỂN THỊ
        // ==========================================

        public DataTable GetAllActive()
        {
            string sql = @"
                SELECT
                    MaTinTuc,
                    TieuDe,
                    NoiDung,
                    HinhAnh,
                    TrangThai,
                    NgayTao

                FROM TinTuc

                WHERE TrangThai = 1

                ORDER BY
                    NgayTao DESC,
                    MaTinTuc DESC
            ";


            using (
                SqlConnection conn =
                    database.GetConnection())
            using (
                SqlCommand cmd =
                    new SqlCommand(sql, conn))
            using (
                SqlDataAdapter adapter =
                    new SqlDataAdapter(cmd))
            {
                DataTable table =
                    new DataTable();

                adapter.Fill(table);

                return table;
            }
        }


        // ==========================================
        // LẤY TIN TỨC THEO MÃ
        // ==========================================

        public DataTable GetById(
            int maTinTuc)
        {
            string sql = @"
                SELECT
                    MaTinTuc,
                    TieuDe,
                    NoiDung,
                    HinhAnh,
                    TrangThai,
                    NgayTao

                FROM TinTuc

                WHERE
                    MaTinTuc = @MaTinTuc

                    AND TrangThai = 1
            ";


            using (
                SqlConnection conn =
                    database.GetConnection())
            using (
                SqlCommand cmd =
                    new SqlCommand(sql, conn))
            using (
                SqlDataAdapter adapter =
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
    }
}