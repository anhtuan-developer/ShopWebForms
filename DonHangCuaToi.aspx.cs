using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using web_ban_hang2.DAL;

namespace web_ban_hang2
{
    public partial class DonHangCuaToi : System.Web.UI.Page
    {
        private readonly DonHangDAL donHangDAL =
            new DonHangDAL();


        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            // ==========================================
            // KIỂM TRA ĐĂNG NHẬP
            // ==========================================

            if (!IsLoggedIn())
            {
                Response.Redirect(
                    "Dang_nhap.aspx?returnUrl=DonHangCuaToi.aspx",
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();

                return;
            }


            if (!IsPostBack)
            {
                LoadDonHang();
            }
        }


        // ==========================================
        // KIỂM TRA ĐĂNG NHẬP
        // ==========================================

        private bool IsLoggedIn()
        {
            int maKhachHang;


            return Session["UserId"] != null
                && int.TryParse(
                    Session["UserId"].ToString(),
                    out maKhachHang)
                && maKhachHang > 0;
        }


        // ==========================================
        // LẤY MÃ KHÁCH HÀNG
        // ==========================================

        private int GetMaKhachHang()
        {
            int maKhachHang;


            if (!int.TryParse(
                Session["UserId"].ToString(),
                out maKhachHang)
                || maKhachHang <= 0)
            {
                throw new InvalidOperationException(
                    "Phiên đăng nhập không hợp lệ.");
            }


            return maKhachHang;
        }


        // ==========================================
        // LOAD ĐƠN HÀNG
        // ==========================================

        private void LoadDonHang()
        {
            try
            {
                DataTable table =
                    donHangDAL.GetByCustomerId(
                        GetMaKhachHang());


                gvDonHang.DataSource =
                    table;

                gvDonHang.DataBind();


                pnlOrders.Visible =
                    true;

                lblMessage.Visible =
                    false;
            }
            catch (Exception)
            {
                pnlOrders.Visible =
                    false;

                lblMessage.Text =
                    "Không thể tải danh sách đơn hàng. "
                    + "Vui lòng thử lại sau.";

                lblMessage.Visible =
                    true;
            }
        }


        // ==========================================
        // TẠO LINK CHI TIẾT
        // ==========================================

        protected void gvDonHang_RowDataBound(
            object sender,
            GridViewRowEventArgs e)
        {
            if (e.Row.RowType !=
                DataControlRowType.DataRow)
            {
                return;
            }


            object maDonHang =
                DataBinder.Eval(
                    e.Row.DataItem,
                    "MaDonHang");


            HyperLink link =
                e.Row.FindControl(
                    "lnkChiTiet")
                as HyperLink;


            if (link != null &&
                maDonHang != null)
            {
                link.NavigateUrl =
                    "DonHangCuaToi_ChiTiet.aspx?id="
                    + maDonHang;
            }
        }
    }
}