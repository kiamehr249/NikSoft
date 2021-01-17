namespace NikSoft.Model
{
    public interface INikSingleSignOn
    {

        bool Authenticate(ISingleSignOnService iuserservice, string username, string pass, bool rememberMe);

        bool AuthenticateForAdmin(ISingleSignOnService iuserservice, string username, int adminId);


        NikPortalUser AuthenticateFromContext(ISingleSignOnService iuserservice);

        void SignOutUser();

        bool ChangePass(ISingleSignOnService iuserservice, string oldPass, string newpass);
    }
}