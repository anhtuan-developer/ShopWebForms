using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using web_ban_hang2.Models;
using web_ban_hang2.Utils;

namespace web_ban_hang2.DAL
{
    public class KhachHangDAL
    {
// =========================================================
// CONNECTION STRING
// =========================================================

    private readonly string connectionString =
        System.Configuration.ConfigurationManager
        .ConnectionStrings["ShopWebFormsConnection"]
        .ConnectionString;


        // =========================================================
        // LẤY TẤT CẢ KHÁCH HÀNG
        // =========================================================

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
            ORDER BY MaKhachHang DESC";


            using (SqlConnection conn =
                new SqlConnection(connectionString))

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
                                reader["MaKhachHang"]);


                        kh.HoTen =
                            reader["HoTen"].ToString();


                        kh.Email =
                            reader["Email"].ToString();


                        kh.SoDienThoai =
                            reader["SoDienThoai"]
                            == DBNull.Value
                                ? ""
                                : reader["SoDienThoai"]
                                    .ToString();


                        kh.DiaChi =
                            reader["DiaChi"]
                            == DBNull.Value
                                ? ""
                                : reader["DiaChi"]
                                    .ToString();


                        kh.NgayTao =
                            Convert.ToDateTime(
                                reader["NgayTao"]);


                        danhSach.Add(kh);
                    }
                }
            }


            return danhSach;
        }


        // =========================================================
        // LẤY KHÁCH HÀNG THEO MÃ
        // =========================================================

        public KhachHang GetById(
            int maKhachHang)
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
            WHERE MaKhachHang = @MaKhachHang";


            using (SqlConnection conn =
                new SqlConnection(connectionString))

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
                        return MapKhachHang(reader);
                    }
                }
            }


            return null;
        }


        // =========================================================
        // LẤY KHÁCH HÀNG THEO EMAIL
        // =========================================================

        public KhachHang GetByEmail(
            string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;


            email =
                email.Trim();


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
            WHERE Email = @Email";


            using (SqlConnection conn =
                new SqlConnection(connectionString))

            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@Email",
                    SqlDbType.NVarChar,
                    200
                ).Value = email;


                conn.Open();


                using (SqlDataReader reader =
                    cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapKhachHangWithPassword(
                            reader);
                    }
                }
            }


            return null;
        }


        // =========================================================
        // ĐĂNG NHẬP
        // =========================================================

        public KhachHang Login(
            string email,
            string matKhau)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrEmpty(matKhau))
            {
                return null;
            }


            email =
                email.Trim();


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
            WHERE Email = @Email";


            using (SqlConnection conn =
                new SqlConnection(connectionString))

            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@Email",
                    SqlDbType.NVarChar,
                    200
                ).Value = email;


                conn.Open();


                using (SqlDataReader reader =
                    cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }


                    KhachHang khachHang =
                        MapKhachHangWithPassword(
                            reader);


                    if (khachHang == null)
                    {
                        return null;
                    }


                    // KIỂM TRA MẬT KHẨU

                    bool valid =
                        PasswordHelper.Verify(
                            matKhau,
                            khachHang.MatKhau);


                    if (!valid)
                    {
                        return null;
                    }

                    if (!khachHang.MatKhau.StartsWith(
                        "PBKDF2-SHA256$",
                        StringComparison.Ordinal))
                    {
                        string newHash =
                            PasswordHelper.Hash(
                                matKhau);

                        khachHang.MatKhau =
                            newHash;


                        reader.Close();


                        UpdatePassword(
                            khachHang.MaKhachHang,
                            newHash);
                    }


                    return khachHang;
                }
            }
        }


        // =========================================================
        // ĐĂNG KÝ KHÁCH HÀNG
        // =========================================================

        public bool Register(
            KhachHang khachHang)
        {
            if (khachHang == null)
                return false;


            if (string.IsNullOrWhiteSpace(
                khachHang.HoTen))
            {
                return false;
            }


            if (string.IsNullOrWhiteSpace(
                khachHang.Email))
            {
                return false;
            }


            if (string.IsNullOrEmpty(
                khachHang.MatKhau))
            {
                return false;
            }


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
            )";


            using (SqlConnection conn =
                new SqlConnection(connectionString))

            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@HoTen",
                    SqlDbType.NVarChar,
                    200
                ).Value =
                    khachHang.HoTen.Trim();


                cmd.Parameters.Add(
                    "@Email",
                    SqlDbType.NVarChar,
                    200
                ).Value =
                    khachHang.Email.Trim();


                string passwordHash =
                    PasswordHelper.Hash(
                        khachHang.MatKhau);


                cmd.Parameters.Add(
                    "@MatKhau",
                    SqlDbType.NVarChar,
                    255
                ).Value =
                    passwordHash;


                cmd.Parameters.Add(
                    "@SoDienThoai",
                    SqlDbType.NVarChar,
                    20
                ).Value =
                    string.IsNullOrWhiteSpace(
                        khachHang.SoDienThoai)
                    ? (object)DBNull.Value
                    : khachHang.SoDienThoai.Trim();


                cmd.Parameters.Add(
                    "@DiaChi",
                    SqlDbType.NVarChar,
                    500
                ).Value =
                    string.IsNullOrWhiteSpace(
                        khachHang.DiaChi)
                    ? (object)DBNull.Value
                    : khachHang.DiaChi.Trim();


                conn.Open();


                int result =
                    cmd.ExecuteNonQuery();


                return result > 0;
            }
        }


        // =========================================================
        // KIỂM TRA EMAIL ĐÃ TỒN TẠI
        // =========================================================

        public bool EmailExists(
            string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;


            string sql = @"
            SELECT COUNT(*)
            FROM KhachHang
            WHERE Email = @Email";


            using (SqlConnection conn =
                new SqlConnection(connectionString))

            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@Email",
                    SqlDbType.NVarChar,
                    200
                ).Value =
                    email.Trim();


                conn.Open();


                int count =
                    Convert.ToInt32(
                        cmd.ExecuteScalar());


                return count > 0;
            }
        }


        // =========================================================
        // CẬP NHẬT THÔNG TIN CÁ NHÂN
        // =========================================================

        public bool UpdateProfile(
            int maKhachHang,
            string hoTen,
            string soDienThoai,
            string diaChi)
        {
            if (maKhachHang <= 0)
                return false;


            if (string.IsNullOrWhiteSpace(
                hoTen))
            {
                return false;
            }


            string sql = @"
            UPDATE KhachHang
            SET
                HoTen = @HoTen,
                SoDienThoai = @SoDienThoai,
                DiaChi = @DiaChi
            WHERE MaKhachHang = @MaKhachHang";


            using (SqlConnection conn =
                new SqlConnection(connectionString))

            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@HoTen",
                    SqlDbType.NVarChar,
                    200
                ).Value =
                    hoTen.Trim();


                cmd.Parameters.Add(
                    "@SoDienThoai",
                    SqlDbType.NVarChar,
                    20
                ).Value =
                    string.IsNullOrWhiteSpace(
                        soDienThoai)
                    ? (object)DBNull.Value
                    : soDienThoai.Trim();


                cmd.Parameters.Add(
                    "@DiaChi",
                    SqlDbType.NVarChar,
                    500
                ).Value =
                    string.IsNullOrWhiteSpace(
                        diaChi)
                    ? (object)DBNull.Value
                    : diaChi.Trim();


                cmd.Parameters.Add(
                    "@MaKhachHang",
                    SqlDbType.Int
                ).Value =
                    maKhachHang;


                conn.Open();


                return cmd.ExecuteNonQuery() > 0;
            }
        }


        // =========================================================
        // ĐỔI MẬT KHẨU
        // =========================================================

        public bool ChangePassword(
            int maKhachHang,
            string currentPassword,
            string newPassword)
        {
            if (maKhachHang <= 0)
                return false;


            if (string.IsNullOrEmpty(
                currentPassword))
            {
                return false;
            }


            if (string.IsNullOrEmpty(
                newPassword))
            {
                return false;
            }


            KhachHang kh =
                GetByIdWithPassword(
                    maKhachHang);


            if (kh == null)
                return false;


            bool valid =
                PasswordHelper.Verify(
                    currentPassword,
                    kh.MatKhau);


            if (!valid)
            {
                return false;
            }



            string newHash =
                PasswordHelper.Hash(
                    newPassword);


            return UpdatePassword(
                maKhachHang,
                newHash);
        }


        private KhachHang GetByIdWithPassword(
            int maKhachHang)
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
            WHERE MaKhachHang = @MaKhachHang";


            using (SqlConnection conn =
                new SqlConnection(connectionString))

            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@MaKhachHang",
                    SqlDbType.Int
                ).Value =
                    maKhachHang;


                conn.Open();


                using (SqlDataReader reader =
                    cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapKhachHangWithPassword(
                            reader);
                    }
                }
            }


            return null;
        }


        // =========================================================
        // CẬP NHẬT MẬT KHẨU
        // =========================================================

        private bool UpdatePassword(
            int maKhachHang,
            string passwordHash)
        {
            string sql = @"
            UPDATE KhachHang
            SET MatKhau = @MatKhau
            WHERE MaKhachHang = @MaKhachHang";


            using (SqlConnection conn =
                new SqlConnection(connectionString))

            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@MatKhau",
                    SqlDbType.NVarChar,
                    255
                ).Value =
                    passwordHash;


                cmd.Parameters.Add(
                    "@MaKhachHang",
                    SqlDbType.Int
                ).Value =
                    maKhachHang;


                conn.Open();


                return cmd.ExecuteNonQuery() > 0;
            }
        }


        // =========================================================
        // TẠO TOKEN QUÊN MẬT KHẨU
        // =========================================================

        public bool CreateResetToken(
            int maKhachHang,
            string tokenHash,
            DateTime expiresAt)
        {
            if (maKhachHang <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(tokenHash))
                return false;


            if (tokenHash.Length != 64)
                return false;


            string disableOldTokenSql = @"
    UPDATE PasswordResetToken
    SET Used = 1
    WHERE
        MaKhachHang = @MaKhachHang
        AND Used = 0";


            string insertSql = @"
    INSERT INTO PasswordResetToken
    (
        MaKhachHang,
        TokenHash,
        ExpiresAt,
        Used
    )
    VALUES
    (
        @MaKhachHang,
        @TokenHash,
        @ExpiresAt,
        0
    )";


            using (SqlConnection conn =
                new SqlConnection(connectionString))
            {
                conn.Open();


                using (SqlTransaction transaction =
                    conn.BeginTransaction())
                {
                    try
                    {
                        // -------------------------------------------------
                        // VÔ HIỆU HÓA TOKEN CŨ
                        // -------------------------------------------------

                        using (SqlCommand cmd =
                            new SqlCommand(
                                disableOldTokenSql,
                                conn,
                                transaction))
                        {
                            cmd.Parameters.Add(
                                "@MaKhachHang",
                                SqlDbType.Int).Value =
                                maKhachHang;


                            cmd.ExecuteNonQuery();
                        }


                        // -------------------------------------------------
                        // TẠO TOKEN MỚI
                        // -------------------------------------------------

                        using (SqlCommand cmd =
                            new SqlCommand(
                                insertSql,
                                conn,
                                transaction))
                        {
                            cmd.Parameters.Add(
                                "@MaKhachHang",
                                SqlDbType.Int).Value =
                                maKhachHang;


                            cmd.Parameters.Add(
                                "@TokenHash",
                                SqlDbType.VarChar,
                                64).Value =
                                tokenHash;


                            cmd.Parameters.Add(
                                "@ExpiresAt",
                                SqlDbType.DateTime).Value =
                                expiresAt;


                            int result =
                                cmd.ExecuteNonQuery();


                            if (result != 1)
                            {
                                transaction.Rollback();

                                return false;
                            }
                        }


                        transaction.Commit();


                        return true;
                    }
                    catch
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                            // Bỏ qua lỗi rollback
                        }


                        return false;
                    }
                }
            }

        }

        // =========================================================
        // KIỂM TRA TOKEN CÒN HỢP LỆ
        // =========================================================

        public bool IsResetTokenValid(
        string tokenHash)
        {
            if (string.IsNullOrWhiteSpace(tokenHash))
                return false;

            if (tokenHash.Length != 64)
                return false;


            string sql = @"
    SELECT COUNT(*)
    FROM PasswordResetToken
    WHERE
        TokenHash = @TokenHash
        AND Used = 0
        AND ExpiresAt > @Now";


            using (SqlConnection conn =
                new SqlConnection(connectionString))

            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@TokenHash",
                    SqlDbType.VarChar,
                    64).Value =
                    tokenHash;


                cmd.Parameters.Add(
                    "@Now",
                    SqlDbType.DateTime).Value =
                    DateTime.Now;


                conn.Open();


                int count =
                    Convert.ToInt32(
                        cmd.ExecuteScalar());


                return count > 0;
            }

        }

        // =========================================================
        // VÔ HIỆU HÓA TOKEN
        // =========================================================

        public bool InvalidateResetToken(
        string tokenHash)
        {
            if (string.IsNullOrWhiteSpace(tokenHash))
                return false;

            if (tokenHash.Length != 64)
                return false;


            string sql = @"
    UPDATE PasswordResetToken
    SET Used = 1
    WHERE
        TokenHash = @TokenHash
        AND Used = 0";


            using (SqlConnection conn =
                new SqlConnection(connectionString))

            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@TokenHash",
                    SqlDbType.VarChar,
                    64).Value =
                    tokenHash;


                conn.Open();


                return
                    cmd.ExecuteNonQuery() > 0;
            }

        }


        // =========================================================
        // ĐẶT LẠI MẬT KHẨU
        // =========================================================

        public bool ResetPassword(
            string tokenHash,
            string newPassword)
        {
            if (string.IsNullOrWhiteSpace(tokenHash))
                return false;

            if (tokenHash.Length != 64)
                return false;


            if (string.IsNullOrEmpty(newPassword))
                return false;


            if (newPassword.Length < 6 ||
                newPassword.Length > 100)
            {
                return false;
            }


            using (SqlConnection conn =
                new SqlConnection(connectionString))
            {
                conn.Open();


                using (SqlTransaction transaction =
                    conn.BeginTransaction(
                        IsolationLevel.Serializable))
                {
                    try
                    {
                        int tokenId = 0;

                        int maKhachHang = 0;


                        // =================================================
                        // TÌM VÀ KHÓA TOKEN
                        // =================================================

                        string findTokenSql = @"
                SELECT TOP 1
                    Id,
                    MaKhachHang
                FROM PasswordResetToken WITH (UPDLOCK, HOLDLOCK)
                WHERE
                    TokenHash = @TokenHash
                    AND Used = 0
                    AND ExpiresAt > @Now
                ORDER BY Id DESC";


                        using (SqlCommand cmd =
                            new SqlCommand(
                                findTokenSql,
                                conn,
                                transaction))
                        {
                            cmd.Parameters.Add(
                                "@TokenHash",
                                SqlDbType.VarChar,
                                64).Value =
                                tokenHash;


                            cmd.Parameters.Add(
                                "@Now",
                                SqlDbType.DateTime).Value =
                                DateTime.Now;


                            using (SqlDataReader reader =
                                cmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    reader.Close();

                                    transaction.Rollback();

                                    return false;
                                }


                                tokenId =
                                    Convert.ToInt32(
                                        reader["Id"]);


                                maKhachHang =
                                    Convert.ToInt32(
                                        reader["MaKhachHang"]);
                            }
                        }


                        // =================================================
                        // HASH MẬT KHẨU MỚI
                        // =================================================

                        string newPasswordHash =
                            PasswordHelper.Hash(
                                newPassword);


                        // =================================================
                        // CẬP NHẬT MẬT KHẨU
                        // =================================================

                        string updatePasswordSql = @"
                            UPDATE KhachHang
                            SET MatKhau = @MatKhau
                            WHERE MaKhachHang = @MaKhachHang";


                        using (SqlCommand cmd =
                            new SqlCommand(
                                updatePasswordSql,
                                conn,
                                transaction))
                        {
                            cmd.Parameters.Add(
                                "@MatKhau",
                                SqlDbType.NVarChar,
                                255).Value =
                                newPasswordHash;


                            cmd.Parameters.Add(
                                "@MaKhachHang",
                                SqlDbType.Int).Value =
                                maKhachHang;


                            int result =
                                cmd.ExecuteNonQuery();


                            if (result != 1)
                            {
                                transaction.Rollback();

                                return false;
                            }
                        }


                        // =================================================
                        // ĐÁNH DẤU TOKEN ĐÃ SỬ DỤNG
                        // =================================================

                        string useTokenSql = @"
                UPDATE PasswordResetToken
                SET Used = 1
                WHERE
                    Id = @Id
                    AND Used = 0";


                        using (SqlCommand cmd =
                            new SqlCommand(
                                useTokenSql,
                                conn,
                                transaction))
                        {
                            cmd.Parameters.Add(
                                "@Id",
                                SqlDbType.Int).Value =
                                tokenId;


                            int result =
                                cmd.ExecuteNonQuery();


                            if (result != 1)
                            {
                                transaction.Rollback();

                                return false;
                            }
                        }


                        // =================================================
                        // HOÀN TẤT
                        // =================================================

                        transaction.Commit();


                        return true;
                    }
                    catch
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                            // Bỏ qua lỗi rollback
                        }


                        return false;
                    }
                }
            }

        }


        // =========================================================
        // XÓA KHÁCH HÀNG
        // =========================================================

        public bool Delete(
            int maKhachHang,
            out string message)
        {
            message = "";


            if (maKhachHang <= 0)
            {
                message =
                    "Mã khách hàng không hợp lệ.";

                return false;
            }


            string checkCustomerSql = @"
            SELECT COUNT(*)
            FROM KhachHang
            WHERE MaKhachHang = @MaKhachHang";


            string checkOrderSql = @"
            SELECT COUNT(*)
            FROM DonHang
            WHERE MaKhachHang = @MaKhachHang";


            string deleteSql = @"
            DELETE FROM KhachHang
            WHERE MaKhachHang = @MaKhachHang";


            using (SqlConnection conn =
                new SqlConnection(connectionString))
            {
                conn.Open();


                // =================================================
                // KIỂM TRA KHÁCH HÀNG
                // =================================================

                using (SqlCommand cmd =
                    new SqlCommand(
                        checkCustomerSql,
                        conn))
                {
                    cmd.Parameters.Add(
                        "@MaKhachHang",
                        SqlDbType.Int
                    ).Value =
                        maKhachHang;


                    int count =
                        Convert.ToInt32(
                            cmd.ExecuteScalar());


                    if (count == 0)
                    {
                        message =
                            "Khách hàng không tồn tại.";

                        return false;
                    }
                }


                // =================================================
                // KIỂM TRA ĐƠN HÀNG
                // =================================================

                using (SqlCommand cmd =
                    new SqlCommand(
                        checkOrderSql,
                        conn))
                {
                    cmd.Parameters.Add(
                        "@MaKhachHang",
                        SqlDbType.Int
                    ).Value =
                        maKhachHang;


                    int orderCount =
                        Convert.ToInt32(
                            cmd.ExecuteScalar());


                    if (orderCount > 0)
                    {
                        message =
                            "Không thể xóa khách hàng này " +
                            "vì khách hàng đã có đơn hàng.";

                        return false;
                    }
                }


                // =================================================
                // XÓA KHÁCH HÀNG
                // =================================================

                using (SqlCommand cmd =
                    new SqlCommand(
                        deleteSql,
                        conn))
                {
                    cmd.Parameters.Add(
                        "@MaKhachHang",
                        SqlDbType.Int
                    ).Value =
                        maKhachHang;


                    int result =
                        cmd.ExecuteNonQuery();


                    if (result > 0)
                    {
                        message =
                            "Xóa khách hàng thành công.";

                        return true;
                    }
                }
            }


            message =
                "Không thể xóa khách hàng.";


            return false;
        }


        // =========================================================
        // ĐẾM KHÁCH HÀNG
        // =========================================================

        public int CountAll()
        {
            string sql =
                "SELECT COUNT(*) FROM KhachHang";


            using (SqlConnection conn =
                new SqlConnection(connectionString))

            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                conn.Open();


                return Convert.ToInt32(
                    cmd.ExecuteScalar());
            }
        }


        // =========================================================
        // MAP KHÁCH HÀNG
        //
        // Không lấy MatKhau.
        // Dùng cho các trang hiển thị thông tin.
        // =========================================================

        private KhachHang MapKhachHang(
            SqlDataReader reader)
        {
            KhachHang kh =
                new KhachHang();


            kh.MaKhachHang =
                Convert.ToInt32(
                    reader["MaKhachHang"]);


            kh.HoTen =
                reader["HoTen"].ToString();


            kh.Email =
                reader["Email"].ToString();


            kh.SoDienThoai =
                reader["SoDienThoai"]
                == DBNull.Value
                    ? ""
                    : reader["SoDienThoai"]
                        .ToString();


            kh.DiaChi =
                reader["DiaChi"]
                == DBNull.Value
                    ? ""
                    : reader["DiaChi"]
                        .ToString();


            kh.NgayTao =
                Convert.ToDateTime(
                    reader["NgayTao"]);


            return kh;
        }


       
        private KhachHang MapKhachHangWithPassword(
            SqlDataReader reader)
        {
            KhachHang kh =
                new KhachHang();


            kh.MaKhachHang =
                Convert.ToInt32(
                    reader["MaKhachHang"]);


            kh.HoTen =
                reader["HoTen"].ToString();


            kh.Email =
                reader["Email"].ToString();


            kh.MatKhau =
                reader["MatKhau"] == DBNull.Value
                    ? ""
                    : reader["MatKhau"].ToString();


            kh.SoDienThoai =
                reader["SoDienThoai"]
                == DBNull.Value
                    ? ""
                    : reader["SoDienThoai"]
                        .ToString();


            kh.DiaChi =
                reader["DiaChi"]
                == DBNull.Value
                    ? ""
                    : reader["DiaChi"]
                        .ToString();


            kh.NgayTao =
                Convert.ToDateTime(
                    reader["NgayTao"]);


            return kh;
        }
    }

}
