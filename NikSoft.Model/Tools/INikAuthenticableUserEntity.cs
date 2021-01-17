namespace NikSoft.Model
{
    public interface INikAuthenticableUserEntity
    {
        int ID { get; }
        string UserName { get; }
        string Password { get; set; }
        string LoginKey { get; }
        int RandomID { get; }
        int PortalID { get; set; }
        string PortalFolderPath { get; set; }
        string NameOfUser { get; set; }
        string EmailOfUser { get; set; }
    }
}