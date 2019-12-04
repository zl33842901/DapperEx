using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace xLiAd.DapperEx.MsSql.Core.Helper
{
    /// <summary>
    /// 对表达式进行And、Or拼接的帮助类
    /// </summary>
    public static class ExpressionBuilder
    {
        /// <summary>
        /// 默认True条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Init<T>()
        {
            return expression => true;
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            if (first == null)
                return second;
            else if (second == null)
                return first;
            else
                return first.Compose(second, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            if (first == null)
                return second;
            else if (second == null)
                return first;
            else
                return first.Compose(second, Expression.OrElse);
        }

        public static Expression<Func<T, bool>> JoinAnd<T>(this IEnumerable<Expression<Func<T, bool>>> expressions){
            Expression<Func<T, bool>> result = null;
            foreach(var exp in expressions){
                result = result.And(exp);
            }
            return result;
        }

        public static Expression<Func<T, bool>> JoinOr<T>(this IEnumerable<Expression<Func<T, bool>>> expressions){
            Expression<Func<T, bool>> result = null;
            foreach(var exp in expressions){
                result = result.Or(exp);
            }
            return result;
        }

        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
            Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters
                .Select((oldParam, index) => new { oldParam, newParam = second.Parameters[index] })
                .ToDictionary(p => p.newParam, p => p.oldParam);

            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
    }

    internal class ParameterRebinder : ExpressionVisitor
    {
        readonly Dictionary<ParameterExpression, ParameterExpression> _parameterMap;

        ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            _parameterMap = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map,
            Expression newParameters)
        {
            return new ParameterRebinder(map).Visit(newParameters);
        }

        protected override Expression VisitParameter(ParameterExpression newParameters)
        {
            if (_parameterMap.TryGetValue(newParameters, out var replacement))
            {
                newParameters = replacement;
            }

            return base.VisitParameter(newParameters);
        }
    }
}
