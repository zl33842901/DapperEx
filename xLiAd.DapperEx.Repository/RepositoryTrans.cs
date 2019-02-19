using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xLiAd.DapperEx.MsSql.Core;
using xLiAd.DapperEx.MsSql.Core.Core.SetC;
using xLiAd.DapperEx.MsSql.Core.Core.SetQ;

namespace xLiAd.DapperEx.Repository
{
    /// <summary>
    /// 带有事务的仓储（事务会锁表，请减小事务操作粒度）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryTrans<T> : Repository<T>
    {
        /// <summary>
        /// 还是不要让人随便 new 了
        /// </summary>
        /// <param name="_con"></param>
        /// <param name="_tran"></param>
        internal RepositoryTrans(SqlConnection _con, IDbTransaction _tran) : base(_con)
        {
            Transaction = _tran;
            Connection = _con;
        }
        readonly SqlConnection Connection;
        readonly IDbTransaction Transaction;
        /// <summary>
        /// 重写 CommandSet
        /// </summary>
        protected override CommandSet<T> CommandSet => new CommandSet<T>(Connection, new SqlProvider<T>(), Transaction);
        /// <summary>
        /// 重写 QuerySet
        /// </summary>
        protected override QuerySet<T> QuerySet => new QuerySet<T>(Connection, new SqlProvider<T>(), Transaction);
        /// <summary>
        /// 重写事务对象
        /// </summary>
        protected override IDbTransaction DbTransaction => Transaction;
    }
}
