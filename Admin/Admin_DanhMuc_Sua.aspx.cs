using System;
using System.Data;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_DanhMuc_Sua : System.Web.UI.Page
    {
        private readonly DanhMucDAL danhMucDAL =
            new DanhMucDAL();


        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDanhMuc();
            }
        }


        // ==========================================
        // LOAD DANH MỤC
        // ==========================================

        private void LoadDanhMuc()
        {
            int maDanhMuc;

            if (!int.TryParse(
                Request.QueryString["id"],
                out maDanhMuc))
            {
                Response.Redirect(
                    "Admin_DanhMuc.aspx");

                return;
            }


            DataTable table =
                danhMucDAL.GetById(maDanhMuc);


            if (table.Rows.Count == 0)
            {
                Response.Redirect(
                    "Admin_DanhMuc.aspx");

                return;
            }


            DataRow row =
                table.Rows[0];


            txtMaDanhMuc.Text =
                row["MaDanhMuc"].ToString();


            txtTenDanhMuc.Text =
                row["TenDanhMuc"].ToString();


            txtMoTa.Text =
                row["MoTa"] == DBNull.Value
                    ? ""
                    : row["MoTa"].ToString();


            chkTrangThai.Checked =
                row["TrangThai"] != DBNull.Value
                &&
                Convert.ToBoolean(
                    row["TrangThai"]);
        }


        // ==========================================
        // CẬP NHẬT
        // ==========================================

        protected void btnCapNhat_Click(
            object sender,
            EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }


            int maDanhMuc;

            if (!int.TryParse(
                txtMaDanhMuc.Text,
                out maDanhMuc))
            {
                lblMessage.Text =
                    "Mã danh mục không hợp lệ.";

                lblMessage.ForeColor =
                    System.Drawing.Color.Red;

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
                    danhMucDAL.Update(
                        maDanhMuc,
                        tenDanhMuc,
                        moTa,
                        trangThai
                    );


                if (result)
                {
                    Response.Redirect(
                        "Admin_DanhMuc.aspx");

                    return;
                }


                lblMessage.Text =
                    "Không tìm thấy danh mục để cập nhật.";

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
                "Admin_DanhMuc.aspx");
        }
    }
}