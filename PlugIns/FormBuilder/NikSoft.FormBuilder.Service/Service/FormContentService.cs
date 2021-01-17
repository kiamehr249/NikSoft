using NikSoft.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.FormBuilder.Service
{
    public interface IFormContentService : INikService<FormContent>
    {
    }
    public class FormContentService : NikService<FormContent>, IFormContentService
    {

        public FormContentService(IFbUnitOfWork uow)
            : base(uow)
        {
        }

        public override IList<FormContent> GetAllPaged(List<Expression<Func<FormContent, bool>>> predicate, int startIndex, int pageSize)
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