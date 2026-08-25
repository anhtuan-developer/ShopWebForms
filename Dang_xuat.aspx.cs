using System;

namespace web_ban_hang2
{
    public partial class Dang_xuat : System.Web.UI.Page
    {
        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            // ==========================================
            // XÓA TOÀN BỘ SESSION
            // ==========================================

            Session.Clear();

            Session.Abandon();


            // ==========================================
            // QUAY VỀ TRANG CHỦ
            // ==========================================

            Response.Redirect(
                "index.aspx",
                false
            );

            Context.ApplicationInstance
                .CompleteRequest();
        }
    }
}