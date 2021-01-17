namespace NikSoft.Model
{
    public class NikPortalUser
    {
        public int ID { get; set; }
        public int PortalID { get; set; }
        public string PortalFolderPath { get; set; }
        public string FullNameOfUser { get; set; }
        public string EmailOfUser { get; set; }
    }
}