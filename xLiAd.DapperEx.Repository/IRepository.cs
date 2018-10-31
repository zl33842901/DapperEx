using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using xLiAd.DapperEx.MsSql.Core.Model;

namespace xLiAd.DapperEx.Repository
{
    public interface IRepository<T>
    {
        void Dispose();
        List<T> All();
        List<T> Where(Expression<Func<T, bool>> predicate);
        List<TResult> WhereSelect<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        List<T> WhereOrder<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, int top = 0);
        List<TResult> WhereOrderSelect<TKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, Expression<Func<T, TResult>> selector, int top = 0);
        int Add(T obj);
        PageList<T> PageList<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> orderBy, int pageindex = 1, int pagesize = 50, bool desc = false);
        PageList<T> PageList(Expression<Func<T, bool>> filter, int pageindex = 1, int pagesize = 50, params Tuple<Expression<Func<T, object>>, SortOrder>[] orders);
        PageList<TResult> PageListSelect<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector, int pageindex = 1, int pagesize = 50, params Tuple<Expression<Func<T, object>>, SortOrder>[] orders);
        T Find(Expression<Func<T, bool>> predicate);
        TResult FindField<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> keySelector);
        int Count(Expression<Func<T, bool>> predicate);
        int CountAll { get; }
        int Delete(Expression<Func<T, bool>> predicate);
        int Delete<TKey>(TKey id);
        int Update(T TObject);
        int Update(T d, params Expression<Func<T, object>>[] efdbd);
        int UpdateWhere<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> key, TKey value);
    }
}
