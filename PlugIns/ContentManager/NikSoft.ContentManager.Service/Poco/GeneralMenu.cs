using System.Collections.Generic;

namespace NikSoft.ContentManager.Service
{
    public class GeneralMenu
    {
        public GeneralMenu()
        {
            this.GeneralMenuItems = new HashSet<GeneralMenuItem>();
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int PortalID { get; set; }
        public bool LoginRequired { get; set; }

        public virtual ICollection<GeneralMenuItem> GeneralMenuItems { get; set; }
    }
}