﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using xLiAd.DapperEx.MsSql.Core.Core.Expression;
using xLiAd.DapperEx.MsSql.Core.Model;

namespace xLiAd.DapperEx.MsSql.Core.Helper
{
    /// <summary>
    /// 各种表达式转换为SQL子句的帮助类
    /// </summary>
    internal static class ResolveExpression
    {
        /// <summary>
        /// 把排序表达式 转换为 排序SQL子句
        /// </summary>
        /// <param name="orderbyExpressionDic"></param>
        /// <returns></returns>
        public static string ResolveOrderBy(List<(EOrderBy Key, LambdaExpression Value)> orderbyExpressionDic)
        {
            var orderByList = orderbyExpressionDic.Select(a =>
            {
                if(a.Value.Body is MemberExpression)
                {
                    var memberExpress = (MemberExpression)a.Value.Body;
                    return memberExpress.Member.GetColumnAttributeName() + (a.Key == EOrderBy.Asc ? " ASC " : " DESC ");
                }
                else if(a.Value.Body is UnaryExpression)
                {
                    var ue = (UnaryExpression)a.Value.Body;
                    var memberExpress = (MemberExpression)ue.Operand;
                    return memberExpress.Member.GetColumnAttributeName() + (a.Key == EOrderBy.Asc ? " ASC " : " DESC ");
                }
                else
                {
                    throw new Exception("only fields can be ordered");
                }
            }).ToList();

            if (!orderByList.Any())
                return "";

            return "ORDER BY " + string.Join(",", orderByList);
        }

        /// <summary>
        /// 把条件表达式 转换为 条件SQL子句（WHERE 子句）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static WhereExpression ResolveWhere(LambdaExpression whereExpression, string prefix = null)
        {
            var where = new WhereExpression(whereExpression, prefix);

            return where;
        }

        /// <summary>
        /// 根据对象的主键、主键值，生成SQL的WHERE子句
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static UpdateEntityWhereExpression ResolveWhere(object obj)
        {
            var where = new UpdateEntityWhereExpression(obj);
            where.Resolve();
            return where;
        }

        /// <summary>
        /// 根据类型的主键、给过来的主键值，生成SQL的WHERE子句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DeleteExpression<T, TKey> ResolveWhere<T, TKey>(TKey id)
        {
            var where = new DeleteExpression<T, TKey>(id);
            where.Resolve();
            return where;
        }

        /// <summary>
        ///  根据给定的 SELECT表达式，生成SELECT子句
        /// </summary>
        /// <param name="type"></param>
        /// <param name="selector"></param>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public static string ResolveSelectZhanglei(Type type, LambdaExpression selector, int? topNum)
        {
            if (selector == null)
                return ResolveSelect(type.GetPropertiesInDb(true), selector, topNum);
            var selectFormat = topNum.HasValue ? " SELECT {1} {0} " : " SELECT {0} ";
            var selectSql = "";
            var lfields = new ExpressionPropertyFinder(selector, type).MemberList;
            /////////////////////////下面要过滤不在DB里的。
            var nameList = type.GetPropertiesInDb(true).Select(x => x.Name).ToArray();
            lfields = lfields.Where(x => nameList.Contains(x.Name));
            /////////////////////////
            selectSql = string.Format(selectFormat, string.Join(",", lfields.Select(x=>x.Name)), $" TOP {topNum} ");
            return selectSql;
        }

        /// <summary>
        /// 根据给定的 SELECT表达式，生成SELECT子句
        /// </summary>
        /// <param name="type"></param>
        /// <param name="selector"></param>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public static string ResolveSelectZhanglei(Type type, IEnumerable<LambdaExpression> selector, int? topNum)
        {
            if (selector == null || selector.Count() < 1)
                return ResolveSelect(type.GetPropertiesInDb(true), null, topNum);
            var selectFormat = topNum.HasValue ? " SELECT {1} {0} " : " SELECT {0} ";
            var selectSql = "";
            List<MemberInfo> lfields = new List<MemberInfo>();
            foreach(var slct in selector)
                lfields.AddRange(new ExpressionPropertyFinder(slct, type).MemberList);
            /////////////////////////下面要过滤不在DB里的。
            var nameList = type.GetPropertiesInDb(true).Select(x => x.Name).ToArray();
            lfields = lfields.Where(x => nameList.Contains(x.Name)).ToList();
            /////////////////////////
            selectSql = string.Format(selectFormat, string.Join(",", lfields.Select(x => x.Name)), $" TOP {topNum} ");
            return selectSql;
        }

        /// <summary>
        /// 根据给定的字段，生成SELECT子句
        /// </summary>
        /// <param name="propertyInfos"></param>
        /// <param name="selector"></param>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public static string ResolveSelect(PropertyInfo[] propertyInfos, LambdaExpression selector, int? topNum)
        {
            var selectFormat = topNum.HasValue ? " SELECT {1} {0} " : " SELECT {0} ";
            var selectSql = "";

            if (selector == null)
            {
                var propertyBuilder = new StringBuilder();
                foreach (var propertyInfo in propertyInfos)
                {
                    if (propertyBuilder.Length > 0)
                        propertyBuilder.Append(",");
                    propertyBuilder.AppendFormat($"{propertyInfo.GetColumnAttributeName()} {propertyInfo.Name}");
                }
                selectSql = string.Format(selectFormat, propertyBuilder, $" TOP {topNum} ");
            }
            else
            {
                var nodeType = selector.Body.NodeType;
                if (nodeType == ExpressionType.MemberAccess)
                {
                    var memberExpression = (MemberExpression)selector.Body;
                    selectSql = string.Format(selectFormat, memberExpression.Member.GetColumnAttributeName(), $" TOP {topNum} ");
                }
                else if (nodeType == ExpressionType.MemberInit)
                {
                    var memberInitExpression = (MemberInitExpression)selector.Body;
                    selectSql = string.Format(selectFormat, string.Join(",", memberInitExpression.Bindings.Select(a => a.Member.GetColumnAttributeName())), $" TOP {topNum} ");
                }
            }

            return selectSql;
        }

        public static string ResolveSelectOfUpdate(PropertyInfo[] propertyInfos, LambdaExpression selector)
        {
            var selectSql = "";

            if (selector == null)
            {
                propertyInfos = propertyInfos.Where(x => x.CanRead && x.CanWrite).ToArray();
                var propertyBuilder = new StringBuilder();
                foreach (var propertyInfo in propertyInfos)
                {
                    if (propertyInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>() != null)
                        continue;
                    if (typeof(IList).IsAssignableFrom(propertyInfo.PropertyType))
                        continue;
                    if (propertyInfo.SetMethod == null || propertyInfo.GetMethod == null)
                        continue;
                    if (!propertyInfo.GetMethod.IsPublic || !propertyInfo.SetMethod.IsPublic)
                        continue;
                    if (propertyBuilder.Length > 0)
                        propertyBuilder.Append(",");
                    propertyBuilder.AppendFormat($"INSERTED.{propertyInfo.GetColumnAttributeName()} {propertyInfo.Name}");
                }
                selectSql = propertyBuilder.ToString();
            }
            else
            {
                var nodeType = selector.Body.NodeType;
                if (nodeType == ExpressionType.MemberAccess)
                {
                    var memberExpression = (MemberExpression)selector.Body;
                    selectSql = "INSERTED." + memberExpression.Member.GetColumnAttributeName();
                }
                else if (nodeType == ExpressionType.MemberInit)
                {
                    var memberInitExpression = (MemberInitExpression)selector.Body;
                    selectSql = string.Join(",", memberInitExpression.Bindings.Select(a => "INSERTED." + a.Member.GetColumnAttributeName()));
                }
            }

            return "OUTPUT " + selectSql;
        }

        public static string ResolveSum(PropertyInfo[] propertyInfos, LambdaExpression selector)
        {
            var selectFormat = " SELECT ISNULL(SUM({0}),0)  ";
            var selectSql = "";

            if (selector == null)
                throw new ArgumentException("selector");

            var nodeType = selector.Body.NodeType;
            if (nodeType == ExpressionType.MemberAccess)
            {
                var memberExpression = (MemberExpression)selector.Body;
                selectSql = string.Format(selectFormat, memberExpression.Member.GetColumnAttributeName());
            }
            else if (nodeType == ExpressionType.MemberInit)
                throw new Exception("不支持该表达式类型");

            return selectSql;
        }

        public static UpdateExpression ResolveUpdate<T>(Expression<Func<T, T>> updateExpression)
        {
            return new UpdateExpression(updateExpression);
        }

        /// <summary>
        /// 更新某对象的多个字段的Update子句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressionList"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static UpdateExpression<T> ResolveUpdateZhanglei<T>(IEnumerable<LambdaExpression> expressionList, T model)
        {
            return new UpdateExpression<T>(expressionList, model);
        }

        /// <summary>
        /// 生成 更新一个字段到指定值的 UpdateExpressionEx
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UpdateExpressionEx<T> ResolveUpdateZhanglei<T>(LambdaExpression expression, object value)
        {
            return new UpdateExpressionEx<T>(expression, value);
        }
    }
}
