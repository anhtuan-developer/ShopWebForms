using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_KhachHang : Page
    {
        private readonly KhachHangDAL khachHangDAL =
            new KhachHangDAL();


        // ==========================================
        // PAGE LOAD
        // ==========================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadKhachHang();

                LoadStatistics();
            }
        }


        // ==========================================
        // LOAD DANH SÁCH KHÁCH HÀNG
        // ==========================================

        private void LoadKhachHang()
        {
            gvKhachHang.DataSource =
                khachHangDAL.GetAll();

            gvKhachHang.DataBind();
        }


        // ==========================================
        // LOAD THỐNG KÊ
        // ==========================================

        private void LoadStatistics()
        {
            lblTongKhachHang.Text =
                khachHangDAL
                .CountAll()
                .ToString();
        }


        // ==========================================
        // XỬ LÝ ROW COMMAND
        // ==========================================

        protected void gvKhachHang_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            if (e.CommandName != "DeleteCustomer")
            {
                return;
            }


            int maKhachHang;


            if (!int.TryParse(
                e.CommandArgument.ToString(),
                out maKhachHang))
            {
                return;
            }


            DeleteKhachHang(maKhachHang);
        }


        // ==========================================
        // XÓA KHÁCH HÀNG
        // ==========================================

        private void DeleteKhachHang(
            int maKhachHang)
        {
            try
            {
                bool result =
                    khachHangDAL.Delete(
                        maKhachHang
                    );


                if (result)
                {
                    LoadKhachHang();

                    LoadStatistics();
                }
            }
            catch (Exception ex)
            {
                string message =
                    System.Web.HttpUtility
                    .JavaScriptStringEncode(
                        ex.Message
                    );


                ClientScript.RegisterStartupScript(
                    GetType(),
                    "deleteCustomerError",
                    "alert('" + message + "');",
                    true
                );
            }
        }
    }
}