﻿using System;
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


            // =====================================
            // THỐNG KÊ CƠ BẢN
            // =====================================

            lblSanPham.Text =
                dal.GetTotalSanPham()
                .ToString();

            lblDanhMuc.Text =
                dal.GetTotalDanhMuc()
                .ToString();

            lblDonHang.Text =
                dal.GetTotalDonHang()
                .ToString();

            lblKhachHang.Text =
                dal.GetTotalKhachHang()
                .ToString();


            // =====================================
            // DOANH THU
            // =====================================

            decimal doanhThuHomNay =
                dal.GetDoanhThuHomNay();

            decimal doanhThuThang =
                dal.GetDoanhThuThang();

            decimal doanhThuNam =
                dal.GetDoanhThuNam();


            lblDoanhThuHomNay.Text =
                doanhThuHomNay
                .ToString("N0") + " ₫";

            lblDoanhThuThang.Text =
                doanhThuThang
                .ToString("N0") + " ₫";

            lblDoanhThuNam.Text =
                doanhThuNam
                .ToString("N0") + " ₫";


            // =====================================
            // TRẠNG THÁI ĐƠN HÀNG
            // =====================================

            lblSoDonDaGiao.Text =
                dal.GetSoDonDaGiao()
                .ToString();

            lblSoDonDangGiao.Text =
                dal.GetSoDonDangGiao()
                .ToString();


            // =====================================
            // TOP SẢN PHẨM
            // =====================================

            gvTopSanPham.DataSource =
                dal.GetTopSanPhamBanChay();

            gvTopSanPham.DataBind();
        }
    }
}
