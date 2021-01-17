namespace NikSoft.Model
{
    public class NikAuthenticableUserEntity : INikAuthenticableUserEntity
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LoginKey { get; set; }
        public int RandomID { get; set; }
        public int PortalID { get; set; }
        public string PortalFolderPath { get; set; }
        public string NameOfUser { get; set; }
        public string EmailOfUser { get; set; }
    }
}