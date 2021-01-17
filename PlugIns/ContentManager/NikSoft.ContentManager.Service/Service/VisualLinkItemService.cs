using NikSoft.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.ContentManager.Service
{
    public interface IVisualLinkItemService : INikService<VisualLinkItem>
    {
    }
    public class VisualLinkItemService : NikService<VisualLinkItem>, IVisualLinkItemService
    {

        public VisualLinkItemService(ICMUnitOFWork uow)
            : base(uow)
        {
        }

        public override IList<VisualLinkItem> GetAllPaged(List<Expression<Func<VisualLinkItem, bool>>> predicate, int startIndex, int pageSize)
        {
            var query = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                query = query.Where(predicate[i]);
            }
            return query.OrderBy(i => i.Ordering).Skip(startIndex).Take(pageSize).ToList();
        }
    }
}