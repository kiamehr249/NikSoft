using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.User
{
    public partial class rd_UserTypeGroup : NikUserControl
    {
        public IUserTypeService iUserTypeServ { get; set; }
        public IUserTypeGroupService iUserTypeGroupServ { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Container.btnDelete.Click += BtnDelete_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCombo();
            }
            BoundData();
        }

        private void BindCombo()
        {
            ddlUserType.FillControl(iUserTypeServ.GetAll(t => true, t => new { t.ID, t.Title }).ToList(), "Title", "ID");
        }

        protected override void BoundData()
        {

            var query = iUserTypeGroupServ.ExpressionMaker();
            query.Add(t => t.UserType.PortalID == PortalUser.PortalID);

            if (!txtTitle.Text.IsEmpty())
            {
                query.Add(t => t.Title.Contains(txtTitle.Text.Trim()));
            }
            if (ddlUserType.SelectedIndex > 0)
            {
                var userTypeID = ddlUserType.SelectedValue.ToInt32();
                query.Add(t => t.UserTypeID == userTypeID);
            }

            base.FillManageFrom(iUserTypeGroupServ, query);
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
                    var deletedItems = iUserTypeGroupServ.GetAll(x => l.Contains(x.ID)).ToList();
                    iUserTypeGroupServ.Remove(deletedItems);
                    uow.SaveChanges();
                    Notification.SetSuccessMessage("حذف آیتم ها با موفقیت حذف شد.");
                }
            }
            catch (Exception ex)
            {
                var z = "Remove faild<br />";
                z += ex.Message;
                if (null != ex.InnerException)
                {
                    z += "<br />" + ex.InnerException.Message;
                }
                if (null != ex.InnerException && null != ex.InnerException.InnerException)
                {
                    z += "<br />" + ex.InnerException.InnerException.Message;
                }
                Notification.SetErrorMessage(z);
                iUserTypeServ.Reaload();
            }
            finally
            {
                this.BoundData();
            }
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
    }
}