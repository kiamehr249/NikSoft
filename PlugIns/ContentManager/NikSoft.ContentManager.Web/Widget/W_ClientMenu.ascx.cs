using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Widget
{
    public partial class W_ClientMenu : WidgetUIContainer
    {
        public IGeneralMenuService iGeneralMenuServ { get; set; }
        public IGeneralMenuItemService iGeneralMenuItemServ { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            Dataload();
        }

        public void Dataload()
        {
            int MenuID = Convert.ToInt32(base.GetConfigurationValue("menu"));
            var data = iGeneralMenuItemServ.GetAll(x => x.GeneralMenuID == MenuID && x.Enabled && x.ParentID == null).OrderBy(x => x.Ordering);
            RepMains.DataSource = data.ToList();
            RepMains.DataBind();
        }
    }
}