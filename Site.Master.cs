using System;
using System.Web.UI;

namespace web_ban_hang2
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUser();
            }
        }


        private void LoadUser()
        {
            if (Session["User"] != null)
            {
                pnlGuest.Visible = false;

                pnlUser.Visible = true;


                if (Session["UserName"] != null)
                {
                    lblUserName.Text =
                        Session["UserName"].ToString();
                }
            }
            else
            {
                pnlGuest.Visible = true;

                pnlUser.Visible = false;
            }
        }
    }
}