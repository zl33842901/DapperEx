using System;
using System.Collections.Generic;
using System.Text;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;

namespace xLiAd.DapperEx.RepositoryOracle
{
    public class OracleDialect : ISqlDialect
    {
        public string ParameterPrefix => ":";

        public bool IsUseLimitInsteadOfTop => false;

        public bool SupportJsonColumn => false;

        public bool HasSerializer { get; private set; } = false;

        public Func<object, string> Serializer { get; private set; }

        public Func<string, Type, object> Deserializer { get; private set; }

        public bool SupportArrayParam => false;

        public PageListDialectEnum pageListDialectEnum => PageListDialectEnum.SqlServerAndPg;

        public object ConvertParameterValue(object value)
        {
            return value;
        }

        public string FormatInsertValues(string identityPropertyName, string paramString, string valueString)
        {
            string outputString;
            if (!string.IsNullOrEmpty(identityPropertyName))
                outputString = $" ;SELECT @@IDENTITY";
            else
                outputString = string.Empty;
            return $"({paramString}) VALUES  ({valueString}) {outputString}";
        }

        public string ParseTableName(string tableName)
        {
            return tableName;
        }

        public string ParseColumnName(string columnName)
        {
            if (columnName.Contains(" "))
            {
                return string.Format("\"\"", columnName);
            }
            return columnName;
        }

        public void SetSerializeFunc(Func<object, string> serializer, Func<string, Type, object> deserializer)
        {
            if (serializer != null && deserializer != null)
            {
                this.Serializer = serializer;
                this.Deserializer = deserializer;
                HasSerializer = true;
            }
        }
    }
}
