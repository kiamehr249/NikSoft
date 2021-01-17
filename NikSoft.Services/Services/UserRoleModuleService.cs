using NikSoft.Model;
using NikSoft.NikModel;
using StructureMap;
using System.Linq;

namespace NikSoft.Services
{
    public interface IUserRoleModuleService : INikService<UserRoleModule>
    {
        bool SinglePermission(string moduleName, int userID, UserGroupPermissionType gType);
    }
    public class UserRoleModuleService : NikService<UserRoleModule>, IUserRoleModuleService
    {

        public UserRoleModuleService(IUnitOfWork uow)
				: base(uow) {
        }

        public bool SinglePermission(string moduleName, int userID, UserGroupPermissionType gType)
        {
            var iNikModulesServ = ObjectFactory.GetInstance<INikModuleService>();
            var iUserServ = ObjectFactory.GetInstance<IUserService>();
            var moduleID = iNikModulesServ.GetModuleID(moduleName);
            var userGroups = iUserServ.GetUserGroup(userID);
            var permissions = Entity.Where(x => x.NikModuleID == moduleID && userGroups.Contains(x.UserTypeGroupID)).Select(x => new { x.NikModuleID, x.PermissionType }).ToList();
            if (!permissions.Any(t => t.PermissionType == gType))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}