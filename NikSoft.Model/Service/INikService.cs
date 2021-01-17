using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace NikSoft.Model
{
    public interface INikService<T> : IDisposable where T : class
    {

        void Reaload();

        void Add(T entity);

        int AddAndSaveChanegs(T entity, int? UserId = null);

        void Remove(T entity);

        int Delete(string wherepart);
        int Delete(Expression<Func<T, bool>> predicate);
        int Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateexpression);

        void Remove(IList<T> entitiesToDelete);

        T Find(Expression<Func<T, bool>> predicate);


        TResult Find<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selectList);

        IList<T> GetRange<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderKeySelector, int range, bool desc);

        IList<T> GetAll(Expression<Func<T, bool>> predicate);

        IList<TResult> GetAll<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selectList);

        IList<TResult> GetAll<TResult, TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selectList, Expression<Func<T, TKey>> orderKeySelector);

        IList<T> GetAll(List<Expression<Func<T, bool>>> predicate);

        IList<TResult> GetAll<TResult>(List<Expression<Func<T, bool>>> predicate, Expression<Func<T, TResult>> selectList);

        IList<TResult> GetAll<TResult, TKey>(List<Expression<Func<T, bool>>> predicate, Expression<Func<T, TResult>> selectList, Expression<Func<T, TKey>> orderKeySelector);

        int Count(Expression<Func<T, bool>> predicate);

        int Count(List<Expression<Func<T, bool>>> predicate);

        IList<T> GetAllPaged(Expression<Func<T, bool>> predicate, int startIndex, int pageSize);

        IList<T> GetAllPaged(List<Expression<Func<T, bool>>> predicate, int startIndex, int pageSize);

        IDbSet<T> Entity { get; }

        int SaveChanges();
        int SaveChanges(int UserID);

        void RejectChanges();

        T Create();

        bool Any(Expression<Func<T, bool>> predicate);

        IList<T> GetAllPaged<TKey>(List<Expression<Func<T, bool>>> predicate, Expression<Func<T, TKey>> orderKeySelector, int startIndex, int pageSize, bool desend);

        List<Expression<Func<T, bool>>> ExpressionMaker();

        IList<TResult> QueryResult<TResult>(IQueryable<TResult> queryFunction, int startIndex, int pageSize);

        int QueryResultCount<TResult>(IQueryable<TResult> queryFunction);

        IQueryable<TResult> QueryMaker<TResult>(Func<IQueryable<T>, IQueryable<TResult>> queryFunction);
        IList<TResult> QueryRun<TResult>(IQueryable<TResult> queryFunction);


        int CountByGroup<TResult>(Func<IQueryable<T>, IQueryable<TResult>> queryFunction);

        //IList<TReturn> NewGetAllPaged<TResult, TKey, TGroup, TReturn>(
        //List<Expression<Func<T, bool>>> predicates,
        //Expression<Func<T, TResult>> firstSelector,
        //Expression<Func<TResult, TKey>> orderSelector,
        //Func<TResult, TGroup> groupSelector,
        //Func<IGrouping<TGroup, TResult>, TReturn> selector);


        IList<T> GetAllPagedDynamic(Expression<Func<T, bool>> predicate, string OrderBy, int startIndex, int pageSize);
        IList<TResult> GetAllPagedDynamic<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selectlist, string OrderBy, int startIndex, int pageSize);

        IList<T> GetAllPagedDynamic(List<Expression<Func<T, bool>>> predicate, string OrderBy, int startIndex, int pageSize);
        IList<TResult> GetAllPagedDynamic<TResult>(List<Expression<Func<T, bool>>> predicate, Expression<Func<T, TResult>> selectlist, string OrderBy, int startIndex, int pageSize);






        IList<TReturn> GetAllPaged2<TResult, TKey, TGroup, TReturn>(
               List<Expression<Func<T, bool>>> predicates,
               Expression<Func<T, TResult>> firstSelector,
               Expression<Func<TResult, TKey>> orderSelector,
               Func<TResult, TGroup> groupSelector,
               Func<IGrouping<TGroup, TResult>, TReturn> selector);



        IList<TResult> GetAllPagedBySelect<TResult>(List<Expression<Func<T, bool>>> predicate, Expression<Func<T, TResult>> selectList, int startIndex, int pageSize);
        IList<TResult> GetAllPagedBySelect<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selectList, int startIndex, int pageSize);


        IList<TResult> GetDescending<TResult, TKey>(Expression<Func<T, bool>> predicate
            , Expression<Func<T, TResult>> selectList
            , Expression<Func<T, TKey>> orderKeySelector);


        IList<TResult> GetDescending<TResult, TKey>(Expression<Func<T, bool>> predicate
                    , Expression<Func<T, TResult>> selectList
                    , Expression<Func<T, TKey>> orderKeySelector, int skipNo, int TakeNo);


        IList<TResult> GetDescending<TResult, TKey>(List<Expression<Func<T, bool>>> predicate
                    , Expression<Func<T, TResult>> selectList
                    , Expression<Func<T, TKey>> orderKeySelector, int skipNo, int TakeNo);



        IList<T> GetAllDescending<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderKeySelector);


        IList<TResult> GetDescending<TResult, TKey>(List<Expression<Func<T, bool>>> predicate
                    , Expression<Func<T, TResult>> selectList
                    , Expression<Func<T, TKey>> orderKeySelector);


        IList<T> GetAllDescending<TKey>(List<Expression<Func<T, bool>>> predicate, Expression<Func<T, TKey>> orderKeySelector);




    }
}