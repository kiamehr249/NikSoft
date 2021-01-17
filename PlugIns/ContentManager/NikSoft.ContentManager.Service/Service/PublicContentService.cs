using NikSoft.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.ContentManager.Service
{
    public interface IPublicContentService : INikService<PublicContent>
    {
    }

    public class PublicContentService : NikService<PublicContent>, IPublicContentService
    {

        public PublicContentService(ICMUnitOFWork uow)
            : base(uow)
        {
        }

        public override IList<PublicContent> GetAllPaged(List<Expression<Func<PublicContent, bool>>> predicate, int startIndex, int pageSize)
        {
            var query = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                query = query.Where(predicate[i]);
            }
            return query.OrderByDescending(i => i.Ordering).Skip(startIndex).Take(pageSize).ToList();
        }
    }
}