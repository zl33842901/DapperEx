﻿using System;
using System.Data;
using System.Linq.Expressions;
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
        protected Aggregation(IDbConnection conn, SqlProvider<T> sqlProvider) : base(conn, sqlProvider)
        {

        }

        protected Aggregation(IDbConnection conn, SqlProvider<T> sqlProvider, IDbTransaction dbTransaction) : base(conn, sqlProvider, dbTransaction)
        {

        }

        public int Count()
        {
            SqlProvider.FormatCount();

            return DbCon.QuerySingle<int>(SqlProvider.SqlString, SqlProvider.Params);
        }

        public TResult Sum<TResult>(Expression<Func<T, TResult>> sumExpression)
        {
            SqlProvider.FormatSum(sumExpression);

            return DbCon.QuerySingle<TResult>(SqlProvider.SqlString, SqlProvider.Params);
        }

        public bool Exists()
        {
            SqlProvider.FormatExists();

            return DbCon.QuerySingle<int>(SqlProvider.SqlString, SqlProvider.Params) == 1;
        }
    }
}
