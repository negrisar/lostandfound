using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.View;
using static System.Collections.Specialized.BitVector32;

namespace WebApplication1
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                int authorityId;
                if (int.TryParse(Session["AuthorityId"]?.ToString(), out authorityId) && authorityId != 3)
                {
                    yetkiVerme.Visible = false;
                }
            }
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            // Clear session or authentication information
            Session.Clear();
            // Redirect to the login page after logout
            Response.Redirect("~/View/Login.aspx");
        }
    }
}