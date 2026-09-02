using System;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using web_ban_hang2.DAL;

namespace web_ban_hang2
{
    public partial class shop : System.Web.UI.Page
    {
        // =========================================================
        // SỐ SẢN PHẨM / TRANG
        // =========================================================

        private const int PageSize = 12;


        // =========================================================
        // TỪ KHÓA TÌM KIẾM
        //
        // shop.aspx?search=iphone
        // =========================================================

        private string SearchKeyword
        {
            get
            {
                return
                    (Request.QueryString["search"]
                    ?? string.Empty)
                    .Trim();
            }
        }


        // =========================================================
        // DANH MỤC
        //
        // 0 = TẤT CẢ
        // =========================================================

        private int CurrentCategory
        {
            get
            {
                int category;

                if (!int.TryParse(
                    Request.QueryString["category"],
                    out category)
                    || category < 1)
                {
                    return 0;
                }

                return category;
            }
        }


        // =========================================================
        // GIÁ TỐI THIỂU
        // =========================================================

        private decimal? MinPrice
        {
            get
            {
                decimal value;

                if (decimal.TryParse(
                    Request.QueryString["minPrice"],
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out value)
                    && value >= 0)
                {
                    return value;
                }

                return null;
            }
        }


        // =========================================================
        // GIÁ TỐI ĐA
        // =========================================================

        private decimal? MaxPrice
        {
            get
            {
                decimal value;

                if (decimal.TryParse(
                    Request.QueryString["maxPrice"],
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out value)
                    && value >= 0)
                {
                    return value;
                }

                return null;
            }
        }


        // =========================================================
        // TRẠNG THÁI KHO
        //
        // 0 = TẤT CẢ
        // 1 = CÒN HÀNG
        // 2 = HẾT HÀNG
        // =========================================================

        private int CurrentStatus
        {
            get
            {
                int status;

                if (!int.TryParse(
                    Request.QueryString["status"],
                    out status)
                    || status < 0
                    || status > 2)
                {
                    return 0;
                }

                return status;
            }
        }


        // =========================================================
        // SẮP XẾP
        //
        // newest
        // price_asc
        // price_desc
        // bestseller
        // =========================================================

        private string CurrentSort
        {
            get
            {
                string sort =
                    (
                        Request.QueryString["sort"]
                        ?? "newest"
                    )
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
                    return "newest";
                }


                return sort;
            }
        }


        // =========================================================
        // TRANG HIỆN TẠI
        // =========================================================

        private int CurrentPage
        {
            get
            {
                int page;

                if (
                    !int.TryParse(
                        Request.QueryString["page"],
                        out page)
                    ||
                    page < 1
                )
                {
                    return 1;
                }

                return page;
            }
        }


        // =========================================================
        // PAGE LOAD
        // =========================================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFilterControls();
                LoadProducts();
            }
        }


        // =========================================================
        // LOAD SẢN PHẨM
        // =========================================================

        private void LoadProducts()
        {
            try
            {
                SanPhamDAL sanPhamDAL =
                    new SanPhamDAL();


                int totalRecords;


                DataTable danhSach =
                    sanPhamDAL.SearchPaged(
                        SearchKeyword,
                        CurrentCategory,
                        MinPrice,
                        MaxPrice,
                        CurrentStatus,
                        CurrentSort,
                        CurrentPage,
                        PageSize,
                        out totalRecords
                    );


                // =================================================
                // BIND SẢN PHẨM
                // =================================================

                rptProducts.DataSource =
                    danhSach;

                rptProducts.DataBind();


                // =================================================
                // KHÔNG CÓ SẢN PHẨM
                // =================================================

                pnlNoProduct.Visible =
                    danhSach.Rows.Count == 0;


                // =================================================
                // TỔNG SỐ TRANG
                // =================================================

                int totalPages =
                    (int)Math.Ceiling(
                        (double)totalRecords
                        / PageSize
                    );


                // =================================================
                // NẾU PAGE VƯỢT QUÁ
                // =================================================

                if (
                    totalPages > 0
                    &&
                    CurrentPage > totalPages
                )
                {
                    Response.Redirect(
                        BuildPageUrl(totalPages),
                        false
                    );

                    Context
                        .ApplicationInstance
                        .CompleteRequest();

                    return;
                }


                // =================================================
                // HIỂN THỊ PHÂN TRANG
                // =================================================

                pnlPager.Visible =
                    totalPages > 1;


                btnPrevious.Enabled =
                    CurrentPage > 1;


                btnNext.Enabled =
                    CurrentPage < totalPages;


                BindPager(totalPages);


                // =================================================
                // THÔNG BÁO KẾT QUẢ
                // =================================================

                string keyword =
                    Server.HtmlEncode(
                        SearchKeyword
                    );


                if (
                    string.IsNullOrWhiteSpace(
                        SearchKeyword)
                    &&
                    CurrentCategory == 0
                )
                {
                    lblSearchResult.Text =
                        totalRecords > 0
                        ?
                        string.Format(
                            "Có {0} sản phẩm đang bán",
                            totalRecords
                        )
                        :
                        "Chưa có sản phẩm đang bán";
                }
                else if (
                    !string.IsNullOrWhiteSpace(
                        SearchKeyword)
                    &&
                    CurrentCategory == 0
                )
                {
                    lblSearchResult.Text =
                        totalRecords > 0
                        ?
                        string.Format(
                            "Tìm thấy {0} sản phẩm cho “{1}”",
                            totalRecords,
                            keyword
                        )
                        :
                        string.Format(
                            "Không tìm thấy sản phẩm cho “{0}”",
                            keyword
                        );
                }
                else if (
                    string.IsNullOrWhiteSpace(
                        SearchKeyword)
                    &&
                    CurrentCategory > 0
                )
                {
                    lblSearchResult.Text =
                        totalRecords > 0
                        ?
                        string.Format(
                            "Có {0} sản phẩm trong danh mục",
                            totalRecords
                        )
                        :
                        "Danh mục này chưa có sản phẩm";
                }
                else
                {
                    lblSearchResult.Text =
                        totalRecords > 0
                        ?
                        string.Format(
                            "Tìm thấy {0} sản phẩm phù hợp",
                            totalRecords
                        )
                        :
                        "Không tìm thấy sản phẩm phù hợp";
                }
            }
            catch (Exception)
            {
                rptProducts.DataSource = null;
                rptProducts.DataBind();

                pnlNoProduct.Visible = true;
                pnlPager.Visible = false;

                lblSearchResult.Text =
                    "Không thể tải danh sách sản phẩm.";
            }
        }


        // =========================================================
        // LOAD DANH MỤC + BỘ LỌC
        // =========================================================

        private void LoadFilterControls()
        {
            try
            {
                DanhMucDAL danhMucDAL =
                    new DanhMucDAL();


                ddlCategory.DataSource =
                    danhMucDAL.GetActive();


                ddlCategory.DataTextField =
                    "TenDanhMuc";


                ddlCategory.DataValueField =
                    "MaDanhMuc";


                ddlCategory.DataBind();


                ddlCategory.Items.Insert(
                    0,
                    new ListItem(
                        "Tất cả danh mục",
                        "0"
                    )
                );
            }
            catch (Exception)
            {
                ddlCategory.Items.Clear();

                ddlCategory.Items.Add(
                    new ListItem(
                        "Tất cả danh mục",
                        "0"
                    )
                );
            }


            // =====================================================
            // CHỌN DANH MỤC HIỆN TẠI
            // =====================================================

            if (
                ddlCategory.Items.FindByValue(
                    CurrentCategory.ToString()
                ) != null
            )
            {
                ddlCategory.SelectedValue =
                    CurrentCategory.ToString();
            }


            // =====================================================
            // GIÁ
            // =====================================================

            txtMinPrice.Text =
                MinPrice.HasValue
                ?
                MinPrice.Value.ToString(
                    "0.##",
                    CultureInfo.InvariantCulture
                )
                :
                string.Empty;


            txtMaxPrice.Text =
                MaxPrice.HasValue
                ?
                MaxPrice.Value.ToString(
                    "0.##",
                    CultureInfo.InvariantCulture
                )
                :
                string.Empty;


            // =====================================================
            // TRẠNG THÁI
            // =====================================================

            if (
                ddlStatus.Items.FindByValue(
                    CurrentStatus.ToString()
                ) != null
            )
            {
                ddlStatus.SelectedValue =
                    CurrentStatus.ToString();
            }


            // =====================================================
            // SẮP XẾP
            // =====================================================

            if (
                ddlSort.Items.FindByValue(
                    CurrentSort
                ) != null
            )
            {
                ddlSort.SelectedValue =
                    CurrentSort;
            }
        }


        // =========================================================
        // CLICK LỌC
        // =========================================================

        protected void btnFilter_Click(
            object sender,
            EventArgs e)
        {
            decimal minPrice = 0;
            decimal maxPrice = 0;


            bool minValid =
                string.IsNullOrWhiteSpace(
                    txtMinPrice.Text
                )
                ||
                decimal.TryParse(
                    txtMinPrice.Text.Trim(),
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out minPrice
                );


            bool maxValid =
                string.IsNullOrWhiteSpace(
                    txtMaxPrice.Text
                )
                ||
                decimal.TryParse(
                    txtMaxPrice.Text.Trim(),
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out maxPrice
                );


            if (!minValid || !maxValid)
            {
                lblSearchResult.Text =
                    "Khoảng giá không hợp lệ.";

                return;
            }


            if (
                !string.IsNullOrWhiteSpace(
                    txtMinPrice.Text)
                &&
                !string.IsNullOrWhiteSpace(
                    txtMaxPrice.Text)
                &&
                minPrice > maxPrice
            )
            {
                lblSearchResult.Text =
                    "Giá từ không được lớn hơn giá đến.";

                return;
            }


            Response.Redirect(
                BuildFilterUrl(),
                false
            );

            Context
                .ApplicationInstance
                .CompleteRequest();
        }


        // =========================================================
        // XÓA BỘ LỌC
        //
        // GIỮ LẠI SEARCH
        // =========================================================

        protected void btnClearFilter_Click(
            object sender,
            EventArgs e)
        {
            string url =
                "shop.aspx";


            if (
                !string.IsNullOrWhiteSpace(
                    SearchKeyword)
            )
            {
                url +=
                    "?search="
                    +
                    Server.UrlEncode(
                        SearchKeyword
                    );
            }


            Response.Redirect(
                url,
                false
            );

            Context
                .ApplicationInstance
                .CompleteRequest();
        }


        // =========================================================
        // TẠO URL FILTER
        // =========================================================

        private string BuildFilterUrl()
        {
            string url =
                "shop.aspx?page=1";


            // SEARCH

            if (
                !string.IsNullOrWhiteSpace(
                    SearchKeyword)
            )
            {
                url +=
                    "&search="
                    +
                    Server.UrlEncode(
                        SearchKeyword
                    );
            }


            // CATEGORY

            int category;

            if (
                int.TryParse(
                    ddlCategory.SelectedValue,
                    out category)
                &&
                category > 0
            )
            {
                url +=
                    "&category="
                    +
                    category;
            }


            // MIN PRICE

            decimal minPrice;

            if (
                decimal.TryParse(
                    txtMinPrice.Text.Trim(),
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out minPrice)
                &&
                minPrice >= 0
            )
            {
                url +=
                    "&minPrice="
                    +
                    Server.UrlEncode(
                        minPrice.ToString(
                            "0.##",
                            CultureInfo.InvariantCulture
                        )
                    );
            }


            // MAX PRICE

            decimal maxPrice;

            if (
                decimal.TryParse(
                    txtMaxPrice.Text.Trim(),
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out maxPrice)
                &&
                maxPrice >= 0
            )
            {
                url +=
                    "&maxPrice="
                    +
                    Server.UrlEncode(
                        maxPrice.ToString(
                            "0.##",
                            CultureInfo.InvariantCulture
                        )
                    );
            }


            // STATUS

            int status;

            if (
                int.TryParse(
                    ddlStatus.SelectedValue,
                    out status)
                &&
                status > 0
            )
            {
                url +=
                    "&status="
                    +
                    status;
            }


            // SORT

            string sort =
                ddlSort.SelectedValue;


            if (
                sort != "newest"
            )
            {
                url +=
                    "&sort="
                    +
                    Server.UrlEncode(
                        sort
                    );
            }


            return url;
        }


        // =========================================================
        // TẠO URL PHÂN TRANG
        //
        // GIỮ TOÀN BỘ BỘ LỌC
        // =========================================================

        private string BuildPageUrl(
            int page)
        {
            string url =
                "shop.aspx?page="
                +
                page;


            if (
                !string.IsNullOrWhiteSpace(
                    SearchKeyword)
            )
            {
                url +=
                    "&search="
                    +
                    Server.UrlEncode(
                        SearchKeyword
                    );
            }


            if (
                CurrentCategory > 0
            )
            {
                url +=
                    "&category="
                    +
                    CurrentCategory;
            }


            if (
                MinPrice.HasValue
            )
            {
                url +=
                    "&minPrice="
                    +
                    Server.UrlEncode(
                        MinPrice.Value.ToString(
                            "0.##",
                            CultureInfo.InvariantCulture
                        )
                    );
            }


            if (
                MaxPrice.HasValue
            )
            {
                url +=
                    "&maxPrice="
                    +
                    Server.UrlEncode(
                        MaxPrice.Value.ToString(
                            "0.##",
                            CultureInfo.InvariantCulture
                        )
                    );
            }


            if (
                CurrentStatus > 0
            )
            {
                url +=
                    "&status="
                    +
                    CurrentStatus;
            }


            if (
                CurrentSort != "newest"
            )
            {
                url +=
                    "&sort="
                    +
                    Server.UrlEncode(
                        CurrentSort
                    );
            }


            return url;
        }


        // =========================================================
        // TẠO PHÂN TRANG
        // =========================================================

        private void BindPager(
            int totalPages)
        {
            List<PageItem> pages =
                new List<PageItem>();


            if (
                totalPages <= 0
            )
            {
                rptPager.DataSource = null;
                rptPager.DataBind();

                return;
            }


            int startPage =
                Math.Max(
                    1,
                    CurrentPage - 2
                );


            int endPage =
                Math.Min(
                    totalPages,
                    CurrentPage + 2
                );


            if (
                endPage - startPage < 4
            )
            {
                if (
                    startPage == 1
                )
                {
                    endPage =
                        Math.Min(
                            totalPages,
                            5
                        );
                }
                else
                {
                    startPage =
                        Math.Max(
                            1,
                            endPage - 4
                        );
                }
            }


            for (
                int i = startPage;
                i <= endPage;
                i++
            )
            {
                pages.Add(
                    new PageItem
                    {
                        Page = i,
                        IsCurrent =
                            i == CurrentPage
                    }
                );
            }


            rptPager.DataSource =
                pages;

            rptPager.DataBind();
        }


        // =========================================================
        // TRANG TRƯỚC
        // =========================================================

        protected void btnPrevious_Click(
            object sender,
            EventArgs e)
        {
            if (
                CurrentPage > 1
            )
            {
                Response.Redirect(
                    BuildPageUrl(
                        CurrentPage - 1
                    ),
                    false
                );

                Context
                    .ApplicationInstance
                    .CompleteRequest();
            }
        }


        // =========================================================
        // TRANG SAU
        // =========================================================

        protected void btnNext_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                int totalRecords;


                new SanPhamDAL()
                    .SearchPaged(
                        SearchKeyword,
                        CurrentCategory,
                        MinPrice,
                        MaxPrice,
                        CurrentStatus,
                        CurrentSort,
                        1,
                        PageSize,
                        out totalRecords
                    );


                int totalPages =
                    (int)Math.Ceiling(
                        (double)totalRecords
                        / PageSize
                    );


                if (
                    CurrentPage < totalPages
                )
                {
                    Response.Redirect(
                        BuildPageUrl(
                            CurrentPage + 1
                        ),
                        false
                    );

                    Context
                        .ApplicationInstance
                        .CompleteRequest();
                }
            }
            catch (Exception)
            {
                // Giữ nguyên trang hiện tại
            }
        }


        // =========================================================
        // CHỌN TRANG
        // =========================================================

        protected void rptPager_ItemCommand(
            object source,
            RepeaterCommandEventArgs e)
        {
            if (
                e.CommandName !=
                "PageNumber"
            )
            {
                return;
            }


            int page;


            if (
                int.TryParse(
                    e.CommandArgument.ToString(),
                    out page)
                &&
                page > 0
            )
            {
                Response.Redirect(
                    BuildPageUrl(page),
                    false
                );

                Context
                    .ApplicationInstance
                    .CompleteRequest();
            }
        }


        // =========================================================
        // CLASS PAGE ITEM
        // =========================================================

        private class PageItem
        {
            public int Page
            {
                get;
                set;
            }


            public bool IsCurrent
            {
                get;
                set;
            }
        }
    }
}