using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.DapperEx.MsSql.Core.Core.Dialect
{
    public interface ISqlDialect
    {
        object ConvertParameterValue(object value);
        string ParameterPrefix { get; }
        string ParseTableName(string tableName);
        string ParseColumnName(string columnName);
        bool IsUseLimitInsteadOfTop { get; }
        string FormatInsertValues(string identityPropertyName, string paramString, string valueString);
    }
}
