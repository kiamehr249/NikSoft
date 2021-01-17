namespace NikSoft.ContentManager.Service
{
    public class VisualLinkItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Link1 { get; set; }
        public string Link2 { get; set; }
        public string Descroption { get; set; }
        public string Btn1Text { get; set; }
        public string Btn2Text { get; set; }
        public string Img1 { get; set; }
        public string Img2 { get; set; }
        public string Img3 { get; set; }
        public string Img4 { get; set; }
        public int Ordering { get; set; }
        public bool Enabled { get; set; }
        public int VisualLinkID { get; set; }
        public virtual VisualLink VisualLink { get; set; }
    }
}
