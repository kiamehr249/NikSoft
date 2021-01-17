using NikSoft.NikModel;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Text.RegularExpressions;

namespace NikSoft.Web.Modules.BaseModules.User
{
    public partial class UserRegister : NikUserControl
    {
        protected bool UnIsEmpty = false;
        protected bool UnNotEngilsh = false;
        protected bool PsEmpty = false;
        protected bool PacEmpty = false;
        protected bool PsNotMatch = false;
        protected bool ExistUser = false;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected bool IsValid()
        {
            int errors = 0;

            if (TxtUsername.Text.IsEmpty())
            {
                UnIsEmpty = true;
                errors += 1;
            }
            else
            {
                if (!Regex.IsMatch(TxtUsername.Text, "^[a-zA-Z0-9_.@]*$"))
                {
                    UnNotEngilsh = true;
                    errors += 1;
                }
                else
                {
                    var userExist = iUserServ.Any(x => x.UserName == TxtUsername.Text);
                    if (userExist)
                    {
                        ExistUser = true;
                        errors += 1;
                    }
                }
            }

            if (TxtPass.Text.IsEmpty())
            {
                PsEmpty = true;
                errors += 1;
            }

            if (TxtPassConf.Text.IsEmpty())
            {
                PsEmpty = true;
                errors += 1;
            }

            if (TxtPass.Text != TxtPassConf.Text)
            {
                PsNotMatch = true;
                errors += 1;
            }

            if (errors > 0)
            {
                return false;
            }

            return true;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid())
            {
                return;
            }

            var userLoginKey = Utilities.Utilities.RandomStringNumber();
            var UserRandomID = Utilities.Utilities.RandomNumber();
            var userPassFullHashed = iUserServ.GetPasswordHash(TxtPass.Text, TxtUsername.Text, userLoginKey, UserRandomID);

            var u = new NikModel.User
            {
                FirstName = TxtFname.Text,
                LastName = TxtLname.Text,
                LoginKey = userLoginKey,
                RandomID = UserRandomID,
                UserName = TxtUsername.Text,
                Password = userPassFullHashed,
                PortalID = CurrentPortalID,
                PassExpireDate = DateTime.Now.AddYears(1),
                UnExpire = true,
                IsLock = false,
                UserType = NikUserType.GeneralUser
            };
            iUserServ.Add(u);
            uow.SaveChanges(CurrentPortalID);
            Response.Redirect("/" + Level + "/UserLogin/?un=" + u.UserName);
        }
    }
}