using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Template
{
    public partial class TemplateWidgets : TemplateCore
    {
        public IWidgetDefinitionService iWidgetDefinitionServ { get; set; }

        private const string ADMIN_WIDGET_CONTAINER_PATH = "~/Modules/BaseModules/Template/PanelWidgetContainer.ascx";

        protected int PageID = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (ModuleParameters.IsNumeric())
            {
                PageID = ModuleParameters.ToInt32();
            }
            else
            {
                PageID = 0;
                return;
            }
            if (!IsPostBack)
            {
                BindCombo();
                LoadWidget();
            }
            var x = iTemplateServ.GetAll(pt => pt.ID == PageID, p => new { p.ID, p.Direction, p.TemplateName });

            if (null != x && x.Count() == 1)
            {
                var z = x.First();
                PageID = z.ID;
                _pagedirection = z.Direction;
                controlFileName = z.TemplateName;
                plc.Controls.Clear();
                BuildSkin(plc);
                if (PageID > 0)
                {
                    FillPlaceHolders(this.Controls);
                    this.SetupWidgets(PageID, ADMIN_WIDGET_CONTAINER_PATH, AdminorUi.AdminPart);
                }
            }
        }

        public void BindCombo()
        {
            if (PortalUser.PortalID != 1)
                ddlTemp.FillControl(iTemplateServ.GetAll(t => t.PortalID == PortalUser.PortalID, t => new { t.Title, t.ID }).ToList(), "Title", "ID", defaultValue: "");
            else
                ddlTemp.FillControl(iTemplateServ.GetAll(t => true, t => new { t.Title, t.ID }).ToList(), "Title", "ID", defaultValue: "");
        }

        private void LoadWidget()
        {
            plcWidgets.Controls.Clear();
            var list = iWidgetDefinitionServ.GetAll(t => true).OrderBy(t => t.Title).ToList();
            foreach (var item in list)
            {
                plcWidgets.Controls.Add(new LiteralControl("<a href='#' onclick='return false;' class='widgetItem btn btn-default btn-widget' data-wid='" + item.ID + "'><button  type='button' id='showdetail_" + item.ID + "' data-toggle='modal' data-target='#exampleModal' data-whatever='@mdo'><img id='iconwidget" + item.ID + "' data-img='" + item.Image + "' data-desc='" + item.Description + "' title='" + item.Title + "' src='/images/widget-icon.png' /> </button> " + item.Title + "</a>"));
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/panel/rd_Template");
        }

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            if (ddlTemp.SelectedIndex <= 0)
            {
                Notification.SetErrorMessage("تمپلیت مورد نظر را انتخاب نمایید");
                return;
            }
            var PageTemplateID = ddlTemp.SelectedValue.ToInt32();
            var t = iWidgetServ.GetAll(x => x.TemplateID == PageTemplateID);
            foreach (var item in t)
            {
                var w = iWidgetServ.Create();
                w = item;
                w.TemplateID = PageID;
                iWidgetServ.Add(w);
                uow.SaveChanges();
            }
            RedirectTo("~/panel/ViewTemplateWidgets/" + PageID);
        }
    }
}