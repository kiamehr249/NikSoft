using NikSoft.Model;
using NikSoft.NikModel;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;

namespace NikSoft.Web.Modules.BaseModules.User
{
    public partial class UserLogin : WidgetUIContainer
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (PortalUser != null)
            {
                if (IsGeneralUser())
                {
                    RedirectTo("~/" + Level + "/UserProfile");
                    return;
                }
                RedirectTo("~/panel/page/default");
                return;
            }

            var FillUsername = Request.QueryString["un"];
            if (!FillUsername.IsEmpty())
            {
                txtUserName.Text = FillUsername;
            }
        }

        protected void btnlogin_Click(object sender, EventArgs e)
        {
            var sbl = new SingleSignOnService();
            if (!sbl.Authenticate(iUserServ, txtUserName.Text, txtPassword.Text, chbRememberMe.Checked))
            {
                msg.Visible = true;
            }
            else
            {
                if (IsGeneralUser())
                {
                    RedirectTo("~/" + Level + "/UserProfile");
                    return;
                }
                RedirectTo("~/panel/page/default");
                return;
            }
        }

        public bool IsGeneralUser()
        {
            if (null == PortalUser)
                return false;
            var user = iUserServ.Find(t => t.ID == PortalUser.ID);

            if (user.UserType == NikUserType.GeneralUser)
            {
                return true;
            }
            return false;
        }
    }
}