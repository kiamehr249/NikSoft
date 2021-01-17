using NikSoft.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.ContentManager.Service
{
    public interface IContentCategoryService : INikService<ContentCategory>
    {
    }

    public class ContentCategoryService : NikService<ContentCategory>, IContentCategoryService
    {

        public ContentCategoryService(ICMUnitOFWork uow)
            : base(uow)
        {
        }

        public override IList<ContentCategory> GetAllPaged(List<Expression<Func<ContentCategory, bool>>> predicate, int startIndex, int pageSize)
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