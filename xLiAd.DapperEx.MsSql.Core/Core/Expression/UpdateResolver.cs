using Dapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
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

        public DynamicParameters Param { get; }

        #endregion
        T Model;
        bool setAnyParam = false;
        public UpdateExpression(IEnumerable<LambdaExpression> expressionList, T model)
        {
            _sqlCmd = new StringBuilder(100);
            Param = new DynamicParameters();
            Model = model;
            foreach(var expression in expressionList)
            {
                Visit(expression);
            }
        }
        protected override System.Linq.Expressions.Expression VisitMember(MemberExpression node)
        {
            var memberInitExpression = node;

            var paramName = node.Member.Name;
            var value = typeof(T).GetProperty(paramName).GetValue(Model);
            var c = node.Member.GetColumnAttributeName();
            SetParam(c, paramName, value);

            return node;
        }
        private void SetParam(string sqlParamName, string paramName, object value)
        {
            paramName = GetParamName(paramName);
            var n = $"@{Prefix}{paramName}";
            if (setAnyParam)
                _sqlCmd.Append(",");
            _sqlCmd.AppendFormat(" {0}={1} ", sqlParamName, n);
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

        public DynamicParameters Param { get; }

        #endregion
        object Value;

        public UpdateExpressionEx(LambdaExpression expression, object value)
        {
            _sqlCmd = new StringBuilder(100);
            Param = new DynamicParameters();
            Value = value;
            Visit(expression);
        }
        protected override System.Linq.Expressions.Expression VisitMember(MemberExpression node)
        {
            var memberInitExpression = node;

            var paramName = node.Member.Name;
            var c = node.Member.GetColumnAttributeName();
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
