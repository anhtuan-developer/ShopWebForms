using System;
using System.Data;
using System.IO;
using web_ban_hang2.DAL;

namespace web_ban_hang2
{
    public partial class index : System.Web.UI.Page
    {
        private readonly SanPhamDAL sanPhamDAL =
            new SanPhamDAL();

        private readonly DanhMucDAL danhMucDAL =
            new DanhMucDAL();


        // ==========================================
        // PAGE LOAD
        // ==========================================

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDanhMuc();
                LoadFeaturedProducts();
            }
        }


        // ==========================================
        // LOAD DANH MỤC TỪ DATABASE
        // ==========================================

        private void LoadDanhMuc()
        {
            try
            {
                DataTable categories =
                    danhMucDAL.GetActive();

                rptDanhMuc.DataSource =
                    categories;

                rptDanhMuc.DataBind();
            }
            catch (Exception)
            {
                rptDanhMuc.DataSource = null;
                rptDanhMuc.DataBind();
            }
        }


        // ==========================================
        // LOAD SẢN PHẨM NỔI BẬT
        // ==========================================

        private void LoadFeaturedProducts()
        {
            try
            {
                DataTable products =
                    sanPhamDAL.GetFeaturedProducts(4);


                rptFeaturedProducts.DataSource =
                    products;


                rptFeaturedProducts.DataBind();
            }
            catch (Exception)
            {
                rptFeaturedProducts.DataSource =
                    null;

                rptFeaturedProducts.DataBind();
            }
        }


        // ==========================================
        // TẠO ĐƯỜNG DẪN ẢNH SẢN PHẨM
        // ==========================================

        protected string GetProductImageUrl(object imageValue)
        {
            string imageName =
                Convert.ToString(imageValue);


            // ======================================
            // KHÔNG CÓ TÊN ẢNH
            // ======================================

            if (string.IsNullOrWhiteSpace(imageName))
            {
                return ResolveUrl(
                    "~/img/17promaxcam.jpg"
                );
            }


            imageName =
                imageName
                .Trim()
                .Replace("\\", "/");


            // ======================================
            // DATABASE LƯU ~/img/...
            // ======================================

            if (imageName.StartsWith("~/"))
            {
                string physicalPath =
                    Server.MapPath(imageName);


                if (File.Exists(physicalPath))
                {
                    return ResolveUrl(imageName);
                }


                return ResolveUrl(
                    "~/img/17promaxcam.jpg"
                );
            }


            // ======================================
            // DATABASE LƯU /img/...
            // ======================================

            if (imageName.StartsWith("/"))
            {
                string appRelativePath =
                    "~" + imageName;


                string physicalPath =
                    Server.MapPath(
                        appRelativePath
                    );


                if (File.Exists(physicalPath))
                {
                    return ResolveUrl(
                        appRelativePath
                    );
                }


                return ResolveUrl(
                    "~/img/17promaxcam.jpg"
                );
            }


            // ======================================
            // DATABASE LƯU img/...
            // ======================================

            if (
                imageName.StartsWith(
                    "img/",
                    StringComparison.OrdinalIgnoreCase
                )
            )
            {
                string appRelativePath =
                    "~/" + imageName;


                string physicalPath =
                    Server.MapPath(
                        appRelativePath
                    );


                if (File.Exists(physicalPath))
                {
                    return ResolveUrl(
                        appRelativePath
                    );
                }


                return ResolveUrl(
                    "~/img/17promaxcam.jpg"
                );
            }


            // ======================================
            // DATABASE CHỈ LƯU TÊN FILE
            // ======================================

            string relativePath =
                "~/img/" + imageName;


            string fullPath =
                Server.MapPath(
                    relativePath
                );


            if (File.Exists(fullPath))
            {
                return ResolveUrl(
                    relativePath
                );
            }


            // ======================================
            // ẢNH MẶC ĐỊNH
            // ======================================

            return ResolveUrl(
                "~/img/17promaxcam.jpg"
            );
        }
    }
}