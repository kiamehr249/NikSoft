using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.UI
{
    public partial class ContentGroupView : NikUserControl
    {
        public IContentGroupService iContentGroupServ { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void LoadData()
        {
            var AllGroups = iContentGroupServ.GetAll(x => x.PortalID == CurrentPortalID && x.Enabled).ToList();
            RepGroup.DataSource = AllGroups;
            RepGroup.DataBind();
        }


    }
}