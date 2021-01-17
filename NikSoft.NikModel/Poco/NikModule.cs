using NikSoft.Model;
using System.Collections.Generic;

namespace NikSoft.NikModel
{
    public class NikModule : LogEntity
    {

        public int ID { get; set; }
        public string Title { get; set; }
        public string ModuleKey { get; set; }
        public string ModuleFile { get; set; }
        public bool IsXMLBase { get; set; }
        public bool LoginRequired { get; set; }
        public int ModuleDefinitionID { get; set; }
        public bool ShowAsModule { get; set; }
        public string SecondTitle { get; set; }
        public bool IsGeneralModule { get; set; }
        public int? PortalID { get; set; }
        public bool Editable { get; set; }
        public bool IsExternal { get; set; }
        public virtual NikModuleDefinition NikModuleDefinition { get; set; }
        public virtual ICollection<UserRoleModule> UserRoleModules { get; set; }
    }
}