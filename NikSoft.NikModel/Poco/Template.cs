using NikSoft.Model;
using System.Collections.Generic;

namespace NikSoft.NikModel
{
    public class Template : LogEntity
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TemplateType Type { get; set; }
        public int PortalID { get; set; }
        public string TemplateName { get; set; }
        public string Direction { get; set; }
        public string Culture { get; set; }
        public bool IsSelected { get; set; }
        public bool UserMustLogin { get; set; }
        public string ModuleKey { get; set; }
        public string ModuleParameter { get; set; }
        public virtual Portal Portal { get; set; }
        public virtual List<TemplateFile> TemplateFiles { get; set; }
    }
}
