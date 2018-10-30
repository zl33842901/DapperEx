using System;
using System.Linq.Expressions;

namespace xLiAd.DapperEx.MsSql.Core.Core.Interfaces
{
    public interface ICommand<T>
    {
        int Update(T entity);
        int Update(Expression<Func<T, T>> updateExpression);
        int Update(T model, params Expression<Func<T, object>>[] updateExpression);
        int Update<TKey>(Expression<Func<T, TKey>> expression, TKey value);
        int Delete();
    }
}
