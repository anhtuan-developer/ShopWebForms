using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using web_ban_hang2.Models;

namespace web_ban_hang2.DAL
{
    public class DonHangDAL
    {
        private readonly Database database;

        public DonHangDAL()
        {
            database = new Database();
        }


        public DataTable GetAll()
        {
            string sql = @"
                SELECT
                    dh.MaDonHang,
                    dh.MaKhachHang,
                    kh.HoTen AS TenKhachHang,
                    dh.HoTenNguoiNhan,
                    dh.SoDienThoai,
                    dh.DiaChiGiaoHang,
                    dh.TongTien,
                    dh.TrangThai,
                    dh.NgayDat
                FROM DonHang dh
                LEFT JOIN KhachHang kh
                    ON dh.MaKhachHang = kh.MaKhachHang
                ORDER BY dh.MaDonHang DESC
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


        public DataTable GetById(
            int maDonHang)
        {
            string sql = @"
                SELECT
                    dh.MaDonHang,
                    dh.MaKhachHang,
                    kh.HoTen AS TenKhachHang,
                    dh.HoTenNguoiNhan,
                    dh.SoDienThoai,
                    dh.DiaChiGiaoHang,
                    dh.TongTien,
                    dh.TrangThai,
                    dh.NgayDat
                FROM DonHang dh
                LEFT JOIN KhachHang kh
                    ON dh.MaKhachHang = kh.MaKhachHang
                WHERE dh.MaDonHang = @MaDonHang
            ";


            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaDonHang",
                        SqlDbType.Int
                    ).Value = maDonHang;


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


        public DataTable GetChiTietByDonHang(
            int maDonHang)
        {
            string sql = @"
                SELECT
                    ctdh.MaChiTiet,
                    ctdh.MaDonHang,
                    ctdh.MaSanPham,
                    sp.TenSanPham,
                    ctdh.SoLuong,
                    ctdh.DonGia,
                    ctdh.ThanhTien
                FROM ChiTietDonHang ctdh
                INNER JOIN SanPham sp
                    ON ctdh.MaSanPham = sp.MaSanPham
                WHERE ctdh.MaDonHang = @MaDonHang
                ORDER BY ctdh.MaChiTiet ASC
            ";


            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaDonHang",
                        SqlDbType.Int
                    ).Value = maDonHang;


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
        public DataTable GetByCustomerId(
    int maKhachHang)
        {
            string sql = @"
        SELECT
            dh.MaDonHang,
            dh.MaKhachHang,
            dh.HoTenNguoiNhan,
            dh.SoDienThoai,
            dh.DiaChiGiaoHang,
            dh.TongTien,
            dh.TrangThai,
            dh.NgayDat

        FROM DonHang dh

        WHERE
            dh.MaKhachHang = @MaKhachHang

        ORDER BY
            dh.MaDonHang DESC
    ";


            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaKhachHang",
                        SqlDbType.Int
                    ).Value =
                        maKhachHang;


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

        public DataTable GetChiTietByCustomerId(
    int maDonHang,
    int maKhachHang)
        {
            string sql = @"
        SELECT
            dh.MaDonHang,
            dh.MaKhachHang,
            dh.HoTenNguoiNhan,
            dh.SoDienThoai,
            dh.DiaChiGiaoHang,
            dh.TongTien,
            dh.TrangThai,
            dh.NgayDat,

            ctdh.MaSanPham,
            sp.TenSanPham,
            ctdh.SoLuong,
            ctdh.DonGia,

            (
                ctdh.SoLuong
                * ctdh.DonGia
            ) AS ThanhTien

        FROM DonHang dh

        INNER JOIN ChiTietDonHang ctdh
            ON dh.MaDonHang =
               ctdh.MaDonHang

        INNER JOIN SanPham sp
            ON ctdh.MaSanPham =
               sp.MaSanPham

        WHERE
            dh.MaDonHang = @MaDonHang

            AND

            dh.MaKhachHang = @MaKhachHang

        ORDER BY
            ctdh.MaSanPham
    ";


            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaDonHang",
                        SqlDbType.Int
                    ).Value =
                        maDonHang;


                    cmd.Parameters.Add(
                        "@MaKhachHang",
                        SqlDbType.Int
                    ).Value =
                        maKhachHang;


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

        public bool IsOrderOwnedByCustomer(
    int maDonHang,
    int maKhachHang)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM DonHang

        WHERE
            MaDonHang = @MaDonHang

            AND

            MaKhachHang = @MaKhachHang
    ";


            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaDonHang",
                        SqlDbType.Int
                    ).Value =
                        maDonHang;


                    cmd.Parameters.Add(
                        "@MaKhachHang",
                        SqlDbType.Int
                    ).Value =
                        maKhachHang;


                    conn.Open();


                    return Convert.ToInt32(
                        cmd.ExecuteScalar()
                    ) > 0;
                }
            }
        }


        public int TaoDonHang(
    DonHang donHang)
        {
            string insertOrderSql = @"
        INSERT INTO DonHang
        (
            MaKhachHang,
            HoTenNguoiNhan,
            SoDienThoai,
            DiaChiGiaoHang,
            TongTien,
            TrangThai,
            NgayDat
        )
        VALUES
        (
            @MaKhachHang,
            @HoTenNguoiNhan,
            @SoDienThoai,
            @DiaChiGiaoHang,
            @TongTien,
            @TrangThai,
            GETDATE()
        );

        SELECT CAST(SCOPE_IDENTITY() AS INT);
    ";


            // ==========================================
            // LẤY SẢN PHẨM VÀ KHÓA DÒNG
            // ==========================================

            string checkProductSql = @"
        SELECT
            SoLuong,
            TrangThai,
            Gia,
            TenSanPham
        FROM SanPham WITH (UPDLOCK, HOLDLOCK)
        WHERE MaSanPham = @MaSanPham;
    ";


            // ==========================================
            // TRỪ TỒN KHO
            // ==========================================

            string updateStockSql = @"
        UPDATE SanPham
        SET
            SoLuong = SoLuong - @SoLuong
        WHERE
            MaSanPham = @MaSanPham
            AND TrangThai = 1
            AND SoLuong >= @SoLuong;
    ";


            // ==========================================
            // TẠO CHI TIẾT ĐƠN
            // ==========================================

            string insertDetailSql = @"
        INSERT INTO ChiTietDonHang
        (
            MaDonHang,
            MaSanPham,
            SoLuong,
            DonGia
        )
        VALUES
        (
            @MaDonHang,
            @MaSanPham,
            @SoLuong,
            @DonGia
        );
    ";


            using (SqlConnection conn =
                database.GetConnection())
            {
                conn.Open();


                // ==========================================
                // TRANSACTION SERIALIZABLE
                // ==========================================

                using (SqlTransaction transaction =
                    conn.BeginTransaction(
                        System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        // ======================================
                        // KIỂM TRA ĐƠN HÀNG
                        // ======================================

                        if (donHang == null)
                        {
                            throw new InvalidOperationException(
                                "Đơn hàng không hợp lệ.");
                        }


                        // ======================================
                        // KIỂM TRA KHÁCH HÀNG
                        // ======================================

                        if (!donHang.MaKhachHang.HasValue ||
                            donHang.MaKhachHang.Value <= 0)
                        {
                            throw new InvalidOperationException(
                                "Khách hàng chưa đăng nhập hoặc không hợp lệ.");
                        }


                        // ======================================
                        // KIỂM TRA CHI TIẾT
                        // ======================================

                        if (donHang.ChiTiet == null ||
                            donHang.ChiTiet.Count == 0)
                        {
                            throw new InvalidOperationException(
                                "Đơn hàng không có sản phẩm.");
                        }


                        // ======================================
                        // KIỂM TRA KHÁCH HÀNG TRONG DATABASE
                        // ======================================

                        const string checkCustomerSql = @"
                    SELECT COUNT(*)
                    FROM KhachHang
                    WHERE MaKhachHang = @MaKhachHang;
                ";


                        using (SqlCommand customerCmd =
                            new SqlCommand(
                                checkCustomerSql,
                                conn,
                                transaction))
                        {
                            customerCmd.Parameters.Add(
                                "@MaKhachHang",
                                SqlDbType.Int)
                                .Value =
                                donHang.MaKhachHang.Value;


                            int customerCount =
                                Convert.ToInt32(
                                    customerCmd.ExecuteScalar());


                            if (customerCount != 1)
                            {
                                throw new InvalidOperationException(
                                    "Khách hàng không tồn tại.");
                            }
                        }


                        // ======================================
                        // VALIDATION HỌ TÊN
                        // ======================================

                        if (string.IsNullOrWhiteSpace(
                            donHang.HoTenNguoiNhan) ||
                            donHang.HoTenNguoiNhan.Trim().Length < 2 ||
                            donHang.HoTenNguoiNhan.Trim().Length > 100)
                        {
                            throw new InvalidOperationException(
                                "Họ tên người nhận không hợp lệ.");
                        }


                        // ======================================
                        // VALIDATION SỐ ĐIỆN THOẠI
                        // ======================================

                        if (string.IsNullOrWhiteSpace(
                            donHang.SoDienThoai) ||
                            !Regex.IsMatch(
                                donHang.SoDienThoai.Trim(),
                                @"^\d{10,11}$"))
                        {
                            throw new InvalidOperationException(
                                "Số điện thoại phải gồm 10 hoặc 11 chữ số.");
                        }


                        // ======================================
                        // VALIDATION ĐỊA CHỈ
                        // ======================================

                        if (string.IsNullOrWhiteSpace(
                            donHang.DiaChiGiaoHang) ||
                            donHang.DiaChiGiaoHang.Trim().Length < 5 ||
                            donHang.DiaChiGiaoHang.Trim().Length > 255)
                        {
                            throw new InvalidOperationException(
                                "Địa chỉ giao hàng không hợp lệ.");
                        }


                        // ======================================
                        // KIỂM TRA TỪNG SẢN PHẨM
                        // ======================================

                        foreach (
                            ChiTietDonHang chiTiet
                            in donHang.ChiTiet)
                        {
                            if (chiTiet == null)
                            {
                                throw new InvalidOperationException(
                                    "Chi tiết đơn hàng không hợp lệ.");
                            }


                            if (chiTiet.MaSanPham <= 0)
                            {
                                throw new InvalidOperationException(
                                    "Mã sản phẩm không hợp lệ.");
                            }


                            if (chiTiet.SoLuong <= 0)
                            {
                                throw new InvalidOperationException(
                                    "Số lượng sản phẩm phải lớn hơn 0.");
                            }


                            using (SqlCommand cmd =
                                new SqlCommand(
                                    checkProductSql,
                                    conn,
                                    transaction))
                            {
                                cmd.Parameters.Add(
                                    "@MaSanPham",
                                    SqlDbType.Int)
                                    .Value =
                                    chiTiet.MaSanPham;


                                using (SqlDataReader reader =
                                    cmd.ExecuteReader())
                                {
                                    if (!reader.Read())
                                    {
                                        throw new InvalidOperationException(
                                            "Sản phẩm mã "
                                            + chiTiet.MaSanPham
                                            + " không tồn tại.");
                                    }


                                    int tonKho =
                                        Convert.ToInt32(
                                            reader["SoLuong"]);


                                    bool trangThai =
                                        Convert.ToBoolean(
                                            reader["TrangThai"]);


                                    string tenSanPham =
                                        reader["TenSanPham"]
                                            .ToString();


                                    decimal giaDatabase =
                                        Convert.ToDecimal(
                                            reader["Gia"]);


                                    // ==============================
                                    // KIỂM TRA TRẠNG THÁI
                                    // ==============================

                                    if (!trangThai)
                                    {
                                        throw new InvalidOperationException(
                                            "Sản phẩm \""
                                            + tenSanPham
                                            + "\" hiện không còn được bán.");
                                    }


                                    // ==============================
                                    // KIỂM TRA HẾT HÀNG
                                    // ==============================

                                    if (tonKho <= 0)
                                    {
                                        throw new InvalidOperationException(
                                            "Sản phẩm \""
                                            + tenSanPham
                                            + "\" đã hết hàng.");
                                    }


                                    // ==============================
                                    // KIỂM TRA VƯỢT KHO
                                    // ==============================

                                    if (chiTiet.SoLuong > tonKho)
                                    {
                                        throw new InvalidOperationException(
                                            "Sản phẩm \""
                                            + tenSanPham
                                            + "\" chỉ còn "
                                            + tonKho
                                            + " sản phẩm.");
                                    }


                                    // ==============================
                                    // LẤY GIÁ MỚI NHẤT
                                    // ==============================

                                    chiTiet.DonGia =
                                        giaDatabase;
                                }
                            }
                        }


                        // ======================================
                        // TÍNH LẠI TỔNG TIỀN
                        // ======================================

                        decimal tongTienThucTe =
                            donHang.ChiTiet.Sum(
                                x =>
                                    x.SoLuong
                                    * x.DonGia);


                        donHang.TongTien =
                            tongTienThucTe;


                        int maDonHang;


                        // ======================================
                        // TẠO DONHANG
                        // ======================================

                        using (SqlCommand cmd =
                            new SqlCommand(
                                insertOrderSql,
                                conn,
                                transaction))
                        {
                            SqlParameter pMaKhachHang =
                                cmd.Parameters.Add(
                                    "@MaKhachHang",
                                    SqlDbType.Int);


                            pMaKhachHang.Value =
                                donHang.MaKhachHang.Value;


                            cmd.Parameters.Add(
                                "@HoTenNguoiNhan",
                                SqlDbType.NVarChar,
                                100)
                                .Value =
                                donHang.HoTenNguoiNhan;


                            cmd.Parameters.Add(
                                "@SoDienThoai",
                                SqlDbType.VarChar,
                                20)
                                .Value =
                                donHang.SoDienThoai;


                            cmd.Parameters.Add(
                                "@DiaChiGiaoHang",
                                SqlDbType.NVarChar,
                                255)
                                .Value =
                                donHang.DiaChiGiaoHang;


                            SqlParameter pTongTien =
                                cmd.Parameters.Add(
                                    "@TongTien",
                                    SqlDbType.Decimal);


                            pTongTien.Precision = 18;
                            pTongTien.Scale = 2;
                            pTongTien.Value =
                                donHang.TongTien;


                            cmd.Parameters.Add(
                                "@TrangThai",
                                SqlDbType.NVarChar,
                                50)
                                .Value =
                                string.IsNullOrWhiteSpace(
                                    donHang.TrangThai)
                                    ? "Chờ xử lý"
                                    : donHang.TrangThai;


                            object result =
                                cmd.ExecuteScalar();


                            if (result == null ||
                                result == DBNull.Value)
                            {
                                throw new InvalidOperationException(
                                    "Không thể tạo đơn hàng.");
                            }


                            maDonHang =
                                Convert.ToInt32(
                                    result);
                        }


                        // ======================================
                        // TRỪ KHO + TẠO CHI TIẾT
                        // ======================================

                        foreach (
                            ChiTietDonHang chiTiet
                            in donHang.ChiTiet)
                        {
                            // ==================================
                            // TRỪ TỒN KHO
                            // ==================================

                            using (SqlCommand cmd =
                                new SqlCommand(
                                    updateStockSql,
                                    conn,
                                    transaction))
                            {
                                cmd.Parameters.Add(
                                    "@MaSanPham",
                                    SqlDbType.Int)
                                    .Value =
                                    chiTiet.MaSanPham;


                                cmd.Parameters.Add(
                                    "@SoLuong",
                                    SqlDbType.Int)
                                    .Value =
                                    chiTiet.SoLuong;


                                int affectedRows =
                                    cmd.ExecuteNonQuery();


                                if (affectedRows != 1)
                                {
                                    throw new InvalidOperationException(
                                        "Không đủ tồn kho cho sản phẩm mã "
                                        + chiTiet.MaSanPham
                                        + ".");
                                }
                            }


                            // ==================================
                            // INSERT CHI TIẾT ĐƠN HÀNG
                            // ==================================

                            using (SqlCommand cmd =
                                new SqlCommand(
                                    insertDetailSql,
                                    conn,
                                    transaction))
                            {
                                cmd.Parameters.Add(
                                    "@MaDonHang",
                                    SqlDbType.Int)
                                    .Value =
                                    maDonHang;


                                cmd.Parameters.Add(
                                    "@MaSanPham",
                                    SqlDbType.Int)
                                    .Value =
                                    chiTiet.MaSanPham;


                                cmd.Parameters.Add(
                                    "@SoLuong",
                                    SqlDbType.Int)
                                    .Value =
                                    chiTiet.SoLuong;


                                SqlParameter pDonGia =
                                    cmd.Parameters.Add(
                                        "@DonGia",
                                        SqlDbType.Decimal);


                                pDonGia.Precision = 18;
                                pDonGia.Scale = 2;
                                pDonGia.Value =
                                    chiTiet.DonGia;


                                int result =
                                    cmd.ExecuteNonQuery();


                                if (result != 1)
                                {
                                    throw new InvalidOperationException(
                                        "Không thể lưu chi tiết đơn hàng.");
                                }
                            }
                        }


                        // ======================================
                        // COMMIT
                        // ======================================

                        transaction.Commit();


                        return maDonHang;
                    }
                    catch
                    {
                        // ======================================
                        // ROLLBACK
                        // ======================================

                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                            // Giữ nguyên lỗi ban đầu.
                        }


                        throw;
                    }
                }
            }
        }

        public int CountAll()
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


        public int CountByStatus(
            string trangThai)
        {
            string sql = @"
                SELECT COUNT(*)
                FROM DonHang
                WHERE TrangThai = @TrangThai
            ";


            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@TrangThai",
                        SqlDbType.NVarChar,
                        50
                    ).Value = trangThai;


                    conn.Open();


                    return Convert.ToInt32(
                        cmd.ExecuteScalar()
                    );
                }
            }
        }


        public bool UpdateTrangThai(
            int maDonHang,
            string trangThai)
        {
            string sql = @"
                UPDATE DonHang
                SET TrangThai = @TrangThai
                WHERE MaDonHang = @MaDonHang
            ";


            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaDonHang",
                        SqlDbType.Int
                    ).Value = maDonHang;


                    cmd.Parameters.Add(
                        "@TrangThai",
                        SqlDbType.NVarChar,
                        50
                    ).Value = trangThai;


                    conn.Open();


                    return
                        cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // ==========================================
        // HỦY ĐƠN HÀNG KHÁCH HÀNG
        // ==========================================

        public bool HuyDonHang(
            int maDonHang,
            int maKhachHang,
            out string message)
        {
            message = "";

            // ------------------------------------------
            // KIỂM TRA THAM SỐ
            // ------------------------------------------

            if (maDonHang <= 0 ||
                maKhachHang <= 0)
            {
                message =
                    "Thông tin đơn hàng không hợp lệ.";

                return false;
            }


            using (SqlConnection conn =
                database.GetConnection())
            {
                conn.Open();


                // ------------------------------------------
                // TRANSACTION
                // ------------------------------------------

                using (SqlTransaction transaction =
                    conn.BeginTransaction(
                        System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        // ------------------------------------------
                        // LẤY TRẠNG THÁI ĐƠN HÀNG
                        // ------------------------------------------

                        string getOrderSql = @"
                    SELECT TrangThai
                    FROM DonHang WITH (UPDLOCK, HOLDLOCK)

                    WHERE
                        MaDonHang = @MaDonHang

                        AND

                        MaKhachHang = @MaKhachHang;
                ";


                        string trangThai;


                        using (SqlCommand cmd =
                            new SqlCommand(
                                getOrderSql,
                                conn,
                                transaction))
                        {
                            cmd.Parameters.Add(
                                "@MaDonHang",
                                SqlDbType.Int
                            ).Value = maDonHang;


                            cmd.Parameters.Add(
                                "@MaKhachHang",
                                SqlDbType.Int
                            ).Value = maKhachHang;


                            object result =
                                cmd.ExecuteScalar();


                            // --------------------------------------
                            // KHÔNG TÌM THẤY ĐƠN
                            // --------------------------------------

                            if (result == null ||
                                result == DBNull.Value)
                            {
                                message =
                                    "Không tìm thấy đơn hàng "
                                    + "hoặc bạn không có quyền "
                                    + "hủy đơn hàng này.";

                                transaction.Rollback();

                                return false;
                            }


                            trangThai =
                                result.ToString();
                        }


                        // ------------------------------------------
                        // CHỈ CHO PHÉP HỦY "CHỜ XỬ LÝ"
                        // ------------------------------------------

                        if (!string.Equals(
                            trangThai,
                            "Chờ xử lý",
                            StringComparison.OrdinalIgnoreCase))
                        {
                            switch (trangThai)
                            {
                                case "Đã xác nhận":

                                    message =
                                        "Đơn hàng đã được xác nhận "
                                        + "nên không thể hủy.";

                                    break;


                                case "Đang giao":

                                    message =
                                        "Đơn hàng đang giao "
                                        + "nên không thể hủy.";

                                    break;


                                case "Đã giao":

                                    message =
                                        "Đơn hàng đã giao "
                                        + "nên không thể hủy.";

                                    break;


                                case "Đã hủy":

                                    message =
                                        "Đơn hàng này đã được "
                                        + "hủy trước đó.";

                                    break;


                                default:

                                    message =
                                        "Trạng thái đơn hàng hiện tại "
                                        + "không cho phép hủy.";

                                    break;
                            }


                            transaction.Rollback();

                            return false;
                        }


                        // ------------------------------------------
                        // LẤY CHI TIẾT ĐƠN HÀNG
                        // ------------------------------------------

                        string restoreStockSql = @"
                    SELECT
                        MaSanPham,
                        SoLuong

                    FROM ChiTietDonHang
                    WITH (UPDLOCK, HOLDLOCK)

                    WHERE
                        MaDonHang = @MaDonHang;
                ";


                        var items =
                            new System.Collections.Generic
                            .List<ChiTietDonHang>();


                        using (SqlCommand cmd =
                            new SqlCommand(
                                restoreStockSql,
                                conn,
                                transaction))
                        {
                            cmd.Parameters.Add(
                                "@MaDonHang",
                                SqlDbType.Int
                            ).Value = maDonHang;


                            using (SqlDataReader reader =
                                cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    items.Add(
                                        new ChiTietDonHang
                                        {
                                            MaSanPham =
                                                Convert.ToInt32(
                                                    reader["MaSanPham"]),

                                            SoLuong =
                                                Convert.ToInt32(
                                                    reader["SoLuong"])
                                        });
                                }
                            }
                        }


                        // ------------------------------------------
                        // ĐƠN KHÔNG CÓ SẢN PHẨM
                        // ------------------------------------------

                        if (items.Count == 0)
                        {
                            message =
                                "Đơn hàng không có sản phẩm "
                                + "để hoàn tồn kho.";

                            transaction.Rollback();

                            return false;
                        }


                        // ------------------------------------------
                        // HOÀN TỒN KHO
                        // ------------------------------------------

                        string updateStockSql = @"
                    UPDATE SanPham

                    SET
                        SoLuong =
                            SoLuong + @SoLuong

                    WHERE
                        MaSanPham = @MaSanPham;
                ";


                        foreach (
                            ChiTietDonHang item
                            in items)
                        {
                            using (SqlCommand cmd =
                                new SqlCommand(
                                    updateStockSql,
                                    conn,
                                    transaction))
                            {
                                cmd.Parameters.Add(
                                    "@MaSanPham",
                                    SqlDbType.Int
                                ).Value =
                                    item.MaSanPham;


                                cmd.Parameters.Add(
                                    "@SoLuong",
                                    SqlDbType.Int
                                ).Value =
                                    item.SoLuong;


                                if (
                                    cmd.ExecuteNonQuery()
                                    != 1)
                                {
                                    throw new InvalidOperationException(
                                        "Không thể hoàn tồn kho "
                                        + "cho sản phẩm mã "
                                        + item.MaSanPham
                                        + ".");
                                }
                            }
                        }


                        // ------------------------------------------
                        // ĐỔI TRẠNG THÁI ĐƠN
                        // ------------------------------------------

                        string cancelSql = @"
                    UPDATE DonHang

                    SET
                        TrangThai = N'Đã hủy'

                    WHERE
                        MaDonHang = @MaDonHang

                        AND

                        MaKhachHang = @MaKhachHang

                        AND

                        TrangThai = N'Chờ xử lý';
                ";


                        using (SqlCommand cmd =
                            new SqlCommand(
                                cancelSql,
                                conn,
                                transaction))
                        {
                            cmd.Parameters.Add(
                                "@MaDonHang",
                                SqlDbType.Int
                            ).Value = maDonHang;


                            cmd.Parameters.Add(
                                "@MaKhachHang",
                                SqlDbType.Int
                            ).Value = maKhachHang;


                            if (
                                cmd.ExecuteNonQuery()
                                != 1)
                            {
                                throw new InvalidOperationException(
                                    "Đơn hàng đã thay đổi trạng thái "
                                    + "và không thể hủy.");
                            }
                        }


                        // ------------------------------------------
                        // COMMIT
                        // ------------------------------------------

                        transaction.Commit();


                        message =
                            "Đơn hàng đã được hủy thành công "
                            + "và tồn kho đã được hoàn lại.";


                        return true;
                    }
                    catch
                    {
                        // ------------------------------------------
                        // ROLLBACK NẾU CÓ LỖI
                        // ------------------------------------------

                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                            // Giữ nguyên lỗi ban đầu.
                        }


                        throw;
                    }
                }
            }
        }

    }
}