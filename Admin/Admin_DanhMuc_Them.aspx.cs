using System;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_DanhMuc_Them : System.Web.UI.Page
    {
        private readonly DanhMucDAL danhMucDAL =
            new DanhMucDAL();


        protected void Page_Load(
            object sender,
            EventArgs e)
        {
        }


        // ==========================================
        // LƯU DANH MỤC
        // ==========================================

        protected void btnLuu_Click(
            object sender,
            EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }


            string tenDanhMuc =
                txtTenDanhMuc.Text.Trim();

            string moTa =
                txtMoTa.Text.Trim();

            bool trangThai =
                chkTrangThai.Checked;


            try
            {
                bool result =
                    danhMucDAL.Insert(
                        tenDanhMuc,
                        moTa,
                        trangThai
                    );


                if (result)
                {
                    Response.Redirect(
                        "Admin_DanhMuc.aspx"
                    );

                    return;
                }


                lblMessage.Text =
                    "Không thể thêm danh mục.";

                lblMessage.ForeColor =
                    System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Có lỗi xảy ra: "
                    + ex.Message;

                lblMessage.ForeColor =
                    System.Drawing.Color.Red;
            }
        }


        // ==========================================
        // HỦY
        // ==========================================

        protected void btnHuy_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "Admin_DanhMuc.aspx"
            );
        }
    }
}