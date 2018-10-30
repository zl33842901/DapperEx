using System.Collections.Generic;
using xLiAd.DapperEx.MsSql.Core.Model;

namespace xLiAd.DapperEx.MsSql.Core.Core.Interfaces
{
    public interface IQuery<T>
    {
        T Get();

        List<T> ToList();

        PageList<T> PageList(int pageIndex, int pageSize);
    }
}
