namespace NikSoft.NikModel
{
    public class NikSetting
    {
        public int ID { get; set; }
        public int PortalID { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public string SettingLabel { get; set; }
        public int MinAllowed { get; set; }
        public int MaxAllowed { get; set; }
        public NikSettingFeildType FieldType { get; set; }
        public bool UseEditor { get; set; }
        public NikSettingType SettingModule { get; set; }
        public bool ShowEditorInEdit { get; set; }

        public virtual Portal Portal { get; set; }
    }
}