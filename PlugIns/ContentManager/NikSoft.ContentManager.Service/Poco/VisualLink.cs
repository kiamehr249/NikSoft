using System.Collections.Generic;

namespace NikSoft.ContentManager.Service
{
    public class VisualLink
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int PortalID { get; set; }
        public virtual ICollection<VisualLinkItem> VisualLinkItems { get; set; }
    }
}
