using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using web_ban_hang2.Models;

namespace web_ban_hang2
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCart();
            }
        }

        private void LoadCart()
        {
            List<CartItem> cart =
                Session["Cart"] as List<CartItem>;

            // Nếu giỏ hàng chưa tồn tại
            if (cart == null || cart.Count == 0)
            {
                pnlCart.Visible = false;
                pnlEmpty.Visible = true;

                lblTotalQuantity.Text = "0";
                lblTotal.Text = "0 ₫";

                return;
            }

            // Có sản phẩm
            pnlCart.Visible = true;
            pnlEmpty.Visible = false;

            // Hiển thị danh sách
            rptCart.DataSource = cart;
            rptCart.DataBind();

            // Tổng số lượng
            int totalQuantity =
                cart.Sum(x => x.SoLuong);

            // Tổng tiền
            decimal total =
                cart.Sum(x => x.ThanhTien);

            lblTotalQuantity.Text =
                totalQuantity.ToString();

            lblTotal.Text =
                total.ToString("N0") + " ₫";
        }

        protected void rptCart_ItemCommand(
            object source,
            RepeaterCommandEventArgs e)
        {
            List<CartItem> cart =
                Session["Cart"] as List<CartItem>;

            if (cart == null)
            {
                return;
            }

            int maSanPham =
                Convert.ToInt32(e.CommandArgument);

            // =========================
            // XÓA SẢN PHẨM
            // =========================

            if (e.CommandName == "RemoveCart")
            {
                CartItem item =
                    cart.FirstOrDefault(
                        x => x.MaSanPham == maSanPham
                    );

                if (item != null)
                {
                    cart.Remove(item);
                }

                Session["Cart"] = cart;

                LoadCart();
            }

            // =========================
            // CẬP NHẬT SỐ LƯỢNG
            // =========================

            else if (e.CommandName == "UpdateCart")
            {
                TextBox txtQuantity =
                    e.Item.FindControl("txtQuantity")
                    as TextBox;

                if (txtQuantity == null)
                {
                    return;
                }

                int quantity;

                if (!int.TryParse(
                    txtQuantity.Text,
                    out quantity))
                {
                    return;
                }

                if (quantity <= 0)
                {
                    CartItem item =
                        cart.FirstOrDefault(
                            x => x.MaSanPham == maSanPham
                        );

                    if (item != null)
                    {
                        cart.Remove(item);
                    }
                }
                else
                {
                    CartItem item =
                        cart.FirstOrDefault(
                            x => x.MaSanPham == maSanPham
                        );

                    if (item != null)
                    {
                        item.SoLuong = quantity;
                    }
                }

                Session["Cart"] = cart;

                LoadCart();
            }
        }

        // =========================
        // THANH TOÁN
        // =========================

        protected void btnCheckout_Click(
            object sender,
            EventArgs e)
        {
            List<CartItem> cart =
                Session["Cart"] as List<CartItem>;

            // Kiểm tra giỏ hàng
            if (cart == null || cart.Count == 0)
            {
                Response.Redirect("Cart.aspx");
                return;
            }

            // Chuyển sang trang thanh toán
            Response.Redirect("Checkout.aspx");
        }
    }
}