using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_DonHang : Page
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
            /*
             * Chỉ xử lý các dòng dữ liệu
             */

            if (e.Row.RowType !=
                DataControlRowType.DataRow)
            {
                return;
            }


            /*
             * Tìm DropDownList trạng thái
             */

            DropDownList ddlTrangThai =
                (DropDownList)e.Row.FindControl(
                    "ddlTrangThai"
                );


            if (ddlTrangThai == null)
            {
                return;
            }


            /*
             * Lấy mã đơn hàng
             * trực tiếp từ DataItem
             */

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


            /*
             * Lấy trạng thái hiện tại
             */

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


            /*
             * Tìm trạng thái tương ứng
             * trong DropDownList
             */

            ListItem item =
                ddlTrangThai.Items.FindByValue(
                    trangThai
                );


            if (item != null)
            {
                ddlTrangThai.ClearSelection();

                item.Selected = true;
            }


            /*
             * Lưu mã đơn hàng vào
             * HiddenField của dòng
             */

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
            /*
             * Lấy DropDownList vừa thay đổi
             */

            DropDownList ddlTrangThai =
                (DropDownList)sender;


            /*
             * Lấy GridViewRow chứa DropDownList
             */

            GridViewRow row =
                (GridViewRow)
                ddlTrangThai.NamingContainer;


            /*
             * Tìm HiddenField chứa
             * mã đơn hàng
             */

            HiddenField hfMaDonHang =
                (HiddenField)
                row.FindControl(
                    "hfMaDonHang"
                );


            /*
             * Kiểm tra HiddenField
             */

            if (hfMaDonHang == null)
            {
                ShowError(
                    "Không tìm thấy mã đơn hàng."
                );

                return;
            }


            /*
             * Chuyển mã đơn hàng sang int
             */

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


            /*
             * Lấy trạng thái mới
             */

            string trangThai =
                ddlTrangThai.SelectedValue;


            try
            {
                /*
                 * Cập nhật database
                 */

                bool result =
                    donHangDAL.UpdateTrangThai(
                        maDonHang,
                        trangThai
                    );


                /*
                 * Cập nhật thành công
                 */

                if (result)
                {
                    /*
                     * Load lại danh sách
                     */

                    LoadDonHang();


                    /*
                     * Load lại thống kê
                     */

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
                ShowError(
                    ex.Message
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