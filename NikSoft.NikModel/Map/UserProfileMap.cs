using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class UserProfileMap : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileMap()
        {
            this.HasKey(x => x.ID);
            this.ToTable("UserProfiles");
            this.HasRequired(x => x.User)
                .WithMany(x => x.UserProfiles)
                .HasForeignKey(x => x.UserID);
        }
    }
}