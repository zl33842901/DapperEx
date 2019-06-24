using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xLiAd.DapperEx.MsSql.Core.Model;
using xLiAd.DapperEx.Repository;

namespace xLiAd.DapperEx.RepositoryMysql
{
    public interface IRepositoryMysql<T> : IRepository<T>
    {
        Task<PageList<T>> PageListBySqlAsync(string sql, int pageIndex, int pageSize, Dictionary<string, string> dic = null);
        PageList<T> PageListBySql(string sql, int pageIndex, int pageSize, Dictionary<string, string> dic = null);
    }
}
