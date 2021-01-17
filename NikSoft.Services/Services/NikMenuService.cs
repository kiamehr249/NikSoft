using NikSoft.Model;
using NikSoft.NikModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.Services
{
    public interface INikMenuService : INikService<NikMenu>
    {
    }
    public class NikMenuService : NikService<NikMenu>, INikMenuService
    {

        public NikMenuService(IUnitOfWork uow)
            : base(uow)
        {
        }

        public override IList<NikMenu> GetAllPaged(List<Expression<Func<NikMenu, bool>>> predicate, int startIndex, int pageSize)
        {
            var query = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                query = query.Where(predicate[i]);
            }
            return query.OrderBy(i => i.ParentID).ThenBy(t => t.Ordering).Skip(startIndex).Take(pageSize).ToList();
        }
    }
}