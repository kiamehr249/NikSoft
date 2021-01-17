namespace NikSoft.NikModel
{
    public class TemplateFile
    {
        public int ID { get; set; }
        public int TemplateID { get; set; }
        public string FilePath { get; set; }
        public virtual Template Template { get; set; }
    }
}
