using System;
using System.IO;
using System.Web;

namespace NikSoft.Utilities
{
    public class CacheHelper
    {
        public static String GetPathToCacheDependencyFile(String cacheDependencyKey)
        {
            if (HttpContext.Current == null) return null;

            return HttpContext.Current.Server.MapPath("~/files/Cache/" + cacheDependencyKey + "cachedependecy.config");
        }

        public static void EnsureCacheFile(string pathToCacheFile)
        {
            if (pathToCacheFile == null) return;

            if (!File.Exists(pathToCacheFile))
            {
                TouchCacheFile(pathToCacheFile);
            }
        }

        public static void TouchCacheFile(String pathToCacheFile)
        {
            if (pathToCacheFile == null) return;
            try
            {
                if (File.Exists(pathToCacheFile))
                {
                    File.SetLastWriteTimeUtc(pathToCacheFile, DateTime.UtcNow);
                }
                else
                {
                    File.CreateText(pathToCacheFile).Close();
                }
            }
            catch
            {
            }
        }
    }
}