using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace NikSoft.Model
{
    public class NikService<T> : INikService<T> where T : class
    {
        protected IUnitOfWork _uow;
        protected IDbSet<T> TEntity;

        protected NikService(IUnitOfWork uow)
        {
            _uow = uow;
            TEntity = _uow.Set<T>();
        }

        public virtual void Add(T entity)
        {
            TEntity.Add(entity);
        }

        public virtual void Remove(T entity)
        {
            TEntity.Remove(entity);
        }

        public virtual int Delete(string wherepart)
        {

            var queryresult1 = TEntity.Where(wherepart);

            return queryresult1.AsQueryable().Delete();
        }


        public virtual int Delete(Expression<Func<T, bool>> predicate)
        {
            var queryresult1 = TEntity.Where(predicate);
            return queryresult1.Delete();
        }

        public virtual int Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateexpression)
        {
            //var queryresult1 = TEntity.Where(predicate);
            return TEntity.Update(predicate, updateexpression);
        }

        public virtual void Remove(IList<T> entitiesToDelete)
        {
            if (entitiesToDelete.Count <= 0)
            {
                return;
            }
            foreach (var item in entitiesToDelete)
            {
                TEntity.Remove(item);
            }
        }

        public virtual T Find(Expression<Func<T, bool>> predicate)
        {
            var x = TEntity.Where(predicate).ToList();
            if (null != x && x.Count() == 1)
            {
                return x.First();
            }
            return null;
        }

        public virtual TResult Find<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selectlist)
        {
            var x = TEntity.Where(predicate).Select(selectlist).ToList();
            if (null != x && x.Count() == 1)
            {
                return x.First();
            }
            return default(TResult);
        }

        public virtual IList<T> GetRange<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderKeySelector, int range, bool desc)
        {
            if (desc)
                return TEntity.Where(predicate).OrderByDescending(orderKeySelector).Take(range).ToList();
            else
                return TEntity.Where(predicate).OrderBy(orderKeySelector).Take(range).ToList();

        }

        public virtual IList<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return TEntity.Where(predicate).ToList();
        }

        public virtual IList<TResult> GetAll<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selectList)
        {
            return TEntity.Where(predicate).Select(selectList).ToList();
        }

        public virtual IList<TResult> GetAll<TResult, TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selectList, Expression<Func<T, TKey>> orderKeySelector)
        {
            return TEntity.Where(predicate).OrderBy(orderKeySelector).Select(selectList).ToList();
        }

        public virtual IList<T> GetAll(List<Expression<Func<T, bool>>> predicate)
        {
            if (predicate.Count <= 0)
            {
                return null;
            }
            var queryresult = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                queryresult = queryresult.Where(predicate[i]);
            }
            return queryresult.ToList();
        }

        public virtual IList<TResult> GetAll<TResult>(List<Expression<Func<T, bool>>> predicate, Expression<Func<T, TResult>> selectList)
        {
            if (predicate.Count <= 0)
            {
                return null;
            }
            var queryresult1 = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                queryresult1 = queryresult1.Where(predicate[i]);
            }
            return queryresult1.Select(selectList).ToList();
        }

        public virtual IList<TResult> GetAll<TResult, TKey>(List<Expression<Func<T, bool>>> predicate, Expression<Func<T, TResult>> selectList, Expression<Func<T, TKey>> orderKeySelector)
        {
            if (predicate.Count <= 0)
            {
                return null;
            }
            var queryresult1 = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                queryresult1 = queryresult1.Where(predicate[i]);
            }
            return queryresult1.OrderBy(orderKeySelector).Select(selectList).ToList();
        }


        //public virtual IList<TResult> GetAll<TResult, TKey, TGroup, TReturn>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selectList, Expression<Func<T, TKey>> orderKeySelector
        //	, Func<TResult, TGroup> groupSelector, Func<IGrouping<TGroup, TResult>, TReturn> selector) {

        //	var z = predicate.aggre
        //		.GroupBy(groupSelector)
        //		.sel




        //	//if (predicate.Count <= 0) {
        //	//	return null;
        //	//}
        //	//var queryresult1 = TEntity.Where(predicate[0]);
        //	//for (int i = 1; i < predicate.Count; i++) {
        //	//	queryresult1 = queryresult1.Where(predicate[i]);
        //	//}
        //	//return queryresult1.OrderBy(orderKeySelector).Select(selectList).ToList();
        //}





        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return TEntity.Count(predicate);
        }

        public virtual int Count(List<Expression<Func<T, bool>>> predicates)
        {
            if (predicates.Count <= 0)
            {
                throw new Exception("Dear Devloper!, Why you send no argument to Count?!!!!?????");
            }
            var queryresult1 = TEntity.Where(predicates[0]);
            for (int i = 1; i < predicates.Count; i++)
            {
                queryresult1 = queryresult1.Where(predicates[i]);
            }
            return queryresult1.Count();
        }

        public virtual IList<T> GetAllPaged(Expression<Func<T, bool>> predicate, int startIndex, int pageSize)
        {
            throw new NotImplementedException("Dear Developer, Hello!!!, Must implement in Service Class yourself");
        }

        public virtual IList<T> GetAllPaged(List<Expression<Func<T, bool>>> predicate, int startIndex, int pageSize)
        {
            throw new NotImplementedException("Dear Developer, Hello!!!, Must implement in Service Class yourself");
        }

        public IDbSet<T> Entity
        {
            get { return TEntity; }
        }

        public int SaveChanges()
        {
            return _uow.SaveChanges();
        }

        public int SaveChanges(int userID)
        {
            return _uow.SaveChanges(userID);
        }

        public int AddAndSaveChanegs(T entity, int? UserId = null)
        {
            TEntity.Add(entity);

            if (null == UserId)
                return SaveChanges();

            return SaveChanges(UserId.Value);
        }


        public void RejectChanges()
        {
            _uow.RejectChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Reaload()
        {
            _uow.Reload();
        }

        public T Create()
        {
            return TEntity.Create();
        }

        public virtual IList<T> GetAllPaged<TKey>(List<Expression<Func<T, bool>>> predicate, Expression<Func<T, TKey>> orderKeySelector, int startIndex, int pageSize, bool desend)
        {
            if (predicate.Count <= 0)
            {
                return null;
            }
            var queryresult1 = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                queryresult1 = queryresult1.Where(predicate[i]);
            }
            if (desend)
                return queryresult1.OrderByDescending(orderKeySelector).Skip(startIndex).Take(pageSize).ToList();

            return queryresult1.OrderBy(orderKeySelector).Skip(startIndex).Take(pageSize).ToList();
        }

        public List<Expression<Func<T, bool>>> ExpressionMaker()
        {
            return new List<Expression<Func<T, bool>>>();
        }

        public IList<TResult> QueryResult<TResult>(IQueryable<TResult> queryFunction, int startIndex, int pageSize)
        {
            return queryFunction.Skip(startIndex).Take(pageSize).ToList();
        }

        public IList<TResult> QueryRun<TResult>(IQueryable<TResult> queryFunction)
        {
            return queryFunction.ToList();
        }

        public int QueryResultCount<TResult>(IQueryable<TResult> queryFunction)
        {
            return queryFunction.Count();
        }

        public IQueryable<TResult> QueryMaker<TResult>(Func<IQueryable<T>, IQueryable<TResult>> queryFunction)
        {
            return queryFunction(TEntity);
        }

        public IQueryable<T> Entities { get; set; }

        //public IList<TReturn> NewGetAllPaged<TResult, TKey, TGroup, TReturn>(
        //List<Expression<Func<T, bool>>> predicates,
        //Expression<Func<T, TResult>> firstSelector,
        //Expression<Func<TResult, TKey>> orderSelector,
        //Func<TResult, TGroup> groupSelector,
        //Func<IGrouping<TGroup, TResult>, TReturn> selector) {




        //	//TEntity
        //	//	  //.Select(z => new { z.Zeit, z.Taetigkeit.Taetigkeit1, z.Firma.Name })
        //	//	  .GroupBy(x => new { x.Taetigkeit1, x.Name })
        //	//	  .Select(g => new Evaluation {
        //	//		  companyName = g.Key.Name,
        //	//		  skillName = g.Key.Taetigkeit1,
        //	//		  time = g.Sum(y => y.Zeit)
        //	//	  });











        //	var queryResult = TEntity.Where(predicates[0]);
        //	for (int i = 1; i < predicates.Count; i++) {
        //		queryResult = queryResult.Where(predicates[i]);
        //	}
        //	return predicates
        //		.Aggregate(queryResult, (current, predicate) => current.Where(predicate))
        //		.Select(firstSelector)
        //		//.OrderBy(orderSelector)
        //		.GroupBy(groupSelector)
        //		.Select(selector)
        //		.ToList();
        //}








        //public IList<TReturn> NewGetAllPaged2<TGroupKey, TReturn, TResult>(
        //	Expression<Func<T, bool>> predicate,
        //	Func<T, TGroupKey> groupSelector,
        //	Func<IGrouping<TGroupKey, TResult>, TReturn> selector
        //		) {


        //	return TEntity.Where(predicate).ToList().Select(x=> x).GroupBy(groupSelector)
        //		  //.Select(z => new { z.Zeit, z.Taetigkeit.Taetigkeit1, z.Firma.Name })
        //		  .GroupBy(groupSelector)
        //		  .Select(selector).ToList();











        //	//var queryResult = TEntity.Where(predicates[0]);
        //	//for (int i = 1; i < predicates.Count; i++) {
        //	//	queryResult = queryResult.Where(predicates[i]);
        //	//}
        //	//return predicates
        //	//	.Aggregate(queryResult, (current, predicate) => current.Where(predicate))
        //	//	.Select(firstSelector)
        //	//	//.OrderBy(orderSelector)
        //	//	.GroupBy(groupSelector)
        //	//	.Select(selector)
        //	//	.ToList();
        //}

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return TEntity.Any(predicate);
        }



        public IList<TReturn> GetAllPaged2<TResult, TKey, TGroup, TReturn>(
               List<Expression<Func<T, bool>>> predicates,
               Expression<Func<T, TResult>> firstSelector,
               Expression<Func<TResult, TKey>> orderSelector,
               Func<TResult, TGroup> groupSelector,
               Func<IGrouping<TGroup, TResult>, TReturn> selector)
        {
            return predicates
                .Aggregate(Entities, (current, predicate) => current.Where(predicate))
                .Select(firstSelector)
                .OrderBy(orderSelector)
                .GroupBy(groupSelector)
                .Select(selector)
                .ToList();
        }

        public int CountByGroup<TResult>(Func<IQueryable<T>, IQueryable<TResult>> queryFunction)
        {
            return queryFunction(TEntity).Count();
        }


        public IList<T> GetAllPagedDynamic(Expression<Func<T, bool>> predicate, string OrderBy, int startIndex, int pageSize)
        {

            var queryresult1 = TEntity.Where(predicate);

            return queryresult1.AsQueryable().OrderBy(OrderBy).Skip(startIndex).Take(pageSize).ToList();
        }

        public IList<T> GetAllPagedDynamic(List<Expression<Func<T, bool>>> predicates, string OrderBy, int startIndex, int pageSize)
        {
            if (predicates.Count <= 0)
            {
                predicates.Add(x => true);
            }
            var queryresult1 = TEntity.Where(predicates[0]);
            for (int i = 1; i < predicates.Count; i++)
            {
                queryresult1 = queryresult1.Where(predicates[i]);
            }
            return queryresult1.AsQueryable().OrderBy(OrderBy).Skip(startIndex).Take(pageSize).ToList();
        }

        public IList<TResult> GetAllPagedDynamic<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selectlist, string OrderBy, int startIndex, int pageSize)
        {
            var queryresult1 = TEntity.Where(predicate);

            return queryresult1.Select(selectlist).AsQueryable().OrderBy(OrderBy).Skip(startIndex).Take(pageSize).ToList();
        }

        public IList<TResult> GetAllPagedDynamic<TResult>(List<Expression<Func<T, bool>>> predicate, Expression<Func<T, TResult>> selectlist, string OrderBy, int startIndex, int pageSize)
        {
            if (predicate.Count <= 0)
            {
                predicate.Add(x => true);
            }
            var queryresult1 = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                queryresult1 = queryresult1.Where(predicate[i]);
            }
            return queryresult1.Select(selectlist).AsQueryable().OrderBy(OrderBy).Skip(startIndex).Take(pageSize).ToList();
        }


        public virtual IList<TResult> GetAllPagedBySelect<TResult>(List<Expression<Func<T, bool>>> predicate, Expression<Func<T, TResult>> selectList, int startIndex, int pageSize)
        {
            throw new NotImplementedException("Dear Developer, Hello!!!, Must implement in Service Class yourself");
            if (predicate.Count <= 0)
            {
                return null;
            }
            var queryresult1 = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                queryresult1 = queryresult1.Where(predicate[i]);
            }

            return queryresult1.Select(selectList).Skip(startIndex).Take(pageSize).ToList();
        }

        public virtual IList<TResult> GetAllPagedBySelect<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selectList, int startIndex, int pageSize)
        {
            throw new NotImplementedException("Dear Developer, Hello!!!, Must implement in Service Class yourself");
        }

        //Order Descending
        public virtual IList<TResult> GetDescending<TResult, TKey>(Expression<Func<T, bool>> predicate
                    , Expression<Func<T, TResult>> selectList
                    , Expression<Func<T, TKey>> orderKeySelector)
        {
            var queryresult1 = TEntity.Where(predicate);
            return queryresult1.OrderByDescending(orderKeySelector).Select(selectList).ToList();
        }


        public virtual IList<T> GetAllDescending<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderKeySelector)
        {
            var queryresult1 = TEntity.Where(predicate);
            return queryresult1.OrderByDescending(orderKeySelector).ToList();
        }



        public virtual IList<TResult> GetDescending<TResult, TKey>(List<Expression<Func<T, bool>>> predicate
                    , Expression<Func<T, TResult>> selectList
                    , Expression<Func<T, TKey>> orderKeySelector)
        {
            if (predicate.Count <= 0)
            {
                return null;
            }
            var queryresult1 = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                queryresult1 = queryresult1.Where(predicate[i]);
            }
            return queryresult1.OrderByDescending(orderKeySelector).Select(selectList).ToList();

        }


        public virtual IList<T> GetAllDescending<TKey>(List<Expression<Func<T, bool>>> predicate, Expression<Func<T, TKey>> orderKeySelector)
        {
            if (predicate.Count <= 0)
            {
                return null;
            }
            var queryresult1 = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                queryresult1 = queryresult1.Where(predicate[i]);
            }
            return queryresult1.OrderByDescending(orderKeySelector).ToList();
        }


        public IList<TResult> GetDescending<TResult, TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selectList, Expression<Func<T, TKey>> orderKeySelector, int skipNo, int TakeNo)
        {
            var queryresult1 = TEntity.Where(predicate).OrderByDescending(orderKeySelector).Skip(skipNo).Take(TakeNo);
            return queryresult1.Select(selectList).ToList();
        }


        public IList<TResult> GetDescending<TResult, TKey>(List<Expression<Func<T, bool>>> predicate, Expression<Func<T, TResult>> selectList, Expression<Func<T, TKey>> orderKeySelector, int skipNo, int TakeNo)
        {

            if (predicate.Count <= 0)
            {
                return null;
            }
            var queryresult1 = TEntity.Where(predicate[0]);
            for (int i = 1; i < predicate.Count; i++)
            {
                queryresult1 = queryresult1.Where(predicate[i]);
            }
            //TODO: BAD BUG
            var queryresult2 = queryresult1.OrderByDescending(orderKeySelector).Skip(skipNo).Take(TakeNo);
            return queryresult2.Select(selectList).ToList();
        }




    }
}