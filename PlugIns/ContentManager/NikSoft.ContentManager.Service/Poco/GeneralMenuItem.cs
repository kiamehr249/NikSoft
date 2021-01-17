using System.Collections.Generic;

namespace NikSoft.ContentManager.Service
{
    public class GeneralMenuItem
    {
        public GeneralMenuItem()
        {
            this.Childs = new HashSet<GeneralMenuItem>();
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string ImgUrl { get; set; }
        public string Font { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        public int Ordering { get; set; }
        public int PortalID { get; set; }
        public int? ParentID { get; set; }
        public int GeneralMenuID { get; set; }
        public bool LoginRequired { get; set; }
        public bool Enabled { get; set; }

        public virtual GeneralMenu GeneralMenu { get; set; }
        public virtual GeneralMenuItem Parent { get; set; }
        public virtual ICollection<GeneralMenuItem> Childs { get; set; }
    }
}