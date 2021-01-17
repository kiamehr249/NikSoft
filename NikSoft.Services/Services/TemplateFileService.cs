﻿using NikSoft.Model;
using NikSoft.NikModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.Services
{
    public interface ITemplateFileService : INikService<TemplateFile>
    {
    }
    public class TemplateFileService : NikService<TemplateFile>, ITemplateFileService
    {

        public TemplateFileService(IUnitOfWork uow)
			: base(uow) {
        }

        public override IList<TemplateFile> GetAllPaged(List<Expression<Func<TemplateFile, bool>>> predicate, int startIndex, int pageSize)
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