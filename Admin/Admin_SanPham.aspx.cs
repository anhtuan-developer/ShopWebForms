using System;
using System.Web.UI.WebControls;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_SanPham :  AdminBasePage
    {
        private readonly SanPhamDAL sanPhamDAL =
            new SanPhamDAL();


        // PAGE LOAD
       
        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSanPham();
            }
        }

        // LOAD SẢN PHẨM
        
        private void LoadSanPham()
        {
            gvSanPham.DataSource =
                sanPhamDAL.GetAll();

            gvSanPham.DataBind();
        }


        // THÊM SẢN PHẨM
       
        protected void btnThemSanPham_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "Admin_SanPham_Them.aspx"
            );
        }


        // GRIDVIEW ROW COMMAND
        
        protected void gvSanPham_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            if (e.CommandName != "DeleteProduct")
            {
                return;
            }


            int maSanPham;


            if (!int.TryParse(
                e.CommandArgument.ToString(),
                out maSanPham))
            {
                ShowMessage(
                    "Lỗi",
                    "Mã sản phẩm không hợp lệ.",
                    "❌"
                );

                return;
            }


            DeleteSanPham(maSanPham);
        }


        // XÓA / NGỪNG BÁN SẢN PHẨM
       
        private void DeleteSanPham(
            int maSanPham)
        {
            try
            {
                 // Kiểm tra sản phẩm đã xuất hiện
                // trong đơn hàng hay chưa
               
                int soLuongDonHang =
                    sanPhamDAL.CountOrderDetails(
                        maSanPham
                    );


                // SẢN PHẨM ĐÃ CÓ TRONG ĐƠN HÀNG
               
                if (soLuongDonHang > 0)
                {
                    bool result =
                        sanPhamDAL.NgungBan(
                            maSanPham
                        );


                    if (result)
                    {
                        LoadSanPham();

                        ShowMessage(
                            "Đã ngừng bán",
                            "Sản phẩm này đã xuất hiện trong "
                            + soLuongDonHang
                            + " chi tiết đơn hàng nên không thể xóa. "
                            + "Sản phẩm đã được chuyển sang trạng thái Ngừng bán.",
                            "⚠️"
                        );
                    }
                    else
                    {
                        ShowMessage(
                            "Thất bại",
                            "Không thể chuyển sản phẩm sang trạng thái Ngừng bán.",
                            "❌"
                        );
                    }


                    return;
                }


               // SẢN PHẨM CHƯA CÓ TRONG ĐƠN HÀNG
               
                bool deleteResult =
                    sanPhamDAL.Delete(
                        maSanPham
                    );


                if (deleteResult)
                {
                    LoadSanPham();

                    ShowMessage(
                        "Xóa thành công",
                        "Sản phẩm đã được xóa khỏi hệ thống.",
                        "✅"
                    );

                    return;
                }


                // KHÔNG TÌM THẤY SẢN PHẨM
                
                ShowMessage(
                    "Xóa thất bại",
                    "Không tìm thấy sản phẩm cần xóa.",
                    "❌"
                );
            }
            catch (Exception ex)
            {
                ShowMessage(
                    "Có lỗi xảy ra",
                    ex.Message,
                    "❌"
                );
            }
        }


        // HIỂN THỊ THÔNG BÁO

        private void ShowMessage(
            string title,
            string message,
            string icon)
        {
            string safeTitle =
                System.Web.HttpUtility
                .JavaScriptStringEncode(
                    title
                );


            string safeMessage =
                System.Web.HttpUtility
                .JavaScriptStringEncode(
                    message
                );


            string safeIcon =
                System.Web.HttpUtility
                .JavaScriptStringEncode(
                    icon
                );


            string script =
                "alert('"
                + safeIcon
                + " "
                + safeTitle
                + "\\n"
                + safeMessage
                + "');";


            ClientScript.RegisterStartupScript(
                GetType(),
                "deleteProductMessage",
                script,
                true
            );
        }
    }
}