using NikSoft.Model;
using NikSoft.NikModel;
using System.Data.Entity;
using StructureMap;

namespace NikSoft.Routing
{
    public class InitStructureMap
    {
        public static void InitHttp()
        {
            Database.SetInitializer<BaseContext>(null);

            ObjectFactory.Initialize(init => {
                init.Scan(scan => {
                    scan.AssembliesFromApplicationBaseDirectory();
                    scan.WithDefaultConventions();
                    scan.ConnectImplementationsToTypesClosing(typeof(INikService<>));
                });
                init.For<IUnitOfWork>().HttpContextScoped().Use<NikContext>();
                init.SetAllProperties(set => {
                    set.OfType<IUnitOfWork>();
                    set.NameMatches(name => name.EndsWith("Service"));
                    set.WithAnyTypeFromNamespace("NikSoft.Services");
                });
            });
        }
    }
}