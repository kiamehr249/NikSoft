using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Widgets
{
    public partial class W_PanelMenu : WidgetUIContainer
    {
        public IUserTypeGroupService iUserTypeGroupService { get; set; }
        public INikMenuService iNikMenuServ { get; set; }
        public IUserRoleMenuService iUserRoleMenuServ { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (PortalUser == null)
            {
                RedirectTo("~/home/login");
                return;
            }
            else
            {
                var thisUser = iUserServ.Find(x => x.ID == PortalUser.ID);
                if (thisUser.UserType != NikUserType.NikUser)
                {
                    RedirectTo("~/home/UserProfile");
                    return;
                }
            }

            if (!IsPostBack)
            {
                BoundMainRep();
            }
        }

        public void BoundMainRep()
        {
            List<int> userGroup = new List<int>();
            List<int> permissions = new List<int>();
            List<MenuModel> MainMenus = new List<MenuModel>();
            userGroup = iUserServ.GetUserGroup(PortalUser.ID);
            permissions = iUserRoleMenuServ.GetAll(t => userGroup.Contains(t.UserTypeGroupID) && t.PermissionType == UserGroupPermissionType.MenuView).Select(x => x.NikMenuID).ToList();
            var Menus = iNikMenuServ.GetAll(x => x.Enabled && permissions.Contains(x.ID), t => new MenuModel { MenuItem = t, ItemChilds = t.Childs }).OrderBy(x => x.MenuItem.Ordering).ToList();
            foreach (MenuModel item in Menus)
            {
                if (item.MenuItem.ParentID == null)
                {
                    MainMenus.Add(item);
                }

            }
            RepMains.DataSource = MainMenus;
            RepMains.DataBind();

            int ItemCount = RepMains.Items.Count;
            int userGroupid = PortalUser.ID;
            for (int i = 0; i < ItemCount; i++)
            {
                HiddenField hidItemID = RepMains.Items[i].FindControl("HidItemID") as HiddenField;

                int ItemID;
                if (!int.TryParse(hidItemID.Value, out ItemID))
                {
                    Notification.SetErrorMessage("Menu Item ID is Wrong!!!");
                }

                //Submenu Data bound
                Repeater RepChilds = RepMains.Items[i].FindControl("RepChilds") as Repeater;
                var ItemSubMenus = iNikMenuServ.GetAll(x => x.ParentID == ItemID && permissions.Contains(x.ID), t => new MenuModel { MenuItem = t, ItemChilds = t.Childs }).OrderBy(x => x.MenuItem.Ordering).ToList();
                RepChilds.DataSource = ItemSubMenus;
                RepChilds.DataBind();
            }
        }

        protected string GetLink(object URI)
        {
            string iLink = URI.ToString().ToLower();
            if (string.IsNullOrEmpty(iLink))
            {
                return string.Empty;
            }
            else if (iLink.StartsWith("www."))
            {
                return "http://" + iLink;
            }
            else if (iLink.StartsWith("http://") || iLink.StartsWith("https://") || iLink.StartsWith("mms://") || iLink.StartsWith("ftp://"))
            {
                return iLink;
            }
            else
            {
                return ResolveUrl("~/" + iLink);
            }
        }

    }

    public class MenuModel
    {
        public NikMenu MenuItem { get; set; }
        public ICollection<NikMenu> ItemChilds { get; set; }
    }
}