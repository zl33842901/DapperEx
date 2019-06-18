using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.DapperEx.MsSql.Core.Core.Dialect
{
    public interface ISqlDialect
    {
        object ConvertParameterValue(object value);
        string ParameterPrefix { get; }
        /// <summary>
        /// 把表名称转成SQL语句中的形式
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string ParseTableName(string tableName);
        /// <summary>
        /// 把列名称转成SQL语句中的形式
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        string ParseColumnName(string columnName);
        /// <summary>
        /// 是否使用 Limit 代替 Top （POSTGRESQL专用）
        /// </summary>
        bool IsUseLimitInsteadOfTop { get; }
        string FormatInsertValues(string identityPropertyName, string paramString, string valueString);
        /// <summary>
        /// 是否支持JSON列 POSTGRESQL为真 其余为假
        /// </summary>
        bool SupportJsonColumn { get; }
        bool HasSerializer { get; }
        bool SupportArrayParam { get; }
        Func<object, string> Serializer { get; }
        Func<string, Type, object> Deserializer { get; }
        void SetSerializeFunc(Func<object, string> serializer, Func<string, Type, object> deserializer);
        PageListDialectEnum pageListDialectEnum { get; }
    }
    /// <summary>
    /// 分页语句风格
    /// </summary>
    public enum PageListDialectEnum
    {
        SqlServerAndPg = 0,
        Mysql = 1
    }
}
