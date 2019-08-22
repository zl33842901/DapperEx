using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;
using xLiAd.DapperEx.MsSql.Core.Core.SetQ;
using xLiAd.DapperEx.MsSql.Core.Model;
using xLiAd.DapperEx.Repository;

namespace xLiAd.DapperEx.RepositoryMysql
{
    public class RepositoryMysql<T> : RepositoryBase<T>, IRepositoryMysql<T>
    {
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="repoXmlProvider"></param>
        /// <param name="exceptionHandler">抛错时的委托</param>
        /// <param name="throws">是否抛出错误，强烈建议保持默认值 true 不然报错时会返回正常数据。</param>
        public RepositoryMysql(string connectionString, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(new MySqlConnection(connectionString), repoXmlProvider, exceptionHandler, throws)
        {

        }
        public RepositoryMysql(IDbConnection _con, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(_con, repoXmlProvider, exceptionHandler, throws)
        {

        }
        public RepositoryMysql(IDbTransaction _tran, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(_tran.Connection, repoXmlProvider, exceptionHandler, throws, _tran)
        {

        }
        protected override ISqlDialect Dialect
        {
            get
            {
                var dialect = new MySqlDialect();
                return dialect;
            }
        }

        /// <summary>
        /// 获取事务提供
        /// </summary>
        /// <returns></returns>
        public override TransactionProviderBase GetTransaction()
        {
            if (DbTransaction != null)
                throw new Exception("已有事务实例的仓储不允许执行此操作。");
            else
                return new TransactionProviderMysql(con, this.RepoXmlProvider, ExceptionHandler, Throws);
        }

        public async Task<PageList<T>> PageListBySqlAsync(string sql, int pageIndex, int pageSize, Dictionary<string, string> dic = null)
        {
            int count = await GetScalarAsync<int>("select count(1) from (" + sql + ") o");
            var listsql = sql + " limit " + ((pageIndex - 1) * pageSize).ToString() + "," + pageSize;
            var list = await QueryBySqlAsync<T>(listsql, param: null);
            var result = new PageList<T>(pageIndex, pageSize, count, list.ToList());
            return result;
        }

        public PageList<T> PageListBySql(string sql, int pageIndex, int pageSize, Dictionary<string, string> dic = null)
        {
            int count = GetScalar<int>("select count(1) from (" + sql + ") o");
            var listsql = sql + " limit " + ((pageIndex - 1) * pageSize).ToString() + "," + pageSize;
            var list = QueryBySql<T>(listsql, param: null);
            var result = new PageList<T>(pageIndex, pageSize, count, list.ToList());
            return result;
        }
        public async Task<PageList<TResult>> PageListBySqlAsync<TResult>(string sql, int pageIndex, int pageSize, Dictionary<string, string> dic = null)
        {
            int count = await GetScalarAsync<int>("select count(1) from (" + sql + ") o");
            var listsql = sql + " limit " + ((pageIndex - 1) * pageSize).ToString() + "," + pageSize;
            var list = await QueryBySqlAsync<TResult>(listsql, param: null);
            var result = new PageList<TResult>(pageIndex, pageSize, count, list.ToList());
            return result;
        }

        public PageList<TResult> PageListBySql<TResult>(string sql, int pageIndex, int pageSize, Dictionary<string, string> dic = null)
        {
            int count = GetScalar<int>("select count(1) from (" + sql + ") o");
            var listsql = sql + " limit " + ((pageIndex - 1) * pageSize).ToString() + "," + pageSize;
            var list = QueryBySql<TResult>(listsql, param: null);
            var result = new PageList<TResult>(pageIndex, pageSize, count, list.ToList());
            return result;
        }
    }
}
