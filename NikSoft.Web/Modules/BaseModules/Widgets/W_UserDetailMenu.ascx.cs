using NikSoft.UILayer;
using System;

namespace NikSoft.Web.Modules.BaseModules.Widgets
{
    public partial class W_UserDetailMenu : WidgetUIContainer
    {
        protected NikModel.User myUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (PortalUser != null)
            {
                myUser = iUserServ.Find(x => x.ID == PortalUser.ID);
            }
            
        }
    }
}