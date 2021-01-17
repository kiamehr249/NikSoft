using NikSoft.NikModel;
using NikSoft.Model;
using StructureMap;
using NikSoft.Services;

namespace NikSoft.UILayer
{
    public class WidgetUI
    {
        public IUnitOfWork uow { get; set; }
        public IWidgetService iWidgetServ { get; set; }

        public WidgetUI(TemplateWidgets tw)
        {
            widget = new Widget()
            {
                Expanded = tw.Expanded,
                ID = tw.WidgetID,
                NewUrl = tw.NewUrl,
                OrderNo = tw.OrderNo,
                TemplateID = -100,
                PanelNo = tw.PanelNo,
                Published = tw.Published,
                ShowTitle = tw.ShowTitle,
                State = tw.State,
                Title = tw.Title,
                TitleLink = tw.TitleLink,
                WidgetDefinitionID = tw.WidgetDefinitionID,
                WidgetSkinPath = tw.WidgetSkinPath
            };

            widgetDefinitionUI = new WidgetDefinitionUI()
            {
                ID = tw.WidgetDefinitionID,
                Title = tw.WidgetDefinitionTitle,
                Url = tw.MainUrl,
                Icon = tw.Icon
            };

            //if (File.Exists("~/files/" + ))

            ObjectFactory.BuildUp(this);
        }


        private WidgetDefinitionUI widgetDefinitionUI;
        public WidgetDefinitionUI WidgetDefinitionUI
        {
            get
            {
                return this.widgetDefinitionUI;
            }
            set
            {
                this.widgetDefinitionUI = value;
            }
        }

        private Widget widget;
        public Widget Widget
        {
            get
            {
                return this.widget;
            }
            set
            {
                this.widget = value;
            }
        }

        public void UpdateState(string state)
        {
            Widget w = iWidgetServ.Find(x => x.ID == widget.ID);
            w.State = state;
            iWidgetServ.SaveChanges();
        }

        public void UpdateSkin(string newSkinPath)
        {
            Widget w = iWidgetServ.Find(x => x.ID == widget.ID);
            w.WidgetSkinPath = newSkinPath;
            iWidgetServ.SaveChanges();
        }
    }
}