using NikSoft.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.FormBuilder.Service
{
    public interface IFileUploadModelService : INikService<FileUploadModel>
    {
    }
    public class FileUploadModelService : NikService<FileUploadModel>, IFileUploadModelService
    {

        public FileUploadModelService(IFbUnitOfWork uow)
            : base(uow)
        {
        }

        public override IList<FileUploadModel> GetAllPaged(List<Expression<Func<FileUploadModel, bool>>> predicate, int startIndex, int pageSize)
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