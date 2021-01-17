using NikSoft.Model;
using NikSoft.NikModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.Services
{
    public interface IThemeService : INikService<Theme>
    {
    }
    public class ThemeService : NikService<Theme>, IThemeService
    {

        public ThemeService(IUnitOfWork uow)
            : base(uow)
        {
        }

        public override IList<Theme> GetAllPaged(List<Expression<Func<Theme, bool>>> predicate, int startIndex, int pageSize)
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