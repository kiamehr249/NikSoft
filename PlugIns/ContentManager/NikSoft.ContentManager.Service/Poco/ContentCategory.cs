using NikSoft.Model;
using System;
using System.Collections.Generic;

namespace NikSoft.ContentManager.Service
{
    public class ContentCategory : LogEntity
    {
        public ContentCategory()
        {
            CreateDateTime = DateTime.Now;
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ModuleKey { get; set; }
        public string ImgUrl { get; set; }
        public string IconFont { get; set; }
        public int Ordering { get; set; }
        public bool Enabled { get; set; }
        public bool IsStore { get; set; }
        public bool HasFeature { get; set; }
        public int PortalID { get; set; }
        public int GroupID { get; set; }
        public int? ParentID { get; set; }
        public virtual ContentGroup ContentGroup { get; set; }
        public virtual ContentCategory Parent { get; set; }
        public virtual ICollection<PublicContent> PublicContents { get; set; }
        public virtual ICollection<ContentCategory> Childs { get; set; }
        public virtual ICollection<CategoryPermission> Permissions { get; set; }
    }
}