using System.Collections.Generic;

namespace NikSoft.NikModel
{
    public class NikMenu
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ModuleLinkTitle { get; set; }
        public string Link { get; set; }
        public string ImageURI { get; set; }
        public string AweSomeFontClass { get; set; }
        public bool Enabled { get; set; }
        public bool ShowInPanel { get; set; }
        public int PortalID { get; set; }
        public string SubMenusQuery { get; set; }
        public int Ordering { get; set; }
        public int? ParentID { get; set; }

        public virtual Portal Portal { get; set; }
        public virtual NikMenu Parent { get; set; }
        public virtual ICollection<NikMenu> Childs { get; set; }
        public virtual ICollection<UserRoleMenu> UserRoleMenus { get; set; }

    }
}