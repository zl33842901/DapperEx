using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xLiAd.DapperEx.Repository
{
    /// <summary>
    /// 事务查询提供者 基类
    /// </summary>
    public abstract class TransactionProviderBase
    {
        /// <summary>
        /// 暂时不让随便 new
        /// </summary>
        /// <param name="_con">数据库连接</param>
        /// <param name="repoXmlProvider">xml查询语句提供器</param>
        /// <param name="exceptionHandler">错误事件</param>
        /// <param name="throws">是否抛出错误</param>
        protected TransactionProviderBase(IDbConnection _con, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
        {
            Connection = _con;
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();
            Transaction = _con.BeginTransaction();
            RepoXmlProvider = repoXmlProvider;
            ExExceptionHandler = exceptionHandler;
            Throws = throws;
        }
        /// <summary>
        /// 事务对象
        /// </summary>
        protected readonly IDbTransaction Transaction;
        /// <summary>
        /// 数据库连接
        /// </summary>
        protected readonly IDbConnection Connection;
        /// <summary>
        /// xml查询语句提供器
        /// </summary>
        protected RepoXmlProvider RepoXmlProvider;
        /// <summary>
        /// 错误事件
        /// </summary>
        protected MsSql.Core.Core.DapperExExceptionHandler ExExceptionHandler;
        /// <summary>
        /// 是否抛出错误
        /// </summary>
        protected bool Throws;

        bool beenProcess = false;

        /// <summary>
        /// 获取事务仓储对象
        /// </summary>
        /// <typeparam name="T">请确认类对应的数据表在此仓储里</typeparam>
        /// <returns></returns>
        public abstract IRepository<T> GetRepository<T>();
        /// <summary>
        /// 尝试提交事务并关闭数据库连接
        /// </summary>
        public void Commit()
        {
            if (!beenProcess)
            {
                try
                {
                    Transaction.Commit();
                }
                catch
                {
                    Transaction.Rollback();
                    throw;
                }
                finally
                {
                    Connection.Close();
                }
                beenProcess = true;
            }
        }
        /// <summary>
        /// 回滚事务  需要在catch 里显示调用，不然直到资源释放才能把表解锁。
        /// </summary>
        public void Rollback()
        {
            if (!beenProcess)
            {
                Transaction.Rollback();
                beenProcess = true;
                Connection.Close();
            }
        }
    }
}
