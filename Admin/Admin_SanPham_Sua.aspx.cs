using System;
using System.Data;
using System.Globalization;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_SanPham_Sua : AdminBasePage
    {
        private readonly SanPhamDAL sanPhamDAL =
            new SanPhamDAL();

        private readonly DanhMucDAL danhMucDAL =
            new DanhMucDAL();


        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDanhMuc();

                LoadSanPham();
            }
        }


        private int GetMaSanPham()
        {
            int maSanPham;

            if (!int.TryParse(
                Request.QueryString["id"],
                out maSanPham))
            {
                return 0;
            }

            return maSanPham;
        }


        private void LoadDanhMuc()
        {
            ddlDanhMuc.DataSource =
                danhMucDAL.GetAll();

            ddlDanhMuc.DataTextField =
                "TenDanhMuc";

            ddlDanhMuc.DataValueField =
                "MaDanhMuc";

            ddlDanhMuc.DataBind();

            ddlDanhMuc.Items.Insert(
                0,
                new System.Web.UI.WebControls.ListItem(
                    "-- Chọn danh mục --",
                    "0"
                )
            );
        }


        private void LoadSanPham()
        {
            int maSanPham =
                GetMaSanPham();


            if (maSanPham <= 0)
            {
                ShowMessage(
                    "Lỗi",
                    "Mã sản phẩm không hợp lệ."
                );

                return;
            }


            DataTable table =
                sanPhamDAL.GetById(
                    maSanPham
                );


            if (table == null ||
                table.Rows.Count == 0)
            {
                ShowMessage(
                    "Lỗi",
                    "Không tìm thấy sản phẩm."
                );

                return;
            }


            DataRow row =
                table.Rows[0];


            txtTenSanPham.Text =
                row["TenSanPham"].ToString();


            txtMoTa.Text =
                row["MoTa"] == DBNull.Value
                    ? ""
                    : row["MoTa"].ToString();


            txtGia.Text =
                Convert.ToDecimal(
                    row["Gia"]
                ).ToString(
                    "0.##",
                    CultureInfo.InvariantCulture
                );


            txtSoLuong.Text =
                row["SoLuong"].ToString();


            txtHinhAnh.Text =
                row["HinhAnh"] == DBNull.Value
                    ? ""
                    : row["HinhAnh"].ToString();


            chkTrangThai.Checked =
                Convert.ToBoolean(
                    row["TrangThai"]
                );


            chkNoiBat.Checked =
                row["NoiBat"] != DBNull.Value &&
                Convert.ToBoolean(
                    row["NoiBat"]
                );


            ddlDanhMuc.SelectedValue =
                row["MaDanhMuc"].ToString();


            string hinhAnh =
                row["HinhAnh"] == DBNull.Value
                    ? ""
                    : row["HinhAnh"].ToString();


            if (!string.IsNullOrWhiteSpace(hinhAnh))
            {
                imgSanPham.ImageUrl =
                    ResolveUrl(
                        "~/img/" + hinhAnh
                    );
            }
            else
            {
                imgSanPham.ImageUrl = "";
            }
        }


        protected void btnCapNhat_Click(
            object sender,
            EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }


            int maSanPham =
                GetMaSanPham();


            if (maSanPham <= 0)
            {
                ShowMessage(
                    "Lỗi",
                    "Mã sản phẩm không hợp lệ."
                );

                return;
            }


            int maDanhMuc;


            if (!int.TryParse(
                ddlDanhMuc.SelectedValue,
                out maDanhMuc) ||
                maDanhMuc <= 0)
            {
                ShowMessage(
                    "Lỗi",
                    "Vui lòng chọn danh mục."
                );

                return;
            }


            decimal gia;


            if (!decimal.TryParse(
                txtGia.Text.Trim(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out gia))
            {
                ShowMessage(
                    "Lỗi",
                    "Giá sản phẩm không hợp lệ."
                );

                return;
            }


            if (gia < 0)
            {
                ShowMessage(
                    "Lỗi",
                    "Giá sản phẩm không được nhỏ hơn 0."
                );

                return;
            }


            int soLuong;


            if (!int.TryParse(
                txtSoLuong.Text.Trim(),
                out soLuong))
            {
                ShowMessage(
                    "Lỗi",
                    "Số lượng không hợp lệ."
                );

                return;
            }


            if (soLuong < 0)
            {
                ShowMessage(
                    "Lỗi",
                    "Số lượng không được nhỏ hơn 0."
                );

                return;
            }


            try
            {
                bool result =
                    sanPhamDAL.Update(
                        maSanPham,
                        maDanhMuc,
                        txtTenSanPham.Text.Trim(),
                        txtMoTa.Text.Trim(),
                        gia,
                        soLuong,
                        txtHinhAnh.Text.Trim(),
                        chkTrangThai.Checked,
                        chkNoiBat.Checked
                    );


                if (result)
                {
                    Response.Redirect(
                        "Admin_SanPham.aspx"
                    );
                }
                else
                {
                    ShowMessage(
                        "Cập nhật thất bại",
                        "Không tìm thấy sản phẩm cần cập nhật."
                    );
                }
            }
            catch (Exception ex)
            {
                ShowMessage(
                    "Có lỗi xảy ra",
                    ex.Message
                );
            }
        }


        protected void btnHuy_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "Admin_SanPham.aspx"
            );
        }


        private void ShowMessage(
            string title,
            string message)
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


            ClientScript.RegisterStartupScript(
                GetType(),
                "message",
                "alert('"
                + safeTitle
                + "\\n"
                + safeMessage
                + "');",
                true
            );
        }
    }
}