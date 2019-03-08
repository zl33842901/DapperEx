using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using xLiAd.DapperEx.MsSql.Core.Core.Expression;
using xLiAd.DapperEx.MsSql.Core.Core.Interfaces;
using xLiAd.DapperEx.MsSql.Core.Helper;
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
        protected void SetSql()
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
            SqlProvider.FormatToList(this.FieldAnyExpression);
            SetSql();
            return Qr(SqlProvider.SqlString, SqlProvider.Params, DbTransaction).ToList();
        }
        public virtual List<T> ToList(LambdaExpression[] selector)
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
                var ps = typeof(T).GetJsonColumnProperty();
                if (ps.Length > 0 && HasSerializer)
                {
                    var Reader = DbCon.ExecuteReader(SqlProvider.SqlString, SqlProvider.Params, DbTransaction);
                    var pageTotal = 0;
                    if (Reader.Read())
                    {
                        pageTotal = Reader.GetInt32(0);
                    }
                    Reader.NextResult();
                    var Parser = Reader.GetRowParser(typeof(T));
                    List<T> lrst = new List<T>();
                    while (Reader.Read())
                    {
                        object rst = Parser(Reader);
                        foreach (var p in ps)
                        {
                            var col = Reader.GetOrdinal($"{p.Name}{ResolveExpression.JsonColumnNameSuffix}");
                            var s = Reader.GetString(col);
                            var pv = Deserializer(s, p.PropertyType);
                            p.SetValue(rst, pv);
                        }
                        lrst.Add((T)rst);
                    }
                    Reader.Close();
                    return new PageList<T>(pageIndex, pageSize, pageTotal, lrst);
                }
                else
                {
                    using (var queryResult = DbCon.QueryMultiple(SqlProvider.SqlString, SqlProvider.Params, DbTransaction))
                    {
                        var pageTotal = queryResult.ReadFirst<int>();

                        var itemList = queryResult.Read<T>().ToList();

                        return new PageList<T>(pageIndex, pageSize, pageTotal, itemList);
                    }
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
        protected IEnumerable<TRst> Q<TRst>(string sqlString, DynamicParameters param, IDbTransaction dbTransaction, int count = 0)
        {
            var ps = typeof(TRst).GetJsonColumnProperty();
            if (ps.Length > 0 && HasSerializer)
            {
                var Reader = DbCon.ExecuteReader(sqlString, param, dbTransaction);
                var Parser = Reader.GetRowParser(typeof(TRst));
                List<TRst> lrst = new List<TRst>();
                int i = 0;
                while (Reader.Read())
                {
                    object rst = Parser(Reader);
                    foreach (var p in ps)
                    {
                        int col;
                        try
                        {
                            col = Reader.GetOrdinal($"{p.Name}{ResolveExpression.JsonColumnNameSuffix}");
                        }
                        catch
                        {
                            continue;
                        }
                        object o = Reader.GetValue(col);
                        if (o == DBNull.Value)
                            continue;
                        var s = Reader.GetString(col);
                        var pv = Deserializer(s, p.PropertyType);
                        p.SetValue(rst, pv);
                    }
                    lrst.Add((TRst)rst);
                    if (++i >= count && count > 0)
                        break;
                }
                Reader.Close();
                return lrst;
            }
            else
                return DbCon.Query<TRst>(sqlString, param, dbTransaction);
        }
        private IEnumerable<T> Qr(string sqlString, DynamicParameters param, IDbTransaction dbTransaction)
        {
            try
            {
                return Q<T>(sqlString, param, dbTransaction);
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
                return Q<T>(sqlString, param, dbTransaction, 1).FirstOrDefault();
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
        protected void CallEvent(string sqlString, DynamicParameters param, string message)
        {
            try
            {
                var args = new DapperExEventArgs(sqlString, param, message);
                ErrorHappened?.Invoke(this, args);
            }
            catch { }
        }
        internal void ResetErrorHandler<Told>(Query<Told> old)
        {
            this.ErrorHappened = old.ErrorHappened;
        }
        /// <summary>
        /// 是否有JSON序列化器
        /// </summary>
        public bool HasSerializer { get; private set; } = false;
        /// <summary>
        /// 序列化器
        /// </summary>
        public Func<object, string> Serializer { get; private set; }
        /// <summary>
        /// 反序列化器
        /// </summary>
        public Func<string, Type, object> Deserializer { get; private set; }
        /// <summary>
        /// 设置序列化和反序列化器
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="deserializer"></param>
        public void SetSerializeFunc(Func<object, string> serializer, Func<string, Type, object> deserializer)
        {
            if (serializer != null && deserializer != null)
            {
                this.Serializer = serializer;
                this.Deserializer = deserializer;
                HasSerializer = true;
            }
        }
        public IFieldAnyExpression FieldAnyExpression { get; set; }
    }
}
