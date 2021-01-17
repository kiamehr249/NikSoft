using NikSoft.Model;
using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Widgets
{
    public partial class W_NikPanelMenu : WidgetUIContainer
    {

        public INikModuleService iNikModuleServ { get; set; }
        public IUserRoleModuleService iUserRoleModuleServ { get; set; }
        public INikMenuService NikMenuServ { get; set; }
        public IUserRoleMenuService iUserRoleMenuServ { get; set; }
        public IPortalService iPortalServ { get; set; }

        private List<int> userGroup;
        private List<UserRoleMenu> permissions;
        private List<menuModel> menus;

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
                if(thisUser.UserType != NikUserType.NikUser)
                {
                    RedirectTo("~/home/UserProfile");
                    return;
                }
            }

            LoadUserData();


            ltrMenu.Text = string.Empty;
            userGroup = iUserServ.GetUserGroup(PortalUser.ID);
            permissions = iUserRoleMenuServ.GetAll(t => userGroup.Contains(t.UserTypeGroupID) && t.PermissionType == UserGroupPermissionType.MenuView).ToList();
            menus = NikMenuServ.GetAll(t => t.Enabled, x => new menuModel { MenuItem = x, SubItems = x.Childs }).ToList();
            LoadMenu(null);
        }

        private void LoadMenu(int? parentID)
        {
            var data = menus.Where(t => t.MenuItem.ParentID == parentID).OrderBy(t => t.MenuItem.Ordering);
            foreach (var item in data)
            {
                var per = permissions.Where(t => t.NikMenuID == item.MenuItem.ID);
                if (per.Count() == 0)
                {
                    continue;
                }

                var Childs = menus.Where(t => t.MenuItem.ParentID == item.MenuItem.ID).OrderBy(t => t.MenuItem.Ordering);
                string ModuleLink = ("panel/" + ModuleName + "/" + ModuleParameters).ToLower();
                if (Childs.Count() > 0)
                {
                    if (null == item.MenuItem.ParentID)
                    {
                        string Class = item.SubItems.Any(t => t.Link.ToLower() == ModuleLink) ? "dropdown" : "";
                        ltrMenu.Text += "<li class='" + Class + "'>";
                        ltrMenu.Text += "<a class='parent dropdown - toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>" + item.MenuItem.AweSomeFontClass + "<span>" + item.MenuItem.Title + "</span></a>";
                        ltrMenu.Text += "<ul class='dropdown-menu'>";
                        LoadMenu(item.MenuItem.ID);
                        ltrMenu.Text += "</ul>";
                        ltrMenu.Text += "</li>";
                    }
                    else
                    {
                        string Class = item.SubItems.Any(t => t.Link.ToLower() == ModuleLink) ? "dropdown" : "";
                        ltrMenu.Text += "<li class='" + Class + " haschilds'>";
                        ltrMenu.Text += "<a class='parent dropdown - toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>" + item.MenuItem.AweSomeFontClass + "<span>" + item.MenuItem.Title + "</span></a>";
                        ltrMenu.Text += "<ul class='dropdown-menu'>";
                        LoadMenu(item.MenuItem.ID);
                        ltrMenu.Text += "</ul>";
                        ltrMenu.Text += "</li>";
                    }
                }
                else
                {

                    if (!string.IsNullOrEmpty(item.MenuItem.SubMenusQuery))
                    {
                        string Class = item.SubItems.Any(t => t.Link.ToLower() == ModuleLink) ? "active" : "";
                        ltrMenu.Text += "<li class='haschilds " + Class + "'>";
                        ltrMenu.Text += "<a class='parent'>" + item.MenuItem.AweSomeFontClass + "</span>" + item.MenuItem.Title + "</span></a>";
                        ltrMenu.Text += "<ul class=''>";
                        LoadDynamicSubMenu(item.MenuItem.SubMenusQuery);
                        ltrMenu.Text += "</ul>";
                        ltrMenu.Text += "</li>";
                    }
                    else
                    {
                        string Class = item.MenuItem.Link.ToLower() == ModuleLink ? "active" : "";
                        ltrMenu.Text += "<li class=\"" + Class + "\"" + "><a href=\"" + GetLink(item.MenuItem.Link) + "\"" + ">" + item.MenuItem.AweSomeFontClass + "<span>" + item.MenuItem.Title + "</span></a></li>";
                    }
                }
            }
        }


        private void LoadDynamicSubMenu(string subQuery)
        {
            subQuery = subQuery.Replace("[portalid]", PortalUser.PortalID.ToString());
            var allMenusDt = new DBUtilities().ExecuteCommand(subQuery);
            var rowCount = allMenusDt.Rows.Count;
            string ModuleLink = ("panel/" + ModuleName + "/" + ModuleParameters).ToLower();
            for (int i = 0; i < rowCount; i++)
            {
                string ItemClass = allMenusDt.Rows[i]["Link"].ToString().ToLower() == ModuleLink ? "active" : "";
                ItemClass = "dropdown" + ItemClass;
                ltrMenu.Text += "<li class=\"" + ItemClass + "\"" + " ><a href=\""
                    + GetLink(allMenusDt.Rows[i]["Link"]) + "\"" + ">" + allMenusDt.Rows[i]["Title"] + "</a></li>";
            }
        }






        private void LoadUserData()
        {
            if (PortalUser == null)
            {
                RedirectTo("~/home/page/default");
                return;
            }
            var user = iUserServ.Find(t => t.ID == PortalUser.ID);
            if (user == null)
            {
                return;
            }

            if (user.UserType != NikUserType.NikUser)
            {
                RedirectTo2("/", true);
                return;
            }

        }

        protected void lbtSingOut_Click(object sender, EventArgs e)
        {
            SingleSignOnService.SignOut();
            if (null != PortalUser)
            {
                var userPortalID = PortalUser.PortalID;
                var portal = iPortalServ.Find(x => x.ID == userPortalID);
                if (null == portal)
                    RedirectTo("~/");
                if (portal.Alias.ToLower() != "home")
                {
                    RedirectTo("~/" + portal.Alias);
                }
                else
                {
                    RedirectTo("~/");
                }
                return;
            }
            RedirectTo("~/");
        }


        protected string getBrandImage()
        {
            if (null != PortalUser)
            {
                var portal = iPortalServ.Find(x => x.ID == PortalUser.PortalID);
                if (portal.ShowOwnLogo)
                {
                    var TitleofPanel = NikSiteTitle;
                    this.Page.Title = NikSiteTitle;

                    return iNikSettingServ.GetSettingValue("SiteLogo", NikSettingType.SystemSetting, PortalUser.PortalID);
                }
            }
            return "images/logo-small.png";
        }


        private bool HaveEditHelpPermission()
        {
            if (null == PortalUser)
            {
                return false;
            }
            var userGroups = iUserServ.GetUserGroup(PortalUser.ID);

            var moduleIDs = iNikModuleServ.Entity.Where(rm => rm.ModuleKey == "helpeditor").Select(x => x.ID).ToList();
            var permissions = iUserRoleModuleServ.Entity.Where(x => moduleIDs.Contains(x.NikModuleID) && userGroups.Contains(x.UserTypeGroupID)).Select(x => new { x.NikModuleID, x.PermissionType }).ToList();

            if (permissions.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private class menuModel
        {
            public NikMenu MenuItem;
            public ICollection<NikMenu> SubItems;
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
}