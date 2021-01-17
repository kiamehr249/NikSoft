using NikSoft.Model;
using System.Data.Entity;

namespace NikSoft.ContentManager.Service
{
    public class CMContext : BaseContext, ICMUnitOFWork
    {
        static CMContext()
        {
            Database.SetInitializer<CMContext>(null);
        }

        public CMContext() : base("name=NikDBContext")
        {

        }

        public DbSet<ContentGroup> ContentGroups { get; set; }
        public DbSet<ContentCategory> ContentCategories { get; set; }
        public DbSet<PublicContent> PublicContents { get; set; }
        public DbSet<ContentFile> ContentFiles { get; set; }
        public DbSet<GeneralMenu> GeneralMenus { get; set; }
        public DbSet<GeneralMenuItem> GeneralMenuItems { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }
        public DbSet<CategoryPermission> CategoryPermissions { get; set; }
        public DbSet<VisualLink> VisualLinks { get; set; }
        public DbSet<VisualLinkItem> VisualLinkItems { get; set; }
        public DbSet<ContactUs> Contacts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ContentGroupMap());
            modelBuilder.Configurations.Add(new ContentCategoryMap());
            modelBuilder.Configurations.Add(new PublicContentMap());
            modelBuilder.Configurations.Add(new ContentFileMap());
            modelBuilder.Configurations.Add(new GeneralMenuMap());
            modelBuilder.Configurations.Add(new GeneralMenuItemMap());
            modelBuilder.Configurations.Add(new GroupPermissionMap());
            modelBuilder.Configurations.Add(new CategoryPermissionMap());
            modelBuilder.Configurations.Add(new VisualLinkMap());
            modelBuilder.Configurations.Add(new VisualLinkItemMap());
            modelBuilder.Configurations.Add(new ContactUsMap());
        }
    }
}