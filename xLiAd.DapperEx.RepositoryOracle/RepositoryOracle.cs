using System;
using System.Data;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;
using xLiAd.DapperEx.Repository;
using Oracle.ManagedDataAccess.Client;

namespace xLiAd.DapperEx.RepositoryOracle
{
    public class RepositoryOracle<T> : RepositoryBase<T>, IRepository<T>
    {
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="repoXmlProvider"></param>
        /// <param name="exceptionHandler">抛错时的委托</param>
        /// <param name="throws">是否抛出错误，强烈建议保持默认值 true 不然报错时会返回不正常数据。</param>
        public RepositoryOracle(string connectionString, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(new OracleConnection(connectionString), repoXmlProvider, exceptionHandler, throws)
        {

        }
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="_con">数据库连接</param>
        /// <param name="repoXmlProvider"></param>
        /// <param name="exceptionHandler">抛错时的委托</param>
        /// <param name="throws">是否抛出错误，强烈建议保持默认值 true 不然报错时会返回不正常数据。</param>
        public RepositoryOracle(IDbConnection _con, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(_con, repoXmlProvider, exceptionHandler, throws)
        {

        }
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="_con">数据库连接</param>
        /// <param name="repoXmlProvider"></param>
        /// <param name="exceptionHandler"></param>
        /// <param name="throws"></param>
        /// <param name="_tran">事务</param>
        public RepositoryOracle(IDbTransaction _tran, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(_tran.Connection, repoXmlProvider, exceptionHandler, throws, _tran)
        {

        }
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="repoXmlProvider"></param>
        /// <param name="exceptionHandler"></param>
        /// <param name="throws"></param>
        /// <param name="connectionHolder"></param>
        public RepositoryOracle(IConnectionHolder connectionHolder, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(connectionHolder, repoXmlProvider, exceptionHandler, throws)
        {

        }
        /// <summary>
        /// 数据库语法器
        /// </summary>
        protected override ISqlDialect Dialect => new OracleDialect();

        /// <summary>
        /// 获取事务提供
        /// </summary>
        /// <returns></returns>
        public override TransactionProviderBase GetTransaction()
        {
            if (DbTransaction != null)
                throw new Exception("已有事务实例的仓储不允许执行此操作。");
            else
                return new TransactionProviderOracle(con, this.RepoXmlProvider, ExceptionHandler, Throws);
        }
    }
}
