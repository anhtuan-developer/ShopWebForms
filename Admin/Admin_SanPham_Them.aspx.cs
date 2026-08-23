using System;
using System.Data;
using web_ban_hang2.DAL;

namespace web_ban_hang2.Admin
{
    public partial class Admin_SanPham_Them : System.Web.UI.Page
    {
        private readonly SanPhamDAL sanPhamDAL =
            new SanPhamDAL();

        private readonly DanhMucDAL danhMucDAL =
            new DanhMucDAL();


        // ==========================================
        // PAGE LOAD
        // ==========================================

        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDanhMuc();

                ddlTrangThai.SelectedValue = "true";
            }
        }


        // ==========================================
        // LOAD DANH MỤC
        // ==========================================

        private void LoadDanhMuc()
        {
            DataTable table =
                danhMucDAL.GetAll();

            ddlDanhMuc.Items.Clear();

            ddlDanhMuc.Items.Add(
                new System.Web.UI.WebControls.ListItem(
                    "-- Chọn danh mục --",
                    ""
                )
            );


            foreach (DataRow row in table.Rows)
            {
                ddlDanhMuc.Items.Add(
                    new System.Web.UI.WebControls.ListItem(
                        row["TenDanhMuc"].ToString(),
                        row["MaDanhMuc"].ToString()
                    )
                );
            }
        }


        // ==========================================
        // LƯU SẢN PHẨM
        // ==========================================

        protected void btnLuu_Click(
            object sender,
            EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }


            try
            {
                // ------------------------------
                // DANH MỤC
                // ------------------------------

                int maDanhMuc;

                if (!int.TryParse(
                    ddlDanhMuc.SelectedValue,
                    out maDanhMuc))
                {
                    ShowMessage(
                        "Vui lòng chọn danh mục."
                    );

                    return;
                }


                // ------------------------------
                // TÊN
                // ------------------------------

                string tenSanPham =
                    txtTenSanPham.Text.Trim();


                // ------------------------------
                // MÔ TẢ
                // ------------------------------

                string moTa =
                    txtMoTa.Text.Trim();


                // ------------------------------
                // GIÁ
                // ------------------------------

                decimal gia;

                string giaText =
                    txtGia.Text.Trim()
                    .Replace(",", ".");


                if (!decimal.TryParse(
                    giaText,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out gia))
                {
                    ShowMessage(
                        "Giá sản phẩm không hợp lệ."
                    );

                    return;
                }


                if (gia < 0)
                {
                    ShowMessage(
                        "Giá sản phẩm không được nhỏ hơn 0."
                    );

                    return;
                }


                // ------------------------------
                // SỐ LƯỢNG
                // ------------------------------

                int soLuong;

                if (!int.TryParse(
                    txtSoLuong.Text.Trim(),
                    out soLuong))
                {
                    ShowMessage(
                        "Số lượng không hợp lệ."
                    );

                    return;
                }


                if (soLuong < 0)
                {
                    ShowMessage(
                        "Số lượng không được nhỏ hơn 0."
                    );

                    return;
                }


                // ------------------------------
                // HÌNH ẢNH
                // ------------------------------

                string hinhAnh =
                    txtHinhAnh.Text.Trim();


                // ------------------------------
                // TRẠNG THÁI
                // ------------------------------

                bool trangThai =
                    ddlTrangThai.SelectedValue == "true";


                // ------------------------------
                // INSERT
                // ------------------------------

                bool result =
                    sanPhamDAL.Insert(
                        maDanhMuc,
                        tenSanPham,
                        moTa,
                        gia,
                        soLuong,
                        hinhAnh,
                        trangThai
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
                        "Không thể thêm sản phẩm."
                    );
                }
            }
            catch (Exception ex)
            {
                ShowMessage(
                    ex.Message
                );
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
                "Admin_SanPham.aspx"
            );
        }


        // ==========================================
        // THÔNG BÁO
        // ==========================================

        private void ShowMessage(
            string message)
        {
            string safeMessage =
                System.Web.HttpUtility
                .JavaScriptStringEncode(
                    message
                );


            ClientScript.RegisterStartupScript(
                GetType(),
                "message",
                "alert('" + safeMessage + "');",
                true
            );
        }
    }
}