using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using xLiAd.DapperEx.MsSql.Core.Core.Interfaces;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace xLiAd.DapperEx.MsSql.Core.Core.Expression
{
    internal sealed class WhereExpression : ExpressionVisitor,IWhereExpression
    {
        #region sql指令

        private readonly StringBuilder _sqlCmd;

        /// <summary>
        /// sql指令
        /// </summary>
        public string SqlCmd => _sqlCmd.Length > 0 ? $" WHERE {_sqlCmd} " : "";

        public DynamicParameters Param { get; }

        private string _tempFileName;

        private string TempFileName
        {
            get => _prefix + _tempFileName;
            set => _tempFileName = value;
        }

        private readonly string _prefix;

        private int ParamIndex = 1;//无名称参数序号

        #endregion
        private ExpressionTypeEnum lastExpression = ExpressionTypeEnum.None;/////最后一次的表达式
        private ExpressionTypeEnum lastSecondExpression = ExpressionTypeEnum.None;
        private ExpressionType? lastBinaryType = null;
        #region 执行解析

        /// <inheritdoc />
        /// <summary>
        /// 执行解析
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="prefix">字段前缀</param>
        /// <returns></returns>
        public WhereExpression(LambdaExpression expression, string prefix)
        {
            _sqlCmd = new StringBuilder(100);
            Param = new DynamicParameters();
            _prefix = prefix;

            var exp = TrimExpression.Trim(expression);
            Visit(exp);
        }
        #endregion

        protected override System.Linq.Expressions.Expression VisitNew(NewExpression node)
        {
            lastSecondExpression = lastExpression;
            lastExpression = ExpressionTypeEnum.New;
            var types = node.Arguments.Select(x => x.NodeType).ToArray();
            if (types.Any(x => x != ExpressionType.Constant))
                return base.VisitNew(node);
            else
            {
                var r = node.Constructor.Invoke(node.Arguments.Select(x => ((ConstantExpression)x).Value).ToArray());
                SetParam(TempFileName, r);
                return node;
            }
        }

        #region 访问成员表达式

        /// <inheritdoc />
        /// <summary>
        /// 访问成员表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override System.Linq.Expressions.Expression VisitMember(MemberExpression node)
        {
            if(lastExpression == ExpressionTypeEnum.None && node.Member is System.Reflection.PropertyInfo && ((System.Reflection.PropertyInfo)node.Member).PropertyType == typeof(bool))
            {
                //////////这里是  表达式只有表的某个BOOL型字段时   x => x.Deleted
                string fn = node.Member.GetColumnAttributeName();
                _sqlCmd.Append(fn);
                _sqlCmd.Append("=");
                SetParam(fn, true);
            }
            else
            {
                _sqlCmd.Append(node.Member.GetColumnAttributeName());
                TempFileName = node.Member.Name;
            }

            lastSecondExpression = lastExpression;
            lastExpression = ExpressionTypeEnum.Member;
            return node;
        }

        #endregion

        #region 访问二元表达式
        /// <inheritdoc />
        /// <summary>
        /// 访问二元表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override System.Linq.Expressions.Expression VisitBinary(BinaryExpression node)
        {
            lastSecondExpression = lastExpression;
            lastExpression = ExpressionTypeEnum.Binary;
            lastBinaryType = node.NodeType;

            if(node.NodeType == ExpressionType.ArrayIndex)
            {
                object vv;
                try { 
                    vv = (((IList)(((ConstantExpression)node.Left).Value))[(int)((ConstantExpression)node.Right).Value]);
                }
                catch(Exception e)
                {
                    throw new Exception("表达式中不要写带参数的数组！", e);
                }
                SetParam(TempFileName, vv);
            }
            else
            {
                _sqlCmd.Append("(");
                Visit(node.Left);

                _sqlCmd.Append(node.GetExpressionType());

                Visit(node.Right);
                _sqlCmd.Append(")");
            }

            return node;
        }
        #endregion

        #region 访问常量表达式
        /// <inheritdoc />
        /// <summary>
        /// 访问常量表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override System.Linq.Expressions.Expression VisitConstant(ConstantExpression node)
        {
            if(lastExpression == ExpressionTypeEnum.None && (node.Value as bool?) == true)
            {
                //x=>true 的情况
            }
            else if (lastExpression == ExpressionTypeEnum.None && (node.Value as bool?) == false)
            {
                //x=>false 的情况
                _sqlCmd.Append("(1=0)");
            }
            else
                SetParam(TempFileName, node.Value);


            lastSecondExpression = lastExpression;
            lastExpression = ExpressionTypeEnum.Constant;
            return node;
        }
        #endregion

        #region 访问方法表达式
        /// <inheritdoc />
        /// <summary>
        /// 访问方法表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override System.Linq.Expressions.Expression VisitMethodCall(MethodCallExpression node)
        {
            lastSecondExpression = lastExpression;
            lastExpression = ExpressionTypeEnum.MethodCall;
            if (node.Method.Name == "Contains" && (typeof(IEnumerable).IsAssignableFrom(node.Method.DeclaringType) || node.Method.DeclaringType == typeof(System.Linq.Enumerable)) &&
                node.Method.DeclaringType != typeof(string))
                In(node);
            else if (node.Method.Name == "get_Item")
                get_Item(node);
            else
                Like(node);

            return node;
        }

        #endregion

        private void SetParam(string fileName, object value)
        {
            if (value != null)
            {
                if (string.IsNullOrEmpty(fileName))
                    fileName = $"Parameter{ParamIndex++}";
                _sqlCmd.Append("@" + fileName);
                Param.Add(fileName, value);
            }
            else
            {
                _sqlCmd.Append("NULL");
            }
        }

        private void Like(MethodCallExpression node)
        {
            Visit(node.Object);
            var paramName = "@" + TempFileName;
            _sqlCmd.AppendFormat(" LIKE {0}", paramName);

            switch (node.Method.Name)
            {
                case "StartsWith":
                    {
                        var argumentExpression = (ConstantExpression)node.Arguments[0];
                        Param.Add(TempFileName, argumentExpression.Value + "%");
                    }
                    break;
                case "EndsWith":
                    {
                        var argumentExpression = (ConstantExpression)node.Arguments[0];
                        Param.Add(TempFileName, "%" + argumentExpression.Value);
                    }
                    break;
                case "Contains":
                    {
                        var argumentExpression = (ConstantExpression)node.Arguments[0];
                        Param.Add(TempFileName, "%" + argumentExpression.Value + "%");
                    }
                    break;
                default:
                    throw new Exception("the expression is no support this function");
            }
        }
        private void get_Item(MethodCallExpression node)
        {
            object vv;
            try
            {
                vv = (((IList)(((ConstantExpression)node.Object).Value))[(int)((ConstantExpression)node.Arguments[0]).Value]);
            }
            catch (Exception e)
            {
                throw new Exception("表达式中不要写带参数的数组！", e);
            }
            SetParam(TempFileName, vv);
        }

        private void In(MethodCallExpression node)
        {
            if(node.Object != null) { 
                var arrayValue = (IList)((ConstantExpression)node.Object).Value;
                if (arrayValue.Count == 0)
                {
                    _sqlCmd.Append(" 1 = 2");
                    return;
                }
                Visit(node.Arguments[0]);
                var paramName = "@" + TempFileName;
                _sqlCmd.AppendFormat(" IN {0}", paramName);
                Param.Add(TempFileName, arrayValue);
            }
            else
            {
                if(node.Arguments.Count == 2 && typeof(IEnumerable).IsAssignableFrom(node.Arguments[0].Type))
                {
                    Visit(node.Arguments[1]);
                    var paramName = "@" + TempFileName;
                    _sqlCmd.AppendFormat(" IN {0}", paramName);
                    var o = ((ConstantExpression)node.Arguments[0]).Value;
                    Param.Add(TempFileName, o);
                }
            }
        }
    }


    public enum ExpressionTypeEnum : byte
    {
        None = 0,
        New = 1,
        Member = 2,
        Binary = 3,
        Constant = 4,
        MethodCall = 5
    }
}
