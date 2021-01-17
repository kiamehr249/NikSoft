using System.Collections.Generic;

namespace NikSoft.NikModel
{
    public class UserTypeGroup
    {
        public int ID { get; set; }
        public int UserTypeID { get; set; }
        public string Title { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<UserRoleModule> UserRoleModules { get; set; }
        public virtual ICollection<UserRoleMenu> UserRoleMenus { get; set; }
    }
}
