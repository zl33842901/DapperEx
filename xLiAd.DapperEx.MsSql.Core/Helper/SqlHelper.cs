using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace xLiAd.DapperEx.MsSql.Core.Helper
{
    public static class SqlHelper
    {
        ///// <summary>
        ///// 批量插入
        ///// </summary>
        ///// <param name="conn"></param>
        ///// <param name="list">源数据</param>
        //internal static void BulkCopy<T>(IDbConnection conn, IEnumerable<T> list)
        //{
        //    var dt = list.ToDataTable();

        //    using (conn)
        //    {
        //        if (conn.State == ConnectionState.Closed)
        //            conn.Open();

        //        using (var sqlbulkcopy = new SqlBulkCopy((SqlConnection)conn))
        //        {
        //            sqlbulkcopy.DestinationTableName = dt.TableName;
        //            for (var i = 0; i < dt.Columns.Count; i++)
        //            {
        //                sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
        //            }
        //            sqlbulkcopy.WriteToServer(dt);
        //        }
        //    }
        //}

        public static string FormatString(this DynamicParameters parameters)
        {
            try
            {
                var di = parameters.ToDictionary();
                StringBuilder sbP = new StringBuilder();
                sbP.Append("{ \r\n");
                List<string> ls = new List<string>();
                foreach (var i in di)
                {
                    if (!(i.Value is string) && i.Value is IEnumerable arr)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach(var ob in arr)
                        {
                            if (sb.Length > 0)
                                sb.Append(", ");
                            sb.Append('"');
                            sb.Append(ob);
                            sb.Append('"');
                        }
                        ls.Add($"  \"{i.Key}\" : [ { sb } ]");
                    }
                    else
                    {
                        ls.Add($"  \"{i.Key}\" : \"{i.Value}\"");
                    }
                }
                sbP.Append(string.Join(",\r\n", ls));
                sbP.Append("\r\n}");
                return sbP.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static Dictionary<string, object> ToDictionary(this DynamicParameters parameters)
        {
            try
            {
                var p = typeof(DynamicParameters).GetField("parameters", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var di = p.GetValue(parameters) as IDictionary;// Dictionary<string, Dapper.DynamicParameters.ParamInfo>;
                Type paramInfoType = Type.GetType("Dapper.DynamicParameters+ParamInfo,Dapper");
                var pp = paramInfoType.GetProperty("Value");
                Dictionary<string, object> ls = new Dictionary<string, object>();
                foreach (DictionaryEntry i in di)
                {
                    ls.Add(i.Key.ToString(), pp.GetValue(i.Value));
                }
                return ls;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
