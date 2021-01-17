using NikSoft.Model;
using System.Collections.Generic;

namespace NikSoft.NikModel
{
    public class UserType : LogEntity
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int PortalID { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<UserTypeGroup> UserTypeGroups { get; set; }
    }
}