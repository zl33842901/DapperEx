using System.Threading.Tasks;

namespace xLiAd.DapperEx.MsSql.Core.Core.Interfaces
{
    public interface IInsert<T>
    {
        int Insert(T entity);
        Task<int> InsertAsync(T entity);
    }
}
