using System;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using xLiAd.DapperEx.MsSql.Core.Core.Interfaces;

namespace xLiAd.DapperEx.MsSql.Core.Core.SetQ
{
    /// <summary>
    /// 聚合查询器 带有的参数和 Order<T>一致
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Aggregation<T> : Order<T>, IAggregation
    {
        protected Aggregation(IDbConnection conn, SqlProvider<T> sqlProvider, IDbTransaction dbTransaction = null, bool throws = true) : base(conn, sqlProvider, dbTransaction, throws)
        {

        }

        public async Task<int> CountAsync()
        {
            SqlProvider.FormatCount();
            SetSql();
            return await DbCon.QuerySingleAsync<int>(SqlProvider.SqlString, SqlProvider.Params);
        }
        public int Count()
        {
            var task = CountAsync();
            return task.Result;
        }

        public async Task<TResult> SumAsync<TResult>(Expression<Func<T, TResult>> sumExpression)
        {
            SqlProvider.FormatSum(sumExpression);
            SetSql();
            return await DbCon.QuerySingleAsync<TResult>(SqlProvider.SqlString, SqlProvider.Params);
        }
        public TResult Sum<TResult>(Expression<Func<T, TResult>> sumExpression)
        {
            var task = SumAsync(sumExpression);
            return task.Result;
        }

        public async Task<bool> ExistsAsync()
        {
            SqlProvider.FormatExists();
            SetSql();
            return await DbCon.QuerySingleAsync<int>(SqlProvider.SqlString, SqlProvider.Params) == 1;
        }
        public bool Exists()
        {
            var task = ExistsAsync();
            return task.Result;
        }
    }
}
