using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Widget
{
    public partial class W_CategoryContents : WidgetUIContainer
    {
        public IPublicContentService iPublicContentServ { get; set; }
        protected int TitleLength = 200;
        protected int LeadLength = 200;
        protected void Page_Load(object sender, EventArgs e)
        {
            Dataload();
        }

        public void Dataload()
        {
            int catid = Convert.ToInt32(base.GetConfigurationValue("category"));
            int countOfData = Convert.ToInt32(base.GetConfigurationValue("count"));
            bool lastorder = Convert.ToBoolean(base.GetConfigurationValue("order"));
            TitleLength = Convert.ToInt32(base.GetConfigurationValue("titlelength"));
            LeadLength = Convert.ToInt32(base.GetConfigurationValue("leadlength"));
            List<PublicContent> data;
            if (lastorder)
            {
                data = iPublicContentServ.GetAll(x => x.CategoryID == catid && x.Enabled && x.PortalID == CurrentPortalID).Take(countOfData).OrderByDescending(x => x.ID).ToList();
            }
            else
            {
                data = iPublicContentServ.GetAll(x => x.CategoryID == catid && x.Enabled && x.PortalID == CurrentPortalID).Take(countOfData).OrderBy(x => x.Ordering).ToList();
            }
            RepContents.DataSource = data;
            RepContents.DataBind();
        }

    }
}