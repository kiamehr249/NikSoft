namespace NikSoft.NikModel
{
    public class UserRoleMenu
    {
        public int NikMenuID { get; set; }
        public int UserTypeGroupID { get; set; }
        public UserGroupPermissionType PermissionType { get; set; }

        public virtual NikMenu NikMenu { get; set; }
        public virtual UserTypeGroup UserTypeGroup { get; set; }
    }
}