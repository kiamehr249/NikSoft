using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Template
{
    public partial class rd_Template : NikUserControl
    {
        public IPortalService iPortalServ { get; set; }
        public ITemplateService iTemplateServ { get; set; }
        public IWidgetService iWidgetServ { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCombo();
            }
            BoundData();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Container.btnDelete.Click += BtnDelete_Click;
        }

        private void BindCombo()
        {
            ddlPortal.FillControl(iPortalServ.GetAll(t => true, t => new { t.ID, t.Title }).ToList(), "Title", "ID");
            if (PortalUser.PortalID != 1)
            {
                ddlPortal.Enabled = false;
            }
            ddlPortal.SelectedValue = PortalUser.PortalID.ToString();
        }

        protected override void BoundData()
        {
            var query = iTemplateServ.ExpressionMaker();
            if (!txtName.Text.IsEmpty())
            {
                query.Add(t => t.Title.Contains(txtName.Text.Trim()));
            }
            if (ddlTemplateType.SelectedIndex > 0)
            {
                var typeID = (TemplateType)ddlTemplateType.SelectedValue.ToInt32();
                query.Add(t => t.Type == typeID);
            }
            if (ddlPortal.SelectedIndex > 0)
            {
                var portalID = ddlPortal.SelectedValue.ToInt32();
                query.Add(t => t.PortalID == portalID);
            }
            if (PortalUser.PortalID != 1)
                query.Add(t => t.PortalID == PortalUser.PortalID);
            if (PortalUser.PortalID == 1)
                query.Add(t => true);
            base.FillManageFrom(iTemplateServ, query);
        }

        protected string GetPageTemplateType(TemplateType ptt)
        {
            return iTemplateServ.GetTemplateType(ptt);
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
            if (null != Request.Form["ch1"])
            {
                string del1 = Request.Form["ch1"].ToString();
                List<int> l = del1.Split(',').ToList().ConvertAll(x => int.Parse(x));
                var deletedItems = iTemplateServ.GetAll(x => l.Contains(x.ID)).ToList();
                foreach (var item in deletedItems)
                {
                    var Widgets = iWidgetServ.GetAll(t => t.TemplateID == item.ID);
                    if (Widgets.Count > 0)
                        ErrorMessage.Add(" Can not Remove the «<strong>" + item.Title + "<strong>» because have items ");
                    else
                    {
                        iTemplateServ.Remove(item);
                        uow.SaveChanges();
                    }
                }
                if (ErrorMessage.Count > 0)
                {
                    Notification.SetErrorMessage(ErrorMessage);
                }
                Cache.Remove("Templates");
                BoundData();
            }
        }

        public string SetImg(bool IsSelected)
        {
            return (IsSelected) ? "../../../images/selected.png" : "../../../images/nselected.png";
        }

        protected void GV1_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "removeall")
            {
                //try {
                var pageID = e.CommandArgument.ToString().ToInt32();
                var Widgets = iWidgetServ.GetAll(t => t.TemplateID == pageID);
                if (null == Widgets || 0 == Widgets.Count)
                {
                    Notification.SetOptionalMessage("Dont have any widget to remove", MessageType.Information, Layout.Top);
                    return;
                }
                var count = Widgets.Count;
                iWidgetServ.Remove(Widgets);
                Notification.SetSuccessMessage(count + " widgets remove");
                uow.SaveChanges();
                //} catch {
                //}
            }
            else if (e.CommandName.ToLower() == "pubunpub")
            {
                var id = Convert.ToInt32(e.CommandArgument.ToString());
                iTemplateServ.SetSelected(id);
                Cache.Remove("Templates");
                BoundData();
            }
            else if (e.CommandName.ToLower() == "copyto")
            {
                var id = Convert.ToInt32(e.CommandArgument.ToString());
                RedirectTo("~/" + Level + "/copytemplate/" + id);
                return;

            }


            //
        }
    }
}