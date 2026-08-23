using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace web_ban_hang2
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtSubject.Text) ||
                string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                lblMessage.Text = "Vui lòng nhập đầy đủ thông tin.";

                lblMessage.CssClass =
                    "text-danger d-block mt-3";

                return;
            }

            lblMessage.Text =
                "Cảm ơn bạn! Tin nhắn đã được tiếp nhận.";

            lblMessage.CssClass =
                "text-success d-block mt-3";

            txtName.Text = "";
            txtEmail.Text = "";
            txtSubject.Text = "";
            txtMessage.Text = "";
        }
    }
}