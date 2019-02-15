using xLiAd.DapperEx.MsSql.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace xLiAd.DapperEx.MsSql.Core.Core.SetQ
{
    /// <inheritdoc />
    /// <summary>
    /// 具有排序条件的查询器（可能同时具有Select参数、Top参数）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Order<T> : Option<T>
    {
        internal List<(EOrderBy Key, LambdaExpression Value)> OrderbyExpressionList { get; set; }

        protected Order(IDbConnection conn, SqlProvider<T> sqlProvider) : base(conn, sqlProvider)
        {
            OrderbyExpressionList = new List<(EOrderBy Key, LambdaExpression Value)>();
        }

        protected Order(IDbConnection conn, SqlProvider<T> sqlProvider, IDbTransaction dbTransaction) : base(conn, sqlProvider, dbTransaction)
        {
            OrderbyExpressionList = new List<(EOrderBy Key, LambdaExpression Value)>();
        }

        /// <summary>
        /// 顺序
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public virtual Order<T> OrderBy<TProperty>(Expression<Func<T, TProperty>> field)
        {
            if (field != null)
                OrderbyExpressionList.Add(( EOrderBy.Asc, field));

            return this;
        }

        /// <summary>
        /// 倒叙
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public virtual Order<T> OrderByDescing<TProperty>(Expression<Func<T, TProperty>> field)
        {
            if (field != null)
                OrderbyExpressionList.Add((EOrderBy.Desc, field));

            return this;
        }
    }
}
