using System.Data.Entity.ModelConfiguration;

namespace NikSoft.ContentManager.Service
{
    public class GeneralMenuMap : EntityTypeConfiguration<GeneralMenu>
    {

        public GeneralMenuMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("CM_GeneralMenus");
        }
    }
}