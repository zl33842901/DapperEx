using System;
using System.Collections.Generic;
using System.Text;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;

namespace xLiAd.DapperEx.RepositoryMysql
{
    public class MySqlDialect : ISqlDialect
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

        public bool IsUseLimitInsteadOfTop => true;

        public bool SupportJsonColumn => true;

        public bool HasSerializer { get; private set; } = false;

        public Func<object, string> Serializer { get; private set; }

        public Func<string, Type, object> Deserializer { get; private set; }

        public bool SupportArrayParam => false;

        public PageListDialectEnum pageListDialectEnum => PageListDialectEnum.Mysql;

        public string ParseTableName(string tableName)
        {
            if (!tableName.StartsWith("`"))
            {
                return string.Format("`{0}`", tableName);
            }

            return tableName;
        }

        public string ParseColumnName(string columnName)
        {
            if (!columnName.StartsWith("`") && !columnName.Contains("."))
            {
                return string.Format("`{0}`", columnName);
            }

            return columnName;
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
