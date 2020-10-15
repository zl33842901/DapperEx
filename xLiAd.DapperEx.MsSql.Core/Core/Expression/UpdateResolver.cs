using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace xLiAd.DapperEx.MsSql.Core.Core.Expression
{
    public class UpdateExpression<T> : SetParamVisitor
    {
        #region sql指令

        private readonly StringBuilder _sqlCmd;

        private const string Prefix = "UPDATE_";
        /// <summary>
        /// sql指令
        /// </summary>
        public string SqlCmd => _sqlCmd.Length > 0 ? $" SET {_sqlCmd} " : "";

        public TheDynamicParameters Param { get; }

        #endregion
        readonly ISqlDialect Dialect;

        T Model;
        bool setAnyParam = false;
        public UpdateExpression(IEnumerable<LambdaExpression> expressionList, T model, ISqlDialect dialect)
        {
            _sqlCmd = new StringBuilder(100);
            Param = new TheDynamicParameters();
            Dialect = dialect;
            Model = model;
            foreach(var expression in expressionList)
            {
                Visit(expression);
            }
        }
        protected override System.Linq.Expressions.Expression VisitMember(MemberExpression node)
        {
            var memberInitExpression = node;
            var paramName = node.Member.Name;//参数名
            var pi = typeof(T).GetProperty(paramName);//要更新的列（属性）
            var value = pi.GetValue(Model);
            var c = node.Member.GetColumnAttributeName(Dialect);
            //要看一看是不是JSON列
            bool isJsonColumn = pi.CustomAttributes.Any(b => b.AttributeType == typeof(JsonColumnAttribute));//是否是JSON列
            if (isJsonColumn && Dialect.SupportJsonColumn && Dialect.HasSerializer)
            {
                value = Dialect.Serializer(value);
                SetParam(c, paramName, value, true);
            }
            else
            {
                SetParam(c, paramName, value);
            }

            return node;
        }
        private void SetParam(string sqlParamName, string paramName, object value, bool addJsonb = false)
        {
            paramName = GetParamName(paramName);
            var n = $"@{Prefix}{paramName}";
            if (setAnyParam)
                _sqlCmd.Append(",");
            _sqlCmd.AppendFormat(" {0} = {1} ", sqlParamName, n + (addJsonb ? "::jsonb" : string.Empty));
            Param.Add(n, value);
            setAnyParam = true;
        }
    }





    public class UpdateExpressionEx<T> : SetParamVisitor
    {
        #region sql指令

        private readonly StringBuilder _sqlCmd;

        private const string Prefix = "UPDATE_";
        /// <summary>
        /// sql指令
        /// </summary>
        public string SqlCmd => _sqlCmd.Length > 0 ? $" SET {_sqlCmd} " : "";

        public TheDynamicParameters Param { get; }

        #endregion
        object Value;
        readonly ISqlDialect Dialect;

        public UpdateExpressionEx(LambdaExpression expression, object value, ISqlDialect dialect)
        {
            _sqlCmd = new StringBuilder(100);
            Param = new TheDynamicParameters();
            Dialect = dialect;
            Value = value;
            Visit(expression);
        }
        protected override System.Linq.Expressions.Expression VisitMember(MemberExpression node)
        {
            var memberInitExpression = node;

            var paramName = node.Member.Name;
            var c = node.Member.GetColumnAttributeName(Dialect);
            SetParam(c, paramName, Value);

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
}
