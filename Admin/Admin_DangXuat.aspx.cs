using System;
using System.Web.UI;

namespace web_ban_hang2.Admin
{
    public partial class Admin_DangXuat : Page
    {
        protected void Page_Load(
            object sender,
            EventArgs e)
        {


            // XÓA SESSION ADMIN

            Session.Remove("AdminMa");
            Session.Remove("AdminEmail");
            Session.Remove("AdminHoTen");

            Response.Redirect(
                "~/Admin/Admin_DangNhap.aspx",
                false);

            Context.ApplicationInstance
                .CompleteRequest();
        }
    }
}