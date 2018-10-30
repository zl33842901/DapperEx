﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using xLiAd.DapperEx.MsSql.Core.Helper;
using xLiAd.DapperEx.MsSql.Core.Model;

namespace xLiAd.DapperEx.MsSql.Core.Core.SetQ
{
    public class QuerySet<T> : Aggregation<T>, Interfaces.ISet<T>
    {
        internal Type TableType { get; set; }

        internal LambdaExpression WhereExpression { get; set; }

        public QuerySet(IDbConnection conn, SqlProvider<T> sqlProvider) : base(conn, sqlProvider)
        {
            TableType = typeof(T);
            SetContext = new DataBaseContext<T>
            {
                Set = this,
                OperateType = EOperateType.Query
            };

            sqlProvider.Context = SetContext;
        }

        public QuerySet(IDbConnection conn, SqlProvider<T> sqlProvider, IDbTransaction dbTransaction) : base(conn, sqlProvider, dbTransaction)
        {
            TableType = typeof(T);
            SetContext = new DataBaseContext<T>
            {
                Set = this,
                OperateType = EOperateType.Query
            };

            sqlProvider.Context = SetContext;
        }

        internal QuerySet(IDbConnection conn, SqlProvider<T> sqlProvider, Type tableType, LambdaExpression whereExpression, LambdaExpression selectExpression, int? topNum, List<(EOrderBy Key, LambdaExpression Value)> orderbyExpressionList, IDbTransaction dbTransaction) : base(conn, sqlProvider, dbTransaction)
        {
            TableType = tableType;
            WhereExpression = whereExpression;
            SelectExpression = selectExpression;
            TopNum = topNum;
            OrderbyExpressionList = orderbyExpressionList;

            SetContext = new DataBaseContext<T>
            {
                Set = this,
                OperateType = EOperateType.Query
            };

            sqlProvider.Context = SetContext;
        }

        public QuerySet<T> Where(Expression<Func<T, bool>> predicate)
        {
            WhereExpression = WhereExpression == null ? predicate : ((Expression<Func<T, bool>>)WhereExpression).And(predicate);

            return this;
        }
    }
}
