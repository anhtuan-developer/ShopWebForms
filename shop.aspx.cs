using System;
using System.Data;
using System.Collections.Generic;
using web_ban_hang2.DAL;

namespace web_ban_hang2
{
    public partial class shop :
        System.Web.UI.Page
    {
        // ==========================================
        // SỐ SẢN PHẨM TRÊN MỖI TRANG
        // ==========================================

        private const int PageSize = 12;


        // ==========================================
        // TỪ KHÓA TÌM KIẾM
        // URL:
        // shop.aspx?search=iphone
        // ==========================================

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


        // ==========================================
        // DANH MỤC HIỆN TẠI
        //
        // 0 = TẤT CẢ DANH MỤC
        //
        // URL:
        // shop.aspx?category=2
        // ==========================================

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


        // ==========================================
        // TRANG HIỆN TẠI
        //
        // URL:
        // shop.aspx?page=2
        // ==========================================

        private int CurrentPage
        {
            get
            {
                int page;

                if (!int.TryParse(
                    Request.QueryString["page"],
                    out page)
                    || page < 1)
                {
                    return 1;
                }

                return page;
            }
        }


        // ==========================================
        // PAGE LOAD
        // ==========================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }


        // ==========================================
        // LOAD SẢN PHẨM
        //
        // Bao gồm:
        // - Tìm kiếm
        // - Lọc danh mục
        // - Phân trang
        // ==========================================

        private void LoadProducts()
        {
            try
            {
                SanPhamDAL sanPhamDAL =
                    new SanPhamDAL();


                int totalRecords;


                // ======================================
                // LẤY DANH SÁCH SẢN PHẨM
                // ======================================

                DataTable danhSach =
                    sanPhamDAL.SearchPaged(
                        SearchKeyword,
                        CurrentCategory,
                        CurrentPage,
                        PageSize,
                        out totalRecords);


                // ======================================
                // BIND SẢN PHẨM
                // ======================================

                rptProducts.DataSource =
                    danhSach;

                rptProducts.DataBind();


                // ======================================
                // KHÔNG CÓ SẢN PHẨM
                // ======================================

                pnlNoProduct.Visible =
                    danhSach.Rows.Count == 0;


                // ======================================
                // TÍNH TỔNG SỐ TRANG
                // ======================================

                int totalPages =
                    (int)Math.Ceiling(
                        (double)totalRecords
                        / PageSize);


                // ======================================
                // NẾU TRANG VƯỢT QUÁ
                // TỔNG SỐ TRANG
                // ======================================

                if (totalPages > 0
                    && CurrentPage > totalPages)
                {
                    Response.Redirect(
                        BuildPageUrl(totalPages),
                        false);

                    Context.ApplicationInstance
                        .CompleteRequest();

                    return;
                }


                // ======================================
                // HIỂN THỊ PHÂN TRANG
                // ======================================

                pnlPager.Visible =
                    totalPages > 1;


                // ======================================
                // NÚT TRANG TRƯỚC
                // ======================================

                btnPrevious.Enabled =
                    CurrentPage > 1;


                // ======================================
                // NÚT TRANG SAU
                // ======================================

                btnNext.Enabled =
                    CurrentPage < totalPages;


                // ======================================
                // TẠO DANH SÁCH SỐ TRANG
                // ======================================

                BindPager(totalPages);


                // ======================================
                // HIỂN THỊ THÔNG BÁO
                // ======================================

                if (
                    string.IsNullOrWhiteSpace(
                        SearchKeyword)
                    &&
                    CurrentCategory == 0)
                {
                    // ----------------------------------
                    // TẤT CẢ SẢN PHẨM
                    // ----------------------------------

                    lblSearchResult.Text =
                        totalRecords > 0
                        ? string.Format(
                            "Có {0} sản phẩm đang bán",
                            totalRecords)
                        : "Chưa có sản phẩm đang bán";
                }
                else if (
                    !string.IsNullOrWhiteSpace(
                        SearchKeyword)
                    &&
                    CurrentCategory == 0)
                {
                    // ----------------------------------
                    // CHỈ TÌM KIẾM
                    // ----------------------------------

                    lblSearchResult.Text =
                        totalRecords > 0
                        ? string.Format(
                            "Tìm thấy {0} sản phẩm cho “{1}”",
                            totalRecords,
                            Server.HtmlEncode(
                                SearchKeyword))
                        : string.Format(
                            "Không tìm thấy sản phẩm cho “{0}”",
                            Server.HtmlEncode(
                                SearchKeyword));
                }
                else if (
                    string.IsNullOrWhiteSpace(
                        SearchKeyword)
                    &&
                    CurrentCategory > 0)
                {
                    // ----------------------------------
                    // CHỈ LỌC DANH MỤC
                    // ----------------------------------

                    lblSearchResult.Text =
                        totalRecords > 0
                        ? string.Format(
                            "Có {0} sản phẩm trong danh mục",
                            totalRecords)
                        : "Danh mục này chưa có sản phẩm";
                }
                else
                {
                    // ----------------------------------
                    // TÌM KIẾM + DANH MỤC
                    // ----------------------------------

                    lblSearchResult.Text =
                        totalRecords > 0
                        ? string.Format(
                            "Tìm thấy {0} sản phẩm",
                            totalRecords)
                        : "Không tìm thấy sản phẩm phù hợp";
                }
            }
            catch (Exception)
            {
                // ======================================
                // XỬ LÝ LỖI
                // ======================================

                rptProducts.DataSource = null;

                rptProducts.DataBind();


                pnlNoProduct.Visible = true;

                pnlPager.Visible = false;


                lblSearchResult.Text =
                    "Không thể tải danh sách sản phẩm. "
                    + "Vui lòng thử lại sau.";
            }
        }


        // ==========================================
        // TẠO DANH SÁCH PHÂN TRANG
        // ==========================================

        private void BindPager(
            int totalPages)
        {
            List<PageItem> pages =
                new List<PageItem>();


            // ======================================
            // KHÔNG CÓ TRANG
            // ======================================

            if (totalPages <= 0)
            {
                rptPager.DataSource = null;

                rptPager.DataBind();

                return;
            }


            // ======================================
            // XÁC ĐỊNH TRANG BẮT ĐẦU
            // ======================================

            int start =
                Math.Max(
                    1,
                    CurrentPage - 2);


            // ======================================
            // XÁC ĐỊNH TRANG KẾT THÚC
            // ======================================

            int end =
                Math.Min(
                    totalPages,
                    CurrentPage + 2);


            // ======================================
            // THÊM TRANG 1
            // ======================================

            if (start > 1)
            {
                pages.Add(
                    new PageItem
                    {
                        Page = 1,

                        IsCurrent =
                            CurrentPage == 1
                    });


                // ----------------------------------
                // DẤU ...
                // ----------------------------------

                if (start > 2)
                {
                    pages.Add(
                        new PageItem
                        {
                            Page = -1,

                            IsCurrent = false
                        });
                }
            }


            // ======================================
            // THÊM CÁC TRANG Ở GIỮA
            // ======================================

            for (
                int i = start;
                i <= end;
                i++)
            {
                pages.Add(
                    new PageItem
                    {
                        Page = i,

                        IsCurrent =
                            i == CurrentPage
                    });
            }


            // ======================================
            // THÊM TRANG CUỐI
            // ======================================

            if (end < totalPages)
            {
                // ----------------------------------
                // DẤU ...
                // ----------------------------------

                if (end < totalPages - 1)
                {
                    pages.Add(
                        new PageItem
                        {
                            Page = -1,

                            IsCurrent = false
                        });
                }


                // ----------------------------------
                // TRANG CUỐI
                // ----------------------------------

                pages.Add(
                    new PageItem
                    {
                        Page = totalPages,

                        IsCurrent =
                            CurrentPage == totalPages
                    });
            }


            // ======================================
            // LỌC CHỈ LẤY TRANG HỢP LỆ
            // ======================================

            List<PageItem> validPages =
                new List<PageItem>();


            foreach (
                PageItem item in pages)
            {
                if (item.Page > 0)
                {
                    validPages.Add(item);
                }
            }


            // ======================================
            // BIND REPEATER PHÂN TRANG
            // ======================================

            rptPager.DataSource =
                validPages;

            rptPager.DataBind();
        }


        // ==========================================
        // TẠO URL PHÂN TRANG
        //
        // Giữ lại:
        // - search
        // - category
        //
        // Ví dụ:
        // shop.aspx?page=2
        //
        // shop.aspx?page=2&category=3
        //
        // shop.aspx?page=2&search=iphone&category=3
        // ==========================================

        private string BuildPageUrl(
            int page)
        {
            string url =
                "shop.aspx?page="
                + page;


            // ======================================
            // GIỮ TỪ KHÓA TÌM KIẾM
            // ======================================

            if (
                !string.IsNullOrWhiteSpace(
                    SearchKeyword))
            {
                url +=
                    "&search="
                    + Server.UrlEncode(
                        SearchKeyword);
            }


            // ======================================
            // GIỮ DANH MỤC
            // ======================================

            if (CurrentCategory > 0)
            {
                url +=
                    "&category="
                    + CurrentCategory;
            }


            return url;
        }


        // ==========================================
        // TRANG TRƯỚC
        // ==========================================

        protected void btnPrevious_Click(
            object sender,
            EventArgs e)
        {
            if (CurrentPage > 1)
            {
                Response.Redirect(
                    BuildPageUrl(
                        CurrentPage - 1),
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();
            }
        }


        // ==========================================
        // TRANG SAU
        // ==========================================

        protected void btnNext_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                int totalRecords;


                // ==================================
                // QUAN TRỌNG:
                // PHẢI TRUYỀN CurrentCategory
                // ==================================

                new SanPhamDAL()
                    .SearchPaged(
                        SearchKeyword,
                        CurrentCategory,
                        1,
                        PageSize,
                        out totalRecords);


                int totalPages =
                    (int)Math.Ceiling(
                        (double)totalRecords
                        / PageSize);


                if (
                    CurrentPage < totalPages)
                {
                    Response.Redirect(
                        BuildPageUrl(
                            CurrentPage + 1),
                        false);

                    Context.ApplicationInstance
                        .CompleteRequest();
                }
            }
            catch (Exception)
            {
                // Không làm gì nếu xảy ra lỗi.
                // Người dùng vẫn ở trang hiện tại.
            }
        }


        // ==========================================
        // CHỌN TRANG
        // ==========================================

        protected void rptPager_ItemCommand(
            object source,
            System.Web.UI.WebControls
                .RepeaterCommandEventArgs e)
        {
            // ======================================
            // CHỈ XỬ LÝ PageNumber
            // ======================================

            if (
                e.CommandName !=
                "PageNumber")
            {
                return;
            }


            int page;


            // ======================================
            // KIỂM TRA SỐ TRANG
            // ======================================

            if (
                int.TryParse(
                    e.CommandArgument.ToString(),
                    out page)
                &&
                page > 0)
            {
                Response.Redirect(
                    BuildPageUrl(page),
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();
            }
        }


        // ==========================================
        // CLASS PAGE ITEM
        // ==========================================

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
