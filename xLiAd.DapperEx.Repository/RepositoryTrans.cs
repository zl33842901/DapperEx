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
            Command = new CommandSet<T>(_con, new SqlProvider<T>(), _tran);
            Query = new QuerySet<T>(_con, new SqlProvider<T>(), _tran);
        }

        readonly IDbTransaction Transaction;
        readonly CommandSet<T> Command;
        readonly QuerySet<T> Query;
        /// <summary>
        /// 重写 CommandSet
        /// </summary>
        protected override CommandSet<T> CommandSet
        {
            get
            {
                return Command;
            }
        }
        protected override QuerySet<T> QuerySet
        {
            get
            {
                return Query;
            }
        }
    }
}
