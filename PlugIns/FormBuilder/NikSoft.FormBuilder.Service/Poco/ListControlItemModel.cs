using System.Collections.Generic;

namespace NikSoft.FormBuilder.Service
{
    public class ListControlItemModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int ListControlID { get; set; }
        public int? ParentID { get; set; }
        public int FormID { get; set; }
        public virtual ListControlModel ListControl { get; set; }
        public virtual ListControlItemModel Parent { get; set; }
        public virtual ICollection<ListControlItemModel> Childs { get; set; }
    }
}