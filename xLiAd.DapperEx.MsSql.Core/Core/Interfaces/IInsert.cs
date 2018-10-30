namespace xLiAd.DapperEx.MsSql.Core.Core.Interfaces
{
    public interface IInsert<T>
    {
        int Insert(T entity);
        int InsertWithIdOut(T entity);
    }
}
