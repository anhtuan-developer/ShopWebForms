using System;
using System.Data;
using System.Collections.Generic;
using web_ban_hang2.DAL;

namespace web_ban_hang2
{
    public partial class shop :
        System.Web.UI.Page
    {
        private const int PageSize = 12;


        // ==========================================
        // TỪ KHÓA TÌM KIẾM
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
        // TRANG HIỆN TẠI
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
        // ==========================================

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
                        CurrentPage,
                        PageSize,
                        out totalRecords);


                rptProducts.DataSource =
                    danhSach;

                rptProducts.DataBind();


                pnlNoProduct.Visible =
                    danhSach.Rows.Count == 0;


                int totalPages =
                    (int)Math.Ceiling(
                        (double)totalRecords
                        / PageSize);


                // ======================================
                // NẾU TRANG VƯỢT QUÁ TỔNG SỐ TRANG
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
                // PHÂN TRANG
                // ======================================

                pnlPager.Visible =
                    totalPages > 1;

                btnPrevious.Enabled =
                    CurrentPage > 1;

                btnNext.Enabled =
                    CurrentPage < totalPages;


                BindPager(totalPages);


                // ======================================
                // HIỂN THỊ KẾT QUẢ
                // ======================================

                if (string.IsNullOrWhiteSpace(
                    SearchKeyword))
                {
                    lblSearchResult.Text =
                        totalRecords > 0
                        ? string.Format(
                            "Có {0} sản phẩm đang bán",
                            totalRecords)
                        : "Chưa có sản phẩm đang bán";
                }
                else
                {
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
            }
            catch (Exception)
            {
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


            int start =
                Math.Max(
                    1,
                    CurrentPage - 2);


            int end =
                Math.Min(
                    totalPages,
                    CurrentPage + 2);


            if (start > 1)
            {
                pages.Add(
                    new PageItem
                    {
                        Page = 1,
                        IsCurrent =
                            CurrentPage == 1
                    });


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


            if (end < totalPages)
            {
                if (end < totalPages - 1)
                {
                    pages.Add(
                        new PageItem
                        {
                            Page = -1,
                            IsCurrent = false
                        });
                }


                pages.Add(
                    new PageItem
                    {
                        Page = totalPages,
                        IsCurrent =
                            CurrentPage == totalPages
                    });
            }


            // Chỉ lấy số trang hợp lệ

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


            rptPager.DataSource =
                validPages;

            rptPager.DataBind();
        }


        // ==========================================
        // TẠO URL PHÂN TRANG
        // ==========================================

        private string BuildPageUrl(
            int page)
        {
            string url =
                "shop.aspx?page="
                + page;


            if (!string.IsNullOrWhiteSpace(
                SearchKeyword))
            {
                url +=
                    "&search="
                    + Server.UrlEncode(
                        SearchKeyword);
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
            int totalRecords;

            int totalPages;


            new SanPhamDAL()
                .SearchPaged(
                    SearchKeyword,
                    1,
                    PageSize,
                    out totalRecords);


            totalPages =
                (int)Math.Ceiling(
                    (double)totalRecords
                    / PageSize);


            if (CurrentPage < totalPages)
            {
                Response.Redirect(
                    BuildPageUrl(
                        CurrentPage + 1),
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();
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
            if (e.CommandName != "PageNumber")
                return;


            int page;


            if (int.TryParse(
                e.CommandArgument.ToString(),
                out page)
                && page > 0)
            {
                Response.Redirect(
                    BuildPageUrl(page),
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();
            }
        }


        // ==========================================
        // PAGE ITEM
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