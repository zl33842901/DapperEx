using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace xLiAd.DapperEx.MsSql.Core.Core.Expression
{
    public class DeleteExpression<T, TKey>
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

        #region 执行解析

        /// <inheritdoc />
        /// <summary>
        /// 执行解析
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DeleteExpression(TKey id)
        {
            _sqlCmd = new StringBuilder(100);
            Param = new DynamicParameters();
            _obj = id;
        }
        #endregion

        public void Resolve()
        {
            var propertyInfo = typeof(T).GetKeyPropertity();
            _sqlCmd.Append(propertyInfo.GetColumnAttributeName());
            _sqlCmd.Append(" = ");
            SetParam(propertyInfo.Name, _obj);
        }

        private void SetParam(string fileName, object value)
        {
            if (value != null)
            {
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
