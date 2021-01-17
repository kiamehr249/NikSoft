using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace NikSoft.PlugIn
{
    public class Importer
    {

        [ImportMany]
        private IEnumerable<Lazy<IPlugin, IMetadata>> operations = null;

        public void DoImport()
        {
            try
            {
                var catalog = new AggregateCatalog();
                catalog.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(HttpContext.Current.Server.MapPath("~/bin/"))));
                CompositionContainer container = new CompositionContainer(catalog);
                container.ComposeParts(this);
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (var item in ex.LoaderExceptions)
                {
                    using (StreamWriter w = File.AppendText(HttpContext.Current.Server.MapPath("~/Files/log.txt")))
                    {
                        Log("Message", item.Message, w);
                        if (null != item.InnerException)
                        {
                            Log("InnerException", item.InnerException.Message, w);
                        }
                        Log("StackTrace", item.StackTrace, w);
                    }
                }
                //throw ex;
            }
        }

        public static void Log(string logTitle, string logMessage, TextWriter w)
        {
            w.Write("\r\n" + logTitle + ": ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }


        public void DoImport(string path, List<string> assemblyName)
        {
            foreach (var item in assemblyName)
            {
                new CompositionContainer(new AssemblyCatalog(Assembly.LoadFrom(path + item))).ComposeParts(this);
                if (OperationCount == 1)
                {
                    return;
                }
            }
        }

        public int OperationCount
        {
            get { return operations != null ? operations.Count() : 0; }
        }

        public void CallInitializer()
        {
            foreach (Lazy<IPlugin, IMetadata> com in operations)
            {
                com.Value.Initialize();
            }
        }

        public IMetadata GetMetaData()
        {
            if (OperationCount == 0)
            {
                return null;
            }
            var com = operations.First() as Lazy<IPlugin, IMetadata>;
            return com.Metadata;
        }
    }
}