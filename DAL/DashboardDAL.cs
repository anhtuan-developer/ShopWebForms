using System;
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


        // =========================================
        // ĐẾM DANH MỤC
        // =========================================

        public int GetTotalDanhMuc()
        {
            string sql = @"
                SELECT COUNT(*)
                FROM DanhMuc
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


        // =========================================
        // ĐẾM KHÁCH HÀNG
        // =========================================

        public int GetTotalKhachHang()
        {
            string sql = @"
                SELECT COUNT(*)
                FROM KhachHang
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


        // =========================================
        // ĐẾM ĐƠN HÀNG
        // =========================================

        public int GetTotalDonHang()
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
    }
}