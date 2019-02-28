using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using xLiAd.DapperEx.MsSql.Core.Core.Interfaces;
using xLiAd.DapperEx.MsSql.Core.Model;

namespace xLiAd.DapperEx.MsSql.Core.Core.SetQ
{
    /// <summary>
    /// 查询器抽象基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Query<T> : IQuery<T>, IUpdateSelect<T>, ISql
    {
        public event DapperExExceptionHandler ErrorHappened;
        protected readonly SqlProvider<T> SqlProvider;
        protected readonly IDbConnection DbCon;
        protected readonly IDbTransaction DbTransaction;
        /// <summary>
        /// 是否抛出错误，如果不抛，可以用 event 实现日志功能
        /// </summary>
        public bool Throws { get; }
        /// <summary>
        /// 刚刚执行过的SQL语句（注：由于单例模式时会发生线程问题，本属性只作为调试用，不应该在程序里引用。）
        /// </summary>
        public string SqlString { get; private set; }
        /// <summary>
        /// 刚刚执行过的语句使用的参数（注：由于单例模式时会发生线程问题，本属性只作为调试用，不应该在程序里引用。）
        /// </summary>
        public DynamicParameters Params { get; private set; }
        private void SetSql()
        {
            if (SqlProvider.SqlString != null)
                this.SqlString = SqlProvider.SqlString;
            if (SqlProvider.Params != null)
                this.Params = SqlProvider.Params;
        }

        protected DataBaseContext<T> SetContext { get; set; }
        /// <summary>
        /// 新建一个查询器
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="sqlProvider">SQL转换器</param>
        /// <param name="dbTransaction">事务</param>
        /// <param name="throws">是否抛出错误</param>
        protected Query(IDbConnection conn, SqlProvider<T> sqlProvider, IDbTransaction dbTransaction = null, bool throws = true)
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
            DbTransaction = dbTransaction;
            Throws = throws;
        }

        public T Get()
        {
            SqlProvider.FormatGet();
            SetSql();
            return QrFd(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }
        public T Get<TKey>(TKey id)
        {
            SqlProvider.FormatGet(id);
            SetSql();
            return QrFd(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
        }

        public virtual List<T> ToList()
        {
            SqlProvider.FormatToList();
            SetSql();
            return Qr(SqlProvider.SqlString, SqlProvider.Params, DbTransaction).ToList();
        }
        public virtual List<T> ToList(IEnumerable<LambdaExpression> selector)
        {
            SqlProvider.FormatToList(selector);
            SetSql();
            return Qr(SqlProvider.SqlString, SqlProvider.Params, DbTransaction).ToList();
        }

        public PageList<T> PageList(int pageIndex, int pageSize)
        {
            SqlProvider.FormatToPageList(pageIndex, pageSize);
            SetSql();
            try {
                using (var queryResult = DbCon.QueryMultiple(SqlProvider.SqlString, SqlProvider.Params, DbTransaction))
                {
                    var pageTotal = queryResult.ReadFirst<int>();

                    var itemList = queryResult.Read<T>().ToList();

                    return new PageList<T>(pageIndex, pageSize, pageTotal, itemList);
                }
            }
            catch (Exception e)
            {
                CallEvent(SqlProvider.SqlString, SqlProvider.Params, e.Message);
                if (Throws)
                    throw new Exception($"{e.Message} sql:{SqlProvider.SqlString} params:{SqlProvider.Params}", e);
                else
                    return new PageList<T>(0, 0, 0, new List<T>());
            }
        }

        public List<T> UpdateSelect(Expression<Func<T, T>> updator)
        {
            SqlProvider.FormatUpdateSelect(updator);
            SetSql();
            return Qr(SqlProvider.SqlString, SqlProvider.Params, DbTransaction).ToList();
        }
        private IEnumerable<T> Qr(string sqlString, DynamicParameters param, IDbTransaction dbTransaction)
        {
            try
            {
                return DbCon.Query<T>(sqlString, param, dbTransaction);
            }
            catch (Exception e)
            {
                CallEvent(sqlString, param, e.Message);
                if (Throws)
                    throw new Exception($"{e.Message} sql:{sqlString} params:{param}", e);
                else
                    return new List<T>();
            }
        }
        private T QrFd(string sqlString, DynamicParameters param, IDbTransaction dbTransaction)
        {
            try
            {
                return DbCon.QueryFirstOrDefault<T>(sqlString, param, dbTransaction);
            }
            catch (Exception e)
            {
                CallEvent(sqlString, param, e.Message);
                if (Throws)
                    throw new Exception($"{e.Message} sql:{sqlString} params:{param}", e);
                else
                    return default(T);
            }
        }
        private void CallEvent(string sqlString, DynamicParameters param, string message)
        {
            try
            {
                var args = new DapperExEventArgs(sqlString, param, message);
                ErrorHappened(this, args);
            }
            catch { }
        }
    }
}
