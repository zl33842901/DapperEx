using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using xLiAd.DapperEx.MsSql.Core.Core.Expression;
using xLiAd.DapperEx.MsSql.Core.Model;

namespace xLiAd.DapperEx.MsSql.Core.Helper
{
    internal static class ResolveExpression
    {
        public static string ResolveOrderBy(List<(EOrderBy Key, LambdaExpression Value)> orderbyExpressionDic)
        {
            var orderByList = orderbyExpressionDic.Select(a =>
            {
                var memberExpress = (MemberExpression)a.Value.Body;
                return memberExpress.Member.GetColumnAttributeName() + (a.Key == EOrderBy.Asc ? " ASC " : " DESC ");
            }).ToList();

            if (!orderByList.Any())
                return "";

            return "ORDER BY " + string.Join(",", orderByList);
        }

        public static WhereExpression ResolveWhere(LambdaExpression whereExpression, string prefix = null)
        {
            var where = new WhereExpression(whereExpression, prefix);

            return where;
        }

        public static UpdateEntityWhereExpression ResolveWhere(object obj)
        {
            var where = new UpdateEntityWhereExpression(obj);
            where.Resolve();
            return where;
        }
        public static DeleteExpression<T, TKey> ResolveWhere<T, TKey>(TKey id)
        {
            var where = new DeleteExpression<T, TKey>(id);
            where.Resolve();
            return where;
        }
        public static string ResolveSelectZhanglei(Type type, LambdaExpression selector, int? topNum)
        {
            if (selector == null)
                return ResolveSelect(type.GetPropertiesInDb(), selector, topNum);
            var selectFormat = topNum.HasValue ? " SELECT {1} {0} " : " SELECT {0} ";
            var selectSql = "";
            var lfields = new ExpressionPropertyFinder(selector, type).MemberList;
            /////////////////////////下面要过滤不在DB里的。
            var nameList = type.GetPropertiesInDb().Select(x => x.Name).ToArray();
            lfields = lfields.Where(x => nameList.Contains(x.Name));
            /////////////////////////
            selectSql = string.Format(selectFormat, string.Join(",", lfields.Select(x=>x.Name)), $" TOP {topNum} ");
            return selectSql;
        }
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

        public static UpdateExpression<T> ResolveUpdateZhanglei<T>(IEnumerable<LambdaExpression> expressionList, T model)
        {
            return new UpdateExpression<T>(expressionList, model);
        }
        public static UpdateExpressionEx<T> ResolveUpdateZhanglei<T>(LambdaExpression expression, object value)
        {
            return new UpdateExpressionEx<T>(expression, value);
        }
    }
}
