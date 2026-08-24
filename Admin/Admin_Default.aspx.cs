using System;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_Default : AdminBasePage
    {
        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboard();
            }
        }


        // =========================================
        // LOAD DASHBOARD
        // =========================================

        private void LoadDashboard()
        {
            DashboardDAL dal =
                new DashboardDAL();


            // SẢN PHẨM

            int totalSanPham =
                dal.GetTotalSanPham();

            lblSanPham.Text =
                totalSanPham.ToString();


            // DANH MỤC

            int totalDanhMuc =
                dal.GetTotalDanhMuc();

            lblDanhMuc.Text =
                totalDanhMuc.ToString();


            // ĐƠN HÀNG

            int totalDonHang =
                dal.GetTotalDonHang();

            lblDonHang.Text =
                totalDonHang.ToString();


            // KHÁCH HÀNG

            int totalKhachHang =
                dal.GetTotalKhachHang();

            lblKhachHang.Text =
                totalKhachHang.ToString();
        }
    }
}