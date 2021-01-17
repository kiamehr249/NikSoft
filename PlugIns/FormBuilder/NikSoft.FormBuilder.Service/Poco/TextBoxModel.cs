namespace NikSoft.FormBuilder.Service
{
    public class TextBoxModel : ControlProperty
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Placeholder { get; set; }
        public int? Rows { get; set; }
        public FbDataType DateType { get; set; }
        public TextBoxType TextMode { get; set; }
        public int MaxLength { get; set; }
        public FbValueType ValueType { get; set; } 
    }
}
