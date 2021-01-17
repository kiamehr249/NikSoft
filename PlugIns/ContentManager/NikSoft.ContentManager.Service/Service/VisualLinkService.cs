using NikSoft.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.ContentManager.Service
{
    public interface IVisualLinkService : INikService<VisualLink>
    {
    }
    public class VisualLinkService : NikService<VisualLink>, IVisualLinkService
    {

        public VisualLinkService(ICMUnitOFWork uow)
            : base(uow)
        {
        }

        public override IList<VisualLink> GetAllPaged(List<Expression<Func<VisualLink, bool>>> predicate, int startIndex, int pageSize)
        {
            var query = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                query = query.Where(predicate[i]);
            }
            return query.OrderByDescending(i => i.ID).Skip(startIndex).Take(pageSize).ToList();
        }
    }
}