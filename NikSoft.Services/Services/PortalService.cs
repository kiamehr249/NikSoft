using NikSoft.Model;
using NikSoft.NikModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.Services
{
    public interface IPortalService : INikService<Portal> {
        string GetPortalName(int? pID);
        string GetPortalName2(object pID);
    }

    public class PortalService : NikService<Portal>, IPortalService
    {

        public PortalService(IUnitOfWork uow)
			: base(uow) {
        }

        public override IList<Portal> GetAllPaged(List<Expression<Func<Portal, bool>>> predicate, int startIndex, int pageSize)
        {
            var query = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                query = query.Where(predicate[i]);
            }
            return query.OrderByDescending(i => i.ID).Skip(startIndex).Take(pageSize).ToList();
        }

        public string GetPortalName(int? pID)
        {
            var pName = Find(x => x.ID == pID);
            if (pName != null)
                return pName.Title;
            return "";
        }

        public string GetPortalName2(object pID)
        {
            if (pID == null)
                return "";
            var Portalid = int.Parse(pID.ToString());
            var pName = Find(x => x.ID == Portalid);
            if (pName != null)
                return pName.Title;
            return "";
        }
    }
}