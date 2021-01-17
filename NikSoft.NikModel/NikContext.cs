using NikSoft.Model;
using System.Data.Entity;

namespace NikSoft.NikModel
{
    public class NikContext : BaseContext
    {
        public DbSet<Template> Templates { get; set; }
        public DbSet<TemplateFile> TemplateFiles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Portal> Portals { get; set; }
        public DbSet<PortalAddress> PortalAddress { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<UserTypeGroup> UserTypeGroups { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<NikModule> NikModules { get; set; }
        public DbSet<NikModuleDefinition> NikModuleDefinitions { get; set; }
        public DbSet<Widget> Widgets { get; set; }
        public DbSet<WidgetDefinition> WidgetDefinitions { get; set; }
        public DbSet<UserRoleModule> UserRoleModules { get; set; }
        public DbSet<NikMenu> NikMenus { get; set; }
        public DbSet<UserRoleMenu> UserRoleMenus { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        static NikContext()
        {
            Database.SetInitializer<NikContext>(null);
        }

        public NikContext() : base("name=NikDBContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new TemplateMap());
            modelBuilder.Configurations.Add(new TemplateFileMap());
            modelBuilder.Configurations.Add(new PortalMap());
            modelBuilder.Configurations.Add(new PortalAddressMap());
            modelBuilder.Configurations.Add(new UserTypeMap());
            modelBuilder.Configurations.Add(new UserTypeGroupMap());
            modelBuilder.Configurations.Add(new ThemeMap());
            modelBuilder.Configurations.Add(new WidgetMap());
            modelBuilder.Configurations.Add(new WidgetDefinitionMap());
            modelBuilder.Configurations.Add(new NikModuleMap());
            modelBuilder.Configurations.Add(new NikModuleDefinitionMap());
            modelBuilder.Configurations.Add(new UserRolModuleMap());
            modelBuilder.Configurations.Add(new UserRoleMenuMap());
            modelBuilder.Configurations.Add(new NikMenuMap());
            modelBuilder.Configurations.Add(new UserProfileMap());
        }
    }
}
