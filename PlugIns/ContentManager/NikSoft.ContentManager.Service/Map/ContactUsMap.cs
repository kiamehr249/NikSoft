using System.Data.Entity.ModelConfiguration;

namespace NikSoft.ContentManager.Service
{
    public class ContactUsMap : EntityTypeConfiguration<ContactUs>
    {
        public ContactUsMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("CM_ContactUs");
        }
    }
}