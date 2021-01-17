using System;
using System.Collections.Generic;

namespace NikSoft.ContentManager.Service
{
    public class PublicContent
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string MiniDesc { get; set; }
        public string FullText { get; set; }
        public string IconUrl { get; set; }
        public string ImgUrl { get; set; }
        public string AttachFile { get; set; }
        public string FontIcon { get; set; }
        public int Ordering { get; set; }
        public int? GroupID { get; set; }
        public int? CategoryID { get; set; }
        public int? ContentAlbumID { get; set; }
        public int PortalID { get; set; }
        public bool Enabled { get; set; }
        public bool IsStore { get; set; }
        public int CreatorID { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual ContentCategory ContentCategory { get; set; }
        public virtual ICollection<ContentFile> ContentFiles { get; set; }
    }
}