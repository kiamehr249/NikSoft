using NikSoft.NikModel;

namespace NikSoft.UILayer
{
    public interface INikRoutedPage
    {
        string Level { get; set; }
        string ModuleName { get; set; }
        string ModuleParameters { get; set; }
        string Language { get; set; }
        string Direction { get; set; }
        string Domain { get; }
        bool ConvertYeKe { get; }
        int CurrentPortalID { get; set; }
        string CurrentPortalPath { get; set; }
        TemplateType TemplateType { get; set; }
        int SelectedPageID { get; set; }

        void Issue404();
    }
}