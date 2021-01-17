using NikSoft.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.ContentManager.Service
{
    public interface IContentFileService : INikService<ContentFile>
    {
    }
    public class ContentFileService : NikService<ContentFile>, IContentFileService
    {

        public ContentFileService(ICMUnitOFWork uow)
            : base(uow)
        {
        }

        public override IList<ContentFile> GetAllPaged(List<Expression<Func<ContentFile, bool>>> predicate, int startIndex, int pageSize)
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