using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Dapper;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;
using xLiAd.DapperEx.MsSql.Core.Core.Interfaces;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace xLiAd.DapperEx.MsSql.Core.Core.Expression
{
    internal sealed class WhereExpression : SetParamVisitor, IWhereExpression
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
        readonly ISqlDialect Dialect;
        #region 执行解析

        /// <inheritdoc />
        /// <summary>
        /// 执行解析
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="prefix">字段前缀</param>
        /// <returns></returns>
        public WhereExpression(LambdaExpression expression, string prefix, ISqlDialect dialect)
        {
            _sqlCmd = new StringBuilder(100);
            Param = new DynamicParameters();
            _prefix = prefix;
            Dialect = dialect;

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
                string fn = node.Member.GetColumnAttributeName(Dialect);
                _sqlCmd.Append(fn);
                _sqlCmd.Append("=");
                SetParam(fn, true);
            }
            else
            {
                if(node.Expression is MemberExpression)
                {
                    var pi = ((MemberExpression)node.Expression).Member;
                    bool isJsonColumn = pi.CustomAttributes.Any(x => x.AttributeType == typeof(JsonColumnAttribute));
                    if (isJsonColumn)
                    {
                        var ss = node.ToString().Split('.');
                        if(ss.Length > 2)
                        {
                            var pmst = new StringBuilder();
                            pmst.Append(pi.GetColumnAttributeName(Dialect));
                            pmst.Append("->>");
                            pmst.Append(string.Join("->>", ss.Skip(2).Select(x => $"'{x}'")));
                            if (node.Member is PropertyInfo && ((PropertyInfo)node.Member).PropertyType == typeof(int))
                            {
                                _sqlCmd.Append("cast(");
                                _sqlCmd.Append(pmst.ToString());
                                _sqlCmd.Append(" as int)");
                            }
                            else if(node.Member is PropertyInfo && ((PropertyInfo)node.Member).PropertyType == typeof(DateTime))
                            {
                                _sqlCmd.Append("cast(");
                                _sqlCmd.Append(pmst.ToString());
                                _sqlCmd.Append(" as timestamp)");
                            }
                            else
                            {
                                _sqlCmd.Append(pmst.ToString());
                            }
                            
                            TempFileName = node.Member.Name;
                        }
                    }
                }
                else
                {
                    _sqlCmd.Append(node.Member.GetColumnAttributeName(Dialect));
                    TempFileName = node.Member.Name;
                }
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
                else
                {
                    fileName = GetParamName(fileName);
                }
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
            _sqlCmd.AppendFormat(" LIKE ");


            var argumentExpression = (ConstantExpression)node.Arguments[0];
            var value = argumentExpression.Value?.ToString() ?? string.Empty;
            value = value.Replace("%", "[%]");
            switch (node.Method.Name)
            {
                case "StartsWith":
                    {
                        SetParam(TempFileName, $"{value}%");
                    }
                    break;
                case "EndsWith":
                    {
                        SetParam(TempFileName, $"%{value}");
                    }
                    break;
                case "Contains":
                    {
                        SetParam(TempFileName, $"%{value}%");
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

        private void InDo(string tempFileName, IList o)
        {
            if (o.Count > 2199)
            {
                _sqlCmd.Append('(');
                int i = 0;
                foreach (var v in o)
                {
                    if (i++ > 0)
                    {
                        _sqlCmd.Append(',');
                    }
                    if (v == null)
                    {
                        _sqlCmd.Append("NULL");
                    }
                    else
                    {
                        _sqlCmd.Append("'");
                        _sqlCmd.Append(v.ToString().Replace("'", "''"));
                        _sqlCmd.Append("'");
                    }
                }
                _sqlCmd.Append(')');
            }
            else
            {
                SetParam(tempFileName, o);
            }
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
                _sqlCmd.AppendFormat(" IN ");
                InDo(TempFileName, arrayValue);
            }
            else
            {
                if(node.Arguments.Count == 2 && typeof(IList).IsAssignableFrom(node.Arguments[0].Type))
                {
                    var arg0 = node.Arguments[0];
                    var arg1 = node.Arguments[1];
                    //正常情况下，应该是 arg0 是常数，包含某列里的对象。但是JSONB列的情况特殊。
                    if(arg0.NodeType == ExpressionType.Constant)
                    {
                        Visit(arg1);
                        _sqlCmd.AppendFormat(" IN ");
                        var o = (IList)((ConstantExpression)arg0).Value;
                        InDo(TempFileName, o);
                    }
                    else
                    {
                        if (arg1.NodeType == ExpressionType.Constant && arg0.NodeType == ExpressionType.MemberAccess)
                        {
                            var pi = ((MemberExpression)arg0).Member as PropertyInfo;
                            if(pi != null && pi.CustomAttributes.Any(x=>x.AttributeType == typeof(JsonColumnAttribute)))
                            {
                                Visit(arg0);
                                _sqlCmd.AppendFormat(" @> '");
                                var cv = (ConstantExpression)arg1;
                                if(cv.Type == typeof(int))
                                {
                                    _sqlCmd.Append(cv.Value);
                                }
                                else
                                {
                                    _sqlCmd.Append("\"");
                                    _sqlCmd.Append(cv.Value.ToString().Replace("'","''"));
                                    _sqlCmd.Append("\"");
                                }
                                _sqlCmd.Append("'");
                            }
                        }
                    }
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
