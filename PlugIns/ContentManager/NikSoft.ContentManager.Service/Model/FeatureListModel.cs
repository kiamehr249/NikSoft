namespace NikSoft.ContentManager.Service
{
    public class FeatureListModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FeatureKey { get; set; }
        public int? FeatureFormID { get; set; }
        public int Ordering { get; set; }
        public bool Enabled { get; set; }
    }
}
