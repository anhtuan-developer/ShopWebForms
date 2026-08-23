using System;
using System.Data;
using web_ban_hang2.DAL;

namespace web_ban_hang2
{
    public partial class shop : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            try
            {
                SanPhamDAL sanPhamDAL = new SanPhamDAL();

                DataTable danhSach = sanPhamDAL.GetAll();

                rptProducts.DataSource = danhSach;

                rptProducts.DataBind();

                if (danhSach.Rows.Count == 0)
                {
                    pnlNoProduct.Visible = true;
                }
                else
                {
                    pnlNoProduct.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Response.Write(
                    "<h3>Lỗi tải sản phẩm:</h3>" +
                    "<p>" +
                    ex.Message +
                    "</p>"
                );
            }
        }
    }
}