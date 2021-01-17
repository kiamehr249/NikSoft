namespace NikSoft.ContentManager.Service
{
    public class FeatuerModel
    {
        public string Title { get; set; }
        public string FeatureKey { get; set; }
        public string Description { get; set; }
        public FeatureType Type { get; set; }
        public int ItemID { get; set; }
        public int FormID { get; set; }
        public bool Enabled { get; set; }
    }
}
