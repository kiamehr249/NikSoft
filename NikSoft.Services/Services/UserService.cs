using NikSoft.Model;
using NikSoft.NikModel;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.Services
{
    public interface IUserService : INikService<User>, ISingleSignOnService
    {
        List<int> GetUserGroup(int userID);

        List<int> GetUserType(int userID);

        bool UsernameIsUnique(string userName);

        string GetName(int Id);

        string GetFamily(int Id);

        string GetFullName(int Id);
    }

    public class UserService : NikService<User>, IUserService
    {

        public UserService(IUnitOfWork uow)
			: base(uow) {
        }

        public override IList<User> GetAllPaged(List<Expression<Func<User, bool>>> predicate, int startIndex, int pageSize)
        {
            var query = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                query = query.Where(predicate[i]);
            }
            return query.OrderByDescending(i => i.ID).Skip(startIndex).Take(pageSize).ToList();
        }

        public INikAuthenticableUserEntity GetUserByUserName(string userName)
        {
            var today = DateTime.Now;
            var theUser = GetAll(x => x.UserName == userName && !x.IsLock && (x.PassExpireDate >= today || x.UnExpire), x => new NikAuthenticableUserEntity
            {
                ID = x.ID,
                Password = x.Password,
                UserName = x.UserName,
                LoginKey = x.LoginKey,
                RandomID = x.RandomID,
                PortalID = x.PortalID,
                EmailOfUser = x.Email,
                NameOfUser = x.FirstName + " " + x.LastName,
                PortalFolderPath = x.Portal.AliasFolder
            });
            if (null == theUser || 1 != theUser.Count())
            {
                return null;
            }
            return theUser.First();
        }



        public INikAuthenticableUserEntity GetUserByOldUserName(string olduserName)
        {
            var today = DateTime.Now;
            var theUser = GetAll(x => x.OldUsername == olduserName && !x.IsLock && (x.PassExpireDate >= today || x.UnExpire), x => new NikAuthenticableUserEntity
            {
                ID = x.ID,
                Password = x.Password,
                UserName = x.OldUsername,
                LoginKey = x.LoginKey,
                RandomID = x.RandomID,
                PortalID = x.PortalID,
                EmailOfUser = x.Email,
                NameOfUser = x.FirstName + " " + x.LastName,
                PortalFolderPath = x.Portal.AliasFolder
            });
            if (null == theUser || 1 != theUser.Count())
            {
                return null;
            }
            return theUser.First();
        }



        public int UpdateUserStats(int userID)
        {
            var userForUpdate = Find(x => x.ID == userID);
            userForUpdate.LastLogin = DateTime.Now;
            return SaveChanges();
        }

        public INikAuthenticableUserEntity GetUserbyID(int userID)
        {
            var today = DateTime.Now;
            var theUser = GetAll(x => x.ID == userID && !x.IsLock && (x.PassExpireDate >= today || x.UnExpire), x => new NikAuthenticableUserEntity
            {
                ID = x.ID,
                Password = x.Password,
                UserName = x.UserName,
                LoginKey = x.LoginKey,
                RandomID = x.RandomID,
                PortalID = x.PortalID,
                PortalFolderPath = x.Portal.AliasFolder,
                EmailOfUser = x.Email,
                NameOfUser = x.FirstName + " " + x.LastName
            });
            if (null == theUser || 1 != theUser.Count())
            {
                return null;
            }
            return theUser.First();
        }

        public bool ChangeUserPassword(User theUser, string oldPass, string newPass)
        {
            var oldPassPhrase = oldPass.CalculateMD5();
            if (oldPassPhrase != theUser.Password)
            {
                return false;
            }
            var np = newPass.CalculateMD5();
            var emp = Find(x => x.ID == theUser.ID);
            emp.Password = np;
            return 1 == SaveChanges();
        }

        public string GetPasswordHash(string pass, string username, string loginkey, int randomID)
        {
            return (username + loginkey + randomID + pass).CalculateMD5();
        }

        public string GenerateHash(User theUser, string const1, string const2)
        {
            return MakeHash(theUser.LoginKey, theUser.RandomID, theUser.ID, const1, const2);
        }

        public string GenerateHash(INikAuthenticableUserEntity theUser, string const1, string const2)
        {
            return MakeHash(theUser.LoginKey, theUser.RandomID, theUser.ID, const1, const2);
        }

        private string MakeHash(string loginKey, int randomID, int userID, string const1, string const2)
        {
            return (loginKey + const1 + randomID + const2 + userID).CalculateMD5();
        }

        public int ChangeUserPassword(int userID, string oldPass, string newPass, string newPassConfrim)
        {
            var theUser = Find(t => t.ID == userID);
            if (theUser == null) { return 0; }
            if (GetPasswordHash(oldPass, theUser.UserName, theUser.LoginKey, theUser.RandomID) != theUser.Password)
            {
                return 11;
            }
            if (GetPasswordHash(newPass, theUser.UserName, theUser.LoginKey, theUser.RandomID) == theUser.Password)
            {
                return 12;
            }
            if (newPass != newPassConfrim)
            {
                return 13;
            }
            var password = GetPasswordHash(newPass, theUser.UserName, theUser.LoginKey, theUser.RandomID);
            var emp = Find(x => x.ID == theUser.ID);
            emp.Password = password;
            return SaveChanges();
        }

        public List<int> GetUserGroup(int userID)
        {
            return this.Find(t => t.ID == userID, t => t.UserTypeGroups).Select(t => t.ID).ToList();
        }

        public List<int> GetUserType(int userID)
        {
            return this.Find(t => t.ID == userID, t => t.UserTypes).Select(t => t.ID).ToList();
        }

        public bool UsernameIsUnique(string userName)
        {
            return !Any(t => t.UserName == userName);
        }

        public string GetName(int Id)
        {
            return this.Find(x => x.ID == Id).FirstName;
        }

        public string GetFamily(int Id)
        {
            return this.Find(x => x.ID == Id).LastName;
        }

        public string GetFullName(int Id)
        {
            var n = this.Find(x => x.ID == Id);
            if (n == null)
                return string.Empty;
            return n.FirstName + " " + n.LastName;
        }

    }
}