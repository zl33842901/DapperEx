using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace xLiAd.DapperEx.MsSql.Core.Core.Interfaces
{
    public interface ICommand<T>
    {
        int Update(T entity);
        Task<int> UpdateAsync(T entity);
        int Delete<TKey>(TKey id);
        Task<int> DeleteAsync<TKey>(TKey id);
        int Update(Expression<Func<T, T>> updateExpression);
        Task<int> UpdateAsync(Expression<Func<T, T>> updateExpression);
        int Update(T model, params Expression<Func<T, object>>[] updateExpression);
        Task<int> UpdateAsync(T model, params Expression<Func<T, object>>[] updateExpression);
        int Update<TKey>(Expression<Func<T, TKey>> expression, TKey value);
        Task<int> UpdateAsync<TKey>(Expression<Func<T, TKey>> expression, TKey value);
        int Delete();
        Task<int> DeleteAsync();
    }
}
