using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace web_ban_hang2.Services
{
    /// <summary>
    /// Lấy dữ liệu thật từ ShopWebForms
    /// để cung cấp cho AI.
    /// </summary>
    public class ChatbotCatalogService
    {
        private readonly DAL.Database database;

        public ChatbotCatalogService()
        {
            database = new DAL.Database();
        }

        public string BuildCatalogContext(
            string question,
            int? maKhachHang)
        {
            StringBuilder context =
                new StringBuilder();

            context.AppendLine(
                "DỮ LIỆU SHOP THỰC TẾ:");

            context.AppendLine(
                "Chỉ sử dụng dữ liệu dưới đây "
                + "cho thông tin sản phẩm "
                + "và đơn hàng.");

            context.AppendLine();

            AppendCategories(context);

            AppendRelevantProducts(
                context,
                question);

            if (maKhachHang.HasValue)
            {
                AppendCustomerOrders(
                    context,
                    maKhachHang.Value);
            }
            else
            {
                context.AppendLine(
                    "KHÁCH HÀNG: Chưa đăng nhập. "
                    + "Không được suy đoán "
                    + "thông tin đơn hàng cá nhân.");
            }

            return context.ToString();
        }

        private void AppendCategories(
            StringBuilder context)
        {
            string sql = @"
                SELECT TOP 30
                    MaDanhMuc,
                    TenDanhMuc
                FROM DanhMuc
                WHERE TrangThai = 1
                ORDER BY TenDanhMuc";

            using (SqlConnection conn =
                database.GetConnection())

            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                conn.Open();

                using (SqlDataReader reader =
                    cmd.ExecuteReader())
                {
                    context.AppendLine(
                        "DANH MỤC ĐANG HOẠT ĐỘNG:");

                    while (reader.Read())
                    {
                        context.AppendLine(
                            "- "
                            + reader["MaDanhMuc"]
                            + ": "
                            + reader["TenDanhMuc"]);
                    }

                    context.AppendLine();
                }
            }
        }

        private void AppendRelevantProducts(
            StringBuilder context,
            string question)
        {
            decimal? maxPrice =
                ExtractMaxPrice(question);

            decimal? minPrice =
                ExtractMinPrice(question);

            string keyword =
                ExtractSearchKeyword(question);

            string sql = @"
                SELECT TOP 20
                    sp.MaSanPham,
                    sp.TenSanPham,
                    sp.MoTa,
                    sp.Gia,
                    sp.SoLuong,
                    sp.HinhAnh,
                    dm.TenDanhMuc
                FROM SanPham sp
                INNER JOIN DanhMuc dm
                    ON sp.MaDanhMuc =
                       dm.MaDanhMuc
                WHERE sp.TrangThai = 1
                  AND dm.TrangThai = 1
                  AND sp.SoLuong > 0

                  AND (
                       @Keyword = ''
                       OR sp.TenSanPham
                            LIKE @LikeKeyword
                       OR ISNULL(
                            sp.MoTa,
                            '') LIKE @LikeKeyword
                       OR dm.TenDanhMuc
                            LIKE @LikeKeyword
                  )

                  AND (
                       @MinPrice IS NULL
                       OR sp.Gia >= @MinPrice
                  )

                  AND (
                       @MaxPrice IS NULL
                       OR sp.Gia <= @MaxPrice
                  )

                ORDER BY
                    CASE
                        WHEN sp.NoiBat = 1
                        THEN 0
                        ELSE 1
                    END,
                    sp.MaSanPham DESC";

            using (SqlConnection conn =
                database.GetConnection())

            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@Keyword",
                    SqlDbType.NVarChar,
                    200).Value = keyword;

                cmd.Parameters.Add(
                    "@LikeKeyword",
                    SqlDbType.NVarChar,
                    205).Value =
                        "%" + keyword + "%";

                SqlParameter pMin =
                    cmd.Parameters.Add(
                        "@MinPrice",
                        SqlDbType.Decimal);

                pMin.Precision = 18;
                pMin.Scale = 2;

                pMin.Value =
                    minPrice.HasValue
                        ? (object)minPrice.Value
                        : DBNull.Value;

                SqlParameter pMax =
                    cmd.Parameters.Add(
                        "@MaxPrice",
                        SqlDbType.Decimal);

                pMax.Precision = 18;
                pMax.Scale = 2;

                pMax.Value =
                    maxPrice.HasValue
                        ? (object)maxPrice.Value
                        : DBNull.Value;

                conn.Open();

                using (SqlDataReader reader =
                    cmd.ExecuteReader())
                {
                    context.AppendLine(
                        "SẢN PHẨM ĐANG BÁN, "
                        + "CÒN HÀNG VÀ PHÙ HỢP "
                        + "VỚI CÂU HỎI:");

                    bool hasRows = false;

                    while (reader.Read())
                    {
                        hasRows = true;

                        context.AppendLine(
                            string.Format(
                                "- MaSP={0}; " +
                                "Tên={1}; " +
                                "Danh mục={2}; " +
                                "Giá={3:N0} VNĐ; " +
                                "Tồn kho={4}; " +
                                "Mô tả={5}; " +
                                "Hình ảnh={6}",

                                reader["MaSanPham"],

                                reader["TenSanPham"],

                                reader["TenDanhMuc"],

                                Convert.ToDecimal(
                                    reader["Gia"]),

                                reader["SoLuong"],

                                CleanText(
                                    Convert.ToString(
                                        reader["MoTa"]),
                                    500),

                                Convert.ToString(
                                    reader["HinhAnh"])));
                    }

                    if (!hasRows)
                    {
                        context.AppendLine(
                            "- Không tìm thấy sản phẩm "
                            + "đang bán/còn hàng phù hợp "
                            + "với bộ lọc hiện tại.");
                    }

                    context.AppendLine();
                }
            }
        }

        private void AppendCustomerOrders(
            StringBuilder context,
            int maKhachHang)
        {
            string sql = @"
                SELECT TOP 5
                    MaDonHang,
                    TongTien,
                    TrangThai,
                    NgayDat
                FROM DonHang
                WHERE MaKhachHang =
                      @MaKhachHang
                ORDER BY MaDonHang DESC";

            using (SqlConnection conn =
                database.GetConnection())

            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@MaKhachHang",
                    SqlDbType.Int).Value =
                        maKhachHang;

                conn.Open();

                using (SqlDataReader reader =
                    cmd.ExecuteReader())
                {
                    context.AppendLine(
                        "5 ĐƠN HÀNG GẦN NHẤT "
                        + "CỦA KHÁCH ĐANG ĐĂNG NHẬP:");

                    bool hasRows = false;

                    while (reader.Read())
                    {
                        hasRows = true;

                        context.AppendLine(
                            string.Format(
                                "- Đơn #{0}; " +
                                "Tổng={1:N0} VNĐ; " +
                                "Trạng thái={2}; " +
                                "Ngày đặt={3:dd/MM/yyyy HH:mm}",

                                reader["MaDonHang"],

                                Convert.ToDecimal(
                                    reader["TongTien"]),

                                reader["TrangThai"],

                                Convert.ToDateTime(
                                    reader["NgayDat"])));
                    }

                    if (!hasRows)
                    {
                        context.AppendLine(
                            "- Khách hàng chưa có đơn hàng.");
                    }

                    context.AppendLine();
                }
            }
        }

        private static decimal? ExtractMaxPrice(
            string question)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return null;
            }

            Match m = Regex.Match(
                question,

                @"(?:dưới|không quá|tối đa|<=|thấp hơn)" +
                @"\s*(\d+(?:[\.,]\d+)?)" +
                @"\s*(triệu|tr|m)?",

                RegexOptions.IgnoreCase);

            if (!m.Success)
            {
                return null;
            }

            decimal value;

            if (!TryParseNumber(
                m.Groups[1].Value,
                out value))
            {
                return null;
            }

            string unit =
                m.Groups[2]
                    .Value
                    .ToLowerInvariant();

            if (unit == "triệu" ||
                unit == "tr" ||
                unit == "m")
            {
                value *= 1000000m;
            }

            if (value < 100000m)
            {
                value *= 1000000m;
            }

            return value;
        }

        private static decimal? ExtractMinPrice(
            string question)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return null;
            }

            Match m = Regex.Match(
                question,

                @"(?:trên|từ|ít nhất|>=|cao hơn)" +
                @"\s*(\d+(?:[\.,]\d+)?)" +
                @"\s*(triệu|tr|m)?",

                RegexOptions.IgnoreCase);

            if (!m.Success)
            {
                return null;
            }

            decimal value;

            if (!TryParseNumber(
                m.Groups[1].Value,
                out value))
            {
                return null;
            }

            string unit =
                m.Groups[2]
                    .Value
                    .ToLowerInvariant();

            if (unit == "triệu" ||
                unit == "tr" ||
                unit == "m")
            {
                value *= 1000000m;
            }

            if (value < 100000m)
            {
                value *= 1000000m;
            }

            return value;
        }

        private static bool TryParseNumber(
            string value,
            out decimal number)
        {
            number = 0;

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            value = value
                .Replace(".", "")
                .Replace(",", ".");

            return decimal.TryParse(
                value,
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture,
                out number);
        }

        private static string ExtractSearchKeyword(
            string question)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return string.Empty;
            }

            string value =
                question.Trim();

            value = Regex.Replace(
                value,

                @"\b(dưới|trên|từ|đến|khoảng|giá|" +
                @"bao nhiêu|sản phẩm|shop|có|không|" +
                @"cho|tôi|mình|muốn|cần|tư vấn|" +
                @"gợi ý|xin|hãy|với|nhé|ạ|triệu|tr|m)\b",

                " ",

                RegexOptions.IgnoreCase);

            value = Regex.Replace(
                value,
                @"\d+(?:[\.,]\d+)?",
                " ");

            value = Regex.Replace(
                value,
                @"[^\p{L}\p{N} ]",
                " ");

            value = Regex.Replace(
                value,
                @"\s+",
                " ")
                .Trim();

            if (value.Length > 120)
            {
                value =
                    value.Substring(0, 120);
            }

            return value;
        }

        private static string CleanText(
            string value,
            int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "Không có mô tả";
            }

            value = Regex.Replace(
                value,
                @"\s+",
                " ")
                .Trim();

            if (value.Length > maxLength)
            {
                value =
                    value.Substring(
                        0,
                        maxLength)
                    + "...";
            }

            return value;
        }
    }
}