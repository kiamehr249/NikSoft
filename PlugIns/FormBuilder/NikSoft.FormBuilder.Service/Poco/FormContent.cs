using System.Collections.Generic;

namespace NikSoft.FormBuilder.Service
{
    public class FormContent
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ItemValue { get; set; }
        public ControlType ControlType { get; set; }
        public int Position { get; set; }
        public int FormID { get; set; }
        public virtual FormModel Form { get; set; }
        public virtual ICollection<FormContentItem> FormContentItems { get; set; }
    }
}
