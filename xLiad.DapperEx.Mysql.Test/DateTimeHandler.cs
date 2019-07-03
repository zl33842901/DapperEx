using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace xLiad.DapperEx.Mysql.Test
{
    public class DateTimeHandler : SqlMapper.TypeHandler<DateTime>
    {
        private DateTimeHandler()
        {
        }

        public static readonly DateTimeHandler Default = new DateTimeHandler();

        public override DateTime Parse(object value)
        {
            if (value is DateTime v)
            {
                return v;
            }

            throw new FormatException("Invalid conversion to DateTime");
        }

        public override void SetValue(IDbDataParameter parameter, DateTime value)
        {
            // ... null, range checks etc ...
            parameter.DbType = System.Data.DbType.DateTime;
            parameter.Value = value;
        }
    }
}
