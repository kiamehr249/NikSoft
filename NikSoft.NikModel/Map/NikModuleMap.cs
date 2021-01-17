using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    class NikModuleMap : EntityTypeConfiguration<NikModule>
    {

        public NikModuleMap()
        {
            this.HasKey(t => t.ID);

            this.ToTable("NikModules");

            this.HasRequired(t => t.NikModuleDefinition)
                .WithMany(t => t.NikModules)
                .HasForeignKey(t => t.ModuleDefinitionID);
        }
    }
}