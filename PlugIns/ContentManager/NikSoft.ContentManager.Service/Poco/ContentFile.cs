namespace NikSoft.ContentManager.Service
{
    public class ContentFile
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public FileType ItemType { get; set; }
        public string CoverImage { get; set; }
        public bool IsCover { get; set; }
        public bool Enabled { get; set; }
        public int Ordering { get; set; }
        public int PublicContentID { get; set; }
        public int PortalID { get; set; }
        public virtual PublicContent PublicContent { get; set; }
    }
}