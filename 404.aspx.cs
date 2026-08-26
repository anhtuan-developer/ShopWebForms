using System;

namespace web_ban_hang2
{
    public partial class _404 :
        System.Web.UI.Page
    {
        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            // ==========================================
            // TRẢ VỀ HTTP STATUS 404
            // ==========================================

            Response.StatusCode = 404;

            // Không để IIS thay thế trang 404
            // bằng trang lỗi mặc định.

            Response.TrySkipIisCustomErrors = true;
        }
    }
}