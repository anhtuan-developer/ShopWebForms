using System;
using System.Data;
using System.Data.SqlClient;

namespace web_ban_hang2.DAL
{
    public class SanPhamDAL
    {
        private readonly Database database;

        public SanPhamDAL()
        {
            database = new Database();
        }


        // ==========================================
        // LẤY TẤT CẢ SẢN PHẨM
        // ==========================================

        public DataTable GetAll()
        {
            string sql = @"
        SELECT
            sp.MaSanPham,
            sp.MaDanhMuc,
            sp.TenSanPham,
            sp.MoTa,
            sp.Gia,
            sp.SoLuong,
            sp.HinhAnh,
            sp.TrangThai,
            sp.NoiBat,
            sp.NgayTao,

            dm.TenDanhMuc

        FROM SanPham sp

        INNER JOIN DanhMuc dm
            ON sp.MaDanhMuc = dm.MaDanhMuc

        ORDER BY sp.MaSanPham DESC
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


        // ==========================================
        // LẤY SẢN PHẨM THEO MÃ
        // ==========================================

        public DataTable GetById(int maSanPham)
        {
            string sql = @"
        SELECT
            MaSanPham,
            MaDanhMuc,
            TenSanPham,
            MoTa,
            Gia,
            SoLuong,
            HinhAnh,
            TrangThai,
            NoiBat,
            NgayTao

        FROM SanPham

        WHERE MaSanPham = @MaSanPham
    ";

            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaSanPham",
                        SqlDbType.Int
                    ).Value = maSanPham;

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


        // ==========================================
        // THÊM SẢN PHẨM
        // ==========================================

        public bool Insert(
    int maDanhMuc,
    string tenSanPham,
    string moTa,
    decimal gia,
    int soLuong,
    string hinhAnh,
    bool trangThai,
    bool noiBat)
        {
            string sql = @"
        INSERT INTO SanPham
        (
            MaDanhMuc,
            TenSanPham,
            MoTa,
            Gia,
            SoLuong,
            HinhAnh,
            TrangThai,
            NoiBat
        )

        VALUES
        (
            @MaDanhMuc,
            @TenSanPham,
            @MoTa,
            @Gia,
            @SoLuong,
            @HinhAnh,
            @TrangThai,
            @NoiBat
        )
    ";

            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaDanhMuc",
                        SqlDbType.Int
                    ).Value = maDanhMuc;

                    cmd.Parameters.Add(
                        "@TenSanPham",
                        SqlDbType.NVarChar,
                        200
                    ).Value = tenSanPham;

                    cmd.Parameters.Add(
                        "@MoTa",
                        SqlDbType.NVarChar
                    ).Value =
                        string.IsNullOrWhiteSpace(moTa)
                            ? (object)DBNull.Value
                            : moTa;

                    SqlParameter pGia =
                        cmd.Parameters.Add(
                            "@Gia",
                            SqlDbType.Decimal
                        );

                    pGia.Precision = 18;
                    pGia.Scale = 2;
                    pGia.Value = gia;


                    cmd.Parameters.Add(
                        "@SoLuong",
                        SqlDbType.Int
                    ).Value = soLuong;

                    cmd.Parameters.Add(
                        "@HinhAnh",
                        SqlDbType.NVarChar,
                        500
                    ).Value =
                        string.IsNullOrWhiteSpace(hinhAnh)
                            ? (object)DBNull.Value
                            : hinhAnh;

                    cmd.Parameters.Add(
                        "@TrangThai",
                        SqlDbType.Bit
                    ).Value = trangThai;

                    cmd.Parameters.Add(
                        "@NoiBat",
                        SqlDbType.Bit
                    ).Value = noiBat;


                    conn.Open();

                    int result =
                        cmd.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }


        // ==========================================
        // CẬP NHẬT SẢN PHẨM
        // ==========================================

        public bool Update(
    int maSanPham,
    int maDanhMuc,
    string tenSanPham,
    string moTa,
    decimal gia,
    int soLuong,
    string hinhAnh,
    bool trangThai,
    bool noiBat)
        {
            string sql = @"
        UPDATE SanPham

        SET
            MaDanhMuc = @MaDanhMuc,
            TenSanPham = @TenSanPham,
            MoTa = @MoTa,
            Gia = @Gia,
            SoLuong = @SoLuong,
            HinhAnh = @HinhAnh,
            TrangThai = @TrangThai,
            NoiBat = @NoiBat

        WHERE
            MaSanPham = @MaSanPham
    ";

            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaSanPham",
                        SqlDbType.Int
                    ).Value = maSanPham;

                    cmd.Parameters.Add(
                        "@MaDanhMuc",
                        SqlDbType.Int
                    ).Value = maDanhMuc;

                    cmd.Parameters.Add(
                        "@TenSanPham",
                        SqlDbType.NVarChar,
                        200
                    ).Value = tenSanPham;

                    cmd.Parameters.Add(
                        "@MoTa",
                        SqlDbType.NVarChar
                    ).Value =
                        string.IsNullOrWhiteSpace(moTa)
                            ? (object)DBNull.Value
                            : moTa;

                    SqlParameter pGia =
                        cmd.Parameters.Add(
                            "@Gia",
                            SqlDbType.Decimal
                        );

                    pGia.Precision = 18;
                    pGia.Scale = 2;
                    pGia.Value = gia;


                    cmd.Parameters.Add(
                        "@SoLuong",
                        SqlDbType.Int
                    ).Value = soLuong;

                    cmd.Parameters.Add(
                        "@HinhAnh",
                        SqlDbType.NVarChar,
                        500
                    ).Value =
                        string.IsNullOrWhiteSpace(hinhAnh)
                            ? (object)DBNull.Value
                            : hinhAnh;

                    cmd.Parameters.Add(
                        "@TrangThai",
                        SqlDbType.Bit
                    ).Value = trangThai;

                    cmd.Parameters.Add(
                        "@NoiBat",
                        SqlDbType.Bit
                    ).Value = noiBat;


                    conn.Open();

                    int result =
                        cmd.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }


        // XÓA SẢN PHẨM

        public bool Delete(int maSanPham)
        {
            string sql = @"
        DELETE FROM SanPham

        WHERE
            MaSanPham = @MaSanPham
    ";

            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaSanPham",
                        SqlDbType.Int
                    ).Value = maSanPham;

                    conn.Open();

                    int result =
                        cmd.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }


        // NGỪNG BÁN SẢN PHẨM

        public bool NgungBan(int maSanPham)
        {
            string sql = @"
        UPDATE SanPham

        SET
            TrangThai = 0

        WHERE
            MaSanPham = @MaSanPham
    ";

            using (SqlConnection conn =
                database.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaSanPham",
                        SqlDbType.Int
                    ).Value = maSanPham;

                    conn.Open();

                    int result =
                        cmd.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }
        public int CountOrderDetails(int maSanPham)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM ChiTietDonHang
        WHERE MaSanPham = @MaSanPham
    ";

            using (SqlConnection conn = database.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@MaSanPham",
                        SqlDbType.Int
                    ).Value = maSanPham;

                    conn.Open();

                    return Convert.ToInt32(
                        cmd.ExecuteScalar()
                    );
                }
            }
        }


        // ==========================================
        // TÌM KIẾM + DANH MỤC + PHÂN TRANG
        // CHỈ HIỂN THỊ SẢN PHẨM ĐANG BÁN
        // ==========================================

        public DataTable SearchPaged(
    string keyword,
    int maDanhMuc,
    decimal? minPrice,
    decimal? maxPrice,
    int status,
    string sort,
    int pageNumber,
    int pageSize,
    out int totalRecords)
        {
            DataTable dt = new DataTable();

            totalRecords = 0;

            keyword =
                (keyword ?? string.Empty)
                .Trim();


            if (pageNumber < 1)
            {
                pageNumber = 1;
            }


            if (pageSize < 1)
            {
                pageSize = 12;
            }


            if (maDanhMuc < 0)
            {
                maDanhMuc = 0;
            }


            if (
                status < 0
                ||
                status > 2
            )
            {
                status = 0;
            }


            sort =
                (sort ?? "newest")
                .Trim()
                .ToLowerInvariant();


            if (
                sort != "price_asc"
                &&
                sort != "price_desc"
                &&
                sort != "bestseller"
            )
            {
                sort = "newest";
            }


            int offset =
                (pageNumber - 1)
                *
                pageSize;


            string sql = @"

        ;WITH ProductData AS
        (
            SELECT

                sp.MaSanPham,

                sp.MaDanhMuc,

                sp.TenSanPham,

                sp.MoTa,

                sp.Gia,

                sp.SoLuong,

                sp.HinhAnh,

                sp.TrangThai,

                sp.NgayTao,

                dm.TenDanhMuc,

                ISNULL(
                    Sales.TotalSold,
                    0
                ) AS TotalSold


            FROM SanPham sp


            INNER JOIN DanhMuc dm

                ON sp.MaDanhMuc =
                   dm.MaDanhMuc


            LEFT JOIN
            (
                SELECT

                    ctdh.MaSanPham,

                    SUM(
                        ctdh.SoLuong
                    ) AS TotalSold

                FROM ChiTietDonHang ctdh


                INNER JOIN DonHang dh

                    ON ctdh.MaDonHang =
                       dh.MaDonHang


                WHERE

                    dh.TrangThai NOT IN
                    (
                        N'Đã hủy',
                        N'Đã huỷ',
                        N'Hủy',
                        N'Huỷ'
                    )


                GROUP BY
                    ctdh.MaSanPham

            ) Sales

                ON sp.MaSanPham =
                   Sales.MaSanPham


            WHERE

                sp.TrangThai = 1

                AND

                dm.TrangThai = 1


                -- =========================================
                -- DANH MỤC
                -- =========================================

                AND
                (
                    @MaDanhMuc = 0

                    OR

                    sp.MaDanhMuc =
                    @MaDanhMuc
                )


                -- =========================================
                -- TÌM KIẾM
                --
                -- Tên sản phẩm
                -- Mô tả
                -- Danh mục
                -- =========================================

                AND
                (
                    @Keyword = N''

                    OR

                    sp.TenSanPham
                        LIKE
                        N'%' +
                        @Keyword +
                        N'%'

                    OR

                    ISNULL(
                        sp.MoTa,
                        N''
                    )
                    LIKE
                    N'%' +
                    @Keyword +
                    N'%'

                    OR

                    dm.TenDanhMuc
                        LIKE
                        N'%' +
                        @Keyword +
                        N'%'
                )


                -- =========================================
                -- GIÁ TỐI THIỂU
                -- =========================================

                AND
                (
                    @MinPrice IS NULL

                    OR

                    sp.Gia >= @MinPrice
                )


                -- =========================================
                -- GIÁ TỐI ĐA
                -- =========================================

                AND
                (
                    @MaxPrice IS NULL

                    OR

                    sp.Gia <= @MaxPrice
                )


                -- =========================================
                -- TRẠNG THÁI KHO
                --
                -- 0 = Tất cả
                -- 1 = Còn hàng
                -- 2 = Hết hàng
                -- =========================================

                AND
                (
                    @Status = 0

                    OR

                    (
                        @Status = 1
                        AND
                        sp.SoLuong > 0
                    )

                    OR

                    (
                        @Status = 2
                        AND
                        sp.SoLuong <= 0
                    )
                )
        )


        -- =====================================================
        -- LẤY DANH SÁCH
        -- =====================================================

        SELECT

            MaSanPham,

            MaDanhMuc,

            TenSanPham,

            MoTa,

            Gia,

            SoLuong,

            HinhAnh,

            TrangThai,

            NgayTao,

            TenDanhMuc


        FROM ProductData


        ORDER BY

            CASE
                WHEN
                    @Sort = 'price_asc'
                THEN Gia
            END ASC,


            CASE
                WHEN
                    @Sort = 'price_desc'
                THEN Gia
            END DESC,


            CASE
                WHEN
                    @Sort = 'bestseller'
                THEN TotalSold
            END DESC,


            CASE
                WHEN
                    @Sort = 'bestseller'
                THEN MaSanPham
            END DESC,


            CASE
                WHEN
                    @Sort = 'newest'
                THEN NgayTao
            END DESC,


            MaSanPham DESC


        OFFSET @Offset ROWS

        FETCH NEXT @PageSize ROWS ONLY;


        -- =====================================================
        -- ĐẾM TỔNG SẢN PHẨM
        -- =====================================================

        SELECT
            COUNT(*)

        FROM SanPham sp


        INNER JOIN DanhMuc dm

            ON sp.MaDanhMuc =
               dm.MaDanhMuc


        WHERE

            sp.TrangThai = 1

            AND

            dm.TrangThai = 1


            AND
            (
                @MaDanhMuc = 0

                OR

                sp.MaDanhMuc =
                @MaDanhMuc
            )


            AND
            (
                @Keyword = N''

                OR

                sp.TenSanPham
                    LIKE
                    N'%' +
                    @Keyword +
                    N'%'

                OR

                ISNULL(
                    sp.MoTa,
                    N''
                )
                LIKE
                N'%' +
                @Keyword +
                N'%'

                OR

                dm.TenDanhMuc
                    LIKE
                    N'%' +
                    @Keyword +
                    N'%'
            )


            AND
            (
                @MinPrice IS NULL

                OR

                sp.Gia >= @MinPrice
            )


            AND
            (
                @MaxPrice IS NULL

                OR

                sp.Gia <= @MaxPrice
            )


            AND
            (
                @Status = 0

                OR

                (
                    @Status = 1
                    AND
                    sp.SoLuong > 0
                )

                OR

                (
                    @Status = 2
                    AND
                    sp.SoLuong <= 0
                )
            );

    ";


            using (
                SqlConnection conn =
                    database.GetConnection()
            )
            using (
                SqlCommand cmd =
                    new SqlCommand(
                        sql,
                        conn
                    )
            )
            {
                // =====================================================
                // KEYWORD
                // =====================================================

                cmd.Parameters.Add(
                    "@Keyword",
                    SqlDbType.NVarChar,
                    200
                ).Value =
                    keyword;


                // =====================================================
                // CATEGORY
                // =====================================================

                cmd.Parameters.Add(
                    "@MaDanhMuc",
                    SqlDbType.Int
                ).Value =
                    maDanhMuc;


                // =====================================================
                // MIN PRICE
                // =====================================================

                SqlParameter pMin =
                    cmd.Parameters.Add(
                        "@MinPrice",
                        SqlDbType.Decimal
                    );


                pMin.Precision = 18;
                pMin.Scale = 2;


                pMin.Value =
                    minPrice.HasValue
                    ?
                    (object)minPrice.Value
                    :
                    DBNull.Value;


                // =====================================================
                // MAX PRICE
                // =====================================================

                SqlParameter pMax =
                    cmd.Parameters.Add(
                        "@MaxPrice",
                        SqlDbType.Decimal
                    );


                pMax.Precision = 18;
                pMax.Scale = 2;


                pMax.Value =
                    maxPrice.HasValue
                    ?
                    (object)maxPrice.Value
                    :
                    DBNull.Value;


                // =====================================================
                // STATUS
                // =====================================================

                cmd.Parameters.Add(
                    "@Status",
                    SqlDbType.Int
                ).Value =
                    status;


                // =====================================================
                // SORT
                // =====================================================

                cmd.Parameters.Add(
                    "@Sort",
                    SqlDbType.VarChar,
                    20
                ).Value =
                    sort;


                // =====================================================
                // OFFSET
                // =====================================================

                cmd.Parameters.Add(
                    "@Offset",
                    SqlDbType.Int
                ).Value =
                    offset;


                // =====================================================
                // PAGE SIZE
                // =====================================================

                cmd.Parameters.Add(
                    "@PageSize",
                    SqlDbType.Int
                ).Value =
                    pageSize;


                // =====================================================
                // THỰC THI
                // =====================================================

                using (
                    SqlDataAdapter adapter =
                        new SqlDataAdapter(cmd)
                )
                {
                    DataSet dataSet =
                        new DataSet();


                    adapter.Fill(
                        dataSet
                    );


                    // =================================================
                    // TỔNG SỐ RECORD
                    // =================================================

                    if (
                        dataSet.Tables.Count > 1
                        &&
                        dataSet.Tables[1]
                            .Rows.Count > 0
                    )
                    {
                        totalRecords =
                            Convert.ToInt32(
                                dataSet
                                    .Tables[1]
                                    .Rows[0][0]
                            );
                    }


                    // =================================================
                    // DATA
                    // =================================================

                    if (
                        dataSet.Tables.Count > 0
                    )
                    {
                        return
                            dataSet.Tables[0];
                    }


                    return
                        new DataTable();
                }
            }
        }


        public DataTable GetFeaturedProducts(int numberOfProducts)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn =
                database.GetConnection())
            {
                string sql = @"
            SELECT TOP (@NumberOfProducts)
                sp.MaSanPham,
                sp.MaDanhMuc,
                sp.TenSanPham,
                sp.MoTa,
                sp.Gia,
                sp.SoLuong,
                sp.HinhAnh,
                sp.NoiBat,
                sp.TrangThai,
                sp.NgayTao,
                dm.TenDanhMuc

            FROM SanPham sp

            INNER JOIN DanhMuc dm
                ON sp.MaDanhMuc = dm.MaDanhMuc

            WHERE
                sp.NoiBat = 1
                AND sp.TrangThai = 1
                AND dm.TrangThai = 1
                AND sp.SoLuong > 0

            ORDER BY
                sp.MaSanPham DESC;
        ";

                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@NumberOfProducts",
                        SqlDbType.Int
                    ).Value = numberOfProducts;

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

    }
}