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
    public class TransactionProvider
    {
        /// <summary>
        /// 不让随便 new
        /// </summary>
        /// <param name="_con"></param>
        internal TransactionProvider(SqlConnection _con)
        {
            Connection = _con;
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();
            Transaction = _con.BeginTransaction();
        }

        readonly IDbTransaction Transaction;
        readonly SqlConnection Connection;
        bool beenProcess = false;

        /// <summary>
        /// 获取事务仓储对象
        /// </summary>
        /// <typeparam name="T">请确认类对应的数据表在此仓储里</typeparam>
        /// <returns></returns>
        public RepositoryTrans<T> GetRepository<T>()
        {
            return new RepositoryTrans<T>(Connection, Transaction);
        }
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
