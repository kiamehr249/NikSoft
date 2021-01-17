namespace NikSoft.Model
{
    public interface ISingleSignOnService
    {

        INikAuthenticableUserEntity GetUserByUserName(string userName);


        INikAuthenticableUserEntity GetUserByOldUserName(string olduserName);

        int UpdateUserStats(int userID);

        INikAuthenticableUserEntity GetUserbyID(int userID);

        string GenerateHash(INikAuthenticableUserEntity theUser, string const1, string const2);

        int ChangeUserPassword(int userID, string oldPass, string newPass, string newPassConfrim);

        string GetPasswordHash(string pass, string username, string loginkey, int randomID);
    }
}