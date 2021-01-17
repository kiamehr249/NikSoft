using System;

namespace NikSoft.ContentManager.Service
{
    public class CategoryPermission
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
        public int PortalID { get; set; }
        public int CreatorID { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ContentCategory ContentCategory { get; set; }
    }
}
