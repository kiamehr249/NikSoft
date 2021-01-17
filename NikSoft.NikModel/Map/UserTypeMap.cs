using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class UserTypeMap : EntityTypeConfiguration<UserType>
    {

        public UserTypeMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("UserTypes");
        }
    }
}