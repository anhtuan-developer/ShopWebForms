using System;
using System.Web.UI;

namespace web_ban_hang2.Admin
{
    public class AdminBasePage : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // KIỂM TRA ĐĂNG NHẬP ADMIN

            int maAdmin;

            bool isAdmin =
                Session != null
                && Session["AdminMa"] != null
                && int.TryParse(
                    Session["AdminMa"].ToString(),
                    out maAdmin)
                && maAdmin > 0;

            if (!isAdmin)
            {
                Response.Redirect(
                    "~/Admin/Admin_DangNhap.aspx",
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();

                return;
            }
        }
    }
}