using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class ThemeMap : EntityTypeConfiguration<Theme>
    {

        public ThemeMap()
        {
            this.HasKey(t => t.ID);

            this.ToTable("Themes");
        }
    }
}