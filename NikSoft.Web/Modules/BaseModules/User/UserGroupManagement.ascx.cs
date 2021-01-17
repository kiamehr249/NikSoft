using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.User
{
    public partial class UserGroupManagement : NikUserControl
    {
        public IUserTypeService iUserTypeServ { get; set; }
        private int UserID = 0;

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

            hlut.NavigateUrl = "/panel/UserSubGroupManagement/" + UserID;

            getUserData(UserID);
            BoundData();
            FormValidation = Validation;
            btnSave.Click += SaveClick;
            SaveNewFunction = SaveNewData;
        }

        protected override void BoundData()
        {
            var query = iUserTypeServ.ExpressionMaker();
            query.Add(t => t.Users.Any(a => a.ID == UserID));
            query.Add(t => t.PortalID == PortalUser.PortalID);
            base.FillManageFrom(iUserTypeServ, query);
        }

        private void BindCombo()
        {
            var userUserType = iUserServ.GetUserType(UserID);
            ddlUserType.FillControl(iUserTypeServ.GetAll(t => t.PortalID == PortalUser.PortalID && !userUserType.Contains(t.ID), t => new { t.ID, t.Title }).ToList(), "Title", "ID");
            if (ModuleName.ToLower() != "usergroupmanagement")
                ddlUserType.Enabled = false;
        }

        private void SaveNewData()
        {
            var userTypeID = ddlUserType.SelectedValue.ToInt32();
            var user = iUserServ.Find(t => t.ID == UserID);
            if (user.UserTypes.Any(t => t.ID == userTypeID))
            {
                Notification.SetErrorMessage("دسته مورد نظر قبلا ثبت شده است");
                return;
            }
            var userType = iUserTypeServ.Find(t => t.ID == userTypeID);
            user.UserTypes.Add(userType);
            uow.SaveChanges(PortalUser.ID);
            BindCombo();
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
                    var deletedItems = iUserTypeServ.GetAll(x => l.Contains(x.ID)).ToList();
                    var user = iUserServ.Find(t => t.ID == UserID);
                    foreach (var item in deletedItems)
                    {
                        user.UserTypes.Remove(item);
                    }
                    uow.SaveChanges(PortalUser.ID);
                    Notification.SetSuccessMessage("حذف با موفقیت انجام شد");
                }
            }
            catch
            {
                Notification.SetErrorMessage("اشکال در بازیابی رکورد مورد نظر بوجود آمده است لطفا علت را بررسی کنید");
                iUserServ.Reaload();
            }
            finally
            {
                BindCombo();
                BoundData();
            }
        }

        private bool Validation()
        {
            if (ddlUserType.SelectedIndex == 0)
            {
                ErrorMessage.Add("دسته کاربری را انتخاب نمایید");
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
                case "usertypemanage":
                    RedirectTo("~/dashboard/rd_user/view");
                    break;

                case "usertypeforummanage":
                    RedirectTo("~/dashboard/rd_userforum/view");
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