using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using xLiAd.DapperEx.MsSql.Core.Helper;
using xLiAd.DapperEx.Repository;

namespace xLiAd.DapperEx.QueryHelper
{
    /// <summary>
    /// 类似JOIN的查询 （即 根据另一个表的 字符串 查到的ID 对主表进行查询）
    /// </summary>
    /// <typeparam name="TMain">主表的实体类型</typeparam>
    public class QueryParamJoiner<TMain>
    {
        private List<IQueryParamJoinerItem<TMain>> Items = new List<IQueryParamJoinerItem<TMain>>();
        /// <summary>
        /// 添加一个查询项
        /// </summary>
        /// <typeparam name="TSub">副表的实体类型</typeparam>
        /// <typeparam name="TKey">主表副表关联字段的类型</typeparam>
        /// <param name="mainField">主表关联字段</param>
        /// <param name="queryField">副表查询字段</param>
        /// <param name="oprater">操作符</param>
        /// <param name="keyField">副表关联字段</param>
        /// <param name="repository">表仓储</param>
        /// <param name="when">参数生效条件 默认值为 不为null和string.Empty</param>
        /// <param name="paramName">参数在键值对里的名称 默认为和字段名称一致</param>
        public void AddItem<TSub, TKey>(Expression<Func<TMain, TKey>> mainField, Expression<Func<TSub,string>> queryField, 
            QueryParamJoinerOprater oprater, Expression<Func<TSub, TKey>> keyField, IRepository<TSub> repository,
            Expression<Func<string, bool>> when = null, string paramName = null)
        {
            if (paramName == null)
                paramName = ((MemberExpression)queryField.Body).Member.Name;
            if (when == null)
            {
                var vvv = default(string);
                ParameterExpression para = Expression.Parameter(typeof(string));
                when = Expression.Lambda<Func<string, bool>>(Expression.NotEqual(para, Expression.Constant(vvv)), para) as Expression<Func<string, bool>>;
                when = when.And(Expression.Lambda<Func<string, bool>>(Expression.NotEqual(para, Expression.Constant(string.Empty)), para));
            }
            QueryParamJoinerItem<TMain, TSub, TKey> item = new QueryParamJoinerItem<TMain, TSub, TKey>(mainField, queryField, oprater,keyField, repository,
                when, paramName);
            Items.Add(item);
        }
        /// <summary>
        /// 获取表达式
        /// </summary>
        /// <param name="nameValue"></param>
        /// <returns></returns>
        public Expression<Func<TMain, bool>> GetExpression(NameValueCollection nameValue)
        {
            Expression<Func<TMain, bool>> expression = null;
            foreach (var i in Items)
            {
                var mtd = i.GetType().GetMethod("GetExpression");
                var v = nameValue[i.ParamName];
                var realv = v.ConvertTo(typeof(string));
                if (!i.ValidWhen()(realv))
                    continue;
                var e = mtd.Invoke(i, new object[] { realv }) as Expression<Func<TMain, bool>>;
                expression = expression.And(e);
            }
            return expression;
        }
    }

    internal class QueryParamJoinerItem<TMain, TSub, TKey> : IQueryParamJoinerItem<TMain>
    {
        public Expression<Func<TMain, TKey>> MainField { get; private set; }
        public Expression<Func<TSub, string>> QueryField { get; private set; }
        /// <summary>
        /// 表达式什么时候生效
        /// </summary>
        public Expression<Func<string, bool>> When { get; private set; }
        public Func<object, bool> ValidWhen()
        {
            Func<object, bool> func = x =>
            {
                if (x == null)
                    return false;
                if (x.GetType() == typeof(string))
                {
                    var vv = (string)x;
                    return When.Compile().Invoke(vv);
                }
                else
                    return false;
            };
            return func;
        }
        public string ParamName { get; private set; }
        public QueryParamJoinerOprater Oprater { get; private set; }
        public Expression<Func<TSub, TKey>> KeyField { get; private set; }
        public IRepository<TSub> Repository { get; private set; }
        public string Value { get; private set; }
        /// <summary>
        /// 这个在这边没什么用了。
        /// </summary>
        public Type FieldType => typeof(TKey);
        public QueryParamJoinerItem(Expression<Func<TMain, TKey>> mainField, Expression<Func<TSub, string>> queryField,
            QueryParamJoinerOprater oprater, Expression<Func<TSub, TKey>> keyField, IRepository<TSub> repository,
            Expression<Func<string, bool>> when, string paramName)
        {
            this.MainField = mainField;
            this.QueryField = queryField;
            this.When = when;
            this.ParamName = paramName;
            this.Oprater = oprater;
            this.KeyField = keyField;
            this.Repository = repository;
        }
        public Expression<Func<TMain, bool>> GetExpression(string v)
        {
            this.Value = v;
            Expression<Func<TSub, bool>> exp;
            switch (this.Oprater)
            {
                case QueryParamJoinerOprater.Contains:
                    ParameterExpression para = Expression.Parameter(typeof(TSub), QueryField.Parameters[0].Name);
                    var mtds = typeof(string).GetMethods().Where(x => x.Name == "Contains").ToArray();
                    MethodCallExpression met = Expression.Call(QueryField.Body, mtds[0], Expression.Constant(Value));
                    exp = Expression.Lambda<Func<TSub, bool>>(met, para);
                    break;
                case QueryParamJoinerOprater.Equal:
                    exp = Expression.Lambda<Func<TSub, bool>>(Expression.Equal(QueryField.Body, Expression.Constant(Value, this.FieldType)), QueryField.Parameters);
                    break;
                default:
                    throw new NotSupportedException();
            }
            var l = Repository.WhereSelect(exp, KeyField);

            ParameterExpression paraM = Expression.Parameter(typeof(TMain));
            MethodCallExpression metM = Expression.Call(Expression.Constant(l), typeof(List<TKey>).GetMethod("Contains"),
                MainField.Body);
            Expression<Func<TMain, bool>> lamb = Expression.Lambda<Func<TMain, bool>>(metM, paraM);
            return lamb;
        }
    }

    internal interface IQueryParamJoinerItem<TMain>
    {
        string ParamName { get; }
        Type FieldType { get; }
        Func<object, bool> ValidWhen();
    }

    public enum QueryParamJoinerOprater : byte
    {
        Equal = 1,
        Contains = 7
    }
}
