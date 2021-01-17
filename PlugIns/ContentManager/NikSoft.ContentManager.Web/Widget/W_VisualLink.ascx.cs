using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Widget
{
    public partial class W_VisualLink : WidgetUIContainer
    {
        public IVisualLinkItemService iVisualLinkItemServ { get; set; }
        public IVisualLinkService iVisualLinkServ { get; set; }
        public VisualLink Parent;
        protected void Page_Load(object sender, EventArgs e)
        {
            Dataload();
        }

        public void Dataload()
        {
            int catid = Convert.ToInt32(base.GetConfigurationValue("category"));
            int countOfData = Convert.ToInt32(base.GetConfigurationValue("count"));
            bool lastorder = Convert.ToBoolean(base.GetConfigurationValue("order"));
            List<VisualLinkItem> data;
            Parent = iVisualLinkServ.Find(x => x.ID == catid);
            if (lastorder)
            {
                data = iVisualLinkItemServ.GetAll(x => x.VisualLinkID == catid && x.Enabled).Take(countOfData).OrderByDescending(x => x.ID).ToList();
            }
            else
            {
                data = iVisualLinkItemServ.GetAll(x => x.VisualLinkID == catid && x.Enabled).Take(countOfData).OrderBy(x => x.Ordering).ToList();
            }
            RepContents.DataSource = data;
            RepContents.DataBind();
            var Rep2 = FindControl("RepTow") as Repeater;
            if(Rep2 != null)
            {
                Rep2.DataSource = data;
                Rep2.DataBind();
            }
        }

    }
}