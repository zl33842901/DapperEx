using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xLiAd.DapperEx.Repository
{
    /// <summary>
    /// 事务查询提供
    /// </summary>
    public class TransactionProvider : TransactionProviderBase
    {
        /// <summary>
        /// 暂时不让随便 new
        /// </summary>
        /// <param name="_con">数据库连接</param>
        /// <param name="repoXmlProvider">xml查询语句提供器</param>
        /// <param name="exceptionHandler">错误事件</param>
        /// <param name="throws">是否抛出错误</param>
        internal TransactionProvider(IDbConnection _con, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(_con, repoXmlProvider, exceptionHandler, throws)
        {
            
        }
        /// <summary>
        /// 获取事务仓储对象
        /// </summary>
        /// <typeparam name="T">请确认类对应的数据表在此仓储里</typeparam>
        /// <returns></returns>
        public override IRepository<T> GetRepository<T>()
        {
            return new Repository<T>(Transaction, this.RepoXmlProvider, ExExceptionHandler, Throws);
        }
    }
}
