using NikSoft.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.ContentManager.Service
{
    public interface IGroupPermissionService : INikService<GroupPermission>
    {
    }
    public class GroupPermissionService : NikService<GroupPermission>, IGroupPermissionService
    {

        public GroupPermissionService(ICMUnitOFWork uow)
            : base(uow)
        {
        }

        public override IList<GroupPermission> GetAllPaged(List<Expression<Func<GroupPermission, bool>>> predicate, int startIndex, int pageSize)
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