using NikSoft.ContentManager.Service;
using NikSoft.PlugIn;
using StructureMap;
using System;
using System.ComponentModel.Composition;
using System.Data.Entity;

namespace NikSoft.ContentManager.Web
{
    [Export(typeof(IPlugin))]
    [ExportMetadata("PluginName", "ContentManager")]
    [ExportMetadata("PlugInVersion", "1.0.0")]
    public class PluginInstaller : IPlugin
    {

        public void Initialize()
        {
            try
            {
                Database.SetInitializer<CMContext>(null);
                ObjectFactory.Configure(config => {
                    config.For<ICMUnitOFWork>().HttpContextScoped().Use<CMContext>();
                    config.SetAllProperties(set => {
                        set.OfType<ICMUnitOFWork>();
                        set.WithAnyTypeFromNamespace("NikSoft.ContentManager.Service");
                    });
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}