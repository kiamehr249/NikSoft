using System.Collections.Generic;

namespace NikSoft.NikModel
{
    public class WidgetDefinition
    {
        public WidgetDefinition()
        {
            this.Widgets = new HashSet<Widget>();
        }
        public int ID { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string DefaultState { get; set; }
        public string Icon { get; set; }
        public int Ordering { get; set; }
        public string Image { get; set; }

        public virtual ICollection<Widget> Widgets { get; set; }
    }
}