using System;
using System.IO;
using System.Web;

namespace NikSoft.WebService
{
    public class FileManager
    {
        public FileManager(DirectoryInfo di)
        {
            Name = di.Name;
            IsFolder = true;
            CreateDate = di.CreationTime.ToString();
            Size = string.Empty;
            FullName = di.FullName;
        }

        public FileManager(FileInfo fi)
        {
            Name = fi.Name;
            IsFolder = false;
            CreateDate = fi.CreationTime.ToString();
            Size = fi.Length.ToString();
            FullName = fi.FullName;
        }

        public FileManager(string name, string path, string fullName)
        {
            Name = name;
            CreateDate = string.Empty;
            Size = string.Empty;
            IsFolder = true;
            FullName = fullName;
        }

        private readonly string FullName = string.Empty;
        public string Name { get; set; }
        public string Url
        {
            get
            {
                if (IsFolder)
                {
                    return string.Empty;
                }
                var path = string.Empty;
                var physicalRootFolder = HttpContext.Current.Request.PhysicalApplicationPath;
                physicalRootFolder = physicalRootFolder.Substring(0, physicalRootFolder.LastIndexOf(@"\"));
                path = FullName.Replace(physicalRootFolder, HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority).ToString());
                path = path.Replace("\\", "/");
                return path;
            }
        }
        public string CreateDate { get; set; }
        public string Size { get; set; }
        public bool IsFolder { get; set; }
        public string Path
        {
            get
            {
                return FullName.Replace(HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"], String.Empty).Replace("\\", "/");
            }
        }
    }
}