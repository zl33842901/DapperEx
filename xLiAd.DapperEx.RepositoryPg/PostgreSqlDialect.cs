using System;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;

namespace xLiAd.DapperEx.RepositoryPg
{
    public class PostgreSqlDialect : ISqlDialect
    {
        const string _Space = " ";

        public object ConvertParameterValue(object value)
        {
            return value;
        }

        const string _parameterPrefix = "@";
        public string ParameterPrefix
        {
            get { return _parameterPrefix; }
        }

        public string ParseTableName(string tableName)
        {
            if (!tableName.StartsWith("\""))
            {
                return string.Format("\"{0}\"", tableName);
            }

            return tableName;
        }

        public string ParseColumnName(string columnName)
        {
            if (!columnName.StartsWith("\"") && !columnName.Contains("."))
            {
                return string.Format("\"{0}\"", columnName);
            }

            return columnName;
        }
    }
}
