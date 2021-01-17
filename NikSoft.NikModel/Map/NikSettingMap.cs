using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class NikSettingMap : EntityTypeConfiguration<NikSetting>
    {

        public NikSettingMap()
        {
            this.HasKey(t => t.ID);

            this.ToTable("RayanSetting");

            this.HasRequired(t => t.Portal)
                .WithMany(t => t.NikSettings)
                .HasForeignKey(d => d.PortalID);
        }
    }
}