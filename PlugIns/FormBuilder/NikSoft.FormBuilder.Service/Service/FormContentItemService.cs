using NikSoft.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.FormBuilder.Service
{
    public interface IFormContentItemService : INikService<FormContentItem>
    {
    }
    public class FormContentItemService : NikService<FormContentItem>, IFormContentItemService
    {

        public FormContentItemService(IFbUnitOfWork uow)
            : base(uow)
        {
        }

        public override IList<FormContentItem> GetAllPaged(List<Expression<Func<FormContentItem, bool>>> predicate, int startIndex, int pageSize)
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