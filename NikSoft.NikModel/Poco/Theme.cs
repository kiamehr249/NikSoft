using NikSoft.Model;

namespace NikSoft.NikModel
{
    public class Theme : LogEntity
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ThemeImg { get; set; }
        public string ThemePath { get; set; }
        public int PortalID { get; set; }
    }
}