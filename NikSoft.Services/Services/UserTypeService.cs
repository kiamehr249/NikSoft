using NikSoft.Model;
using NikSoft.NikModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace NikSoft.Services
{
    public interface IUserTypeService : INikService<UserType>
    {
    }
    public class UserTypeService : NikService<UserType>, IUserTypeService
    {

        public UserTypeService(IUnitOfWork uow)
            : base(uow)
        {
        }

        public override IList<UserType> GetAllPaged(List<Expression<Func<UserType, bool>>> predicate, int startIndex, int pageSize)
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