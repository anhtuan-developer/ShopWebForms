using System;
using System.Web.UI.WebControls;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_DanhGia :
        AdminBasePage
    {
        private readonly DanhGiaDAL danhGiaDAL =
            new DanhGiaDAL();


        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDanhGia();
            }
        }


        private void LoadDanhGia()
        {
            gvDanhGia.DataSource =
                danhGiaDAL.GetAll();

            gvDanhGia.DataBind();
        }


        protected void gvDanhGia_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            if (e.CommandName != "ToggleStatus")
            {
                return;
            }


            int maDanhGia;

            if (!int.TryParse(
                e.CommandArgument.ToString(),
                out maDanhGia))
            {
                return;
            }


            // Lấy trạng thái hiện tại
            // và đảo trạng thái.
            System.Data.DataTable table =
                danhGiaDAL.GetAll();


            foreach (
                System.Data.DataRow row
                in table.Rows)
            {
                if (
                    Convert.ToInt32(
                        row["MaDanhGia"]
                    ) == maDanhGia)
                {
                    bool currentStatus =
                        Convert.ToBoolean(
                            row["TrangThai"]
                        );

                    danhGiaDAL.UpdateStatus(
                        maDanhGia,
                        !currentStatus
                    );

                    break;
                }
            }


            LoadDanhGia();
        }
    }
}