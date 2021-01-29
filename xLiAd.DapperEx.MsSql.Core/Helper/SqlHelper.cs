using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xLiAd.DapperEx.MsSql.Core.LocalParser;
using xLiAd.DapperEx.MsSql.Core.Model;

namespace xLiAd.DapperEx.MsSql.Core.Helper
{
    public static class SqlHelper
    {
        /// <summary>
        /// 是否使用本地模型转化器（而不使用Dapper的）
        /// </summary>
        public static bool UseLocalParser { get; set; } = false;
        private static TypeMapper TypeMapper = new TypeMapper();
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



        public static T QuerySingle<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var guid = DiagnosticExtension.Write(sql, param, cnn);
            try
            {
                var result = Dapper.SqlMapper.QuerySingle<T>(cnn, sql, param, transaction, commandTimeout, commandType);
                DiagnosticExtension.WriteAfter(guid);
                return result;
            }
            catch(Exception ex)
            {
                DiagnosticExtension.WriteError(guid, ex);
                throw;
            }
        }

        public static async Task<T> QuerySingleAsync<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var guid = DiagnosticExtension.Write(sql, param, cnn);
            try
            {
                var result = await Dapper.SqlMapper.QuerySingleAsync<T>(cnn, sql, param, transaction, commandTimeout, commandType);
                DiagnosticExtension.WriteAfter(guid);
                return result;
            }
            catch (Exception ex)
            {
                DiagnosticExtension.WriteError(guid, ex);
                throw;
            }
        }

        public static async Task<T> ExecuteScalarAsync<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var guid = DiagnosticExtension.Write(sql, param, cnn);
            try
            {
                var result = await Dapper.SqlMapper.ExecuteScalarAsync<T>(cnn, sql, param, transaction, commandTimeout, commandType);
                DiagnosticExtension.WriteAfter(guid);
                return result;
            }
            catch (Exception ex)
            {
                DiagnosticExtension.WriteError(guid, ex);
                throw;
            }
        }

        public static T ExecuteScalar<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var guid = DiagnosticExtension.Write(sql, param, cnn);
            try
            {
                var result = Dapper.SqlMapper.ExecuteScalar<T>(cnn, sql, param, transaction, commandTimeout, commandType);
                DiagnosticExtension.WriteAfter(guid);
                return result;
            }
            catch (Exception ex)
            {
                DiagnosticExtension.WriteError(guid, ex);
                throw;
            }
        }

        public static async Task<int> ExecuteAsync(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var guid = DiagnosticExtension.Write(sql, param, cnn);
            try
            {
                var result = await Dapper.SqlMapper.ExecuteAsync(cnn, sql, param, transaction, commandTimeout, commandType);
                DiagnosticExtension.WriteAfter(guid);
                return result;
            }
            catch (Exception ex)
            {
                DiagnosticExtension.WriteError(guid, ex);
                throw;
            }
        }

        public static int Execute(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var guid = DiagnosticExtension.Write(sql, param, cnn);
            try
            {
                var result = Dapper.SqlMapper.Execute(cnn, sql, param, transaction, commandTimeout, commandType);
                DiagnosticExtension.WriteAfter(guid);
                return result;
            }
            catch (Exception ex)
            {
                DiagnosticExtension.WriteError(guid, ex);
                throw;
            }
        }

        public static async Task<IDataReader> ExecuteReaderAsync(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var guid = DiagnosticExtension.Write(sql, param, cnn);
            try
            {
                var result = await Dapper.SqlMapper.ExecuteReaderAsync(cnn, sql, param, transaction, commandTimeout, commandType);
                DiagnosticExtension.WriteAfter(guid);
                return result;
            }
            catch (Exception ex)
            {
                DiagnosticExtension.WriteError(guid, ex);
                throw;
            }
        }

        public static IDataReader ExecuteReader(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var guid = DiagnosticExtension.Write(sql, param, cnn);
            try
            {
                var result = Dapper.SqlMapper.ExecuteReader(cnn, sql, param, transaction, commandTimeout, commandType);
                DiagnosticExtension.WriteAfter(guid);
                return result;
            }
            catch (Exception ex)
            {
                DiagnosticExtension.WriteError(guid, ex);
                throw;
            }
        }
        #region ReadGrid
        public static IEnumerable<T> ReadGrid<T>(this IDataReader reader)
        {
            var parser = GetRowParser<T>(reader);
            while (reader.Read())
                yield return parser(reader);
        }
        #endregion
        public static async Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var guid = DiagnosticExtension.Write(sql, param, cnn);
            try
            {
                IEnumerable<T> result;
                if (UseLocalParser)
                {
                    var reader = await cnn.ExecuteReaderAsync(sql, param, transaction, commandTimeout, commandType);
                    result = reader.ReadGrid<T>().ToList();
                    reader.Close();
                }
                else
                    result = await Dapper.SqlMapper.QueryAsync<T>(cnn, sql, param, transaction, commandTimeout, commandType);
                DiagnosticExtension.WriteAfter(guid);
                return result;
            }
            catch (Exception ex)
            {
                DiagnosticExtension.WriteError(guid, ex);
                throw;
            }
        }

        public static IEnumerable<T> Query<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var guid = DiagnosticExtension.Write(sql, param, cnn);
            try
            {
                IEnumerable<T> result;
                if (UseLocalParser)
                {
                    var reader = cnn.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
                    result = reader.ReadGrid<T>().ToList();
                    reader.Close();
                }
                else
                    result = Dapper.SqlMapper.Query<T>(cnn, sql, param, transaction, buffered, commandTimeout, commandType);
                DiagnosticExtension.WriteAfter(guid);
                return result;
            }
            catch (Exception ex)
            {
                DiagnosticExtension.WriteError(guid, ex);
                throw;
            }
        }

        public static Func<IDataReader, T> GetRowParser<T>(this IDataReader reader)
        {
            Func<IDataReader, T> result;
            if (UseLocalParser)
                result = TypeConvert.GetSerializer<T>(TypeMapper, reader);
            else
                result = Dapper.SqlMapper.GetRowParser<T>(reader);
            return result;
        }
    }
}
