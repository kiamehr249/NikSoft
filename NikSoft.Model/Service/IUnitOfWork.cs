using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace NikSoft.Model
{
    public interface IUnitOfWork
    {

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges(int UserID);

        int SaveChanges();

        void RejectChanges();

        void Reload();

        DbChangeTracker ChangeTracker { get; }
    }
}