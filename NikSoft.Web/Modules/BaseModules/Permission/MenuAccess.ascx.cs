using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Permission
{
    public partial class MenuAccess : WidgetUIContainer
    {
        public IUserTypeGroupService iUserTypeGroupService { get; set; }
        public INikMenuService iNikMenuServ { get; set; }
        public IUserRoleMenuService iUserRoleMenuServ { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBoundDDl();
                BoundMainRep();
            }
        }

        private List<MenuModel> menus = new List<MenuModel>();

        public void BoundMainRep()
        {
            var MainMenus = iNikMenuServ.GetAll(x => x.Enabled, t => new MenuModel { MenuItem = t, ItemChilds = t.Childs }).ToList();
            foreach (var item in MainMenus)
            {
                if (item.MenuItem.ParentID == null)
                {
                    menus.Add(item);
                }

            }
            RepMains.DataSource = menus;
            RepMains.DataBind();

            int ItemCount = RepMains.Items.Count;
            int userGroupid = Convert.ToInt32(ddlUserGroups.SelectedValue);
            for (int i = 0; i < ItemCount; i++)
            {
                CheckBox chmenu = RepMains.Items[i].FindControl("chMenu") as CheckBox;
                HiddenField hidItemID = RepMains.Items[i].FindControl("HidItemID") as HiddenField;

                int ItemID;
                if (!int.TryParse(hidItemID.Value, out ItemID))
                {
                    Notification.SetErrorMessage("ID پاس داده شده اشتباه است!!!");
                }

                //Submenu Data bound
                Repeater RepChilds = RepMains.Items[i].FindControl("RepChilds") as Repeater;
                var ItemSubMenus = iNikMenuServ.GetAll(x => x.ParentID == ItemID, t => new MenuModel { MenuItem = t, ItemChilds = t.Childs });
                RepChilds.DataSource = ItemSubMenus;
                RepChilds.DataBind();


                if (userGroupid == 0)
                {
                    continue;
                }

                var AccessdMenu = iUserRoleMenuServ.Find(x => x.NikMenuID == ItemID && x.UserTypeGroupID == userGroupid);
                if (AccessdMenu != null)
                {
                    chmenu.Checked = true;
                }


                int ChildCount = RepChilds.Items.Count;
                for (int j = 0; j < ChildCount; j++)
                {
                    CheckBox chSubmenu = RepChilds.Items[j].FindControl("chSubMenu") as CheckBox;
                    HiddenField hidSubmenuID = RepChilds.Items[j].FindControl("HidSubitemID") as HiddenField;
                    int SubItemID;
                    if (!int.TryParse(hidSubmenuID.Value, out SubItemID))
                    {
                        Notification.SetErrorMessage("ID پاس داده شده اشتباه است!!!");
                    }
                    var AccessSubmenu = iUserRoleMenuServ.Find(x => x.NikMenuID == SubItemID && x.UserTypeGroupID == userGroupid);
                    if (AccessSubmenu != null)
                    {
                        chSubmenu.Checked = true;
                    }

                }
            }
        }

        public void DataBoundDDl()
        {
            var userGroups = iUserTypeGroupService.GetAll(x => true, t => new { t.ID, t.Title }).ToList();
            ddlUserGroups.FillControl(userGroups, "Title", "ID");
        }

        protected void ddlUserGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            BoundMainRep();
        }
    }

    public class MenuModel
    {
        public NikMenu MenuItem { get; set; }
        public ICollection<NikMenu> ItemChilds { get; set; }
    }
}