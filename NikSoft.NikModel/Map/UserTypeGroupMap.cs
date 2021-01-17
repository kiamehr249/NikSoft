using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class UserTypeGroupMap : EntityTypeConfiguration<UserTypeGroup>
    {

        public UserTypeGroupMap()
        {
            this.HasKey(t => t.ID);

            this.ToTable("UserTypeGroups");

            this.HasRequired(t => t.UserType)
                .WithMany(t => t.UserTypeGroups)
                .HasForeignKey(t => t.UserTypeID);
        }
    }
}