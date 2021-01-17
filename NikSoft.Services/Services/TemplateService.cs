using NikSoft.Model;
using NikSoft.NikModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI.WebControls;

namespace NikSoft.Services
{
    public interface ITemplateService : INikService<Template> {
        string GetTemplateType(TemplateType ptt);
        List<ListItem> GetTemplates();
        void SetSelected(int ID);
    }

    public class TemplateService : NikService<Template>, ITemplateService
    {

        public TemplateService(IUnitOfWork uow)
			: base(uow) {
        }

        public override IList<Template> GetAllPaged(List<Expression<Func<Template, bool>>> predicate, int startIndex, int pageSize)
        {
            var query = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                query = query.Where(predicate[i]);
            }
            return query.OrderByDescending(i => i.ID).Skip(startIndex).Take(pageSize).ToList();
        }

        public string GetTemplateType(TemplateType ptt)
        {
            switch (ptt)
            {
                case TemplateType.HomePage:
                    {
                        return "صفحه اصلی";
                    }
                case TemplateType.InnerPage:
                    {
                        return "صفحه داخلی";
                    }
                case TemplateType.PanelHome:
                    {
                        return "صفحه اصلی پنل";
                    }
                case TemplateType.PanelInner:
                    {
                        return "صفحه داخلی پنل";
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }

        public List<ListItem> GetTemplates()
        {
            var list = new List<ListItem>();
            list.Add(new ListItem("صفحه اصلی", "1"));
            list.Add(new ListItem("صفحه داخلی", "2"));
            list.Add(new ListItem("صفحه اصلی پنل", "3"));
            list.Add(new ListItem("صفحه داخلی پنل", "4"));
            return list;
        }

        public void SetSelected(int ID)
        {
            var Stemplate = this.Find(t => t.ID == ID && t.ModuleKey == string.Empty);
            if (Stemplate == null)
            {
                return;
            }
            if (Stemplate != null)
            {
                var tems = this.GetAll(t => t.PortalID == Stemplate.PortalID && t.Type == Stemplate.Type && t.ModuleKey == string.Empty);
                foreach (var item in tems)
                {
                    item.IsSelected = false;
                }
                Stemplate.IsSelected = true;
                SaveChanges();
            }
        }
    }
}