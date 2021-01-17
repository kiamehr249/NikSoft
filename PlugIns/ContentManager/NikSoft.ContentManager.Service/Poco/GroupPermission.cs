using System;

namespace NikSoft.ContentManager.Service
{
    public class GroupPermission
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
        public int PortalID { get; set; }
        public int CreatorID { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ContentGroup ContentGroup { get; set; }
    }
}
