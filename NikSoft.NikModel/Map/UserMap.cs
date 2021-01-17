using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.HasKey(x => x.ID);
            this.ToTable("Users");
            this.HasRequired(t => t.Portal)
                .WithMany(t => t.Users)
                .HasForeignKey(d => d.PortalID);

            this.HasMany(t => t.UserTypeGroups)
               .WithMany(t => t.Users)
               .Map(m => {
                   m.ToTable("UserTypeGroupBridg");
                   m.MapLeftKey("UserID");
                   m.MapRightKey("UserTypeGroupID");
               });

            this.HasMany(t => t.UserTypes)
                .WithMany(t => t.Users)
                .Map(m => {
                    m.ToTable("UserTypeBridg");
                    m.MapLeftKey("UserID");
                    m.MapRightKey("UserTypeID");
                });
        }
    }
}
