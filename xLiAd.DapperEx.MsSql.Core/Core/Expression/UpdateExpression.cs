using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Dapper;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace xLiAd.DapperEx.MsSql.Core.Core.Expression
{
    internal class UpdateExpression : SetParamVisitor
    {
        #region sql指令

        private readonly StringBuilder _sqlCmd;

        private const string Prefix = "UPDATE_";
        /// <summary>
        /// sql指令
        /// </summary>
        public string SqlCmd => _sqlCmd.Length > 0 ? $" SET {_sqlCmd} " : "";

        public DynamicParameters Param { get; }

        #endregion
        readonly ISqlDialect Dialect;

        #region 执行解析
        /// <inheritdoc />
        /// <summary>
        /// 执行解析
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public UpdateExpression(LambdaExpression expression, ISqlDialect dialect)
        {
            _sqlCmd = new StringBuilder(100);
            Param = new DynamicParameters();
            Dialect = dialect;
            Visit(expression);
        }
        #endregion

        protected virtual bool SkipByOtherCondition(object o, PropertyInfo propertyInfo)
        {
            return false;
        }

        protected override System.Linq.Expressions.Expression VisitMember(MemberExpression node)
        {
            var memberInitExpression = node;

            var entity = ((ConstantExpression)TrimExpression.Trim(memberInitExpression)).Value;

            var properties = memberInitExpression.Type.GetPropertiesInDb(false);
            foreach (var item in properties)
            {
                if (item.CustomAttributes.Any(b => b.AttributeType == typeof(KeyAttribute)))
                    continue;
                if (item.CustomAttributes.Any(b => b.AttributeType == typeof(NoUpdateAttribute)))
                    continue;
                if (SkipByOtherCondition(entity, item))
                    continue;

                if (_sqlCmd.Length > 0)
                    _sqlCmd.Append(",");

                var paramName = item.Name;
                var value = item.GetValue(entity);
                var c = item.GetColumnAttributeName(Dialect);
                SetParam(c, paramName, value);
            }

            return node;
        }


        protected override System.Linq.Expressions.Expression VisitMemberInit(MemberInitExpression node)
        {
            var memberInitExpression = node;

            foreach (var item in memberInitExpression.Bindings)
            {
                var memberAssignment = (MemberAssignment)item;

                if (_sqlCmd.Length > 0)
                    _sqlCmd.Append(",");

                var paramName = memberAssignment.Member.Name;
                var c = memberAssignment.Member.GetColumnAttributeName(Dialect);
                var constantExpression = (ConstantExpression)memberAssignment.Expression;
                SetParam(c, paramName, constantExpression.Value);
            }

            return node;
        }

        private void SetParam(string sqlParamName, string paramName, object value)
        {
            paramName = GetParamName(paramName);
            var n = $"@{Prefix}{paramName}";
            _sqlCmd.AppendFormat(" {0}={1} ", sqlParamName, n);
            Param.Add(n, value);
        }
    }

    /// <summary>
    /// 只更新值不为 default 的字段
    /// </summary>
    internal class UpdateNotDefaultExpression : UpdateExpression
    {
        public UpdateNotDefaultExpression(LambdaExpression expression, ISqlDialect dialect) : base(expression, dialect) { }

        protected override bool SkipByOtherCondition(object o, PropertyInfo propertyInfo)
        {
            var ptype = propertyInfo.PropertyType;
            var pvalue = propertyInfo.GetValue(o);
            if (pvalue == null)
                return true;
            var defaultvalue = ptype.IsValueType ? Activator.CreateInstance(ptype) : null;
            return pvalue.Equals(defaultvalue);
        }
    }
}
