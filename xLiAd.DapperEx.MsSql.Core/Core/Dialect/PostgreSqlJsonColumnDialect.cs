using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.DapperEx.MsSql.Core.Core.Dialect
{
    public class PostgreSqlJsonColumnDialect : ISqlDialect
    {
        public string ParameterPrefix => throw new NotImplementedException();

        public bool IsUseLimitInsteadOfTop => throw new NotImplementedException();

        public bool SupportJsonColumn => false;

        public bool HasSerializer => throw new NotImplementedException();

        public Func<object, string> Serializer => throw new NotImplementedException();

        public Func<string, Type, object> Deserializer => throw new NotImplementedException();

        public bool SupportArrayParam => throw new NotImplementedException();

        public object ConvertParameterValue(object value)
        {
            throw new NotImplementedException();
        }

        public string FormatInsertValues(string identityPropertyName, string paramString, string valueString)
        {
            throw new NotImplementedException();
        }

        public string ParseColumnName(string columnName)
        {
            if (!columnName.StartsWith("'"))
            {
                return string.Format("'{0}'", columnName);
            }

            return columnName;
        }

        public string ParseTableName(string tableName)
        {
            throw new NotImplementedException();
        }

        public void SetSerializeFunc(Func<object, string> serializer, Func<string, Type, object> deserializer)
        {
            throw new NotImplementedException();
        }
    }
}
