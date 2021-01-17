using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Portal
{
    public partial class rd_Portal : NikUserControl
    {
        public IPortalService iPortalServ { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            BoundData();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Container.btnDelete.Click += BtnDelete_Click;
        }

        protected override void BoundData()
        {
            var query = iPortalServ.ExpressionMaker();
            if (PortalUser.PortalID != 1)
            {
                query.Add(t => t.ParentID == PortalUser.PortalID);
            }
            if (!txtTitle.Text.IsEmpty())
            {
                query.Add(t => t.Title.Contains(txtTitle.Text.Trim()));
            }
            if (query.Count == 0)
            {
                query.Add(t => true);
            }
            base.FillManageFrom(iPortalServ, query);
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
                    var deletedItems = iPortalServ.GetAll(x => l.Contains(x.ID)).ToList();
                    foreach (var item in deletedItems) {
                        if (!item.Favicon.IsEmpty())
                        {
                            Utilities.Utilities.RemoveItemFile(item.Favicon);
                        }
                    }
                    iPortalServ.Remove(deletedItems);
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BoundData();
        }

        protected void breset_Click(object sender, EventArgs e)
        {
            this.ClearForm();
            BoundData();
        }
    }
}