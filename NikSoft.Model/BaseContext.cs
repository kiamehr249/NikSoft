using NikSoft.Utilities;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Web;

namespace NikSoft.Model
{
    public class BaseContext : DbContext, IUnitOfWork
    {

        public BaseContext()
            : base()
        {
        }



        public BaseContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //entityva
        }

        public void RejectChanges()
        {
            foreach (var entry in this.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        {
                            entry.State = EntityState.Unchanged;
                            break;
                        }
                    case EntityState.Added:
                        {
                            entry.State = EntityState.Detached;
                            break;
                        }
                }
            }
        }

        public override int SaveChanges()
        {
            ApplyYeKeCorrection();
            AuditFields();
            try
            {
                return base.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                ;
            }
            return 0;
        }

        public int SaveChanges(int userID)
        {

            ApplyYeKeCorrection();
            AuditFields(userID);
            try
            {

                return base.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    HttpContext.Current.Response.Write("Entity of type <b>" + eve.Entry.Entity.GetType().Name
                        + "</b>in state <b>" + eve.Entry.State + "</b> has the following validation errors:<br />");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        HttpContext.Current.Response.Write("- Property: <b>" + ve.PropertyName + "</b>, Error: <b>" + ve.ErrorMessage + "</b><br />");
                    }
                }
                //throw;
            }
            return -1;

        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void Reload()
        {
            foreach (var entry in this.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        {
                            entry.Reload();
                            break;
                        }
                }
            }
        }

        private void ApplyYeKeCorrection()
        {
            var page = HttpContext.Current.Handler;
            if (page != null)
            {
                if (page.HasProperty("ConvertYeKe"))
                {
                    var propInfos = page.GetPropValue("ConvertYeKe");
                    if (propInfos != null)
                    {
                        var tmp = true;
                        if (bool.TryParse(propInfos.ToString(), out tmp))
                        {
                            if (tmp == false)
                            {
                                return;
                            }
                        }
                    }
                }
            }
            var changedEntities = this.ChangeTracker
                                      .Entries()
                                      .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var item in changedEntities)
            {
                if (item.Entity == null) continue;
                var propertyInfos = item.Entity.GetType().GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                    ).Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));
                var pr = new PropertyReflector();
                foreach (var propertyInfo in propertyInfos)
                {
                    var propName = propertyInfo.Name;
                    var val = pr.GetValue(item.Entity, propName);
                    if (val != null)
                    {
                        var newVal = val.ToString().Replace("ي", "ی").Replace("ك", "ک").Replace("ي", "ی");
                        if (newVal == val.ToString()) continue;
                        pr.SetValue(item.Entity, propName, newVal);
                    }
                }
            }
        }

        private void AuditFields(int userID = 0)
        {
            var auditDate = DateTime.Now;
            foreach (var entry in this.ChangeTracker.Entries<LogEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        {
                            entry.Entity.CreateDateTime = auditDate;
                            entry.Entity.LastModifiedDateTime = auditDate;
                            if (userID != 0)
                            {
                                entry.Entity.CreatedBy = userID;
                                entry.Entity.ModifiedBy = userID;
                            }
                            break;
                        }
                    case EntityState.Modified:
                        {
                            entry.Entity.LastModifiedDateTime = auditDate;
                            if (userID != 0)
                            {
                                entry.Entity.ModifiedBy = userID;
                            }
                            break;
                        }
                }
            }
        }
    }
}