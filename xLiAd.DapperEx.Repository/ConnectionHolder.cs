using System;
using System.Data;

namespace xLiAd.DapperEx.Repository
{
    /// <summary>
    /// 为实现切片事务，Repository 需要在实例化之后决定是否采用事务的方事执行。
    /// </summary>
    public interface IConnectionHolder
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        IDbConnection Connection { get; }
        /// <summary>
        /// 事务实例
        /// </summary>
        IDbTransaction Transaction { get; }
        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();
    }
    /// <summary>
    /// 为实现切片事务，Repository 需要在实例化之后决定是否采用事务的方事执行。
    /// </summary>
    public class ConnectionHolder : IConnectionHolder
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        public IDbConnection Connection { get; private set; }
        /// <summary>
        /// 事务实例
        /// </summary>
        public IDbTransaction Transaction { get; private set; } = null;
        bool beenProcess = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnection"></param>
        public ConnectionHolder(IDbConnection dbConnection)
        {
            this.Connection = dbConnection;
        }
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();
            Transaction = Connection.BeginTransaction();
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
