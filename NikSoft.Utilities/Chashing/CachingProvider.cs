using System.Collections;
using System.Web;
using System.Web.Caching;

namespace NikSoft.Utilities
{
    public class CachingProvider
    {
        private static System.Web.Caching.Cache objCache;

        public static System.Web.Caching.Cache Cache
        {
            get
            {
                if (objCache == null)
                {
                    objCache = HttpRuntime.Cache;
                }
                return objCache;
            }
        }

        public static IDictionaryEnumerator GetEnumerator()
        {
            return Cache.GetEnumerator();
        }

        public static object GetItem(string CacheKey)
        {
            return Cache[CacheKey];
        }

        public static T GetItem<T>(string CacheKey)
        {
            T obj;
            obj = (T)Cache[CacheKey];
            return obj;
        }

        public static void Remove(string CacheKey)
        {
            if (Cache[CacheKey] != null)
            {
                Cache.Remove(CacheKey);
            }
        }

        public static void Insert(string CacheKey, object objObject)
        {
            Insert(CacheKey, objObject, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }

        public static void Insert(string CacheKey, object objObject, System.DateTime AbsoluteExpiration)
        {
            Insert(CacheKey, objObject, AbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }

        public static void Insert(string CacheKey, object objObject, System.DateTime AbsoluteExpiration, System.TimeSpan SlidingExpiration)
        {
            Insert(CacheKey, objObject, AbsoluteExpiration, SlidingExpiration, CacheItemPriority.Default, null);
        }

        public static void Insert(string CacheKey, object objObject, System.DateTime AbsoluteExpiration, System.TimeSpan SlidingExpiration, CacheItemPriority Priority)
        {
            Insert(CacheKey, objObject, AbsoluteExpiration, SlidingExpiration, Priority, null);
        }

        public static void Insert(string CacheKey, object Value, System.DateTime AbsoluteExpiration, System.TimeSpan SlidingExpiration, CacheItemPriority Priority, CacheItemRemovedCallback OnRemoveCallback)
        {
            Insert(CacheKey, Value, null, AbsoluteExpiration, SlidingExpiration, Priority, OnRemoveCallback);
        }

        public static void Insert(string CacheKey, object Value, CacheDependency dependencies, System.DateTime AbsoluteExpiration, System.TimeSpan SlidingExpiration, CacheItemPriority Priority, CacheItemRemovedCallback OnRemoveCallback)
        {
            Cache.Insert(CacheKey, Value, dependencies, AbsoluteExpiration, SlidingExpiration, Priority, OnRemoveCallback);
        }
    }
}