using NikSoft.Model;
using System;
using System.Collections.Generic;

namespace NikSoft.ContentManager.Service
{
    public class ContentGroup : LogEntity
    {
        public ContentGroup()
        {
            CreateDateTime = DateTime.Now;
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public string FontIcon { get; set; }
        public bool Enabled { get; set; }
        public int Ordering { get; set; }
        public int PortalID { get; set; }
        public virtual ICollection<ContentCategory> ContentCategories { get; set; }
        public virtual ICollection<GroupPermission> Permissions { get; set; }
    }
}
