using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;

namespace NikSoft.ContentManager.Web.Widget
{
    public partial class W_ContentInfoByParam : WidgetUIContainer
    {
        public IContentGroupService iContentGroupServ { get; set; }
        public IPublicContentService iPublicContentServ { get; set; }
        public IContentCategoryService iContentCategoryServ { get; set; }

        protected ContentCategory thisCat;
        protected ContentGroup thisGroup;
        protected PublicContent thisConetnt;

        protected int contentType;

        protected void Page_Load(object sender, EventArgs e)
        {
            Dataload();
        }

        public void Dataload()
        {
            contentType = Convert.ToInt32(base.GetConfigurationValue("contentType"));
            if (ModuleParameters == null || ModuleParameters == "default")
            {
                return;
            }

            if (ModuleParameters.IsNumeric())
            {
                getInfo(contentType, true);
            }
            else
            {
                getInfo(contentType, false);
            }

        }


        protected void getInfo(int ctype, bool isNum)
        {
            int currentID = 0;
            if (isNum)
            {
                currentID = ModuleParameters.ToInt32();
            }

            switch (ctype)
            {
                case 1:
                    if (isNum)
                    {
                        thisGroup = iContentGroupServ.Find(x => x.ID == currentID);
                    }
                    break;
                case 2:
                    if (isNum)
                    {
                        thisCat = iContentCategoryServ.Find(x => x.ID == currentID);
                    }
                    else
                    {
                        thisCat = iContentCategoryServ.Find(x => x.ModuleKey == ModuleParameters);
                    }
                    break;
                case 3:
                    if (isNum)
                    {
                        thisConetnt = iPublicContentServ.Find(x => x.ID == currentID);
                    }
                    break;
            }
        }

    }
}