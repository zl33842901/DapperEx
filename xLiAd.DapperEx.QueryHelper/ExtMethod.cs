using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using xLiAd.DapperEx.MsSql.Core.Helper;
using xLiAd.DapperEx.Repository;

namespace xLiAd.DapperEx.QueryHelper
{
    public static class ExtMethod
    {
        /// <summary>
        /// 把一个字符串转换成数据库中可能存在的类型值
        /// </summary>
        /// <param name="v"></param>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        internal static object ConvertTo(this string v, Type fieldType)
        {
            object realv;
            if (fieldType == typeof(string))
            {
                realv = v;
            }
            else if (fieldType == typeof(int?))
            {
                int? tv = v.NullOrEmpty() ? null : new Nullable<int>(v.ToInt());
                realv = tv;
            }
            else if (fieldType == typeof(int))
            {
                realv = v.ToInt();
            }
            else if (fieldType == typeof(DateTime?))
            {
                DateTime? tv = v.NullOrEmpty() ? null : (DateTime?)v.ToDateTime(DateTime.MinValue);
                realv = tv;
            }
            else if (fieldType == typeof(DateTime))
            {
                realv = v.ToDateTime(DateTime.MinValue);
            }
            else if (fieldType == typeof(decimal?))
            {
                decimal? tv = v.NullOrEmpty() ? null : (decimal?)v.ToDecimal();
                realv = tv;
            }
            else if (fieldType == typeof(decimal))
            {
                realv = v.ToDecimal();
            }
            else if (fieldType == typeof(byte?))
            {
                byte? tv = v.NullOrEmpty() ? null : (byte?)v.ToInt().ToByte(0);
                realv = tv;
            }
            else if (fieldType == typeof(byte))
            {
                realv = v.ToInt().ToByte(0);
            }
            else if (fieldType == typeof(bool?))
            {
                bool? tv = v.NullOrEmpty() ? null : (bool?)Convert.ToBoolean(v);
                realv = tv;
            }
            else if (fieldType == typeof(bool))
            {
                realv = Convert.ToBoolean(v);
            }
            else
                realv = new object();//这个可以确定不会匹配任何值
            return realv;
        }

        public static ICollection<TMain> LeftJoin<TMain, TSub, TKey>(this ICollection<TMain> l, IRepository<TSub> table, Expression<Func<TMain,TKey>> foreinKey,
            Expression<Func<TSub, TKey>> onEquals, Action<TMain, TSub> action, params Expression<Func<TSub, object>>[] fields)
        {
            var lid = l.Select(foreinKey.Compile()).ToList();

            ParameterExpression paraM = Expression.Parameter(typeof(TSub));
            MethodCallExpression metM = Expression.Call(Expression.Constant(lid), typeof(List<TKey>).GetMethod("Contains"), foreinKey.Body);
            Expression<Func<TSub, bool>> lamb = Expression.Lambda<Func<TSub, bool>>(metM, paraM);

            var eee = Expression.Lambda(Expression.Convert(onEquals.Body, typeof(object)), onEquals.Parameters) as Expression<Func<TSub, object>>;
            var fieldll = fields.ToList();
            fieldll.Add(eee);
            var lr = table.Where(lamb, fieldll.ToArray());

            foreach(var i in l)
            {
                var value = foreinKey.Compile().Invoke(i);
                var ee = Expression.Lambda<Func<TSub, bool>>(Expression.Equal(onEquals.Body, Expression.Constant(value)), onEquals.Parameters);
                var r = lr.Where(ee.Compile()).FirstOrDefault();
                action.Invoke(i, r);
            }
            return l;
        }
    }
}
