namespace NikSoft.FormBuilder.Service
{
    public class FileUploadModel : ControlProperty
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public FbFileType FileType { get; set; }
        public bool IsNullable { get; set; }
        public string Path { get; set; }
    }
}
