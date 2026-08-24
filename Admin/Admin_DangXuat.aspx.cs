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
            // ==========================================
            // XÓA SESSION ADMIN
            // ==========================================

            Session.Remove("AdminMa");

            Session.Remove("AdminEmail");

            Session.Remove("AdminHoTen");


            // ==========================================
            // HỦY SESSION
            // ==========================================

            Session.Clear();

            Session.Abandon();


            // ==========================================
            // QUAY VỀ TRANG ĐĂNG NHẬP
            // ==========================================

            Response.Redirect(
                "~/Admin/Admin_DangNhap.aspx"
            );
        }
    }
}