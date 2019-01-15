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
    /// <typeparam name="TMain"></typeparam>
    public class QueryParamJoiner<TMain>
    {
        private List<IQueryParamJoinerItem<TMain>> Items = new List<IQueryParamJoinerItem<TMain>>();
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
            }
            QueryParamJoinerItem<TMain, TSub, TKey> item = new QueryParamJoinerItem<TMain, TSub, TKey>(mainField, queryField, oprater,keyField, repository,
                when, paramName);
            Items.Add(item);
        }
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
