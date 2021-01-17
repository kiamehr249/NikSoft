namespace NikSoft.NikModel
{
    public class UserRoleModule
    {
        public int NikModuleID { get; set; }
        public int UserTypeGroupID { get; set; }
        public UserGroupPermissionType PermissionType { get; set; }

        public virtual NikModule NikModule { get; set; }
        public virtual UserTypeGroup UserTypeGroup { get; set; }
    }
}