using System;

namespace web_ban_hang2
{
    public partial class Dat_hang_thanh_cong
        : System.Web.UI.Page
    {
        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                string maDonHang =
                    Request.QueryString["maDonHang"];

                lblMaDonHang.Text =
                    "Mã đơn hàng của bạn: #"
                    + maDonHang;
            }
        }
    }
}