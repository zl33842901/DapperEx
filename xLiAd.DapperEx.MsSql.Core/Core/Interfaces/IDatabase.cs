using System.Data;
using xLiAd.DapperEx.MsSql.Core.Core.SetC;
using xLiAd.DapperEx.MsSql.Core.Core.SetQ;

namespace xLiAd.DapperEx.MsSql.Core.Core.Interfaces
{
    public interface IDatabase
    {
        QuerySet<T> QuerySet<T>();

        CommandSet<T> CommandSet<T>();

        IDbConnection GetConnection();
    }
}
