using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class PortalAddressMap : EntityTypeConfiguration<PortalAddress>
    {

        public PortalAddressMap()
        {
            ToTable("PortalAddresses");
            HasKey(x => x.ID);

            Property(x => x.DomainAddress).HasColumnName("DomainAddress").IsRequired().HasColumnType("nvarchar").HasMaxLength(60);
            Property(x => x.Desciption).HasColumnName("Desciption").IsOptional().HasColumnType("nvarchar").HasMaxLength(500);

            HasRequired(a => a.Portal).WithMany(b => b.PortalAddresses).HasForeignKey(c => c.PortalID);
        }
    }
}
