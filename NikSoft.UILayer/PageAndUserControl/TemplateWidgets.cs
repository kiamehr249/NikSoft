namespace NikSoft.UILayer
{
    public class TemplateWidgets
    {

        public TemplateWidgets()
        {

        }

        public int WidgetID { get; set; }
        public int WidgetDefinitionID { get; set; }
        public string WidgetSkinPath { get; set; }
        public int PanelNo { get; set; }
        public int OrderNo { get; set; }
        public bool Expanded { get; set; }
        public string Title { get; set; }
        public string TitleLink { get; set; }
        public string State { get; set; }
        public bool ShowTitle { get; set; }
        public bool Published { get; set; }
        public string NewUrl { get; set; }

        public string WidgetDefinitionTitle { get; set; }
        public string MainUrl { get; set; }
        public string Icon { get; set; }

    }
}
