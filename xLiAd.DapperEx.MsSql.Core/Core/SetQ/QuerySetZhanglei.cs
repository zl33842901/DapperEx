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
        public QuerySet(IDbConnection conn, SqlProvider<TResult> sqlProvider) : base(conn, sqlProvider) { }
        internal QuerySet(IDbConnection conn, SqlProvider<TResult> sqlProvider, Type tableType, LambdaExpression whereExpression, LambdaExpression selectExpression, int? topNum, List<(EOrderBy Key, LambdaExpression Value)> orderbyExpressionList, IDbTransaction dbTransaction) : base(conn, sqlProvider, tableType, whereExpression,selectExpression,topNum,orderbyExpressionList, dbTransaction) { }
        public override List<TResult> ToList()
        {
            SqlProvider.FormatToListZhanglei(typeof(TSource));

            return DbCon.Query<TSource>(SqlProvider.SqlString, SqlProvider.Params, DbTransaction).ToList().Select(((Expression<Func<TSource,TResult>>)SelectExpression).Compile()).ToList();
        }
    }
}
