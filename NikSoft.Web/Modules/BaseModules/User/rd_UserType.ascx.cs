using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.User
{
    public partial class rd_UserType : NikUserControl
    {
        public IUserTypeService iUserTypeServ { get; set; }

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
            var query = iUserTypeServ.ExpressionMaker();
            query.Add(t => t.PortalID == PortalUser.PortalID);
            if (!txtTitle.Text.IsEmpty())
            {
                query.Add(t => t.Title.Contains(txtTitle.Text.Trim()));
            }
            base.FillManageFrom(iUserTypeServ, query);
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
                    var deletedItems = iUserTypeServ.GetAll(x => l.Contains(x.ID)).ToList();
                    iUserTypeServ.Remove(deletedItems);
                    uow.SaveChanges();
                    Notification.SetSuccessMessage("آیتم ها با موفقیت حذف شدند.");
                }
            }
            catch
            {
                Notification.SetErrorMessage("حذف آیتم ها انجام نشد.");
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
    }
}