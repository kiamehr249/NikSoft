using NikSoft.Model;
using NikSoft.UILayer;
using System;

namespace NikSoft.Web.Modules.BaseModules.User
{
    public partial class LogOut : WidgetUIContainer
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SingleSignOnService.SignOut3();

            if (Level.ToLower() != "home")
            {
                RedirectTo2("~/" + Level, true);
                return;
            }
            RedirectTo("~/");
            return;
        }
    }
}