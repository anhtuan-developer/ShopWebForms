using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using web_ban_hang2.Models;

namespace web_ban_hang2.DAL
{
    public class KhachHangDAL
    {
        private readonly string connectionString =
            System.Configuration.ConfigurationManager
            .ConnectionStrings["ShopWebFormsConnection"]
            .ConnectionString;


        // ==========================================
        // LẤY TẤT CẢ KHÁCH HÀNG
        // ==========================================

        public List<KhachHang> GetAll()
        {
            List<KhachHang> danhSach =
                new List<KhachHang>();

            string sql = @"
                SELECT
                    MaKhachHang,
                    HoTen,
                    Email,
                    SoDienThoai,
                    DiaChi,
                    NgayTao
                FROM KhachHang
                ORDER BY MaKhachHang DESC
            ";

            using (SqlConnection conn =
                new SqlConnection(connectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            KhachHang kh =
                                new KhachHang();

                            kh.MaKhachHang =
                                Convert.ToInt32(
                                    reader["MaKhachHang"]
                                );

                            kh.HoTen =
                                reader["HoTen"].ToString();

                            kh.Email =
                                reader["Email"].ToString();

                            kh.SoDienThoai =
                                reader["SoDienThoai"] == DBNull.Value
                                ? ""
                                : reader["SoDienThoai"].ToString();

                            kh.DiaChi =
                                reader["DiaChi"] == DBNull.Value
                                ? ""
                                : reader["DiaChi"].ToString();

                            kh.NgayTao =
                                Convert.ToDateTime(
                                    reader["NgayTao"]
                                );

                            danhSach.Add(kh);
                        }
                    }
                }
            }

            return danhSach;
        }


        // ==========================================
        // LẤY KHÁCH HÀNG THEO MÃ
        // ==========================================

        public KhachHang GetById(int maKhachHang)
        {
            string sql = @"
                SELECT
                    MaKhachHang,
                    HoTen,
                    Email,
                    SoDienThoai,
                    DiaChi,
                    NgayTao
                FROM KhachHang
                WHERE MaKhachHang = @MaKhachHang
            ";

            using (SqlConnection conn =
                new SqlConnection(connectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaKhachHang",
                        SqlDbType.Int
                    ).Value = maKhachHang;

                    conn.Open();

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            KhachHang kh =
                                new KhachHang();

                            kh.MaKhachHang =
                                Convert.ToInt32(
                                    reader["MaKhachHang"]
                                );

                            kh.HoTen =
                                reader["HoTen"].ToString();

                            kh.Email =
                                reader["Email"].ToString();

                            kh.SoDienThoai =
                                reader["SoDienThoai"] == DBNull.Value
                                ? ""
                                : reader["SoDienThoai"].ToString();

                            kh.DiaChi =
                                reader["DiaChi"] == DBNull.Value
                                ? ""
                                : reader["DiaChi"].ToString();

                            kh.NgayTao =
                                Convert.ToDateTime(
                                    reader["NgayTao"]
                                );

                            return kh;
                        }
                    }
                }
            }

            return null;
        }


        // ==========================================
        // ĐĂNG NHẬP
        // ==========================================

        public KhachHang Login(
            string email,
            string matKhau)
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

            using (SqlConnection conn =
                new SqlConnection(connectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@Email",
                        SqlDbType.NVarChar,
                        200
                    ).Value = email;

                    cmd.Parameters.Add(
                        "@MatKhau",
                        SqlDbType.NVarChar,
                        200
                    ).Value = matKhau;

                    conn.Open();

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            KhachHang khachHang =
                                new KhachHang();

                            khachHang.MaKhachHang =
                                Convert.ToInt32(
                                    reader["MaKhachHang"]
                                );

                            khachHang.HoTen =
                                reader["HoTen"].ToString();

                            khachHang.Email =
                                reader["Email"].ToString();

                            khachHang.MatKhau =
                                reader["MatKhau"].ToString();

                            khachHang.SoDienThoai =
                                reader["SoDienThoai"] == DBNull.Value
                                ? ""
                                : reader["SoDienThoai"].ToString();

                            khachHang.DiaChi =
                                reader["DiaChi"] == DBNull.Value
                                ? ""
                                : reader["DiaChi"].ToString();

                            khachHang.NgayTao =
                                Convert.ToDateTime(
                                    reader["NgayTao"]
                                );

                            return khachHang;
                        }
                    }
                }
            }

            return null;
        }


        // ==========================================
        // XÓA KHÁCH HÀNG
        // ==========================================

        public bool Delete(int maKhachHang)
        {
            string sql = @"
                DELETE FROM KhachHang
                WHERE MaKhachHang = @MaKhachHang
            ";

            using (SqlConnection conn =
                new SqlConnection(connectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaKhachHang",
                        SqlDbType.Int
                    ).Value = maKhachHang;

                    conn.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }


        // ==========================================
        // ĐẾM KHÁCH HÀNG
        // ==========================================

        public int CountAll()
        {
            string sql =
                "SELECT COUNT(*) FROM KhachHang";

            using (SqlConnection conn =
                new SqlConnection(connectionString))
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
        // KIỂM TRA EMAIL ĐÃ TỒN TẠI
        // ==========================================

        public bool EmailExists(string email)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM KhachHang
        WHERE Email = @Email
    ";

            using (SqlConnection conn =
                new SqlConnection(connectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@Email",
                        SqlDbType.NVarChar,
                        200
                    ).Value = email;

                    conn.Open();

                    int count =
                        Convert.ToInt32(
                            cmd.ExecuteScalar()
                        );

                    return count > 0;
                }
            }
        }
        // ==========================================
        // ĐĂNG KÝ KHÁCH HÀNG
        // ==========================================

        public bool Register(KhachHang khachHang)
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

            using (SqlConnection conn =
                new SqlConnection(connectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@HoTen",
                        SqlDbType.NVarChar,
                        200
                    ).Value = khachHang.HoTen;

                    cmd.Parameters.Add(
                        "@Email",
                        SqlDbType.NVarChar,
                        200
                    ).Value = khachHang.Email;

                    cmd.Parameters.Add(
                        "@MatKhau",
                        SqlDbType.NVarChar,
                        200
                    ).Value = khachHang.MatKhau;

                    cmd.Parameters.Add(
                        "@SoDienThoai",
                        SqlDbType.NVarChar,
                        20
                    ).Value =
                        string.IsNullOrWhiteSpace(
                            khachHang.SoDienThoai)
                        ? (object)DBNull.Value
                        : khachHang.SoDienThoai;

                    cmd.Parameters.Add(
                        "@DiaChi",
                        SqlDbType.NVarChar,
                        500
                    ).Value =
                        string.IsNullOrWhiteSpace(
                            khachHang.DiaChi)
                        ? (object)DBNull.Value
                        : khachHang.DiaChi;

                    conn.Open();

                    int result =
                        cmd.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }
    }
}