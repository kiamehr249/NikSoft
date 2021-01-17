using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.Utilities;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace NikSoft.UILayer
{
    public class TemplateCore : NikBaseUserControl
    {
        public IWidgetService iWidgetServ { get; set; }
        protected const string TEMPLATE_CACHE = "Templates";
        protected string _pagedirection = "", controlFileName = "";

        private Dictionary<int, HtmlGenericControl> PlaceHolders = new Dictionary<int, HtmlGenericControl>();

        public TemplateCore()
        {
            ObjectFactory.BuildUp(this);
        }

        public ITemplateService iTemplateServ { get; set; }

        protected TemplateType TemplateType
        {
            get
            {
                var NikRoutedPage = (this.Page) as INikRoutedPage;
                if (NikRoutedPage != null)
                    return NikRoutedPage.TemplateType;
                return TemplateType.HomePage;
            }
        }

        protected void SetupWidgets(int tPageID, string pcontrolToLoad, AdminorUi typeofUI = AdminorUi.UiPart)
        {

            if (iWidgetServ == null)
            {
                iWidgetServ = new WidgetService(uow);
            }
            var queryOfWidgets = iWidgetServ.ExpressionMaker();
            queryOfWidgets.Add(t => t.TemplateID == tPageID);
            if (typeofUI == AdminorUi.UiPart)
            {
                queryOfWidgets.Add(t => t.Published);
            }

            var currentPageWidgets2 = iWidgetServ.GetAll(
                            //Query
                            queryOfWidgets,
                            //Select
                            x => new TemplateWidgets
                            {
                                NewUrl = x.NewUrl,
                                Expanded = x.Expanded,
                                Icon = x.WidgetDefinition.Icon,
                                MainUrl = x.WidgetDefinition.Url,
                                OrderNo = x.OrderNo,
                                PanelNo = x.PanelNo,
                                Published = x.Published,
                                ShowTitle = x.ShowTitle,
                                State = x.State,
                                Title = x.Title,
                                TitleLink = x.TitleLink,
                                WidgetDefinitionID = x.WidgetDefinitionID,
                                WidgetDefinitionTitle = x.WidgetDefinition.Title,
                                WidgetID = x.ID,
                                WidgetSkinPath = x.WidgetSkinPath
                            }
                            //Order by
                            , y => new { y.PanelNo, y.OrderNo })
                            .ToList();
            foreach (var instance in currentPageWidgets2)
            {
                try
                {
                    var panel = GetPanelByNumber(instance.PanelNo);
                    if (null == panel)
                    {
                        continue;
                    }
                    var newWidgetID = "ycw" + instance.WidgetID.ToString();
                    var widget = LoadControl(pcontrolToLoad) as ContainerBase;

                    panel.Controls.Remove(panel.FindControl(newWidgetID));
                    if (widget == null)
                    {
                        continue;
                    }
                    widget.ID = newWidgetID;
                    widget.WidgetInstance = new WidgetUI(instance);
                    widget.PageDirection = _pagedirection;
                    panel.Controls.Add(widget);
                }
                catch (ThreadAbortException)
                {
                    Response.Write("Additional UInfo: NOPU");
                }
                catch (Exception ex)
                {
                    ;
                }
            }


        }

        protected void BuildSkin(Control ctrlToAddSkin)
        {
            string controlFullPath = "";
            controlFullPath = "~/" + controlFileName;
            var ctrl = Page.LoadControl(controlFullPath);
            if (ctrl != null)
            {
                ctrlToAddSkin.Controls.Add(ctrl);
            }
        }

        protected void FillPlaceHolders()
        {
            FindPlaceHolders(this.Controls);
        }

        protected void FillPlaceHolders(ControlCollection controls)
        {
            FindPlaceHolders(controls);
        }

        private void FindPlaceHolders(ControlCollection controls)
        {
            foreach (Control item in controls)
            {
                if (item is HtmlGenericControl)
                {
                    if (!(item as HtmlGenericControl).Attributes["data-mc"].IsEmpty() && (item as HtmlGenericControl).Attributes["data-mc"].IsNumeric())
                    {
                        PlaceHolders.Add((item as HtmlGenericControl).Attributes["data-mc"].ToInt32(), item as HtmlGenericControl);
                    }
                }
                FindPlaceHolders(item.Controls);
            }
        }

        protected HtmlGenericControl GetPanelByNumber(int panelNumber)
        {
            if (PlaceHolders.ContainsKey(panelNumber))
            {
                return PlaceHolders[panelNumber];
            }
            return null;
        }



    }
}
