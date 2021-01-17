using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Theme
{
    public partial class rd_Theme : NikUserControl
    {

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

            var query = iThemeServ.ExpressionMaker();
            if (!chbAllPortals.Checked)
            {
                query.Add(t => t.PortalID == PortalUser.PortalID);
            }
            if (query.Count == 0)
            {
                query.Add(t => true);
            }
            base.FillManageFrom(iThemeServ, query);
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
                    var deletedItems = iThemeServ.GetAll(x => l.Contains(x.ID) && x.PortalID == PortalUser.PortalID).ToList();
                    iThemeServ.Remove(deletedItems);
                    uow.SaveChanges();
                    Notification.SetSuccessMessage("Delete is success");
                }
            }
            catch
            {
                Notification.SetErrorMessage("Something is wrong");
                iUserServ.Reaload();
            }
            finally
            {
                this.BoundData();
            }
        }

        protected void GV1_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow gvr in GV1.Rows)
            {
                var labStatus = (HiddenField)gvr.FindControl("hf_iId");
                var ThisItems_pId = labStatus.Value.ToInt32();
                if (ThisItems_pId != PortalUser.PortalID)
                {
                    gvr.CssClass = "warning";
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BoundData();
        }
    }
}