using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;
using xLiAd.DapperEx.MsSql.Core.Core.Expression;
using xLiAd.DapperEx.MsSql.Core.Model;

namespace xLiAd.DapperEx.MsSql.Core.Helper
{
    /// <summary>
    /// 各种表达式转换为SQL子句的帮助类
    /// </summary>
    internal class ResolveExpression
    {
        public const string JsonColumnNameSuffix = "_DapperEx_JsonColumn";
        readonly ISqlDialect Dialect;
        public ResolveExpression(ISqlDialect dialect)
        {
            Dialect = dialect;
        }
        public static ResolveExpression Instance(ISqlDialect dialect)
        {
            return new ResolveExpression(dialect);
        }
        /// <summary>
        /// 把排序表达式 转换为 排序SQL子句
        /// </summary>
        /// <param name="orderbyExpressionDic"></param>
        /// <returns></returns>
        public string ResolveOrderBy(List<(EOrderBy Key, LambdaExpression Value)> orderbyExpressionDic)
        {
            var orderByList = orderbyExpressionDic.Select(a =>
            {
                if(a.Value.Body is MemberExpression)
                {
                    var memberExpress = (MemberExpression)a.Value.Body;
                    return memberExpress.Member.GetColumnAttributeName(Dialect) + (a.Key == EOrderBy.Asc ? " ASC " : " DESC ");
                }
                else if(a.Value.Body is UnaryExpression)
                {
                    var ue = (UnaryExpression)a.Value.Body;
                    var memberExpress = (MemberExpression)ue.Operand;
                    return memberExpress.Member.GetColumnAttributeName(Dialect) + (a.Key == EOrderBy.Asc ? " ASC " : " DESC ");
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
        public WhereExpression ResolveWhere(LambdaExpression whereExpression, string prefix = null)
        {
            var where = new WhereExpression(whereExpression, prefix, Dialect);

            return where;
        }

        /// <summary>
        /// 根据对象的主键、主键值，生成SQL的WHERE子句
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public UpdateEntityWhereExpression ResolveWhere(object obj)
        {
            var where = new UpdateEntityWhereExpression(obj, Dialect);
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
        public DeleteExpression<T, TKey> ResolveWhere<T, TKey>(TKey id)
        {
            var where = new DeleteExpression<T, TKey>(id, Dialect);
            where.Resolve();
            return where;
        }

        /// <summary>
        /// 根据给定的 SELECT表达式，生成SELECT子句
        /// </summary>
        /// <param name="type"></param>
        /// <param name="selector"></param>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public string ResolveSelect(Type type, int? topNum, params LambdaExpression[] selector)
        {
            if (selector == null || selector.Count() < 1 || selector.Count(x => x != null) < 1)
                return ResolveSelect(type.GetPropertiesInDb(true), null, topNum);
            var selectFormat = topNum.HasValue && !Dialect.IsUseLimitInsteadOfTop ? " SELECT {1} {0} " : " SELECT {0} ";
            var selectSql = "";
            List<MemberInfo> lfields = new List<MemberInfo>();
            foreach(var slct in selector)
                lfields.AddRange(new ExpressionPropertyFinder(slct, type).MemberList);
            /////////////////////////下面要过滤不在DB里的。
            var nameList = type.GetPropertiesInDb(true).Select(x => x.Name).ToArray();
            lfields = lfields.Where(x => nameList.Contains(x.Name)).ToList();
            /////////////////////////
            selectSql = string.Format(selectFormat, string.Join(",", lfields.Select(x => $"{x.GetColumnAttributeName(Dialect)} {Dialect.ParseColumnName(x.Name + GetJsonColumnNameSuffixIf(x))}")), $" TOP {topNum} ");
            return selectSql;
        }

        /// <summary>
        /// 获取一个属性在查询时要不要加后辍 加啥返回啥，不加返回空
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private string GetJsonColumnNameSuffixIf(MemberInfo propertyInfo)
        {
            bool jsonColumn = propertyInfo.CustomAttributes.Any(b => b.AttributeType == typeof(JsonColumnAttribute));
            string jsonColumnNameSuffix;
            if (jsonColumn && Dialect.HasSerializer && Dialect.SupportJsonColumn)
                jsonColumnNameSuffix = JsonColumnNameSuffix;
            else
                jsonColumnNameSuffix = string.Empty;
            return jsonColumnNameSuffix;
        }

        /// <summary>
        /// 根据给定的字段，生成SELECT子句
        /// </summary>
        /// <param name="propertyInfos"></param>
        /// <param name="selector"></param>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public string ResolveSelect(PropertyInfo[] propertyInfos, LambdaExpression selector, int? topNum)
        {
            var selectFormat = topNum.HasValue && !Dialect.IsUseLimitInsteadOfTop ? " SELECT {1} {0} " : " SELECT {0} ";
            var selectSql = "";

            if (selector == null)
            {
                var propertyBuilder = new StringBuilder();
                foreach (var propertyInfo in propertyInfos)
                {
                    if (propertyBuilder.Length > 0)
                        propertyBuilder.Append(",");

                    string jsonColumnNameSuffix = GetJsonColumnNameSuffixIf(propertyInfo);
                    propertyBuilder.AppendFormat($"{propertyInfo.GetColumnAttributeName(Dialect)} {Dialect.ParseColumnName(propertyInfo.Name + jsonColumnNameSuffix)}");
                }
                selectSql = string.Format(selectFormat, propertyBuilder, $" TOP {topNum} ");
            }
            else
            {
                var nodeType = selector.Body.NodeType;
                if (nodeType == ExpressionType.MemberAccess)
                {
                    var memberExpression = (MemberExpression)selector.Body;
                    selectSql = string.Format(selectFormat, memberExpression.Member.GetColumnAttributeName(Dialect), $" TOP {topNum} ");
                }
                else if (nodeType == ExpressionType.MemberInit)
                {
                    var memberInitExpression = (MemberInitExpression)selector.Body;
                    selectSql = string.Format(selectFormat, string.Join(",", memberInitExpression.Bindings.Select(a => a.Member.GetColumnAttributeName(Dialect))), $" TOP {topNum} ");
                }
            }

            return selectSql;
        }

        public string ResolveLimit(int? topNum)
        {
            if (topNum.HasValue && Dialect.IsUseLimitInsteadOfTop)
                return $" Limit {topNum}";
            else
                return string.Empty;
        }

        public string ResolveSelectOfUpdate(PropertyInfo[] propertyInfos, LambdaExpression selector)
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
                    propertyBuilder.AppendFormat($"INSERTED.{propertyInfo.GetColumnAttributeName(Dialect)} {Dialect.ParseColumnName(propertyInfo.Name + GetJsonColumnNameSuffixIf(propertyInfo))}");
                }
                selectSql = propertyBuilder.ToString();
            }
            else
            {
                var nodeType = selector.Body.NodeType;
                if (nodeType == ExpressionType.MemberAccess)
                {
                    var memberExpression = (MemberExpression)selector.Body;
                    selectSql = "INSERTED." + memberExpression.Member.GetColumnAttributeName(Dialect);
                }
                else if (nodeType == ExpressionType.MemberInit)
                {
                    var memberInitExpression = (MemberInitExpression)selector.Body;
                    selectSql = string.Join(",", memberInitExpression.Bindings.Select(a => "INSERTED." + a.Member.GetColumnAttributeName(Dialect)));
                }
            }

            return "OUTPUT " + selectSql;
        }

        public string ResolveSum(PropertyInfo[] propertyInfos, LambdaExpression selector)
        {
            var selectFormat = " SELECT ISNULL(SUM({0}),0)  ";
            var selectSql = "";

            if (selector == null)
                throw new ArgumentException("selector");

            var nodeType = selector.Body.NodeType;
            if (nodeType == ExpressionType.MemberAccess)
            {
                var memberExpression = (MemberExpression)selector.Body;
                selectSql = string.Format(selectFormat, memberExpression.Member.GetColumnAttributeName(Dialect));
            }
            else if (nodeType == ExpressionType.MemberInit)
                throw new Exception("不支持该表达式类型");

            return selectSql;
        }

        public UpdateExpression ResolveUpdate<T>(Expression<Func<T, T>> updateExpression)
        {
            return new UpdateExpression(updateExpression, Dialect);
        }

        /// <summary>
        /// 更新某对象的多个字段的Update子句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressionList"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public UpdateExpression<T> ResolveUpdateZhanglei<T>(IEnumerable<LambdaExpression> expressionList, T model)
        {
            return new UpdateExpression<T>(expressionList, model, Dialect);
        }

        /// <summary>
        /// 生成 更新一个字段到指定值的 UpdateExpressionEx
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public UpdateExpressionEx<T> ResolveUpdateZhanglei<T>(LambdaExpression expression, object value)
        {
            return new UpdateExpressionEx<T>(expression, value, Dialect);
        }
    }
}
