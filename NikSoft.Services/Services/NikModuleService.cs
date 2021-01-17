using NikSoft.Model;
using NikSoft.NikModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.Services
{
    public interface INikModuleService : INikService<NikModule>
    {
        int? GetModuleID(string moduleName);
    }
    public class NikModuleService : NikService<NikModule>, INikModuleService
    {

        public NikModuleService(IUnitOfWork uow)
			: base(uow) {
        }

        public override IList<NikModule> GetAllPaged(List<Expression<Func<NikModule, bool>>> predicate, int startIndex, int pageSize)
        {
            var query = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                query = query.Where(predicate[i]);
            }
            return query.OrderByDescending(i => i.ID).Skip(startIndex).Take(pageSize).ToList();
        }

        public int? GetModuleID(string moduleName)
        {
            var item = Find(t => t.ModuleKey.ToLower() == moduleName.ToLower());
            if (item == null)
            {
                return null;
            }
            return item.ID;
        }

    }
}