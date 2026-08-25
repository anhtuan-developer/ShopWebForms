using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_KhachHang : AdminBasePage
    {
        private readonly KhachHangDAL khachHangDAL =
            new KhachHangDAL();


        // PAGE LOAD
        
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


        // LOAD DANH SÁCH KHÁCH HÀNG
        
        private void LoadKhachHang()
        {
            gvKhachHang.DataSource =
                khachHangDAL.GetAll();

            gvKhachHang.DataBind();
        }


        // LOAD THỐNG KÊ
        
        private void LoadStatistics()
        {
            lblTongKhachHang.Text =
                khachHangDAL
                .CountAll()
                .ToString();
        }


        // XỬ LÝ ROW COMMAND
        
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


        // XÓA KHÁCH HÀNG
        
        private void DeleteKhachHang(
            int maKhachHang)
        {
            try
            {
                string message;

                bool result =
                    khachHangDAL.Delete(
                        maKhachHang,
                        out message);


                // XÓA THÀNH CÔNG
                
                if (result)
                {
                    LoadKhachHang();

                    LoadStatistics();

                    ShowMessage(
                        message,
                        false);

                    return;
                }


                // KHÔNG THỂ XÓA
                
                ShowMessage(
                    message,
                    true);
            }
            catch (Exception ex)
            {
                ShowMessage(
                    "Có lỗi xảy ra khi xóa khách hàng: "
                    + ex.Message,
                    true);
            }
        }


        // HIỂN THỊ THÔNG BÁO
        
        private void ShowMessage(
            string message,
            bool isError)
        {
            string safeMessage =
                System.Web.HttpUtility
                .JavaScriptStringEncode(
                    message ?? "");

            ClientScript.RegisterStartupScript(
                GetType(),
                "customerMessage",
                "alert('" +
                safeMessage +
                "');",
                true);
        }
    }
}