using NikSoft.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.FormBuilder.Service
{
    public interface IFormModelService : INikService<FormModel>
    {
    }
    public class FormModelService : NikService<FormModel>, IFormModelService
    {

        public FormModelService(IFbUnitOfWork uow)
            : base(uow)
        {
        }

        public override IList<FormModel> GetAllPaged(List<Expression<Func<FormModel, bool>>> predicate, int startIndex, int pageSize)
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