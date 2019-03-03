using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;
using xLiAd.DapperEx.Repository;

namespace xLiAd.DapperEx.RepositoryPg
{
    public class RepositoryPg<T> : RepositoryBase<T>, IRepository<T>
    {
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="repoXmlProvider"></param>
        /// <param name="exceptionHandler">抛错时的委托</param>
        /// <param name="throws">是否抛出错误，强烈建议保持默认值 true 不然报错时会返回正常数据。</param>
        public RepositoryPg(string connectionString, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(new Npgsql.NpgsqlConnection(connectionString), repoXmlProvider, exceptionHandler, throws)
        {

        }
        public RepositoryPg(IDbConnection _con, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(_con, repoXmlProvider, exceptionHandler, throws)
        {

        }
        internal RepositoryPg(IDbConnection _con, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true, IDbTransaction _tran = null)
            : base(_con, repoXmlProvider, exceptionHandler, throws, _tran)
        {

        }
        protected override ISqlDialect Dialect => new PostgreSqlDialect();

        /// <summary>
        /// 获取事务提供
        /// </summary>
        /// <returns></returns>
        public override TransactionProviderBase GetTransaction()
        {
            if (DbTransaction != null)
                throw new Exception("已有事务实例的仓储不允许执行此操作。");
            else
                return new TransactionProviderPg(con, this.RepoXmlProvider, ExceptionHandler, Throws);
        }
    }
}
