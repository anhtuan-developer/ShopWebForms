using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using web_ban_hang2.Models;
using web_ban_hang2.Services;

namespace web_ban_hang2
{
    public partial class Cart : System.Web.UI.Page
    {
        private CartService cartService;

        protected void Page_Load(object sender, EventArgs e)
        {
            cartService = new CartService();

            if (!IsPostBack)
            {
                LoadCart();
            }
        }

        private void LoadCart()
        {
            string validationMessage;

            cartService.ValidateCart(
                out validationMessage);

            if (!string.IsNullOrEmpty(validationMessage))
            {
                // Nếu Cart.aspx có lblMessage thì có thể
                // hiển thị tại đây.
                // Không giả định control này tồn tại
                // để tránh lỗi Designer.
            }

            List<CartItem> cart =
                cartService.GetItems();

            if (cart == null || cart.Count == 0)
            {
                pnlCart.Visible = false;
                pnlEmpty.Visible = true;

                lblTotalQuantity.Text = "0";
                lblTotal.Text = "0 ₫";

                return;
            }

            pnlCart.Visible = true;
            pnlEmpty.Visible = false;

            rptCart.DataSource = cart;
            rptCart.DataBind();

            lblTotalQuantity.Text =
                cartService
                    .GetTotalQuantity()
                    .ToString();

            lblTotal.Text =
                cartService
                    .GetTotal()
                    .ToString("N0")
                    + " ₫";
        }

        protected void rptCart_ItemCommand(
            object source,
            RepeaterCommandEventArgs e)
        {
            if (!int.TryParse(
                Convert.ToString(
                    e.CommandArgument),
                out int maSanPham))
            {
                LoadCart();
                return;
            }

            if (e.CommandName == "RemoveCart")
            {
                cartService.Remove(
                    maSanPham);

                LoadCart();

                return;
            }

            if (e.CommandName == "UpdateCart")
            {
                TextBox txtQuantity =
                    e.Item.FindControl(
                        "txtQuantity")
                    as TextBox;

                if (txtQuantity == null ||
                    !int.TryParse(
                        txtQuantity.Text,
                        out int quantity))
                {
                    LoadCart();
                    return;
                }

                string message;

                bool success =
                    cartService.UpdateQuantity(
                        maSanPham,
                        quantity,
                        out message);

                if (!success &&
                    !string.IsNullOrEmpty(message))
                {
                    ClientScript.RegisterStartupScript(
                        GetType(),
                        "CartMessage",
                        "alert(" +
                        ToJavaScriptString(message) +
                        ");",
                        true);
                }

                LoadCart();
            }
        }

        protected void btnCheckout_Click(
            object sender,
            EventArgs e)
        {
            List<CartItem> cart =
                cartService.GetItems();

            if (cart == null ||
                cart.Count == 0)
            {
                Response.Redirect(
                    "Cart.aspx");

                return;
            }

            string message;

            bool valid =
                cartService.ValidateCart(
                    out message);

            cart =
                cartService.GetItems();

            if (!valid ||
                cart.Count == 0)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    ClientScript.RegisterStartupScript(
                        GetType(),
                        "CheckoutCartMessage",
                        "alert(" +
                        ToJavaScriptString(message) +
                        ");",
                        true);
                }

                LoadCart();

                return;
            }

            Response.Redirect(
                "Checkout.aspx");
        }

        private string ToJavaScriptString(
            string value)
        {
            if (value == null)
            {
                return "''";
            }

            return "'" +
                value
                    .Replace(
                        "\\",
                        "\\\\")
                    .Replace(
                        "'",
                        "\\'")
                    .Replace(
                        "\r",
                        "\\r")
                    .Replace(
                        "\n",
                        "\\n")
                + "'";
        }
    }
}