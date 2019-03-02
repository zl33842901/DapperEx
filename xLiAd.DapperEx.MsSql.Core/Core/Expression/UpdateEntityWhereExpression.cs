using System.Text;
using Dapper;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;
using xLiAd.DapperEx.MsSql.Core.Core.Interfaces;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace xLiAd.DapperEx.MsSql.Core.Core.Expression
{
    /// <summary>
    /// 给我一个对象，我根据对象的主键、对象的主键的值，返回给你一个条件语句(这个类名起得有点不对头，看起来跟高耦合似的)
    /// </summary>
    internal class UpdateEntityWhereExpression : SetParam,IWhereExpression
    {
        #region sql指令

        private readonly StringBuilder _sqlCmd;

        /// <summary>
        /// sql指令
        /// </summary>
        public string SqlCmd => _sqlCmd.Length > 0 ? $" WHERE {_sqlCmd} " : "";

        public DynamicParameters Param { get; }

        private readonly object _obj;

        #endregion
        readonly ISqlDialect Dialect;

        #region 执行解析

        /// <inheritdoc />
        /// <summary>
        /// 执行解析
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public UpdateEntityWhereExpression(object obj, ISqlDialect dialect)
        {
            _sqlCmd = new StringBuilder(100);
            Param = new DynamicParameters();
            _obj = obj;
            Dialect = dialect;
        }
        #endregion

        public void Resolve()
        {
            var propertyInfo = _obj.GetKeyPropertity();
            _sqlCmd.Append(propertyInfo.GetColumnAttributeName(Dialect));
            _sqlCmd.Append(" = ");
            SetParam(propertyInfo.Name, propertyInfo.GetValue(_obj));
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
