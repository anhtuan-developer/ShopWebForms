using System;
using System.Data.SqlClient;
using web_ban_hang2.Models;

namespace web_ban_hang2.DAL
{
    public class KhachHangDAL
    {
        private Database database = new Database();


        // Kiểm tra email đã tồn tại
        public bool EmailExists(string email)
        {
            using (SqlConnection conn =
                database.GetConnection())
            {
                string sql = @"
                    SELECT COUNT(*)
                    FROM KhachHang
                    WHERE Email = @Email
                ";

                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "@Email",
                        email
                    );

                    conn.Open();

                    int count =
                        Convert.ToInt32(
                            cmd.ExecuteScalar()
                        );

                    return count > 0;
                }
            }
        }


        // Đăng ký
        public bool Register(KhachHang khachHang)
        {
            using (SqlConnection conn =
                database.GetConnection())
            {
                string sql = @"
                    INSERT INTO KhachHang
                    (
                        HoTen,
                        Email,
                        MatKhau,
                        SoDienThoai,
                        DiaChi
                    )
                    VALUES
                    (
                        @HoTen,
                        @Email,
                        @MatKhau,
                        @SoDienThoai,
                        @DiaChi
                    )
                ";

                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "@HoTen",
                        khachHang.HoTen
                    );

                    cmd.Parameters.AddWithValue(
                        "@Email",
                        khachHang.Email
                    );

                    cmd.Parameters.AddWithValue(
                        "@MatKhau",
                        khachHang.MatKhau
                    );

                    cmd.Parameters.AddWithValue(
                        "@SoDienThoai",
                        string.IsNullOrEmpty(
                            khachHang.SoDienThoai)
                            ? (object)DBNull.Value
                            : khachHang.SoDienThoai
                    );

                    cmd.Parameters.AddWithValue(
                        "@DiaChi",
                        string.IsNullOrEmpty(
                            khachHang.DiaChi)
                            ? (object)DBNull.Value
                            : khachHang.DiaChi
                    );

                    conn.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }


        // Đăng nhập
        public KhachHang Login(
            string email,
            string matKhau)
        {
            using (SqlConnection conn =
                database.GetConnection())
            {
                string sql = @"
                    SELECT
                        MaKhachHang,
                        HoTen,
                        Email,
                        MatKhau,
                        SoDienThoai,
                        DiaChi,
                        NgayTao
                    FROM KhachHang
                    WHERE Email = @Email
                    AND MatKhau = @MatKhau
                ";

                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "@Email",
                        email
                    );

                    cmd.Parameters.AddWithValue(
                        "@MatKhau",
                        matKhau
                    );

                    conn.Open();

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new KhachHang
                            {
                                MaKhachHang =
                                    Convert.ToInt32(
                                        reader["MaKhachHang"]
                                    ),

                                HoTen =
                                    reader["HoTen"].ToString(),

                                Email =
                                    reader["Email"].ToString(),

                                MatKhau =
                                    reader["MatKhau"].ToString(),

                                SoDienThoai =
                                    reader["SoDienThoai"] ==
                                    DBNull.Value
                                    ? ""
                                    : reader["SoDienThoai"].ToString(),

                                DiaChi =
                                    reader["DiaChi"] ==
                                    DBNull.Value
                                    ? ""
                                    : reader["DiaChi"].ToString(),

                                NgayTao =
                                    Convert.ToDateTime(
                                        reader["NgayTao"]
                                    )
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}