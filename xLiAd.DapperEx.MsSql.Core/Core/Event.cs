using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xLiAd.DapperEx.MsSql.Core.Core
{
    public class DapperExEventArgs : EventArgs
    {
        public string Sql { get; }
        public DynamicParameters Params { get; }
        public string ExtMessage { get; }
        public DapperExEventArgs(string sql, DynamicParameters param, string extMessage) : base()
        {
            Sql = sql;
            Params = param;
            ExtMessage = extMessage;
        }
        public override string ToString()
        {
            try
            {
                var p = typeof(DynamicParameters).GetField("parameters", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var di = p.GetValue(this.Params) as IDictionary;// Dictionary<string, Dapper.DynamicParameters.ParamInfo>;
                Type paramInfoType = Type.GetType("Dapper.DynamicParameters+ParamInfo,Dapper");
                var pp = paramInfoType.GetProperty("Value");
                StringBuilder sbP = new StringBuilder();
                sbP.Append("{ ");
                foreach (DictionaryEntry i in di)
                {
                    sbP.Append($"\"{i.Key}\" : \"{pp.GetValue(i.Value)}\",\r\n");
                }
                sbP.Append("}");
                return $"sql:{this.Sql} params:{sbP.ToString()} message:{this.ExtMessage}";
            }
            catch (Exception)
            {
                return $"sql:{this.Sql} message:{this.ExtMessage}";
            }
        }
    }

    public delegate void DapperExExceptionHandler(object sender, DapperExEventArgs e);
}
