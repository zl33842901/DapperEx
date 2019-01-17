using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace xLiAd.DapperEx.QueryHelper
{
    /// <summary>
    /// 参数查询类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class QueryParamProvider<T>
    {
        private List<IQueryParamProviderItem<T>> Items = new List<IQueryParamProviderItem<T>>();
        /// <summary>
        /// 添加一个参数查询项
        /// </summary>
        /// <typeparam name="TKey">参数字段类型</typeparam>
        /// <param name="field">参数字段 请统一表达式参数名</param>
        /// <param name="oprater">比较符号 默认为相等</param>
        /// <param name="when">参数生效条件 默认为不为默认值(string 的默认值为 不为null和string.Empty)</param>
        /// <param name="paramName">参数在键值对里的名称 默认为和字段名称一致</param>
        public void AddItem<TKey>(Expression<Func<T, TKey>> field, QueryParamProviderOprater oprater = QueryParamProviderOprater.Equal, Expression<Func<TKey, bool>> when = null, string paramName = null)
        {
            if (paramName == null)
                paramName = ((MemberExpression)field.Body).Member.Name;
            if (when == null)
            {
                var vvv = default(TKey);
                ParameterExpression para = Expression.Parameter(typeof(TKey));
                when = Expression.Lambda<Func<TKey, bool>>(Expression.NotEqual(para, Expression.Constant(vvv)), para) as Expression<Func<TKey, bool>>;
                //////////////////////////////////对string 的默认值进行处理
                if (typeof(TKey) == typeof(string))
                {
                    when = when.And(Expression.Lambda<Func<TKey, bool>>(Expression.NotEqual(para, Expression.Constant(string.Empty)), para));
                }
            }
            QueryParamProviderItem<T, TKey> item = new QueryParamProviderItem<T, TKey>(field, when, paramName, oprater);
            Items.Add(item);
        }
        /// <summary>
        /// 获取表达式
        /// </summary>
        /// <param name="nameValue"></param>
        /// <returns></returns>
        public Expression<Func<T, bool>> GetExpression(NameValueCollection nameValue)
        {
            Expression<Func<T, bool>> expression = null;
            foreach(var i in Items)
            {
                var mtd = i.GetType().GetMethod("GetExpression");
                var v = nameValue[i.ParamName];
                if (v.NullOrEmpty() && i.FieldType != typeof(string))
                    continue;
                var realv = v.ConvertTo(i.FieldType);
                if (!i.ValidWhen()(realv))
                    continue;
                var e = mtd.Invoke(i, new object[] { realv }) as Expression<Func<T, bool>>;
                expression = expression.And(e);
            }
            return expression;
        }
    }
    internal class QueryParamProviderItem<T, TKey> : IQueryParamProviderItem<T>
    {
        /// <summary>
        /// 要查询的字段（属性）
        /// </summary>
        public Expression<Func<T, TKey>> Field { get; private set; }
        /// <summary>
        /// 表达式什么时候生效
        /// </summary>
        public Expression<Func<TKey, bool>> When { get; private set; }
        /// <summary>
        /// 根据 When 计算出来的委托，表达式什么时候生效
        /// </summary>
        /// <returns></returns>
        public Func<object, bool> ValidWhen()
        {
            Func<object, bool> func = x =>
            {
                if (x == null)
                    return false;
                if (x.GetType() == typeof(TKey) || (typeof(TKey).IsGenericType && typeof(TKey).GetGenericTypeDefinition() == typeof(Nullable<>) && typeof(TKey).GetGenericArguments()[0] == x.GetType()))
                {
                    var vv = (TKey)x;
                    return When.Compile().Invoke(vv);
                }
                else
                    return false;
            };
            return func;
        }
        /// <summary>
        /// 参数名称 多数情况和属性名一样  在本类内只作记录，无实际用途
        /// </summary>
        public string ParamName { get; private set; }
        /// <summary>
        /// 操作符
        /// </summary>
        public QueryParamProviderOprater Oprater { get; private set; }
        /// <summary>
        /// 查询相关的值
        /// </summary>
        public TKey Value { get; private set; }
        /// <summary>
        /// 属性的类型
        /// </summary>
        public Type FieldType => typeof(TKey);
        /// <summary>
        /// 生成一个查询项
        /// </summary>
        /// <param name="field">相关的属性（字段）</param>
        /// <param name="paramName">参数名，仅作为记录</param>
        /// <param name="oprater">操作方法</param>
        internal QueryParamProviderItem(Expression<Func<T,TKey>> field, Expression<Func<TKey, bool>> when, string paramName, QueryParamProviderOprater oprater)
        {
            Field = field;
            When = when;
            ParamName = paramName;
            Oprater = oprater;
        }
        /// <summary>
        /// 获取表达式
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Expression<Func<T, bool>> GetExpression(TKey v)
        {
            this.Value = v;
            switch (this.Oprater)
            {
                case QueryParamProviderOprater.Equal:
                    return Expression.Lambda<Func<T, bool>>(Expression.Equal(Field.Body, Expression.Constant(Value, this.FieldType)), Field.Parameters);
                case QueryParamProviderOprater.NotEqual:
                    return Expression.Lambda<Func<T, bool>>(Expression.NotEqual(Field.Body, Expression.Constant(Value, this.FieldType)), Field.Parameters);
                case QueryParamProviderOprater.LessThan:
                    return Expression.Lambda<Func<T, bool>>(Expression.LessThan(Field.Body, Expression.Constant(Value, this.FieldType)), Field.Parameters);
                case QueryParamProviderOprater.LessThanOrEqual:
                    return Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(Field.Body, Expression.Constant(Value, this.FieldType)), Field.Parameters);
                case QueryParamProviderOprater.GreaterThan:
                    return Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(Field.Body, Expression.Constant(Value, this.FieldType)), Field.Parameters);
                case QueryParamProviderOprater.GreaterThanOrEqual:
                    return Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(Field.Body, Expression.Constant(Value, this.FieldType)), Field.Parameters);
                case QueryParamProviderOprater.Contains:
                    ParameterExpression para = Expression.Parameter(typeof(T), Field.Parameters[0].Name);
                    var mtds = typeof(string).GetMethods().Where(x => x.Name == "Contains").ToArray();
                    MethodCallExpression met = Expression.Call(Field.Body, mtds[0],
                        Expression.Constant(Value));
                    return Expression.Lambda<Func<T, bool>>(met, para);
                default:
                    return null;
            }
        }
    }
    internal interface IQueryParamProviderItem<T> { string ParamName { get; } Type FieldType { get; } Func<object, bool> ValidWhen(); }
    public enum QueryParamProviderOprater : byte
    {
        Equal = 1,
        NotEqual = 2,
        LessThan = 3,
        LessThanOrEqual = 4,
        GreaterThan = 5,
        GreaterThanOrEqual = 6,
        Contains = 7
    }
}
