using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.User
{
    public partial class rd_User : NikUserControl
    {
        public IUserTypeService iUserTypeService { get; set; }
        public IUserTypeGroupService iUserTypeGroupServ { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Container.btnDelete.Click += BtnDelete_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BoundData();
        }

        protected override void BoundData()
        {
            var query = iUserServ.ExpressionMaker();
            query.Add(x => x.UserType == NikUserType.NikUser);
            if (PortalUser.PortalID != 1)
            {
                query.Add(x => x.PortalID == PortalUser.PortalID);
            }

            //dont show user it self
            if (PortalUser.ID != 1)
            {
                query.Add(x => x.ID != PortalUser.ID);
            }

            if (ddlStatus.SelectedIndex > 0)
            {
                var today = DateTime.Now;
                if (ddlStatus.SelectedValue == "1")
                {
                    query.Add(x => today.CompareTo(x.PassExpireDate) <= 0 || x.UnExpire);
                }
                else if (ddlStatus.SelectedValue == "2")
                {
                    query.Add(x => today.CompareTo(x.PassExpireDate) > 0 && x.UnExpire == false);
                }
            }

            if (!txtUsername.Text.IsEmpty())
            {
                query.Add(x => x.UserName.Contains(txtUsername.Text.Trim()));
            }

            if (!txtName.Text.IsEmpty())
            {
                query.Add(x => x.FirstName.Contains(txtName.Text.Trim()) || x.LastName.Contains(txtName.Text.Trim()));
            }

            if (ddlLock.SelectedIndex > 0)
            {
                var isLock = Convert.ToBoolean(Convert.ToInt32(ddlLock.SelectedValue) - 1);
                query.Add(x => x.IsLock == isLock);
            }

            if (query.Count == 0)
            {
                query.Add(t => true);
            }
            base.FillManageFrom(iUserServ, query);
        }

        protected void breset_Click(object sender, EventArgs e)
        {
            this.ClearForm();
            BoundData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BoundData();
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            string del1;
            try
            {
                if (null != Request.Form["ch1"])
                {
                    del1 = Request.Form["ch1"].ToString();
                    List<int> l = del1.Split(',').ToList().ConvertAll(x => int.Parse(x));
                    var deletedItems = iUserServ.GetAll(x => l.Contains(x.ID)).ToList();
                    iUserServ.Remove(deletedItems);
                    uow.SaveChanges();
                    Notification.SetSuccessMessage("آیتم ها با موفقیت حذف شدند.");
                }
                else
                {
                    Notification.SetErrorMessage("حداقل یک آیتم را انتخاب کنید.");
                }
            }
            catch
            {
                Notification.SetErrorMessage("حذف آیتم ها انجام نشد.");
                iUserServ.Reaload();
            }
            finally
            {
                this.BoundData();
            }
        }

        public string GetUrlUserType(int ID)
        {
            string Url = "";
            switch (ModuleName.ToLower())
            {
                case "rd_user":
                    Url = "~/panel/UserGroupManagement/" + ID;
                    break;
                case "rd_userforum":
                    Url = "~/panel/usertypeforummanage/" + ID;
                    break;
            }
            return Url;
        }

        public string GetUrlUserGroup(int ID)
        {
            string Url = "";
            switch (ModuleName.ToLower())
            {
                case "rd_user":
                    Url = "~/panel/UserSubGroupManagement/" + ID;
                    break;
                case "rd_userforum":
                    Url = "~/panel/usergroupforummanage/" + ID;
                    break;
            }
            return Url;
        }
    }
}