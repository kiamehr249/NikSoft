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
    public partial class ModuleAccess : NikUserControl
    {
        public IUserRoleModuleService iUserRoleModuleService { get; set; }
        public IUserTypeGroupService iUserTypeGroupService { get; set; }
        public IUserService iUserService { get; set; }
        public INikModuleService iNikModuleService { get; set; }
        public INikModuleDefinitionService iNikModuleDefinitionService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindComboUser();
                BindComboModuleCat();
            }

            DataBoundDeAccess();
            DataAccessBound();

        }

        protected void BtnAddAccess_Click(object sender, System.EventArgs e)
        {
            if (Convert.ToInt32(DDLUserTypeCat.SelectedValue) == 0)
            {
                Notification.SetErrorMessage("گروه کاربری را انتخاب کنید.");
                return;
            }

            if (null != Request.Form["chAcc"])
            {
                var Checks = GetItemCkecked("chAcc");
                var SelectedModules = iNikModuleService.GetAll(x => Checks.Contains(x.ID), x => new { x.ID, x.ModuleKey });
                foreach (var item in SelectedModules)
                {
                    var query = iUserRoleModuleService.Create();
                    string ModuleType = item.ModuleKey.Substring(0, 2).ToLower();
                    if (ModuleType == "cu")
                    {
                        query.PermissionType = UserGroupPermissionType.CreateAndUpdate;
                    }
                    else if (ModuleType == "rd")
                    {
                        query.PermissionType = UserGroupPermissionType.ReadAndDelete;
                    }
                    else
                    {
                        query.PermissionType = UserGroupPermissionType.None;
                    }
                    query.NikModuleID = item.ID;
                    query.UserTypeGroupID = Convert.ToInt32(DDLUserTypeCat.SelectedValue);
                    iUserRoleModuleService.Add(query);
                }

                iUserRoleModuleService.SaveChanges();

                DataBoundDeAccess();
                DataAccessBound();
                Notification.SetSuccessMessage("به موارد انتخاب شده دسترسی داده شد.");
            }
            else
            {
                Notification.SetErrorMessage("حداقل یکی از آیتم ها را انتخاب کنید.");
            }
        }


        protected void BtnRemoveAccess_Click(object sender, EventArgs e)
        {
            int UserCatID = Convert.ToInt32(DDLUserTypeCat.SelectedValue);
            if (UserCatID == 0)
            {
                Notification.SetErrorMessage("گروه کاربری را انتخاب کنید.");
                return;
            }

            if (null != Request.Form["chDAcc"])
            {
                var Checks = GetItemCkecked("chDAcc");

                var DeleteItems = iUserRoleModuleService.GetAll(x => Checks.Contains(x.NikModuleID) && x.UserTypeGroupID == UserCatID);
                iUserRoleModuleService.Remove(DeleteItems);
                iUserRoleModuleService.SaveChanges();
                Notification.SetSuccessMessage("آیتم های انتخاب شده منع دسترسی شدند.");
            }
            else
            {
                Notification.SetErrorMessage("حداقل یکی از آیتم ها را انتخاب کنید.");
            }

            DataAccessBound();
            DataBoundDeAccess();
        }

        protected List<int> GetItemCkecked(string chName)
        {
            string CheckedItems;
            CheckedItems = Request.Form[chName].ToString();
            List<int> Checks = CheckedItems.Split(',').ToList().ConvertAll(x => int.Parse(x));
            return Checks;
        }

        protected void BindComboUser()
        {
            var query = iUserTypeGroupService.GetAll(x => true, x => new { x.Title, x.ID });
            DDLUserTypeCat.FillControl(query.ToList(), "Title", "ID", true, true);
        }

        protected void BindComboModuleCat()
        {
            var query = iNikModuleDefinitionService.GetAll(x => true, x => new { x.Title, x.ID }).ToList();
            DDLModulsCategory.FillControl(query, "Title", "ID", true, true);
        }

        protected void DataBoundDeAccess()
        {
            int selectedCatID = Convert.ToInt32(DDLModulsCategory.SelectedValue);
            int selectedUserGroup = Convert.ToInt32(DDLUserTypeCat.SelectedValue);
            var Accessed = iUserRoleModuleService.GetAll(x => x.NikModule.ModuleDefinitionID == selectedCatID && x.UserTypeGroupID == selectedUserGroup, y => new { y.NikModuleID }).Select(x => x.NikModuleID).ToList();
            if (Accessed != null)
            {
                var AllModules1 = iNikModuleService.GetAll(x => !Accessed.Contains(x.ID) && x.ModuleDefinitionID == selectedCatID && !x.ShowAsModule && x.LoginRequired).ToList();
                GVDeAccess.DataSource = AllModules1;
                GVDeAccess.DataBind();
            }
            else
            {
                var AllModules = iNikModuleService.GetAll(x => x.ModuleDefinitionID == selectedCatID && x.ShowAsModule && x.LoginRequired).ToList();
                GVDeAccess.DataSource = AllModules;
                GVDeAccess.DataBind();
            }


        }

        protected void DataAccessBound()
        {
            int selectedUserCat = Convert.ToInt32(DDLUserTypeCat.SelectedValue);
            int selectedModulDef = Convert.ToInt32(DDLModulsCategory.SelectedValue);
            var Accessed = iUserRoleModuleService.GetAll(x => x.UserTypeGroupID == selectedUserCat && x.NikModule.ModuleDefinitionID == selectedModulDef);
            var query = iUserRoleModuleService.ExpressionMaker();
            query.Add(x => x.UserTypeGroupID == selectedUserCat && x.NikModule.ModuleDefinitionID == selectedModulDef);
            this.FillManageFrom(iUserRoleModuleService, query,(x => x.NikModuleID), gridID: "GVAccess");
        }

        protected void DDLModulsCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBoundDeAccess();
        }

        protected void DDLUserTypeCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataAccessBound();
        }


    }
}