using Dapper;
using xLiAd.DapperEx.MsSql.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace xLiAd.DapperEx.MsSql.Core.Core.SetQ
{
    /// <summary>
    /// 具有不同类型的查询类和结果类的查询器
    /// </summary>
    /// <typeparam name="TResult">结果类型</typeparam>
    /// <typeparam name="TSource">查询类型</typeparam>
    public class QuerySet<TResult,TSource> : QuerySet<TResult>
    {
        protected readonly SqlProvider<TSource> SqlProviderSource;
        internal QuerySet(IDbConnection conn, SqlProvider<TResult> sqlProvider, Type tableType, LambdaExpression whereExpression, LambdaExpression selectExpression, int? topNum, List<(EOrderBy Key, LambdaExpression Value)> orderbyExpressionList, IDbTransaction dbTransaction, bool throws = true)
            : base(conn, sqlProvider, tableType, whereExpression,selectExpression,topNum,orderbyExpressionList, dbTransaction, throws) { }
        public override List<TResult> ToList()
        {
            SqlProvider.FormatToListZhanglei(typeof(TSource));
            try
            {
                return DbCon.Query<TSource>(SqlProvider.SqlString, SqlProvider.Params, DbTransaction).ToList().Select(((Expression<Func<TSource, TResult>>)SelectExpression).Compile()).ToList();
            }
            catch (Exception e)
            {
                CallEvent(SqlProvider.SqlString, SqlProvider.Params, e.Message);
                if (Throws)
                    throw new Exception($"{e.Message} sql:{SqlProvider.SqlString} params:{SqlProvider.Params}", e);
                else
                    return new List<TResult>();
            }
        }
    }
}
