using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace xLiAd.DapperEx.MsSql.Core.Core
{
    public class DapperExEventArgs : EventArgs
    {
        public string Sql { get; }
        public TheDynamicParameters Params { get; }
        public string ExtMessage { get; }
        public DapperExEventArgs(string sql, TheDynamicParameters param, string extMessage) : base()
        {
            Sql = sql;
            Params = param;
            ExtMessage = extMessage;
        }
        public override string ToString()
        {
            return $"sql:{this.Sql} params:{this.Params.FormatString()} message:{this.ExtMessage}";
        }
    }

    public delegate void DapperExExceptionHandler(object sender, DapperExEventArgs e);
}
