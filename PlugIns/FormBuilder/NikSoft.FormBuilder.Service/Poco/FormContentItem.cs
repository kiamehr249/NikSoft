namespace NikSoft.FormBuilder.Service
{
    public class FormContentItem
    {
        public int ID { get; set; }
        public string ItemValue { get; set; }
        public int Ordering { get; set; }
        public int FormContentID { get; set; }
        public virtual FormContent FormContent { get; set; }
    }
}
