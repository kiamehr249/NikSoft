using NikSoft.Model;
using NikSoft.NikModel;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.Services
{
    public interface INikSettingService : INikService<NikSetting>
    {

        void ClearCahce(int portalID);

        string GetSettingValue(string settingName, NikSettingType settingModule);

        string GetSettingValue(string settingName, NikSettingType settingModule, int PortalID);

        int GetMinMaxAllowed(string settingName, NikSettingType settingModule, NikSettingMinorMax MinOrMax);

        Tuple<int, int> GetRangeValue(string settingName, NikSettingType settingModule);
    }
    public class NikSettingService : NikService<NikSetting>, INikSettingService
    {

        public NikSettingService(IUnitOfWork uow)
            : base(uow)
        {
        }


        public void ClearCahce(int portalID)
        {
            CachingProvider.Remove("NikSetting" + NikSettingType.MessagesSetting + portalID);
            CachingProvider.Remove("NikSetting" + NikSettingType.SystemSetting + portalID);
        }



        public override IList<NikSetting> GetAllPaged(List<Expression<Func<NikSetting, bool>>> predicate, int StartIndex, int PageSize)
        {
            var query = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                query = query.Where(predicate[i]);
            }
            return query.OrderByDescending(i => i.ID).Skip(StartIndex).Take(PageSize).ToList();
        }

        public string GetSettingValue(string settingName, NikSettingType settingModule)
        {
            List<NikSetting> settings = new List<NikSetting>();
            if (CachingProvider.Cache["NikSetting" + settingModule] != null)
            {
                settings = CachingProvider.GetItem<List<NikSetting>>("NikSetting" + settingModule);
            }
            else
            {
                settings = this.GetAll(t => t.SettingModule == settingModule).ToList();
                CachingProvider.Insert("NikSetting" + settingModule, settings, DateTime.Now.AddHours(1));
            }
            var i = settings.FirstOrDefault(x => x.SettingName == settingName);
            if (i != null)
            {
                return i.SettingValue;
            }
            return "";
        }

        public string GetSettingValue(string settingName, NikSettingType settingModule, int PortalID)
        {
            List<NikSetting> settings = new List<NikSetting>();
            if (CachingProvider.Cache["NikSetting" + settingModule + PortalID] != null)
            {
                settings = CachingProvider.GetItem<List<NikSetting>>("NikSetting" + settingModule + PortalID);
            }
            else
            {
                settings = this.GetAll(t => t.SettingModule == settingModule && t.PortalID == PortalID).ToList();
                CachingProvider.Insert("NikSetting" + settingModule + PortalID, settings, DateTime.Now.AddHours(1));
            }
            var i = settings.FirstOrDefault(x => x.SettingName == settingName);
            if (i != null)
            {
                return i.SettingValue;
            }
            return "";
        }

        public int GetMinMaxAllowed(string settingName, NikSettingType settingModule, NikSettingMinorMax MinOrMax)
        {
            var i = this.Find(x => x.SettingModule == settingModule && x.SettingName == settingName);
            if (i != null)
            {
                switch (MinOrMax)
                {
                    case NikSettingMinorMax.MinValue:
                        {
                            return i.MinAllowed;
                        }
                    case NikSettingMinorMax.MaxValue:
                        {
                            return i.MaxAllowed;
                        }
                }
            }
            return 0;
        }

        public Tuple<int, int> GetRangeValue(string settingName, NikSettingType settingModule)
        {
            var i = this.Find(x => x.SettingModule == settingModule && x.SettingName == settingName);
            if (i != null)
            {
                return new Tuple<int, int>(i.MinAllowed, i.MaxAllowed);
            }
            return null;
        }


    }
}