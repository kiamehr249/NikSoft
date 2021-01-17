using System.Collections.Generic;

namespace NikSoft.FormBuilder.Service
{
    public class ListControlModel : ControlProperty
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public ListControlType ControlType { get; set; }
        public bool IsNullable { get; set; }
        public int? ParentID { get; set; }
        public virtual ListControlModel Parent { get; set; }
        public virtual ICollection<ListControlModel> Childs { get; set; }
        public virtual ICollection<ListControlItemModel> ListItems { get; set; }
    }
}
