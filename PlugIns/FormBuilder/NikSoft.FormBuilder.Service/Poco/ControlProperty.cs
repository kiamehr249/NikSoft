namespace NikSoft.FormBuilder.Service
{
    public class ControlProperty
    {
        public string Class { get; set; }
        public string IdentityValue { get; set; }
        public string KeyWord { get; set; }
        public string Message { get; set; }
        public int Position { get; set; }
        public int FormID { get; set; }
        public virtual FormModel Form { get; set; }
    }
}
