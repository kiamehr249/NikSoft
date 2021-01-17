using NikSoft.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.ContentManager.Service
{
    public interface ICategoryPermissionService : INikService<CategoryPermission>
    {
    }
    public class CategoryPermissionService : NikService<CategoryPermission>, ICategoryPermissionService
    {

        public CategoryPermissionService(ICMUnitOFWork uow)
            : base(uow)
        {
        }

        public override IList<CategoryPermission> GetAllPaged(List<Expression<Func<CategoryPermission, bool>>> predicate, int startIndex, int pageSize)
        {
            var query = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                query = query.Where(predicate[i]);
            }
            return query.OrderBy(i => i.ID).Skip(startIndex).Take(pageSize).ToList();
        }
    }
}