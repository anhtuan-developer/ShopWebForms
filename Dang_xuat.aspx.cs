using System;

namespace web_ban_hang2
{
    public partial class Dang_xuat : System.Web.UI.Page
    {
        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            Session.Clear();

            Session.Abandon();

            Response.Redirect(
                "index.aspx"
            );
        }
    }
}