using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class NikModuleDefinitionMap : EntityTypeConfiguration<NikModuleDefinition>
    {

        public NikModuleDefinitionMap()
        {
            this.HasKey(t => t.ID);

            this.ToTable("NikModuleDefinitions");
        }
    }
}