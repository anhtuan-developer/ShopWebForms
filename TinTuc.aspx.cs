using System;
using System.Data;
using web_ban_hang2.DAL;

namespace web_ban_hang2
{
    public partial class TinTucPage :
        System.Web.UI.Page
    {
        private readonly TinTucDAL tinTucDAL =
            new TinTucDAL();


        // ==========================================
        // PAGE LOAD
        // ==========================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTinTuc();
            }
        }


        // ==========================================
        // LOAD TIN TỨC
        // ==========================================

        private void LoadTinTuc()
        {
            try
            {
                DataTable table =
                    tinTucDAL.GetAllActive();


                if (table == null ||
                    table.Rows.Count == 0)
                {
                    pnlEmpty.Visible =
                        true;

                    return;
                }


                rptTinTuc.DataSource =
                    table;

                rptTinTuc.DataBind();
            }
            catch (Exception)
            {
                pnlError.Visible =
                    true;

                lblMessage.Text =
                    "Không thể tải danh sách tin tức. " +
                    "Vui lòng thử lại sau.";
            }
        }


        // ==========================================
        // LẤY URL HÌNH ẢNH
        // ==========================================

        protected string GetImageUrl(
            object image)
        {
            string fileName =
                image == null ||
                image == DBNull.Value
                    ? ""
                    : image.ToString().Trim();


            if (string.IsNullOrWhiteSpace(
                fileName))
            {
                return ResolveUrl(
                    "~/img/about.jpg");
            }


            return ResolveUrl(
                "~/img/" + fileName);
        }


        // ==========================================
        // TẠO MÔ TẢ NGẮN
        // ==========================================

        protected string GetSummary(
            object content)
        {
            string text =
                content == null ||
                content == DBNull.Value
                    ? ""
                    : content.ToString().Trim();


            // Loại bỏ HTML nếu sau này
            // nội dung có HTML.

            text =
                System.Text.RegularExpressions
                    .Regex.Replace(
                        text,
                        "<.*?>",
                        " ");


            text =
                Server.HtmlEncode(
                    text);


            if (text.Length > 150)
            {
                return
                    text.Substring(0, 150)
                    + "...";
            }


            return text;
        }
    }
}