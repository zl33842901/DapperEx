﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Data;

namespace xLiAd.DapperEx.MsSql.Core
{
    public static class DiagnosticExtension
    {
        internal static DiagnosticSource SqlDiagnosticSource = new DiagnosticListener("DapperExDiagnosticListener");
        internal static readonly string SqlDiagnosticSourceName = "xLiAd.DapperEx.CommandBefore";
        internal static readonly string SqlDiagnosticSourceAfterName = "xLiAd.DapperEx.CommandAfter";
        internal static readonly string SqlDiagnosticSourceErrorName = "xLiAd.DapperEx.CommandError";
        internal static bool SqlDiagnosticSourceEnabled;
        public static bool Enable { get; set; } = true;
        static DiagnosticExtension() { SqlDiagnosticSourceEnabled = SqlDiagnosticSource.IsEnabled(SqlDiagnosticSourceName); }
        public static Guid Write(string sql, object para, IDbConnection dbConnection)
        {
            if (SqlDiagnosticSourceEnabled && Enable)
            {
                var guid = Guid.NewGuid();
                SqlDiagnosticSource.Write(SqlDiagnosticSourceName, new { SqlString = sql, Params = para, OperationId = guid, DbConnection = dbConnection });
                return guid;
            }
            return Guid.Empty;
        }

        public static void WriteAfter(Guid guid)
        {
            if (SqlDiagnosticSourceEnabled && Enable)
                SqlDiagnosticSource.Write(SqlDiagnosticSourceAfterName, new { OperationId = guid });
        }
        public static void WriteError(Guid guid, Exception ex)
        {
            if (SqlDiagnosticSourceEnabled && Enable)
                SqlDiagnosticSource.Write(SqlDiagnosticSourceErrorName, new { OperationId = guid, Exception = ex });
        }
    }
}
