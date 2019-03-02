using System;
using System.Data;
using System.Data.SqlClient;
using xLiAd.DapperEx.MsSql.Core.Core.SetC;
using xLiAd.DapperEx.MsSql.Core.Core.SetQ;

namespace xLiAd.DapperEx.MsSql.Core
{
    public static class DataBase
    {

        public static void Transaction(this IDbConnection sqlConnection, Action<TransContext> action)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();

            IDbTransaction transaction = sqlConnection.BeginTransaction();
            try
            {
                action(new TransContext { IDbTransaction = transaction, SqlConnection = sqlConnection });
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }

    public class TransContext
    {
        public IDbConnection SqlConnection { internal get; set; }

        public IDbTransaction IDbTransaction { internal get; set; }

        public QuerySet<T> QuerySet<T>()
        {
            return new QuerySet<T>(SqlConnection, new SqlProvider<T>(), IDbTransaction);
        }

        public CommandSet<T> CommandSet<T>()
        {
            return new CommandSet<T>(SqlConnection, new SqlProvider<T>(), IDbTransaction);
        }
    }
}
