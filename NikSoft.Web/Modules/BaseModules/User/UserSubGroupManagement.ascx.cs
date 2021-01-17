using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.User
{
    public partial class UserSubGroupManagement : NikUserControl
    {
        public IUserTypeService iUserTypeServ { get; set; }
        public IUserTypeGroupService iUserTypeGroupServ { get; set; }

        protected string SelectData = string.Empty;
        protected int UserID = 0;


        protected string ThisName = "";
        protected string ThisLastname = "";
        protected string ThisUsername = "";

        protected override void OnInit(EventArgs e)
        {
            ShowFunctionButton = false;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ModuleParameters.IsNumeric())
            {
                UserID = ModuleParameters.ToInt32();
            }
            if (UserID == 0)
            {
                RedirectTo("~/panel/rd_user/view");
                return;
            }
            if (!IsPostBack)
            {
                BindCombo();
            }

            getUserData(UserID);

            BoundData();
            FormValidation = Validation;
            btnSave.Click += SaveClick;
            SaveNewFunction = SaveNewData;
        }

        protected override void BoundData()
        {
            var query = iUserTypeGroupServ.ExpressionMaker();
            query.Add(t => t.Users.Any(a => a.ID == UserID));
            query.Add(t => t.UserType.PortalID == PortalUser.PortalID);
            base.FillManageFrom(iUserTypeGroupServ, query);
        }

        private void BindCombo()
        {
            var userUserType = iUserServ.GetUserType(UserID);
            ddlUserType.FillControl(iUserTypeServ.GetAll(t => t.PortalID == PortalUser.PortalID && userUserType.Contains(t.ID), t => new { t.ID, t.Title }).ToList(), "Title", "ID");
        }

        private void SaveNewData()
        {
            var userTypeID = ddlUserType.SelectedValue.ToInt32();
            var userGroupID = ddlUserTypeGroup.GetDropDownValue(Request.Form);
            var user = iUserServ.Find(t => t.ID == UserID);
            if (user.UserTypeGroups.Any(t => t.ID == userGroupID))
            {
                Notification.SetErrorMessage("دسته مورد نظر قبلا ثبت شده است");
                return;
            }
            var userGroup = iUserTypeGroupServ.Find(t => t.ID == userGroupID);
            user.UserTypeGroups.Add(userGroup);
            uow.SaveChanges(PortalUser.ID);
            this.ClearForm();
            BoundData();
        }

        protected void bd_Click(object sender, EventArgs e)
        {
            string del1;
            try
            {
                if (null != Request.Form["ch1"])
                {
                    del1 = Request.Form["ch1"].ToString();
                    List<int> l = del1.Split(',').ToList().ConvertAll(x => int.Parse(x));
                    var deletedItems = iUserTypeGroupServ.GetAll(x => l.Contains(x.ID)).ToList();
                    var user = iUserServ.Find(t => t.ID == UserID);
                    foreach (var item in deletedItems)
                    {
                        user.UserTypeGroups.Remove(item);
                    }
                    uow.SaveChanges(PortalUser.ID);
                    Notification.SetSuccessMessage("آیتم های انتخاب شده حذف شدند.");
                }
            }
            catch
            {
                Notification.SetErrorMessage("حذف انجام نشد.");
                iUserServ.Reaload();
            }
            finally
            {
                this.ClearForm();
                BoundData();
            }
        }

        private bool Validation()
        {
            if (ddlUserType.SelectedIndex == 0)
            {
                ErrorMessage.Add("دسته کاربری را انتخاب کنید.");
            }
            var groupID = ddlUserTypeGroup.GetDropDownValue(Request.Form);
            SelectData = "GetUserGroup(" + ddlUserType.SelectedValue.ToInt32() + "," + groupID + ");\n";
            if (groupID == 0)
            {
                ErrorMessage.Add("گروه کاربری را انتخاب کنید.");
            }
            if (ErrorMessage.Count > 0)
            {
                return false;
            }
            return true;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            switch (ModuleName.ToLower())
            {
                case "usersubgroupmanagement":
                    RedirectTo("~/panel/rd_user/view");
                    break;
                case "usergroupforummanage":
                    RedirectTo("~/panel/rd_userforum/view");
                    break;

            }
        }

        public void getUserData(int id)
        {
            var CurrentUser = iUserServ.Find(x => x.ID == id);
            ThisName = CurrentUser.FirstName;
            ThisLastname = CurrentUser.LastName;
            ThisUsername = CurrentUser.UserName;
        }

    }
}