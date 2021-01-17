using System.Collections.Generic;

namespace NikSoft.FormBuilder.Service
{
    public class FormModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Template { get; set; }
        public bool LoginRequired { get; set; }
        public string Message { get; set; }
        public bool RecordIP { get; set; }
        public bool Enabled { get; set; }
        public int? ParentID { get; set; }
        public int Ordering { get; set; }
        public int PortalID { get; set; }
        public virtual FormModel Parent { get; set; }
        public virtual ICollection<FormModel> Childs { get; set; }
        public virtual ICollection<TextBoxModel> TextBoxes { get; set; }
        public virtual ICollection<ListControlModel> ListControls { get; set; }
        public virtual ICollection<FileUploadModel> FileUploads { get; set; }
        public virtual ICollection<CheckBoxModel> CheckBoxes { get; set; }
        public virtual ICollection<FormContent> FormContents { get; set; }
    }
}
