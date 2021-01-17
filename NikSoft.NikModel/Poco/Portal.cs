using System.Collections.Generic;

namespace NikSoft.NikModel
{
    public class Portal
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? ParentID { get; set; }
        public string Alias { get; set; }
        public string Direction { get; set; }
        public PortalLanguage PortalLanguage { get; set; }
        public string Domain { get; set; }
        public string Favicon { get; set; }
        public int MaxVol { get; set; }
        public string MetaDescription { get; set; }
        public string AliasFolder { get; set; }
        public bool ShowOwnLogo { get; set; }
        public virtual Portal Parent { get; set; }
        public virtual ICollection<Portal> Childs { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Template> Templates { get; set; }
        public virtual ICollection<PortalAddress> PortalAddresses { get; set; }
        public virtual ICollection<NikSetting> NikSettings { get; set; }
        public virtual ICollection<NikMenu> NikMenus { get; set; }

    }
}
