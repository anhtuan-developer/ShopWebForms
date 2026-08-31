using System;
using System.Web.UI;
using web_ban_hang2.DAL;

namespace web_ban_hang2
{
    public partial class SiteMaster : MasterPage
    {
        // ==========================================
        // PAGE LOAD
        // ==========================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUser();

                LoadDanhMuc();
            }
        }


        // ==========================================
        // TÌM KIẾM SẢN PHẨM
        // ==========================================

        protected void btnSearch_Click(
            object sender,
            EventArgs e)
        {
            string keyword =
                (txtSearch.Text ?? string.Empty)
                .Trim();

            string url = "shop.aspx";

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                url += "?search="
                    + Server.UrlEncode(keyword);
            }

            Response.Redirect(
                url,
                false);

            Context.ApplicationInstance
                .CompleteRequest();
        }


        // ==========================================
        // LOAD DANH MỤC
        // ==========================================

        private void LoadDanhMuc()
        {
            try
            {
                DanhMucDAL danhMucDAL =
                    new DanhMucDAL();

                rptDanhMuc.DataSource =
                    danhMucDAL.GetActive();

                rptDanhMuc.DataBind();
            }
            catch (Exception)
            {
                rptDanhMuc.DataSource = null;

                rptDanhMuc.DataBind();
            }
        }


        // ==========================================
        // LOAD USER
        // ==========================================

        private void LoadUser()
        {
            if (Session["User"] != null)
            {
                pnlGuest.Visible = false;

                pnlUser.Visible = true;

                if (Session["UserName"] != null)
                {
                    lblUserName.Text =
                        Session["UserName"]
                        .ToString();
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