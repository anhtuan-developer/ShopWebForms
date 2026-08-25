using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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


            string checkProductSql = @"
        SELECT
            SoLuong,
            TrangThai,
            Gia,
            TenSanPham
        FROM SanPham WITH (UPDLOCK, HOLDLOCK)
        WHERE MaSanPham = @MaSanPham;
    ";


            string updateStockSql = @"
        UPDATE SanPham
        SET SoLuong = SoLuong - @SoLuong
        WHERE
            MaSanPham = @MaSanPham
            AND TrangThai = 1
            AND SoLuong >= @SoLuong;
    ";


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


                using (SqlTransaction transaction =
                    conn.BeginTransaction(
                        System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        
                        if (donHang == null)
                        {
                            throw new InvalidOperationException(
                                "Đơn hàng không hợp lệ.");
                        }


                        if (donHang.ChiTiet == null ||
                            donHang.ChiTiet.Count == 0)
                        {
                            throw new InvalidOperationException(
                                "Đơn hàng không có sản phẩm.");
                        }


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


                                    if (!trangThai)
                                    {
                                        throw new InvalidOperationException(
                                            "Sản phẩm \""
                                            + tenSanPham
                                            + "\" hiện không còn được bán.");
                                    }


                                    if (tonKho <= 0)
                                    {
                                        throw new InvalidOperationException(
                                            "Sản phẩm \""
                                            + tenSanPham
                                            + "\" đã hết hàng.");
                                    }


                                    if (chiTiet.SoLuong > tonKho)
                                    {
                                        throw new InvalidOperationException(
                                            "Sản phẩm \""
                                            + tenSanPham
                                            + "\" chỉ còn "
                                            + tonKho
                                            + " sản phẩm.");
                                    }


                                    chiTiet.DonGia =
                                        giaDatabase;
                                }
                            }
                        }


                        decimal tongTienThucTe =
                            donHang.ChiTiet.Sum(
                                x =>
                                    x.SoLuong
                                    * x.DonGia);


                        donHang.TongTien =
                            tongTienThucTe;


                        int maDonHang;


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


                            if (donHang.MaKhachHang.HasValue)
                            {
                                pMaKhachHang.Value =
                                    donHang.MaKhachHang.Value;
                            }
                            else
                            {
                                pMaKhachHang.Value =
                                    DBNull.Value;
                            }


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


                        foreach (
                            ChiTietDonHang chiTiet
                            in donHang.ChiTiet)
                        {
                           
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


                        transaction.Commit();


                        return maDonHang;
                    }
                    catch
                    {
                        
                        transaction.Rollback();

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
    }
}