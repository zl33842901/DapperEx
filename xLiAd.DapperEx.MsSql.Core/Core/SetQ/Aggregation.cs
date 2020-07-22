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
        #region Count
        public async Task<int> CountAsync()
        {
            SqlProvider.FormatCount();
            var guid = SetSql();
            var result = await DbCon.QuerySingleAsync<int>(SqlProvider.SqlString, SqlProvider.Params);
            OverSql(guid);
            return result;
        }
        public int Count()
        {
            SqlProvider.FormatCount();
            var guid = SetSql();
            var result = DbCon.QuerySingle<int>(SqlProvider.SqlString, SqlProvider.Params);
            OverSql(guid);
            return result;
        }
        #endregion
        #region Sum
        public async Task<TResult> SumAsync<TResult>(Expression<Func<T, TResult>> sumExpression)
        {
            SqlProvider.FormatSum(sumExpression);
            var guid = SetSql();
            var result = await DbCon.QuerySingleAsync<TResult>(SqlProvider.SqlString, SqlProvider.Params);
            OverSql(guid);
            return result;
        }
        public TResult Sum<TResult>(Expression<Func<T, TResult>> sumExpression)
        {
            SqlProvider.FormatSum(sumExpression);
            var guid = SetSql();
            var result = DbCon.QuerySingle<TResult>(SqlProvider.SqlString, SqlProvider.Params);
            OverSql(guid);
            return result;
        }
        #endregion
        #region Exists
        public async Task<bool> ExistsAsync()
        {
            SqlProvider.FormatExists();
            var guid = SetSql();
            var result = await DbCon.QuerySingleAsync<int>(SqlProvider.SqlString, SqlProvider.Params) == 1;
            OverSql(guid);
            return result;
        }
        public bool Exists()
        {
            SqlProvider.FormatExists();
            var guid = SetSql();
            var result = DbCon.QuerySingle<int>(SqlProvider.SqlString, SqlProvider.Params) == 1;
            OverSql(guid);
            return result;
        }
        #endregion
    }
}
