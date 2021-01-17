using NikSoft.FormBuilder.Service;
using NikSoft.PlugIn;
using StructureMap;
using System;
using System.ComponentModel.Composition;
using System.Data.Entity;

namespace NikSoft.FormBuilder.Web
{
    [Export(typeof(IPlugin))]
    [ExportMetadata("PluginName", "FormBuilder")]
    [ExportMetadata("PlugInVersion", "1.0.0")]
    public class PluginInstaller : IPlugin
    {

        public void Initialize()
        {
            try
            {
                Database.SetInitializer<FbContext>(null);
                ObjectFactory.Configure(config => {
                    config.For<IFbUnitOfWork>().HttpContextScoped().Use<FbContext>();
                    config.SetAllProperties(set => {
                        set.OfType<IFbUnitOfWork>();
                        set.WithAnyTypeFromNamespace("NikSoft.FormBuilder.Service");
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