using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.DapperEx.MsSql.Core.Core.Dialect
{
    public class SqlServerDialect : ISqlDialect
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
            if (!tableName.StartsWith("["))
            {
                return string.Format("[{0}]", tableName);
            }

            return tableName;
        }

        public string ParseColumnName(string columnName)
        {
            if (!columnName.StartsWith("[") && !columnName.Contains("."))
            {
                return string.Format("[{0}]", columnName);
            }

            return columnName;
        }

        public bool IsUseLimitInsteadOfTop => false;

        public bool SupportJsonColumn => false;

        public bool HasSerializer => false;

        public Func<object, string> Serializer => throw new NotImplementedException();

        public Func<string, Type, object> Deserializer => throw new NotImplementedException();

        public bool SupportArrayParam => true;

        public PageListDialectEnum pageListDialectEnum => PageListDialectEnum.SqlServerAndPg;

        public string FormatInsertValues(string identityPropertyName, string paramString, string valueString)
        {
            string outputString;
            if (!string.IsNullOrEmpty(identityPropertyName))
                outputString = $" OUTPUT INSERTED.{identityPropertyName} as insertedid ";
            else
                outputString = string.Empty;
            return $"({paramString}) {outputString} VALUES  ({valueString})";
        }

        public void SetSerializeFunc(Func<object, string> serializer, Func<string, Type, object> deserializer)
        {
            throw new NotSupportedException();
        }
    }
}
