using System;
using System.Configuration;
using System.Data.SqlClient;

namespace web_ban_hang2.DAL
{
    public class AdminAccountDAL
    {
        private readonly string connectionString =
            ConfigurationManager
                .ConnectionStrings["ShopWebFormsConnection"]
                .ConnectionString;


        // =====================================================
        // ĐĂNG NHẬP ADMIN
        // Email được sử dụng làm tên đăng nhập
        // =====================================================

        public AdminLoginResult Login(
            string email,
            string matKhau)
        {
            AdminLoginResult result =
                new AdminLoginResult();


            string sql = @"
                SELECT
                    MaAdmin,
                    HoTen,
                    Email,
                    MatKhau,
                    TrangThai
                FROM Admin
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
                        System.Data.SqlDbType.VarChar,
                        150
                    ).Value = email;


                    cmd.Parameters.Add(
                        "@MatKhau",
                        System.Data.SqlDbType.VarChar,
                        255
                    ).Value = matKhau;


                    conn.Open();


                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Kiểm tra trạng thái tài khoản

                            bool trangThai =
                                Convert.ToBoolean(
                                    reader["TrangThai"]
                                );


                            if (!trangThai)
                            {
                                result.Success = false;

                                result.Message =
                                    "Tài khoản Admin đã bị khóa.";

                                return result;
                            }


                            // =================================
                            // ĐĂNG NHẬP THÀNH CÔNG
                            // =================================

                            result.Success = true;


                            result.MaAdmin =
                                Convert.ToInt32(
                                    reader["MaAdmin"]
                                );


                            result.HoTen =
                                reader["HoTen"]
                                .ToString();


                            result.Email =
                                reader["Email"]
                                .ToString();


                            result.Message =
                                "Đăng nhập thành công.";

                            return result;
                        }
                    }
                }
            }


            // =============================================
            // ĐĂNG NHẬP THẤT BẠI
            // =============================================

            result.Success = false;

            result.Message =
                "Email hoặc mật khẩu không chính xác.";

            return result;
        }
    }


    // =========================================================
    // KẾT QUẢ ĐĂNG NHẬP
    // =========================================================

    public class AdminLoginResult
    {
        public bool Success
        {
            get;
            set;
        }


        public int MaAdmin
        {
            get;
            set;
        }


        public string HoTen
        {
            get;
            set;
        }


        public string Email
        {
            get;
            set;
        }


        public string Message
        {
            get;
            set;
        }
    }
}