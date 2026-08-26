using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_DonHang : AdminBasePage
    {
        private readonly DonHangDAL donHangDAL =
            new DonHangDAL();


        // ==========================================
        // PAGE LOAD
        // ==========================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDonHang();

                LoadStatistics();
            }
        }


        // ==========================================
        // LOAD ĐƠN HÀNG
        // ==========================================

        private void LoadDonHang()
        {
            gvDonHang.DataSource =
                donHangDAL.GetAll();

            gvDonHang.DataBind();
        }


        // ==========================================
        // LOAD THỐNG KÊ
        // ==========================================

        private void LoadStatistics()
        {
            lblTongDonHang.Text =
                donHangDAL
                .CountAll()
                .ToString();


            lblChoXuLy.Text =
                donHangDAL
                .CountByStatus("Chờ xử lý")
                .ToString();


            lblDangGiao.Text =
                donHangDAL
                .CountByStatus("Đang giao")
                .ToString();


            lblDaGiao.Text =
                donHangDAL
                .CountByStatus("Đã giao")
                .ToString();
        }


        // ==========================================
        // CSS TRẠNG THÁI
        // ==========================================

        protected string GetStatusClass(
            object value)
        {
            if (value == null)
            {
                return "status-default";
            }


            string trangThai =
                value.ToString();


            switch (trangThai)
            {
                case "Chờ xử lý":

                    return "status-pending";


                case "Đã xác nhận":

                    return "status-confirmed";


                case "Đang giao":

                    return "status-shipping";


                case "Đã giao":

                    return "status-completed";


                case "Đã hủy":

                    return "status-cancelled";


                default:

                    return "status-default";
            }
        }


        // ==========================================
        // GRIDVIEW ROW DATA BOUND
        // ==========================================

        protected void gvDonHang_RowDataBound(
            object sender,
            GridViewRowEventArgs e)
        {
            // Chỉ xử lý dòng dữ liệu
            if (e.Row.RowType !=
                DataControlRowType.DataRow)
            {
                return;
            }


            // ======================================
            // LẤY DROPDOWNLIST
            // ======================================

            DropDownList ddlTrangThai =
                (DropDownList)e.Row.FindControl(
                    "ddlTrangThai"
                );


            if (ddlTrangThai == null)
            {
                return;
            }


            // ======================================
            // LẤY MÃ ĐƠN HÀNG TỪ DỮ LIỆU GRIDVIEW
            // ======================================

            object maDonHangObject =
                DataBinder.Eval(
                    e.Row.DataItem,
                    "MaDonHang"
                );


            if (maDonHangObject == null)
            {
                return;
            }


            int maDonHang;


            if (!int.TryParse(
                maDonHangObject.ToString(),
                out maDonHang))
            {
                return;
            }


            // ======================================
            // LẤY TRẠNG THÁI HIỆN TẠI
            // ======================================

            object trangThaiObject =
                DataBinder.Eval(
                    e.Row.DataItem,
                    "TrangThai"
                );


            if (trangThaiObject == null)
            {
                return;
            }


            string trangThai =
                trangThaiObject.ToString();


            // ======================================
            // CHỌN TRẠNG THÁI HIỆN TẠI
            // ======================================

            ListItem item =
                ddlTrangThai.Items.FindByValue(
                    trangThai
                );


            if (item != null)
            {
                ddlTrangThai.ClearSelection();

                item.Selected = true;
            }


            // ======================================
            // LƯU MÃ ĐƠN HÀNG VÀO HIDDENFIELD
            // ======================================

            HiddenField hfMaDonHang =
                (HiddenField)e.Row.FindControl(
                    "hfMaDonHang"
                );


            if (hfMaDonHang != null)
            {
                hfMaDonHang.Value =
                    maDonHang.ToString();
            }
        }


        // ==========================================
        // CẬP NHẬT TRẠNG THÁI ĐƠN HÀNG
        // ==========================================

        protected void ddlTrangThai_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            // ======================================
            // LẤY DROPDOWNLIST
            // ======================================

            DropDownList ddlTrangThai =
                (DropDownList)sender;


            // ======================================
            // LẤY DÒNG GRIDVIEW
            // ======================================

            GridViewRow row =
                (GridViewRow)
                ddlTrangThai.NamingContainer;


            // ======================================
            // TÌM HIDDENFIELD
            // ======================================

            HiddenField hfMaDonHang =
                (HiddenField)
                row.FindControl(
                    "hfMaDonHang"
                );


            // ======================================
            // KIỂM TRA HIDDENFIELD
            // ======================================

            if (hfMaDonHang == null)
            {
                ShowError(
                    "Không tìm thấy mã đơn hàng."
                );

                return;
            }


            // ======================================
            // CHUYỂN MÃ ĐƠN HÀNG SANG INT
            // ======================================

            int maDonHang;


            if (!int.TryParse(
                hfMaDonHang.Value,
                out maDonHang))
            {
                ShowError(
                    "Mã đơn hàng không hợp lệ."
                );

                return;
            }


            // ======================================
            // LẤY TRẠNG THÁI MỚI
            // ======================================

            string trangThai =
                ddlTrangThai.SelectedValue;


            try
            {
                // ==================================
                // CẬP NHẬT DATABASE
                // ==================================

                bool result =
                    donHangDAL.UpdateTrangThai(
                        maDonHang,
                        trangThai
                    );


                // ==================================
                // CẬP NHẬT THÀNH CÔNG
                // ==================================

                if (result)
                {
                    LoadDonHang();

                    LoadStatistics();
                }
                else
                {
                    ShowError(
                        "Không thể cập nhật trạng thái đơn hàng."
                    );
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    ex.ToString()
                );

                ShowError(
                    "Có lỗi xảy ra khi cập nhật trạng thái đơn hàng."
                );
            }
        }


        // ==========================================
        // HIỂN THỊ LỖI
        // ==========================================

        private void ShowError(
            string message)
        {
            string safeMessage =
                System.Web.HttpUtility
                .JavaScriptStringEncode(
                    message
                );


            ClientScript.RegisterStartupScript(
                GetType(),
                "orderError",
                "alert('" +
                safeMessage +
                "');",
                true
            );
        }
    }
}