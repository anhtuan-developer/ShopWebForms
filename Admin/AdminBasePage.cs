using System;
using System.Web.UI;

namespace web_ban_hang2.Admin
{
    public class AdminBasePage : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // ==========================================
            // KIỂM TRA ĐĂNG NHẬP ADMIN
            // ==========================================

            if (Session["AdminMa"] == null)
            {
                Response.Redirect(
                    "~/Admin/Admin_DangNhap.aspx",
                    true
                );

                return;
            }
        }
    }
}