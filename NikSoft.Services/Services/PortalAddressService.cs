using NikSoft.Model;
using NikSoft.NikModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.Services
{
    public interface IPortalAddressService : INikService<PortalAddress>
    {

    }
    public class PortalAddressService : NikService<PortalAddress>, IPortalAddressService
    {

        public PortalAddressService(IUnitOfWork uow)
            : base(uow)
        {
        }

        public override IList<PortalAddress> GetAllPaged(List<Expression<Func<PortalAddress, bool>>> predicate, int startIndex, int pageSize)
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