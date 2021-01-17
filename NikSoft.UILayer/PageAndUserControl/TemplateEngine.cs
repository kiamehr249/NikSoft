using NikSoft.NikModel;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NikSoft.UILayer
{
    public class TemplateEngine : TemplateCore
    {
        private const string WIDGET_CONTAINER = "~/Modules/BaseModules/EngineBase/WidgetContainer.ascx";
        protected void Page_Init(object sender, EventArgs e)
        {
            int PortalID = CurrentPortalID;
            if (null == Cache[TEMPLATE_CACHE])
            {
                var allTemplates = iTemplateServ.GetAll(x => true, x => new TemplateCache()
                {
                    Direction = x.Direction,
                    ID = x.ID,
                    TemplateName = x.TemplateName,
                    TemplateType = x.Type,
                    IsSelected = x.IsSelected,
                    PortalID = x.PortalID,
                    ModuleKey = x.ModuleKey.ToLower(),
                    ModuleParameter = x.ModuleParameter.ToLower()
                }).ToList();
                Cache.Insert(TEMPLATE_CACHE, allTemplates, null, DateTime.Now.AddDays(3), TimeSpan.Zero);
            }

            var allthemesFromCache = Cache[TEMPLATE_CACHE] as List<TemplateCache>;

            IEnumerable<TemplateCache> selectedPage;

            if (TemplateType == TemplateType.InnerPage)
            {
                selectedPage = allthemesFromCache
                                .Where(pt => pt.TemplateType == TemplateType && pt.PortalID == PortalID && pt.ModuleKey == ModuleName && pt.ModuleParameter == ModuleParameters)
                                .Select(p => new TemplateCache { ID = p.ID, Direction = p.Direction, TemplateName = p.TemplateName });
                if (selectedPage.Count() != 1)
                {
                    selectedPage = allthemesFromCache
                                .Where(pt => pt.TemplateType == TemplateType && pt.PortalID == PortalID && pt.ModuleKey == ModuleName && (pt.ModuleParameter == string.Empty || pt.ModuleParameter == null))
                                .Select(p => new TemplateCache { ID = p.ID, Direction = p.Direction, TemplateName = p.TemplateName });
                }
                if (selectedPage.Count() != 1)
                {
                    selectedPage = allthemesFromCache
                            .Where(pt => pt.TemplateType == TemplateType && pt.PortalID == PortalID && pt.IsSelected && (pt.ModuleKey == string.Empty || pt.ModuleKey == null))
                            .Select(p => new TemplateCache { ID = p.ID, Direction = p.Direction, TemplateName = p.TemplateName });
                }
            }
            else if (TemplateType == TemplateType.PanelHome || TemplateType == TemplateType.PanelInner)
            {
                if (null != PortalUser)
                {
                    selectedPage = allthemesFromCache
                                .Where(pt => pt.TemplateType == TemplateType && pt.PortalID == PortalUser.PortalID && pt.IsSelected)
                                .Select(p => new TemplateCache { ID = p.ID, Direction = p.Direction, TemplateName = p.TemplateName });
                    if (null == selectedPage || selectedPage.Count() != 1)
                    {
                        selectedPage = allthemesFromCache
                                .Where(pt => pt.TemplateType == TemplateType && pt.PortalID == PortalID && pt.IsSelected)
                                .Select(p => new TemplateCache { ID = p.ID, Direction = p.Direction, TemplateName = p.TemplateName });
                    }
                }
                else
                {
                    selectedPage = allthemesFromCache
                            .Where(pt => pt.TemplateType == TemplateType && pt.PortalID == PortalID && pt.IsSelected)
                            .Select(p => new TemplateCache { ID = p.ID, Direction = p.Direction, TemplateName = p.TemplateName });
                }
            }
            else
            {
                selectedPage = allthemesFromCache
                            .Where(pt => pt.TemplateType == TemplateType && pt.PortalID == PortalID && pt.IsSelected)
                            .Select(p => new TemplateCache { ID = p.ID, Direction = p.Direction, TemplateName = p.TemplateName });
            }

            if (Request.QueryString["templateID"] != null)
            {
                try
                {
                    var templateID = Request.QueryString["templateID"].ToInt32();
                    selectedPage = allthemesFromCache
                        .Where(pt => pt.PortalID == PortalID && pt.ID == templateID)
                        .Select(p => new TemplateCache { ID = p.ID, Direction = p.Direction, TemplateName = p.TemplateName });
                }
                catch
                {
                }
            }

            if (selectedPage.Count() == 1)
            {
                var z = selectedPage.First();
                ((INikRoutedPage)this.Page).SelectedPageID = z.ID;
                _pagedirection = z.Direction;
                controlFileName = z.TemplateName;
                BuildSkin(this);
                if (((INikRoutedPage)this.Page).SelectedPageID > 0)
                {
                    this.FillPlaceHolders();
                    this.SetupWidgets(((INikRoutedPage)this.Page).SelectedPageID, WIDGET_CONTAINER);
                }
            }
            else
            {
                Response.Write("Pleas chech the Engine, maybe have not Jacket or more!!!!");
            }

        }

        private class TemplateCache
        {
            public int ID { get; set; }
            public string Direction { get; set; }
            public string TemplateName { get; set; }
            public TemplateType TemplateType { get; set; }
            public int PortalID { get; set; }
            public bool IsSelected { get; set; }
            public string ModuleKey { get; set; }
            public string ModuleParameter { get; set; }
        }
    }
}
