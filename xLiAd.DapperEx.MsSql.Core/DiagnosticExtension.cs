using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Dapper;

namespace xLiAd.DapperEx.MsSql.Core
{
    internal static class DiagnosticExtension
    {
        internal static DiagnosticSource SqlDiagnosticSource = new DiagnosticListener("DapperExDiagnosticListener");
        internal static readonly string SqlDiagnosticSourceName = "xLiAd.DapperEx.CommandBefore";
        internal static bool SqlDiagnosticSourceEnabled;
        static DiagnosticExtension() { SqlDiagnosticSourceEnabled = SqlDiagnosticSource.IsEnabled(SqlDiagnosticSourceName); }
        public static void Write(string sql, DynamicParameters para)
        {
            if (SqlDiagnosticSourceEnabled)
                SqlDiagnosticSource.Write(SqlDiagnosticSourceName, new { SqlString = sql, Params =  para });
        }
    }
}
