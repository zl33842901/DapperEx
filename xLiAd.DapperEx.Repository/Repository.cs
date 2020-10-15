using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using xLiAd.DapperEx.MsSql.Core;
using xLiAd.DapperEx.MsSql.Core.Core.Interfaces;
using xLiAd.DapperEx.MsSql.Core.Core.SetC;
using xLiAd.DapperEx.MsSql.Core.Core.SetQ;
using xLiAd.DapperEx.MsSql.Core.Model;
using xLiAd.DapperEx.MsSql.Core.Helper;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;

namespace xLiAd.DapperEx.Repository
{
    /// <summary>
    /// 仓储类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : RepositoryBase<T>, IRepository<T>
    {
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="repoXmlProvider"></param>
        /// <param name="exceptionHandler">抛错时的委托</param>
        /// <param name="throws">是否抛出错误，强烈建议保持默认值 true 不然报错时会返回不正常数据。</param>
        public Repository(string connectionString, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(new SqlConnection(connectionString), repoXmlProvider, exceptionHandler, throws)
        {

        }
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="_con">数据库连接</param>
        /// <param name="repoXmlProvider"></param>
        /// <param name="exceptionHandler">抛错时的委托</param>
        /// <param name="throws">是否抛出错误，强烈建议保持默认值 true 不然报错时会返回不正常数据。</param>
        public Repository(IDbConnection _con, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(_con, repoXmlProvider, exceptionHandler, throws)
        {

        }
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="repoXmlProvider"></param>
        /// <param name="exceptionHandler"></param>
        /// <param name="throws"></param>
        /// <param name="_tran">事务</param>
        public Repository(IDbTransaction _tran, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
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
        public Repository(IConnectionHolder connectionHolder, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(connectionHolder, repoXmlProvider, exceptionHandler, throws)
        {

        }
        /// <summary>
        /// 数据库语法器
        /// </summary>
        protected override ISqlDialect Dialect => new SqlServerDialect();

        /// <summary>
        /// 获取事务提供
        /// </summary>
        /// <returns></returns>
        public override TransactionProviderBase GetTransaction()
        {
            if (DbTransaction != null)
                throw new Exception("已有事务实例的仓储不允许执行此操作。");
            else
                return new TransactionProvider(con, this.RepoXmlProvider, ExceptionHandler, Throws);
        }
        /*
         con.QuerySet<Model>().Sum(a => a.IntField);

            var r = con.QuerySet<Model>().Where(predicate)
                .OrderBy(field)
                .Select(a => a.field)
                .UpdateSelect();

            con.Transaction(tc =>
            {
                var m = tc.QuerySet<Model>().Where(predicate).Select(a => a.field).Get();
                tc.CommandSet<Model>().Where(a => a.field == m).Delete();
                tc.CommandSet<Model>().Insert(new Model
                {
                    Name = "我"
                });
            });
            con.Dispose();
         */
    }

    /// <summary>
    /// 提供修改 UseLocalParser 的类
    /// </summary>
    public static class Repository
    {
        /// <summary>
        /// 是否使用本地模型转换器（而不使用 Dapper 模型转换器）
        /// Dapper 模型转换器： 成熟，应用广泛
        /// 本地模型转换器：性能好，源码不是Emit 可修改，解决了mysql的时间类型与 Datetime? 类型转换的BUG；未经过大量测试或许有BUG。
        /// </summary>
        public static bool UseLocalParser
        {
            get => SqlHelper.UseLocalParser;
            set => SqlHelper.UseLocalParser = value;
        }
    }
}
