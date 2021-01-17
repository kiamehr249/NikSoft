using NikSoft.Model;
using NikSoft.NikModel;

namespace NikSoft.Services
{
    public interface IUserRoleMenuService : INikService<UserRoleMenu>
    {
    }
    public class UserRoleMenuService : NikService<UserRoleMenu>, IUserRoleMenuService
    {
        public UserRoleMenuService(IUnitOfWork uow)
			: base(uow) {
        }
    }
}