using NikSoft.Model;
using NikSoft.NikModel;
using NikSoft.Utilities;
using StructureMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.Services
{
    public interface INikModuleDefinitionService : INikService<NikModuleDefinition>
    {

        InstallState InstallPlugIn(string moduleTitle, string version, ref int moduleDefID, out string currentVersion);

        InstallState InstallSql(string path, InstallState lastState, string newVersion, string currentVersion);

        void CopyFolder(string sourceFolder, string destFolder, string moduleName, int moduleDefID);

        void DownGradeSql(string path, string currentVersion, string newVersion);

        void UpGradeSql(string path, string currentVersion, string newVersion);
    }

    public class NikModuleDefinitionService : NikService<NikModuleDefinition>, INikModuleDefinitionService
    {

        public NikModuleDefinitionService(IUnitOfWork uow)
			: base(uow) {
        }

        public override IList<NikModuleDefinition> GetAllPaged(List<Expression<Func<NikModuleDefinition, bool>>> predicate, int startIndex, int PageSize)
        {
            var query = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                query = query.Where(predicate[i]);
            }
            return query.OrderByDescending(i => i.ID).Skip(startIndex).Take(PageSize).ToList();
        }

        public InstallState InstallPlugIn(string moduleTitle, string version, ref int moduleDefID, out string currentVersion)
        {
            currentVersion = string.Empty;
            if (moduleTitle == null || version == null)
            {
                return InstallState.NothingHappend;
            }
            else if (string.IsNullOrWhiteSpace(moduleTitle) || string.IsNullOrWhiteSpace(version))
            {
                return InstallState.NothingHappend;
            }
            var module = Find(t => t.Title == moduleTitle);
            if (module == null)
            {
                module = Create();
                module.Title = moduleTitle;
                module.Version = version;
                module.Description = string.Empty;
                Add(module);
                SaveChanges();
                moduleDefID = module.ID;
                return InstallState.Installing;
            }
            else
            {
                if (module.Version.CompareTo(version) < 0)
                {
                    currentVersion = module.Version;
                    module.Version = version;
                    SaveChanges();
                    moduleDefID = module.ID;
                    return InstallState.Upgrade;
                }
                if (module.Version.CompareTo(version) > 0)
                {
                    moduleDefID = module.ID;
                    return InstallState.NotLatestVersion;
                }
                else
                {
                    moduleDefID = module.ID;
                    return InstallState.Installed;
                }
            }
        }

        public InstallState InstallSql(string path, InstallState lastState, string newVersion, string currentVersion)
        {
            using (var tdb = new NikContext())
            {
                if (lastState == InstallState.Installed || lastState == InstallState.NothingHappend || lastState == InstallState.NotLatestVersion)
                {
                    return InstallState.NothingHappend;
                }
                var dir = new DirectoryInfo(path);
                List<FileInfo> files = new List<FileInfo>();
                if (lastState == InstallState.Installing || lastState == InstallState.Upgrade)
                {
                    if (currentVersion.IsEmpty())
                    {
                        files = dir.GetFiles("U*.sql").ToList().Where(t => t.Name.Replace("U", "").Replace(".sql", "").CompareTo(newVersion) <= 0).ToList();
                    }
                    else
                    {
                        files = dir.GetFiles("U*.sql").ToList().Where(t => t.Name.Replace("U", "").Replace(".sql", "").CompareTo(currentVersion) > 0 && t.Name.Replace("U", "").Replace(".sql", "").CompareTo(newVersion) <= 0).ToList();
                    }
                }
                files = files.OrderBy(t => t.Name).ToList();
                foreach (var file in files)
                {
                    string script = file.OpenText().ReadToEnd().Replace("GO", string.Empty);
                    var splited = script.Split(new string[] { @"/*[RayanSQL]*/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (var split in splited)
                    {
                        var af = tdb.Database.ExecuteSqlCommand(split.Trim());
                    }
                }
                return InstallState.Installing;
            }
        }

        public void DownGradeSql(string path, string currentVersion, string newVersion)
        {
            using (var tdb = new NikContext())
            {
                var dir = new DirectoryInfo(path);
                List<FileInfo> files = new List<FileInfo>();
                files = dir.GetFiles("D*.sql").ToList().Where(t => t.Name.Replace("D", "").Replace(".sql", "").CompareTo(currentVersion) <= 0 && t.Name.Replace("D", "").Replace(".sql", "").CompareTo(newVersion) > 0).ToList();
                files = files.OrderBy(t => t.Name).ToList();
                foreach (var file in files)
                {
                    string script = file.OpenText().ReadToEnd().Replace("GO", string.Empty);
                    var splited = script.Split(new string[] { @"/*[RayanSQL]*/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (var split in splited)
                    {
                        var af = tdb.Database.ExecuteSqlCommand(split.Trim());
                    }
                }
            }
        }

        public void UpGradeSql(string path, string currentVersion, string newVersion)
        {
            using (var tdb = new NikContext())
            {
                var dir = new DirectoryInfo(path);
                List<FileInfo> files = new List<FileInfo>();
                files = dir.GetFiles("U*.sql").ToList().Where(t => t.Name.Replace("U", "").Replace(".sql", "").CompareTo(currentVersion) > 0 && t.Name.Replace("U", "").Replace(".sql", "").CompareTo(newVersion) <= 0).ToList();
                files = files.OrderBy(t => t.Name).ToList();
                foreach (var file in files)
                {
                    string script = file.OpenText().ReadToEnd().Replace("GO", string.Empty);
                    var splited = script.Split(new string[] { @"/*[RayanSQL]*/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (var split in splited)
                    {
                        var af = tdb.Database.ExecuteSqlCommand(split.Trim());
                    }
                }
            }
        }

        public void CopyFolder(string sourceFolder, string destFolder, string moduleName, int moduleDefID)
        {
            var iRayanModuelServ = ObjectFactory.GetInstance<INikModuleService>();

            if (!Directory.Exists(sourceFolder))
            {
                return;
            }
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            var files = Directory.GetFiles(sourceFolder).ToList();
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                string moduleKey = Path.GetFileNameWithoutExtension(file);
                string moduleFile = dest.Replace(System.Web.HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"], String.Empty);
                File.Copy(file, dest, true);
                if (iRayanModuelServ.Any(t => t.ModuleDefinitionID == moduleDefID && t.ModuleKey == moduleKey && t.ModuleFile == moduleFile))
                {
                    continue;
                }
                var m = iRayanModuelServ.Create();
                m.ModuleDefinitionID = moduleDefID;
                m.Title = moduleName;
                m.ModuleKey = moduleKey;
                m.ModuleFile = moduleFile;
                m.IsXMLBase = false;
                m.LoginRequired = true;
                iRayanModuelServ.Add(m);
                SaveChanges();
            }
            var folders = Directory.GetDirectories(sourceFolder).ToList();
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest, moduleName, moduleDefID);
            }
        }
    }
}