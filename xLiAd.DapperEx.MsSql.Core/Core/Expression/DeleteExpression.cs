using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;
using xLiAd.DapperEx.MsSql.Core.Core.Interfaces;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace xLiAd.DapperEx.MsSql.Core.Core.Expression
{
    /// <summary>
    /// 给我一个类型、一个和类型对应的值，我获取到这个类型的主键，再根据主键和值，返回给你一个SQL条件子句（这个类名起的有问题，应该叫KeyWhereExpression）
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public class DeleteExpression<T, TKey> : SetParam, IWhereExpression
    {
        #region sql指令

        private readonly StringBuilder _sqlCmd;

        /// <summary>
        /// sql指令
        /// </summary>
        public string SqlCmd => _sqlCmd.Length > 0 ? $" WHERE {_sqlCmd} " : "";

        public DynamicParameters Param { get; }

        private readonly TKey _obj;

        #endregion
        readonly ISqlDialect Dialect;

        #region 执行解析

        /// <inheritdoc />
        /// <summary>
        /// 执行解析
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DeleteExpression(TKey id, ISqlDialect dialect)
        {
            _sqlCmd = new StringBuilder(100);
            Param = new DynamicParameters();
            _obj = id;
            Dialect = dialect;
        }
        #endregion

        public void Resolve()
        {
            var propertyInfo = typeof(T).GetKeyPropertity();
            _sqlCmd.Append(propertyInfo.GetColumnAttributeName(Dialect));
            _sqlCmd.Append(" = ");
            SetParam(propertyInfo.Name, _obj);
        }

        private void SetParam(string fileName, object value)
        {
            if (value != null)
            {
                fileName = GetParamName(fileName);
                _sqlCmd.Append("@" + fileName);
                Param.Add(fileName, value);
            }
            else
            {
                _sqlCmd.Append("NULL");
            }
        }
    }
}
