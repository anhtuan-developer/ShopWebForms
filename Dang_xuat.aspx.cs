using System;

namespace web_ban_hang2
{
    public partial class Dang_xuat : System.Web.UI.Page
    {
        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            // ĐĂNG XUẤT KHÁCH HÀNG
            
            Session.Remove("User");
            Session.Remove("UserId");
            Session.Remove("UserName");
            Session.Remove("MaKhachHang");


            // QUAY VỀ TRANG CHỦ
           
            Response.Redirect(
                "index.aspx",
                false
            );

            Context.ApplicationInstance
                .CompleteRequest();
        }
    }
}