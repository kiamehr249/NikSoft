using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.User
{
    public partial class PublicUserProfile : WidgetUIContainer
    {
        public IUserProfileService iUserProfileServ { get; set; }
        protected NikModel.User myUser;
        protected UserProfile myProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (PortalUser == null)
            {
                RedirectTo("~/home/login");
                return;
            }

            var thisUser = iUserServ.Find(x => x.ID == PortalUser.ID);
            myUser = thisUser;

            var thisProfile = iUserProfileServ.Find(x => x.UserID == thisUser.ID);
            myProfile = thisProfile;

        }
    }
}