using Dapper;
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
using System.Threading.Tasks;

namespace xLiAd.DapperEx.Repository
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T>
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        protected readonly IDbConnection con;
        /// <summary>
        /// XML方式SQL语句提供器
        /// </summary>
        protected readonly RepoXmlProvider RepoXmlProvider;
        /// <summary>
        /// 错误事件
        /// </summary>
        protected readonly MsSql.Core.Core.DapperExExceptionHandler ExceptionHandler;
        /// <summary>
        /// 是否抛出错误
        /// </summary>
        protected readonly bool Throws;
        /// <summary>
        /// SQL方言
        /// </summary>
        protected abstract ISqlDialect Dialect { get; }
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="repoXmlProvider"></param>
        /// <param name="exceptionHandler">抛错时的委托</param>
        /// <param name="throws">是否抛出错误，强烈建议保持默认值 true 不然报错时会返回正常数据。</param>
        public RepositoryBase(string connectionString, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
        {
            con = new SqlConnection(connectionString);
            RepoXmlProvider = repoXmlProvider;
            ExceptionHandler = exceptionHandler;
            Throws = throws;
        }
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="_con">数据库连接串</param>
        /// <param name="repoXmlProvider"></param>
        /// <param name="exceptionHandler">抛错时的委托</param>
        /// <param name="throws">是否抛出错误，强烈建议保持默认值 true 不然报错时会返回正常数据。</param>
        public RepositoryBase(IDbConnection _con, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
        {
            con = _con;
            RepoXmlProvider = repoXmlProvider;
            ExceptionHandler = exceptionHandler;
            Throws = throws;
        }
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="_con"></param>
        /// <param name="repoXmlProvider"></param>
        /// <param name="exceptionHandler"></param>
        /// <param name="throws"></param>
        /// <param name="_tran"></param>
        protected RepositoryBase(IDbConnection _con, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true, IDbTransaction _tran = null)
            : this(_con, repoXmlProvider, exceptionHandler, throws)
        {
            DbTransaction = _tran;
        }
        private ISql Sql { get; set; }
        private void DoSetSql()
        {
            if (this.Sql?.SqlString != null)
                this.SqlString = this.Sql?.SqlString;
            if (this.Sql?.Params != null)
                this.Params = this.Sql?.Params;
        }
        private void DoSetSql(ISql sql)
        {
            if (sql.SqlString != null)
                this.SqlString = sql.SqlString;
            if (sql.Params != null)
                this.Params = sql.Params;
        }
        /// <summary>
        /// 刚刚执行过的SQL语句（注：由于单例模式时会发生线程问题，本属性只作为调试用，不应该在程序里引用。）
        /// </summary>
        public string SqlString { get; private set; }
        /// <summary>
        /// 刚刚执行过的语句使用的参数（注：由于单例模式时会发生线程问题，本属性只作为调试用，不应该在程序里引用。）
        /// </summary>
        public DynamicParameters Params { get; private set; }
        /// <summary>
        /// 刚刚执行过的语句使用的参数的字符串形式（注：由于单例模式时会发生线程问题，本属性只作为调试用，不应该在程序里引用。）
        /// </summary>
        public string ParamsString => Params?.FormatString() ?? string.Empty;

        /// <summary>
        /// QuerySet 实例
        /// </summary>
        protected virtual QuerySet<T> QuerySet
        {
            get
            {
                var qs = new QuerySet<T>(con, new SqlProvider<T>(Dialect), DbTransaction, Throws);
                qs.ErrorHappened += ExceptionHandler;
                this.Sql = qs;
                return qs;
            }
        }
        /// <summary>
        /// CommandSet 实例
        /// </summary>
        protected virtual CommandSet<T> CommandSet
        {
            get
            {
                var cs = new CommandSet<T>(con, new SqlProvider<T>(Dialect), DbTransaction, Throws);
                cs.ErrorHappened += ExceptionHandler;
                this.Sql = cs;
                return cs;
            }
        }
        /// <summary>
        /// 事务对象
        /// </summary>
        protected virtual IDbTransaction DbTransaction { get; }
        /// <summary>
        /// 使用这个 CommandSet 方法的方法，不和 RepositoryTrans 使用同一事务对象
        /// </summary>
        /// <param name="tc"></param>
        /// <returns></returns>
        private CommandSet<T> GetCommandSet(TransContext tc)
        {
            var cs = tc.CommandSet<T>(new SqlProvider<T>(Dialect), Throws);
            this.Sql = cs;
            return cs;
        }
        /// <summary>
        /// 释放连接（如果连接还有其他用途，请不要释放）
        /// </summary>
        public void Dispose() { if (con != null) con.Dispose(); }


        #region All
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> AllAsync()
        {
            var rst = await QuerySet.ToListAsync();
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public virtual List<T> All()
        {
            var rst = QuerySet.ToList();
            DoSetSql();
            return rst;
        }
        #endregion
        #region Where
        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public async Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate)
        {
            var rst = await QuerySet.Where(predicate).ToListAsync();
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public List<T> Where(Expression<Func<T, bool>> predicate)
        {
            var rst = QuerySet.Where(predicate).ToList();
            DoSetSql();
            return rst;
        }
        #endregion
        #region Where
        /// <summary>
        /// 只获取指定字段
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="efdbd"></param>
        /// <returns></returns>
        public async Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] efdbd)
        {
            var rst = await QuerySet.Where(predicate).ToListAsync(efdbd);
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 只获取指定字段
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="efdbd"></param>
        /// <returns></returns>
        public List<T> Where(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] efdbd)
        {
            var rst = QuerySet.Where(predicate).ToList(efdbd);
            DoSetSql();
            return rst;
        }
        #endregion
        #region WhereSelect
        /// <summary>
        /// 根据条件获取数据并投影。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="selector">投影表达式</param>
        /// <returns></returns>
        public async Task<List<TResult>> WhereSelectAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            var qs = QuerySet.Where(predicate).Select(selector);
            var rst = await qs.ToListAsync();
            DoSetSql(qs);
            return rst;
        }
        /// <summary>
        /// 根据条件获取数据并投影。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="selector">投影表达式</param>
        /// <returns></returns>
        public List<TResult> WhereSelect<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            var qs = QuerySet.Where(predicate).Select(selector);
            var rst = qs.ToList();
            DoSetSql(qs);
            return rst;
        }
        #endregion
        #region WhereOrder
        /// <summary>
        /// 根据条件排序查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="order">排序字段</param>
        /// <param name="top">取前 top 条，为0时取全部，默认为0.</param>
        /// <param name="desc">是否倒序</param>
        /// <returns></returns>
        public async Task<List<T>> WhereOrderAsync<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, int top = 0, bool desc = false)
        {
            var q = QuerySet.Where(predicate);
            Order<T> o;
            if (desc)
                o = q.OrderByDescing(order);
            else
                o = q.OrderBy(order);
            List<T> rst;
            if (top > 0)
            {
                rst = await o.Top(top).ToListAsync();
            }
            else
            {
                rst = await o.ToListAsync();
            }
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据条件排序查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="order">排序字段</param>
        /// <param name="top">取前 top 条，为0时取全部，默认为0.</param>
        /// <param name="desc">是否倒序</param>
        /// <returns></returns>
        public List<T> WhereOrder<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, int top = 0, bool desc = false)
        {
            var q = QuerySet.Where(predicate);
            Order<T> o;
            if (desc)
                o = q.OrderByDescing(order);
            else
                o = q.OrderBy(order);
            List<T> rst;
            if (top > 0)
            {
                rst = o.Top(top).ToList();
            }
            else
            {
                rst = o.ToList();
            }
            DoSetSql();
            return rst;
        }
        #endregion
        #region WhereOrderSelect
        /// <summary>
        /// 根据条件排序查询 并投影
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="order">排序字段</param>
        /// <param name="selector">投影表达式</param>
        /// <param name="top">取前 top 条，为0时取全部，默认为0.</param>
        /// <returns></returns>
        public async Task<List<TResult>> WhereOrderSelectAsync<TKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, Expression<Func<T, TResult>> selector, int top = 0)
        {
            var q = QuerySet.Where(predicate).OrderBy(order);
            Query<TResult> qst;
            if (top > 0)
                qst = q.Top(top).Select(selector);
            else
                qst = q.Select(selector);
            var rst = await qst.ToListAsync();
            DoSetSql(qst);
            return rst;
        }
        /// <summary>
        /// 根据条件排序查询 并投影
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="order">排序字段</param>
        /// <param name="selector">投影表达式</param>
        /// <param name="top">取前 top 条，为0时取全部，默认为0.</param>
        /// <returns></returns>
        public List<TResult> WhereOrderSelect<TKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, Expression<Func<T, TResult>> selector, int top = 0)
        {
            var q = QuerySet.Where(predicate).OrderBy(order);
            Query<TResult> qst;
            if (top > 0)
                qst = q.Top(top).Select(selector);
            else
                qst = q.Select(selector);
            var rst = qst.ToList();
            DoSetSql(qst);
            return rst;
        }
        #endregion
        #region Add
        /// <summary>
        /// 单条数据插入  请在标识属性上加 Identity 特性
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual async Task<int> AddAsync(T obj)
        {
            var rst = await CommandSet.InsertAsync(obj);
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 单条数据插入  请在标识属性上加 Identity 特性
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int Add(T obj)
        {
            var rst = CommandSet.Insert(obj);
            DoSetSql();
            return rst;
        }
        #endregion
        #region Add
        /// <summary>
        /// 多条数据插入  请在标识属性上加 Identity 特性
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public virtual async Task<int> AddAsync(IEnumerable<T> objs)
        {
            var rst = await CommandSet.InsertAsync(objs);
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 多条数据插入  请在标识属性上加 Identity 特性
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public virtual int Add(IEnumerable<T> objs)
        {
            var rst = CommandSet.Insert(objs);
            DoSetSql();
            return rst;
        }
        #endregion

        /// <summary>
        /// 多条数据插入(事务操作)  请在标识属性上加 Identity 特性
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public virtual int AddTrans(IEnumerable<T> objs)
        {
            int c = 0;
            con.Transaction(tc =>
            {
                c = GetCommandSet(tc).Insert(objs);
            });
            DoSetSql();
            return c;
        }
        #region PageList
        /// <summary>
        /// 根据条件排序分页
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="filter">条件表达式</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页条数</param>
        /// <param name="desc">是否倒序，默认否</param>
        /// <returns></returns>
        public async Task<PageList<T>> PageListAsync<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> orderBy, int pageindex = 1, int pagesize = 50, bool desc = false)
        {
            var q = QuerySet.Where(filter);
            Order<T> qq;
            if (desc)
                qq = q.OrderByDescing(orderBy);
            else
                qq = q.OrderBy(orderBy);
            var rst = await qq.PageListAsync(pageindex, pagesize);
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据条件排序分页
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="filter">条件表达式</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页条数</param>
        /// <param name="desc">是否倒序，默认否</param>
        /// <returns></returns>
        public PageList<T> PageList<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> orderBy, int pageindex = 1, int pagesize = 50, bool desc = false)
        {
            var q = QuerySet.Where(filter);
            Order<T> qq;
            if (desc)
                qq = q.OrderByDescing(orderBy);
            else
                qq = q.OrderBy(orderBy);
            var rst = qq.PageList(pageindex, pagesize);
            DoSetSql();
            return rst;
        }
        #endregion
        #region PageList
        /// <summary>
        /// 根据条件分页（多排序条件）
        /// </summary>
        /// <param name="filter">条件表达式</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页条数</param>
        /// <param name="orders">排序字段</param>
        /// <returns></returns>
        public async Task<PageList<T>> PageListAsync(Expression<Func<T, bool>> filter, int pageindex = 1, int pagesize = 50, params Tuple<Expression<Func<T, object>>, SortOrder>[] orders)
        {
            var q = QuerySet.Where(filter);
            Order<T> qq = q;
            foreach (var order in orders)
            {
                if (order.Item2 == SortOrder.Descending)
                    qq = qq.OrderByDescing(order.Item1);
                else
                    qq = qq.OrderBy(order.Item1);
            }
            var rst = await qq.PageListAsync(pageindex, pagesize);
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据条件分页（多排序条件）
        /// </summary>
        /// <param name="filter">条件表达式</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页条数</param>
        /// <param name="orders">排序字段</param>
        /// <returns></returns>
        public PageList<T> PageList(Expression<Func<T, bool>> filter, int pageindex = 1, int pagesize = 50, params Tuple<Expression<Func<T, object>>, SortOrder>[] orders)
        {
            var q = QuerySet.Where(filter);
            Order<T> qq = q;
            foreach (var order in orders)
            {
                if (order.Item2 == SortOrder.Descending)
                    qq = qq.OrderByDescing(order.Item1);
                else
                    qq = qq.OrderBy(order.Item1);
            }
            var rst = qq.PageList(pageindex, pagesize);
            DoSetSql();
            return rst;
        }
        #endregion
        #region PageList
        /// <summary>
        /// 根据条件分页（两个排序条件）
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="filter">条件表达式</param>
        /// <param name="order1">排序1</param>
        /// <param name="order1Desc">是否倒序1</param>
        /// <param name="order2">排序2</param>
        /// <param name="order2Desc">是否倒序2</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页条数</param>
        /// <returns></returns>
        public async Task<PageList<T>> PageListAsync<TKey1, TKey2>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey1>> order1, bool order1Desc, Expression<Func<T, TKey2>> order2, bool order2Desc, int pageindex = 1, int pagesize = 50)
        {
            var q = QuerySet.Where(filter);
            Order<T> qq = q;
            if (order1Desc)
                qq = qq.OrderByDescing(order1);
            else
                qq = qq.OrderBy(order1);
            if (order2Desc)
                qq = qq.OrderByDescing(order2);
            else
                qq = qq.OrderBy(order2);
            var rst = await qq.PageListAsync(pageindex, pagesize);
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据条件分页（两个排序条件）
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="filter">条件表达式</param>
        /// <param name="order1">排序1</param>
        /// <param name="order1Desc">是否倒序1</param>
        /// <param name="order2">排序2</param>
        /// <param name="order2Desc">是否倒序2</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页条数</param>
        /// <returns></returns>
        public PageList<T> PageList<TKey1, TKey2>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey1>> order1, bool order1Desc, Expression<Func<T, TKey2>> order2, bool order2Desc, int pageindex = 1, int pagesize = 50)
        {
            var q = QuerySet.Where(filter);
            Order<T> qq = q;
            if (order1Desc)
                qq = qq.OrderByDescing(order1);
            else
                qq = qq.OrderBy(order1);
            if (order2Desc)
                qq = qq.OrderByDescing(order2);
            else
                qq = qq.OrderBy(order2);
            var rst = qq.PageList(pageindex, pagesize);
            DoSetSql();
            return rst;
        }
        #endregion
        #region PageListSelect
        /// <summary>
        /// 根据条件排序分页 并投影
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="filter">条件表达式</param>
        /// <param name="selector">投影表达式</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页条数</param>
        /// <param name="orders">排序字段</param>
        /// <returns></returns>
        public async Task<PageList<TResult>> PageListSelectAsync<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector, int pageindex = 1, int pagesize = 50, params Tuple<Expression<Func<T, object>>, SortOrder>[] orders)
        {
            var q = QuerySet.Where(filter);
            Order<T> qq = q;
            foreach (var order in orders)
            {
                if (order.Item2 == SortOrder.Descending)
                    qq = qq.OrderByDescing(order.Item1);
                else
                    qq = qq.OrderBy(order.Item1);
            }
            var rst = await qq.Select(selector).PageListAsync(pageindex, pagesize);
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据条件排序分页 并投影
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="filter">条件表达式</param>
        /// <param name="selector">投影表达式</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页条数</param>
        /// <param name="orders">排序字段</param>
        /// <returns></returns>
        public PageList<TResult> PageListSelect<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector, int pageindex = 1, int pagesize = 50, params Tuple<Expression<Func<T, object>>, SortOrder>[] orders)
        {
            var q = QuerySet.Where(filter);
            Order<T> qq = q;
            foreach (var order in orders)
            {
                if (order.Item2 == SortOrder.Descending)
                    qq = qq.OrderByDescing(order.Item1);
                else
                    qq = qq.OrderBy(order.Item1);
            }
            var rst = qq.Select(selector).PageList(pageindex, pagesize);
            DoSetSql();
            return rst;
        }
        #endregion
        #region Find
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            var rst = await QuerySet.Where(predicate).GetAsync();
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public T Find(Expression<Func<T, bool>> predicate)
        {
            var rst = QuerySet.Where(predicate).Get();
            DoSetSql();
            return rst;
        }
        #endregion
        #region Find
        /// <summary>
        /// 根据主键获取一条数据（实体类需要设置主键 在主键属性上加 Key特性）
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> FindAsync<TKey>(TKey id)
        {
            var rst = await QuerySet.GetAsync(id);
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据主键获取一条数据（实体类需要设置主键 在主键属性上加 Key特性）
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find<TKey>(TKey id)
        {
            var rst = QuerySet.Get(id);
            DoSetSql();
            return rst;
        }
        #endregion
        #region FindField
        /// <summary>
        /// 根据条件获取一条数据并投影
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="keySelector">投影表达式</param>
        /// <returns></returns>
        public virtual async Task<TResult> FindFieldAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> keySelector)
        {
            var rst = await QuerySet.Where(predicate).Select(keySelector).GetAsync();
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据条件获取一条数据并投影
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="keySelector">投影表达式</param>
        /// <returns></returns>
        public virtual TResult FindField<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> keySelector)
        {
            var rst = QuerySet.Where(predicate).Select(keySelector).Get();
            DoSetSql();
            return rst;
        }
        #endregion
        #region Count
        /// <summary>
        /// 根据条件取得数据数量
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            var rst = await QuerySet.Where(predicate).CountAsync();
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据条件取得数据数量
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> predicate)
        {
            var rst = QuerySet.Where(predicate).Count();
            DoSetSql();
            return rst;
        }
        #endregion
        #region Exist
        /// <summary>
        /// 是否存在符合条件的记录
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<bool> ExistAsync(Expression<Func<T, bool>> predicate)
        {
            return await CountAsync(predicate) > 0;
        }
        /// <summary>
        /// 是否存在符合条件的记录
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Exist(Expression<Func<T, bool>> predicate)
        {
            return Count(predicate) > 0;
        }
        #endregion
        private int? countAll = null;
        /// <summary>
        /// 取得数据总数量
        /// </summary>
        public int CountAll
        {
            get
            {
                if (countAll == null)
                    countAll = Count(false);
                return countAll.Value;
            }
        }
        #region Count
        /// <summary>
        /// 取得数据总数量
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync(bool setSql = true)
        {
            var rst = await QuerySet.CountAsync();
            if (setSql)
                DoSetSql();
            return rst;
        }
        /// <summary>
        /// 取得数据总数量
        /// </summary>
        /// <returns></returns>
        public int Count(bool setSql = true)
        {
            var rst = QuerySet.Count();
            if (setSql)
                DoSetSql();
            return rst;
        }
        #endregion
        #region Delete
        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var rst = await CommandSet.Where(predicate).DeleteAsync();
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public virtual int Delete(Expression<Func<T, bool>> predicate)
        {
            var rst = CommandSet.Where(predicate).Delete();
            DoSetSql();
            return rst;
        }
        #endregion
        #region Delete
        /// <summary>
        /// 根据主键删除数据（实体类需要设置主键 在主键属性上加 Key特性）
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync<TKey>(TKey id)
        {
            var rst = await CommandSet.DeleteAsync(id);
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据主键删除数据（实体类需要设置主键 在主键属性上加 Key特性）
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns></returns>
        public int Delete<TKey>(TKey id)
        {
            var rst = CommandSet.Delete(id);
            DoSetSql();
            return rst;
        }
        #endregion
        /// <summary>
        /// 根据主键删除数据(事务操作)（实体类需要设置主键 在主键属性上加 Key特性）
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="idList"></param>
        /// <returns></returns>
        public int DeleteTrans<TKey>(IEnumerable<TKey> idList)
        {
            int c = 0;
            con.Transaction(tc =>
            {
                foreach (var i in idList)
                    c += GetCommandSet(tc).Delete(i);
            });
            DoSetSql();
            return c;
        }
        #region Update
        /// <summary>
        /// 根据主键更新一条数据的 除主键外的全部属性字段
        /// </summary>
        /// <param name="TObject">实体</param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(T TObject)
        {
            var rst = await CommandSet.UpdateAsync(TObject);
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据主键更新一条数据的 除主键外的全部属性字段
        /// </summary>
        /// <param name="TObject">实体</param>
        /// <returns></returns>
        public virtual int Update(T TObject)
        {
            var rst = CommandSet.Update(TObject);
            DoSetSql();
            return rst;
        }
        #endregion
        #region UpdateNotDefault
        /// <summary>
        /// 根据主键更新一条数据的 除主键外的值不为默认的字段
        /// </summary>
        /// <param name="TObject"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateNotDefaultAsync(T TObject)
        {
            var rst = await CommandSet.UpdateNotDefaultAsync(TObject);
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据主键更新一条数据的 除主键外的值不为默认的字段
        /// </summary>
        /// <param name="TObject"></param>
        /// <returns></returns>
        public virtual int UpdateNotDefault(T TObject)
        {
            var rst = CommandSet.UpdateNotDefault(TObject);
            DoSetSql();
            return rst;
        }
        #endregion
        /// <summary>
        /// 根据主键更新一些数据的 除主键外的全部属性字段（事务操作）
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public virtual int UpdateTrans(IEnumerable<T> entityList)
        {
            int c = 0;
            con.Transaction(tc =>
            {
                foreach (var m in entityList)
                    c += GetCommandSet(tc).Update(m);
            });
            DoSetSql();
            return c;
        }
        #region Update
        /// <summary>
        /// 根据主键更新一条数据的指定字段
        /// </summary>
        /// <param name="d">实体</param>
        /// <param name="efdbd">指定字段</param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(T d, params Expression<Func<T, object>>[] efdbd)
        {
            var rst = await CommandSet.UpdateAsync(d, efdbd);
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据主键更新一条数据的指定字段
        /// </summary>
        /// <param name="d">实体</param>
        /// <param name="efdbd">指定字段</param>
        /// <returns></returns>
        public virtual int Update(T d, params Expression<Func<T, object>>[] efdbd)
        {
            var rst = CommandSet.Update(d, efdbd);
            DoSetSql();
            return rst;
        }
        #endregion
        #region UpdateWhere
        /// <summary>
        /// 根据条件更新某个字段
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="key">要更新的字段</param>
        /// <param name="value">字段要更新的值</param>
        /// <returns></returns>
        public async Task<int> UpdateWhereAsync<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> key, TKey value)
        {
            var rst = await CommandSet.Where(predicate).UpdateAsync(key, value);
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据条件更新某个字段
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="key">要更新的字段</param>
        /// <param name="value">字段要更新的值</param>
        /// <returns></returns>
        public int UpdateWhere<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> key, TKey value)
        {
            var rst = CommandSet.Where(predicate).Update(key, value);
            DoSetSql();
            return rst;
        }
        #endregion
        #region UpdateWhere
        /// <summary>
        /// 根据条件更新若干字段
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="d">存储字段值的实体（主键不作为更新条件）</param>
        /// <param name="efdbd">要更新的字段</param>
        /// <returns></returns>
        public async Task<int> UpdateWhereAsync(Expression<Func<T, bool>> predicate, T d, params Expression<Func<T, object>>[] efdbd)
        {
            var rst = await CommandSet.Where(predicate).UpdateAsync(d, efdbd);
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据条件更新若干字段
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="d">存储字段值的实体（主键不作为更新条件）</param>
        /// <param name="efdbd">要更新的字段</param>
        /// <returns></returns>
        public int UpdateWhere(Expression<Func<T, bool>> predicate, T d, params Expression<Func<T, object>>[] efdbd)
        {
            var rst = CommandSet.Where(predicate).Update(d, efdbd);
            DoSetSql();
            return rst;
        }
        #endregion
        /// <summary>
        /// 把参数字典转换为动态参数， 并替换语句中的转义参数名（如果有的话）
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="sqlToReplace"></param>
        /// <param name="sqlReplaced"></param>
        /// <returns></returns>
        private DynamicParameters ConvertDicToParam(Dictionary<string, string> dic, string sqlToReplace, out string sqlReplaced)
        {
            sqlReplaced = sqlToReplace;
            DynamicParameters param = null;
            if (dic != null && dic.Count > 0)
            {
                param = new DynamicParameters();
                foreach (var d in dic)
                {
                    param.Add($"@{d.Key}", d.Value);
                    if (!string.IsNullOrWhiteSpace(sqlToReplace))
                        sqlReplaced = sqlReplaced.Replace($"#{{{d.Key}}}", $"@{d.Key}");
                }
            }
            return param;
        }
        #region ExecuteSql
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="cmdType">命令类别</param>
        /// <returns></returns>
        public virtual async Task<bool> ExecuteSqlAsync(string sql, object param = null, CommandType cmdType = CommandType.Text)
        {
            if(param is Dictionary<string, string> dic)
                param = ConvertDicToParam(dic, null, out string _);
            return await con.ExecuteAsync(sql, param, commandType: cmdType, transaction: DbTransaction) > 0;
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="cmdType">命令类别</param>
        /// <returns></returns>
        public virtual bool ExecuteSql(string sql, object param = null, CommandType cmdType = CommandType.Text)
        {
            if (param is Dictionary<string, string> dic)
                param = ConvertDicToParam(dic, null, out string _);
            return con.Execute(sql, param, commandType: cmdType, transaction: DbTransaction) > 0;
        }
        #endregion
        #region ExecuteProcedure
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procedureName">存储过程</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public virtual async Task<bool> ExecuteProcedureAsync(string procedureName, object param = null)
        {
            return await ExecuteSqlAsync(procedureName, param, CommandType.StoredProcedure);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procedureName">存储过程</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public virtual bool ExecuteProcedure(string procedureName, object param = null)
        {
            return ExecuteSql(procedureName, param, CommandType.StoredProcedure);
        }
        #endregion
        #region GetScalar
        /// <summary>
        /// 执行查询 返回第一条结果
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public virtual async Task<TResult> GetScalarAsync<TResult>(string sql, Dictionary<string, string> dic = null)
        {
            DynamicParameters param = ConvertDicToParam(dic, null, out string _);
            return await con.ExecuteScalarAsync<TResult>(sql, param, transaction: DbTransaction);
        }
        /// <summary>
        /// 执行查询 返回第一条结果
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public virtual TResult GetScalar<TResult>(string sql, Dictionary<string, string> dic = null)
        {
            DynamicParameters param = ConvertDicToParam(dic, null, out string _);
            return con.ExecuteScalar<TResult>(sql, param, transaction: DbTransaction);
        }
        #endregion
        #region QueryBySql
        /// <summary>
        /// 根据SQL语句，或存储过程 查询实体
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param">参数</param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TResult>> QueryBySqlAsync<TResult>(string sql, object param = null, CommandType cmdType = CommandType.Text)
        {
            if(param is Dictionary<string, string> paramDic)
            {
                param = ConvertDicToParam(paramDic, null, out string _);
            }
            return await con.QueryAsync<TResult>(sql, param, commandType: cmdType, transaction: DbTransaction);
        }
        /// <summary>
        /// 根据SQL语句，或存储过程 查询实体
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public virtual IEnumerable<TResult> QueryBySql<TResult>(string sql, object param = null, CommandType cmdType = CommandType.Text)
        {
            if (param is Dictionary<string, string> paramDic)
            {
                param = ConvertDicToParam(paramDic, null, out string _);
            }
            return con.Query<TResult>(sql, param, commandType: cmdType, transaction: DbTransaction);
        }
        #endregion
        /// <summary>
        /// 判断XML文件载入的状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private XmlSqlModel CheckXml(string id, out string msg)
        {
            if (RepoXmlProvider == null)
            {
                msg = "没有配置 XML 文件！";
                return null;
            }
            if (RepoXmlProvider.Statu != RepoXmlStatu.Loaded)
            {
                msg = "XML 文件不在正确状态！";
                return null;
            }
            var rpx = RepoXmlProvider.DataList.Where(x => x.Id == id).FirstOrDefault();
            if (rpx == null)
            {
                msg = "未找到对应 SQL 语句！";
                return null;
            }
            msg = null;
            return rpx;
        }
        #region ExecuteXml
        /// <summary>
        /// 从XML中获取SQL执行
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteXmlAsync(string id, Dictionary<string, string> dic = null)
        {
            XmlSqlModel xsm = CheckXml(id, out string msg);
            if (xsm == null)
                throw new Exception(msg);
            DynamicParameters param = ConvertDicToParam(dic, xsm.Sql, out string sql);
            return await ExecuteSqlAsync(sql, dic);
        }
        /// <summary>
        /// 从XML中获取SQL执行
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public bool ExecuteXml(string id, Dictionary<string, string> dic = null)
        {
            XmlSqlModel xsm = CheckXml(id, out string msg);
            if (xsm == null)
                throw new Exception(msg);
            DynamicParameters param = ConvertDicToParam(dic, xsm.Sql, out string sql);
            return ExecuteSql(sql, dic);
        }
        #endregion
        #region QueryXml
        /// <summary>
        /// 从XML中获取SQL查询
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="id"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TResult>> QueryXmlAsync<TResult>(string id, Dictionary<string, string> dic = null)
        {
            XmlSqlModel xsm = CheckXml(id, out string msg);
            if (xsm == null)
                throw new Exception(msg);
            DynamicParameters param = ConvertDicToParam(dic, xsm.Sql, out string sql);
            return await QueryBySqlAsync<TResult>(sql, dic);
        }
        /// <summary>
        /// 从XML中获取SQL查询
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="id"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public IEnumerable<TResult> QueryXml<TResult>(string id, Dictionary<string, string> dic = null)
        {
            XmlSqlModel xsm = CheckXml(id, out string msg);
            if (xsm == null)
                throw new Exception(msg);
            DynamicParameters param = ConvertDicToParam(dic, xsm.Sql, out string sql);
            return QueryBySql<TResult>(sql, dic);
        }
        #endregion
        /// <summary>
        /// 获取事务提供
        /// </summary>
        /// <returns></returns>
        public abstract TransactionProviderBase GetTransaction();
    }
}
