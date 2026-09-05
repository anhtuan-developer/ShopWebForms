using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace web_ban_hang2.Services
{
    /// <summary>
    /// Lấy dữ liệu thật từ ShopWebForms
    /// để cung cấp cho AI.
    ///
    /// Hỗ trợ:
    /// - tìm nhiều keyword
    /// - từ đồng nghĩa
    /// - ngân sách
    /// - câu hỏi tự nhiên
    /// - câu hỏi nối tiếp
    /// - fallback sản phẩm
    /// </summary>
    public class ChatbotCatalogService
    {
        private readonly DAL.Database database;

        public ChatbotCatalogService()
        {
            database =
                new DAL.Database();
        }

        public string BuildCatalogContext(
            string question,
            int? maKhachHang,
            IList<ChatMessage> history = null)
        {
            StringBuilder context =
                new StringBuilder();

            bool productQuestion =
                IsProductQuestion(
                    question);

            bool orderQuestion =
                IsOrderQuestion(
                    question);

            context.AppendLine(
                "DỮ LIỆU SHOP THỰC TẾ:");

            context.AppendLine(
                "Chỉ sử dụng dữ liệu này cho thông tin riêng của SHOP.");

            context.AppendLine();

            /*
             * Chỉ truy vấn sản phẩm khi câu hỏi
             * thực sự liên quan tới sản phẩm.
             */
            if (productQuestion)
            {
                AppendCategories(
                    context);

                AppendRelevantProducts(
                    context,
                    BuildSearchQuestion(
                        question,
                        history));
            }

            /*
             * Chỉ truy vấn đơn hàng khi khách
             * thực sự hỏi về đơn hàng.
             */
            if (
                orderQuestion &&
                maKhachHang.HasValue)
            {
                AppendCustomerOrders(
                    context,
                    maKhachHang.Value);
            }
            else if (orderQuestion)
            {
                context.AppendLine(
                    "KHÁCH HÀNG: Chưa đăng nhập. Không có dữ liệu đơn hàng cá nhân.");

                context.AppendLine();
            }

            /*
             * Câu hỏi kiến thức chung không cần
             * truy vấn database.
             */
            if (
                !productQuestion &&
                !orderQuestion)
            {
                context.AppendLine(
                    "CÂU HỎI KHÔNG YÊU CẦU TRA CỨU DỮ LIỆU SẢN PHẨM/ĐƠN HÀNG.");

                context.AppendLine();
            }

            return context.ToString();
        }

        /// <summary>
        /// Xác định câu hỏi có liên quan sản phẩm hay không.
        /// </summary>
        private static bool IsProductQuestion(
            string question)
        {
            string q =
                Normalize(question);

            string[] keys =
            {
                "san pham",
                "mua",
                "gia",
                "bao nhieu",
                "con hang",
                "ton kho",

                "laptop",
                "notebook",

                "dien thoai",
                "smartphone",
                "iphone",
                "samsung",
                "xiaomi",
                "oppo",
                "realme",
                "vivo",

                "tai nghe",
                "ban phim",
                "chuot",
                "man hinh",
                "may tinh",
                "pc",
                "tablet",
                "may tinh bang",
                "phu kien",

                "shop",
                "goi y",
                "tu van",
                "phu hop",

                "duoi",
                "tren",
                "trieu",

                "choi game",
                "gaming",
                "lap trinh",
                "hoc tap",
                "sinh vien",

                "camera",
                "pin",
                "hieu nang",

                "so sanh"
            };

            foreach (string key in keys)
            {
                if (q.Contains(key))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Xác định câu hỏi có liên quan đơn hàng hay không.
        /// </summary>
        private static bool IsOrderQuestion(
            string question)
        {
            string q =
                Normalize(question);

            string[] keys =
            {
                "don hang",
                "ma don",
                "don #",
                "trang thai don",
                "don cua toi",
                "don cua minh",
                "don toi",
                "dat hang",
                "mua hang cua toi",
                "giao hang",
                "dang giao",
                "da giao",
                "huy don",
                "don moi"
            };

            foreach (string key in keys)
            {
                if (q.Contains(key))
                {
                    return true;
                }
            }

            return false;
        }

        
        private static string BuildSearchQuestion(
            string question,
            IList<ChatMessage> history)
        {
            if (
                history == null ||
                history.Count == 0)
            {
                return question;
            }

            string current =
                Normalize(question);

            bool followUp =
                current.Length < 45
                || current.Contains("do")
                || current.Contains("nay")
                || current.Contains("no")
                || current.Contains("cai do")
                || current.Contains("may do")
                || current.Contains("san pham do")
                || current.Contains("bao nhieu")
                || current.Contains("con hang")
                || current.Contains("gia bao nhieu");

            if (!followUp)
            {
                return question;
            }

            StringBuilder combined =
                new StringBuilder(
                    question);

            int added = 0;

            /*
             * Lấy tối đa 2 câu hỏi gần nhất
             * của khách hàng.
             */
            for (
                int i = history.Count - 1;
                i >= 0 && added < 2;
                i--)
            {
                ChatMessage message =
                    history[i];

                if (
                    message == null ||
                    !string.Equals(
                        message.Role,
                        "user",
                        StringComparison.OrdinalIgnoreCase) ||
                    string.IsNullOrWhiteSpace(
                        message.Content))
                {
                    continue;
                }

                combined.Append(" ");

                combined.Append(
                    message.Content);

                added++;
            }

            return combined.ToString();
        }

        private void AppendCategories(
            StringBuilder context)
        {
            const string sql = @"
                SELECT TOP 10
                    MaDanhMuc,
                    TenDanhMuc
                FROM DanhMuc
                WHERE TrangThai = 1
                ORDER BY TenDanhMuc";

            using (
                SqlConnection conn =
                    database.GetConnection())

            using (
                SqlCommand cmd =
                    new SqlCommand(
                        sql,
                        conn))
            {
                conn.Open();

                using (
                    SqlDataReader reader =
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
                            + CleanText(
                                Convert.ToString(
                                    reader["TenDanhMuc"]),
                                100));
                    }

                    context.AppendLine();
                }
            }
        }

        private void AppendRelevantProducts(
            StringBuilder context,
            string question)
        {
            PriceRange price =
                ExtractPriceRange(
                    question);

            List<string> terms =
                ExtractSearchTerms(
                    question);

            List<ProductRow> rows =
                QueryProducts(
                    terms,
                    price.Min,
                    price.Max);

            /*
             * Nếu không tìm được keyword chính xác,
             * tìm sản phẩm dự phòng trong khoảng giá.
             */
            if (rows.Count == 0)
            {
                rows =
                    QueryFallbackProducts(
                        price.Min,
                        price.Max);
            }

            context.AppendLine("SẢN PHẨM ĐANG BÁN VÀ CÒN HÀNG:");

            if (rows.Count == 0)
            {
                context.AppendLine(
                    "- Không tìm thấy sản phẩm đang bán/còn hàng trong khoảng giá yêu cầu.");
            }
            else
            {
                foreach (
                    ProductRow row in rows)
                {
                    context.AppendLine(
                        string.Format(
                            CultureInfo.InvariantCulture,

                            "- MaSP={0}; Tên={1}; Danh mục={2}; Giá={3:N0} VNĐ; Tồn kho={4}; Mô tả={5}; Hình ảnh={6}",

                            row.MaSanPham,

                            row.TenSanPham,

                            row.TenDanhMuc,

                            row.Gia,

                            row.SoLuong,

                            CleanText(
                                row.MoTa,
                                160),

                            row.HinhAnh));
                }
            }

            context.AppendLine();
        }

        /// <summary>
        /// Tìm sản phẩm bằng nhiều từ khóa.
        /// Sản phẩm khớp tên được ưu tiên cao nhất.
        /// </summary>
        private List<ProductRow> QueryProducts(
            List<string> terms,
            decimal? minPrice,
            decimal? maxPrice)
        {
            List<ProductRow> rows =
                new List<ProductRow>();

            if (
                terms == null ||
                terms.Count == 0)
            {
                return rows;
            }

            StringBuilder score =
                new StringBuilder();

            StringBuilder match =
                new StringBuilder();

            for (
                int i = 0;
                i < terms.Count;
                i++)
            {
                if (i > 0)
                {
                    match.AppendLine(
                        " OR");

                    match.Append(" ");
                }

                match.Append(
                    "sp.TenSanPham LIKE @LikeTerm"
                    + i);

                match.Append(
                    " OR ISNULL(sp.MoTa,'') LIKE @LikeTerm"
                    + i);

                match.Append(
                    " OR dm.TenDanhMuc LIKE @LikeTerm"
                    + i);

                score.Append(
                    "CASE WHEN sp.TenSanPham LIKE @LikeTerm"
                    + i
                    + " THEN 8 ELSE 0 END + ");

                score.Append(
                    "CASE WHEN dm.TenDanhMuc LIKE @LikeTerm"
                    + i
                    + " THEN 6 ELSE 0 END + ");

                score.Append(
                    "CASE WHEN ISNULL(sp.MoTa,'') LIKE @LikeTerm"
                    + i
                    + " THEN 3 ELSE 0 END");

                if (
                    i < terms.Count - 1)
                {
                    score.Append(
                        " + ");
                }
            }

            string sql =
                @"
                SELECT TOP 6
                    sp.MaSanPham,
                    sp.TenSanPham,
                    sp.MoTa,
                    sp.Gia,
                    sp.SoLuong,
                    sp.HinhAnh,
                    dm.TenDanhMuc,
                    ("
                + score +
                @") AS MatchScore

                FROM SanPham sp

                INNER JOIN DanhMuc dm
                    ON sp.MaDanhMuc =
                       dm.MaDanhMuc

                WHERE sp.TrangThai = 1

                  AND dm.TrangThai = 1

                  AND sp.SoLuong > 0

                  AND ("
                + match +
                @")

                  AND (
                    @MinPrice IS NULL
                    OR sp.Gia >= @MinPrice
                  )

                  AND (
                    @MaxPrice IS NULL
                    OR sp.Gia <= @MaxPrice
                  )

                ORDER BY
                    MatchScore DESC,

                    CASE
                        WHEN sp.NoiBat = 1
                        THEN 0
                        ELSE 1
                    END,

                    sp.MaSanPham DESC";

            using (
                SqlConnection conn =
                    database.GetConnection())

            using (
                SqlCommand cmd =
                    new SqlCommand(
                        sql,
                        conn))
            {
                AddPriceParameter(
                    cmd,
                    "@MinPrice",
                    minPrice);

                AddPriceParameter(
                    cmd,
                    "@MaxPrice",
                    maxPrice);

                for (
                    int i = 0;
                    i < terms.Count;
                    i++)
                {
                    cmd.Parameters.Add(
                        "@LikeTerm" + i,
                        SqlDbType.NVarChar,
                        120)
                        .Value =
                            "%"
                            + terms[i]
                            + "%";
                }

                conn.Open();

                using (
                    SqlDataReader reader =
                        cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rows.Add(
                            ReadProduct(
                                reader));
                    }
                }
            }

            return rows;
        }

        /// <summary>
        /// Fallback:
        /// nếu keyword không trùng database,
        /// lấy sản phẩm nổi bật/còn hàng trong khoảng giá.
        /// </summary>
        private List<ProductRow>
            QueryFallbackProducts(
                decimal? minPrice,
                decimal? maxPrice)
        {
            List<ProductRow> rows =
                new List<ProductRow>();

            const string sql = @"
                SELECT TOP 6
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

            using (
                SqlConnection conn =
                    database.GetConnection())

            using (
                SqlCommand cmd =
                    new SqlCommand(
                        sql,
                        conn))
            {
                AddPriceParameter(
                    cmd,
                    "@MinPrice",
                    minPrice);

                AddPriceParameter(
                    cmd,
                    "@MaxPrice",
                    maxPrice);

                conn.Open();

                using (
                    SqlDataReader reader =
                        cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rows.Add(
                            ReadProduct(
                                reader));
                    }
                }
            }

            return rows;
        }

        private static ProductRow ReadProduct(
            SqlDataReader reader)
        {
            return new ProductRow
            {
                MaSanPham =
                    Convert.ToInt32(
                        reader["MaSanPham"]),

                TenSanPham =
                    Convert.ToString(
                        reader["TenSanPham"]),

                TenDanhMuc =
                    Convert.ToString(
                        reader["TenDanhMuc"]),

                Gia =
                    Convert.ToDecimal(
                        reader["Gia"]),

                SoLuong =
                    Convert.ToInt32(
                        reader["SoLuong"]),

                MoTa =
                    Convert.ToString(
                        reader["MoTa"]),

                HinhAnh =
                    Convert.ToString(
                        reader["HinhAnh"])
            };
        }

        private void AppendCustomerOrders(
            StringBuilder context,
            int maKhachHang)
        {
            const string sql = @"
                SELECT TOP 5
                    MaDonHang,
                    TongTien,
                    TrangThai,
                    NgayDat

                FROM DonHang

                WHERE MaKhachHang =
                      @MaKhachHang

                ORDER BY
                    MaDonHang DESC";

            using (
                SqlConnection conn =
                    database.GetConnection())

            using (
                SqlCommand cmd =
                    new SqlCommand(
                        sql,
                        conn))
            {
                cmd.Parameters.Add(
                    "@MaKhachHang",
                    SqlDbType.Int)
                    .Value =
                        maKhachHang;

                conn.Open();

                using (
                    SqlDataReader reader =
                        cmd.ExecuteReader())
                {
                    context.AppendLine(
                        "5 ĐƠN HÀNG GẦN NHẤT CỦA KHÁCH ĐANG ĐĂNG NHẬP:");

                    bool hasRows =
                        false;

                    while (reader.Read())
                    {
                        hasRows =
                            true;

                        context.AppendLine(
                            string.Format(
                                CultureInfo.InvariantCulture,

                                "- Đơn #{0}; Tổng={1:N0} VNĐ; Trạng thái={2}; Ngày đặt={3:dd/MM/yyyy HH:mm}",

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

        /// <summary>
        /// Trích xuất các keyword có ý nghĩa.
        /// </summary>
        private static List<string>
            ExtractSearchTerms(
                string question)
        {
            List<string> terms =
                new List<string>();

            string value =
                question == null
                    ? string.Empty
                    : question
                        .Trim()
                        .ToLowerInvariant();

            string normalizedValue =
                Normalize(question);

            value =
                Regex.Replace(
                    value,
                    @"[^\p{L}\p{N}\s]",
                    " ");

            value =
                Regex.Replace(
                    value,
                    @"\s+",
                    " ")
                .Trim();

            /*
             * Cụm từ quan trọng.
             */
            string[] phrases =
            {
                "điện thoại",
                "máy tính bảng",
                "tai nghe",
                "bàn phím",
                "màn hình",
                "máy tính",
                "phụ kiện",
                "chơi game",
                "lập trình",
                "học tập",
                "sinh viên",
                "pin tốt",
                "camera đẹp",
                "chụp ảnh",
                "hiệu năng cao",
                "giá rẻ"
            };

            foreach (
                string phrase in phrases)
            {
                if (value.Contains(
                    phrase))
                {
                    AddTerm(
                        terms,
                        phrase);
                }
            }

            string[] words =
                value.Split(
                    new[]
                    {
                        ' ',
                        '\t',
                        '\r',
                        '\n'
                    },
                    StringSplitOptions
                        .RemoveEmptyEntries);

            string[] stopWords =
            {
                "toi",
                "minh",
                "mình",
                "ban",
                "bạn",

                "muon",
                "muốn",
                "can",
                "cần",

                "cho",
                "shop",
                "co",
                "có",
                "khong",
                "không",

                "xin",
                "hay",
                "hãy",

                "tu",
                "tư",
                "van",
                "vấn",

                "goi",
                "gợi",
                "y",
                "ý",

                "mot",
                "một",

                "cai",
                "cái",

                "nao",
                "nào",

                "voi",
                "với",

                "nhe",
                "nhé",

                "a",
                "ạ",

                "la",
                "là",

                "va",
                "và",

                "the",
                "thế",

                "bao",
                "nhieu",
                "nhiêu",

                "san",
                "pham",
                "sản",
                "phẩm",

                "gia",

                "duoi",
                "dưới",

                "tren",
                "trên",

                "tu",
                "từ",

                "den",
                "đến",

                "khoang",
                "khoảng",

                "tam",
                "tầm",

                "trieu",
                "triệu",

                "tr",
                "m",

                "dong",
                "đong",

                "vnd",
                "vnđ",

                "de",
                "để",

                "phu",
                "phù",

                "hop",
                "hợp"
            };

            foreach (
                string rawWord in words)
            {
                string word =
                    rawWord.Trim();

                if (
                    word.Length < 3 ||
                    IsStopWord(
                        word,
                        stopWords))
                {
                    continue;
                }

                if (
                    Regex.IsMatch(
                        word,
                        @"^\d+$"))
                {
                    continue;
                }

                AddTerm(
                    terms,
                    word);
            }

            /*
             * Từ đồng nghĩa.
             */
            AddSynonymIfContains(
                normalizedValue,
                terms,
                "dien thoai",
                "smartphone");

            AddSynonymIfContains(
                normalizedValue,
                terms,
                "laptop",
                "notebook");

            AddSynonymIfContains(
                normalizedValue,
                terms,
                "choi game",
                "gaming");

            AddSynonymIfContains(
                normalizedValue,
                terms,
                "chup anh",
                "camera");

            AddSynonymIfContains(
                normalizedValue,
                terms,
                "pin tot",
                "pin");

            AddSynonymIfContains(
                normalizedValue,
                terms,
                "hoc tap",
                "sinh vien");

            /*
             * Tối đa 8 keyword để SQL không quá dài.
             */
            if (terms.Count > 8)
            {
                terms =
                    terms.GetRange(
                        0,
                        8);
            }

            if (terms.Count > 6)
            {
                terms =
                    terms.GetRange(
                        0,
                        6);
            }

            return terms;
        }

        private static void AddSynonymIfContains(
            string value,
            List<string> terms,
            string phrase,
            string synonym)
        {
            if (
                value.Contains(
                    phrase))
            {
                AddTerm(
                    terms,
                    synonym);
            }
        }

        private static void AddTerm(
            List<string> terms,
            string value)
        {
            if (
                string.IsNullOrWhiteSpace(
                    value))
            {
                return;
            }

            value =
                Regex.Replace(
                    value
                        .Trim()
                        .ToLowerInvariant(),
                    @"\s+",
                    " ");

            if (value.Length < 2)
            {
                return;
            }

            foreach (
                string existing in terms)
            {
                if (
                    string.Equals(
                        existing,
                        value,
                        StringComparison
                            .OrdinalIgnoreCase))
                {
                    return;
                }
            }

            terms.Add(
                value);
        }

        private static bool IsStopWord(
            string word,
            string[] stopWords)
        {
            foreach (
                string stop in stopWords)
            {
                if (
                    string.Equals(
                        word,
                        Normalize(stop),
                        StringComparison
                            .OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static PriceRange
            ExtractPriceRange(
                string question)
        {
            PriceRange result =
                new PriceRange();

            if (
                string.IsNullOrWhiteSpace(
                    question))
            {
                return result;
            }

            string q =
                Normalize(question);

            Match range =
                Regex.Match(
                    q,

                    @"(?:tu\s*)?"
                    + @"(\d+(?:[\.,]\d+)?)"
                    + @"\s*(trieu|tr|m|k)?"
                    + @"\s*(?:den|-)\s*"
                    + @"(\d+(?:[\.,]\d+)?)"
                    + @"\s*(trieu|tr|m|k)?",

                    RegexOptions.IgnoreCase);

            if (range.Success)
            {
                string unit1 =
                    range.Groups[2].Value;

                string unit2 =
                    range.Groups[4].Value;

                if (
                    string.IsNullOrWhiteSpace(
                        unit1))
                {
                    unit1 =
                        unit2;
                }

                decimal min;
                decimal max;

                if (
                    TryParsePrice(
                        range.Groups[1].Value,
                        unit1,
                        out min)
                    &&
                    TryParsePrice(
                        range.Groups[3].Value,
                        unit2,
                        out max))
                {
                    result.Min =
                        Math.Min(
                            min,
                            max);

                    result.Max =
                        Math.Max(
                            min,
                            max);

                    return result;
                }
            }

            /*
             * khoảng 15 triệu
             * tầm 15 triệu
             */
            Match around =
                Regex.Match(
                    q,

                    @"(?:khoang|tam|tam gia)"
                    + @"\s*"
                    + @"(\d+(?:[\.,]\d+)?)"
                    + @"\s*(trieu|tr|m|k)?",

                    RegexOptions.IgnoreCase);

            if (around.Success)
            {
                decimal value;

                if (
                    TryParsePrice(
                        around.Groups[1].Value,
                        around.Groups[2].Value,
                        out value))
                {
                    /*
                     * Khoảng = ±20%.
                     */
                    result.Min =
                        value * 0.8m;

                    result.Max =
                        value * 1.2m;

                    return result;
                }
            }

            /*
             * dưới 20 triệu
             */
            Match maxMatch =
                Regex.Match(
                    q,

                    @"(?:duoi|khong qua|toi da|thap hon|<=)"
                    + @"\s*"
                    + @"(\d+(?:[\.,]\d+)?)"
                    + @"\s*(trieu|tr|m|k)?",

                    RegexOptions.IgnoreCase);

            if (maxMatch.Success)
            {
                decimal value;

                if (
                    TryParsePrice(
                        maxMatch.Groups[1].Value,
                        maxMatch.Groups[2].Value,
                        out value))
                {
                    result.Max =
                        value;
                }
            }

            /*
             * trên 10 triệu
             */
            Match minMatch =
                Regex.Match(
                    q,

                    @"(?:tren|tu|it nhat|cao hon|>=)"
                    + @"\s*"
                    + @"(\d+(?:[\.,]\d+)?)"
                    + @"\s*(trieu|tr|m|k)?",

                    RegexOptions.IgnoreCase);

            if (minMatch.Success)
            {
                decimal value;

                if (
                    TryParsePrice(
                        minMatch.Groups[1].Value,
                        minMatch.Groups[2].Value,
                        out value))
                {
                    result.Min =
                        value;
                }
            }

            return result;
        }

        private static bool TryParsePrice(
            string number,
            string unit,
            out decimal value)
        {
            value = 0;

            if (
                string.IsNullOrWhiteSpace(
                    number))
            {
                return false;
            }

            string raw =
                number.Trim();

            string normalizedUnit =
                Normalize(unit);

            /*
             * 15.5 triệu
             * 15,5 triệu
             */
            if (
                normalizedUnit == "trieu" ||
                normalizedUnit == "tr" ||
                normalizedUnit == "m")
            {
                raw =
                    raw.Replace(
                        ',',
                        '.');

                decimal millions;

                if (
                    !decimal.TryParse(
                        raw,
                        NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture,
                        out millions))
                {
                    return false;
                }

                value =
                    millions * 1000000m;

                return true;
            }

            /*
             * 500k
             */
            if (
                normalizedUnit == "k")
            {
                raw =
                    raw.Replace(
                        ',',
                        '.');

                decimal thousands;

                if (
                    !decimal.TryParse(
                        raw,
                        NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture,
                        out thousands))
                {
                    return false;
                }

                value =
                    thousands * 1000m;

                return true;
            }

            /*
             * Không có đơn vị:
             *
             * 15000000
             * 15.000.000
             */
            raw =
                raw
                    .Replace(
                        ".",
                        "")
                    .Replace(
                        ",",
                        "");

            decimal plain;

            if (
                !decimal.TryParse(
                    raw,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out plain))
            {
                return false;
            }

            /*
             * Nếu người dùng nhập:
             * 15 -> hiểu là 15 triệu.
             */
            if (plain < 100000m)
            {
                plain *=
                    1000000m;
            }

            value =
                plain;

            return true;
        }

        private static void AddPriceParameter(
            SqlCommand cmd,
            string name,
            decimal? value)
        {
            SqlParameter parameter =
                cmd.Parameters.Add(
                    name,
                    SqlDbType.Decimal);

            parameter.Precision =
                18;

            parameter.Scale =
                2;

            parameter.Value =
                value.HasValue
                    ? (object)value.Value
                    : DBNull.Value;
        }

        /// <summary>
        /// Chuẩn hóa tiếng Việt để phân loại câu hỏi.
        /// </summary>
        private static string Normalize(
    string value)
        {
            if (
                string.IsNullOrWhiteSpace(
                    value))
            {
                return string.Empty;
            }

            value =
                value
                    .Trim()
                    .ToLowerInvariant();

            string normalized =
                value.Normalize(
                    NormalizationForm.FormD);

            StringBuilder result =
                new StringBuilder();

            foreach (
                char c in normalized)
            {
                System.Globalization.UnicodeCategory category =
                    CharUnicodeInfo.GetUnicodeCategory(c);

                if (
                    category !=
                    System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    result.Append(c);
                }
            }

            return result
                .ToString()
                .Normalize(
                    NormalizationForm.FormC)
                .Replace(
                    'đ',
                    'd')
                .Replace(
                    'Đ',
                    'd');
        }

        private static string CleanText(
            string value,
            int maxLength)
        {
            if (
                string.IsNullOrWhiteSpace(
                    value))
            {
                return "Không có mô tả";
            }

            value =
                Regex.Replace(
                    value,
                    @"\s+",
                    " ")
                .Trim();

            if (
                value.Length > maxLength)
            {
                value =
                    value.Substring(
                        0,
                        maxLength)
                    + "...";
            }

            return value;
        }

        private class PriceRange
        {
            public decimal? Min
            {
                get;
                set;
            }

            public decimal? Max
            {
                get;
                set;
            }
        }

        private class ProductRow
        {
            public int MaSanPham
            {
                get;
                set;
            }

            public string TenSanPham
            {
                get;
                set;
            }

            public string TenDanhMuc
            {
                get;
                set;
            }

            public decimal Gia
            {
                get;
                set;
            }

            public int SoLuong
            {
                get;
                set;
            }

            public string MoTa
            {
                get;
                set;
            }

            public string HinhAnh
            {
                get;
                set;
            }
        }
    }
}