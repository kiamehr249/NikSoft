using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Widget
{
    public partial class W_ContentByParam : WidgetUIContainer
    {
        public IPublicContentService iPublicContentServ { get; set; }

        protected List<PublicContent> ContentList;
        protected int TitleLength = 200;
        protected int LeadLength = 200;

        protected int contentType;

        protected void Page_Load(object sender, EventArgs e)
        {
            Dataload();
        }

        public void Dataload()
        {
            contentType = Convert.ToInt32(base.GetConfigurationValue("contentType"));
            int countOfData = Convert.ToInt32(base.GetConfigurationValue("count"));
            bool lastorder = Convert.ToBoolean(base.GetConfigurationValue("order"));
            TitleLength = Convert.ToInt32(base.GetConfigurationValue("titlelength"));
            LeadLength = Convert.ToInt32(base.GetConfigurationValue("leadlength"));

            if (ModuleParameters == null || ModuleParameters == "default")
            {
                return;
            }

            if (ModuleParameters.IsNumeric())
            {
                int cid = ModuleParameters.ToInt32();
                var thisCont = iPublicContentServ.Find(x => x.ID == cid, y => new { y.ID, y.CategoryID, y.GroupID });
                if (contentType == 1)
                {
                    if (lastorder)
                        ContentList = iPublicContentServ.GetAll(x => x.PortalID == CurrentPortalID && x.Enabled && x.GroupID == thisCont.GroupID && x.ID != thisCont.ID).Take(countOfData).OrderByDescending(x => x.ID).ToList();
                    else
                        ContentList = iPublicContentServ.GetAll(x => x.PortalID == CurrentPortalID && x.Enabled && x.GroupID == thisCont.GroupID && x.ID != thisCont.ID).Take(countOfData).OrderBy(x => x.Ordering).ToList();

                }
                else if (contentType == 2)
                {
                    if (lastorder)
                        ContentList = iPublicContentServ.GetAll(x => x.PortalID == CurrentPortalID && x.Enabled && x.CategoryID == thisCont.CategoryID && x.ID != thisCont.ID).Take(countOfData).OrderByDescending(x => x.ID).ToList();
                    else
                        ContentList = iPublicContentServ.GetAll(x => x.PortalID == CurrentPortalID && x.Enabled && x.CategoryID == thisCont.CategoryID && x.ID != thisCont.ID).Take(countOfData).OrderBy(x => x.Ordering).ToList();
                }

            }

            RepParam.DataSource = ContentList;
            RepParam.DataBind();

        }

    }
}