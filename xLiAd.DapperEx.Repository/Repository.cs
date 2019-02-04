﻿using Dapper;
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

namespace xLiAd.DapperEx.Repository
{
    /// <summary>
    /// 仓储类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T>
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        protected SqlConnection con;
        RepoXmlProvider RepoXmlProvider;
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="repoXmlProvider"></param>
        public Repository(string connectionString, RepoXmlProvider repoXmlProvider = null)
        {
            con = new SqlConnection(connectionString);
            RepoXmlProvider = repoXmlProvider;
        }
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="_con"></param>
        /// <param name="repoXmlProvider"></param>
        public Repository(SqlConnection _con, RepoXmlProvider repoXmlProvider = null)
        {
            con = _con;
            RepoXmlProvider = repoXmlProvider;
        }
        private ISql Sql { get; set; }
        private void DoSetSql()
        {
            if (this.Sql?.SqlString != null)
                this.SqlString = this.Sql?.SqlString;
            if (this.Sql?.Params != null)
                this.Params = this.Sql?.Params;
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
        /// QuerySet 实例
        /// </summary>
        protected virtual QuerySet<T> QuerySet
        {
            get
            {
                var qs = con.QuerySet<T>();
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
                var cs = con.CommandSet<T>();
                this.Sql = cs;
                return cs;
            }
        }
        /// <summary>
        /// 事务对象
        /// </summary>
        protected virtual IDbTransaction DbTransaction => null;
        /// <summary>
        /// 使用这个 CommandSet 方法的方法，不和 RepositoryTrans 使用同一事务对象
        /// </summary>
        /// <param name="tc"></param>
        /// <returns></returns>
        private CommandSet<T> GetCommandSet(TransContext tc)
        {
            var cs = tc.CommandSet<T>();
            this.Sql = cs;
            return cs;
        }
        /// <summary>
        /// 释放连接（如果连接还有其他用途，请不要释放）
        /// </summary>
        public void Dispose() { if (con != null) con.Dispose(); }
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
        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public List<T> Where(Expression<Func<T, bool>> predicate) {
            var rst = QuerySet.Where(predicate).ToList();
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 只获取指定字段
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="efdbd"></param>
        /// <returns></returns>
        public List<T> Where(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] efdbd) {
            var rst = QuerySet.Where(predicate).ToList(efdbd);
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据条件获取数据并投影。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="selector">投影表达式</param>
        /// <returns></returns>
        public List<TResult> WhereSelect<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T,TResult>> selector)
        {
            var rst = QuerySet.Where(predicate).Select(selector).ToList();
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
        /// <returns></returns>
        public List<T> WhereOrder<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T,TKey>> order, int top =0, bool desc = false)
        {
            var q = QuerySet.Where(predicate);
            Order<T> o;
            if (desc)
                o = q.OrderByDescing(order);
            else
                o = q.OrderBy(order);
            if (top > 0)
                return o.Top(top).ToList();
            var rst = o.ToList();
            DoSetSql();
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
        public List<TResult> WhereOrderSelect<TKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, Expression<Func<T,TResult>> selector, int top =0)
        {
            var q = QuerySet.Where(predicate).OrderBy(order);
            if(top > 0)
                return q.Top(top).Select(selector).ToList();
            var rst = q.Select(selector).ToList();
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
        /// <summary>
        /// 根据条件分页（多排序条件）
        /// </summary>
        /// <param name="filter">条件表达式</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页条数</param>
        /// <param name="orders">排序字段</param>
        /// <returns></returns>
        public PageList<T> PageList(Expression<Func<T, bool>> filter, int pageindex = 1, int pagesize = 50, params Tuple<Expression<Func<T,object>>, SortOrder>[] orders)
        {
            var q = QuerySet.Where(filter);
            Order<T> qq = q;
            foreach(var order in orders)
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
        public PageList<TResult> PageListSelect<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T,TResult>> selector, int pageindex = 1, int pagesize = 50, params Tuple<Expression<Func<T, object>>, SortOrder>[] orders)
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
        /// <summary>
        /// 根据条件获取一条数据并投影
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="keySelector">投影表达式</param>
        /// <returns></returns>
        public TResult FindField<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> keySelector)
        {
            var rst = QuerySet.Where(predicate).Select(keySelector).Get();
            DoSetSql();
            return rst;
        }
        /// <summary>
        /// 根据条件取得数据数量
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> predicate) {
            var rst = QuerySet.Where(predicate).Count();
            DoSetSql();
            return rst;
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

        private int? countAll = null;
        /// <summary>
        /// 取得数据总数量
        /// </summary>
        public int CountAll {
            get {
                if (countAll == null)
                    countAll = Count();
                return countAll.Value;
            }
        }
        /// <summary>
        /// 取得数据总数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            var rst = QuerySet.Count();
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
                foreach(var i in idList)
                    c += GetCommandSet(tc).Delete(i);
            });
            DoSetSql();
            return c;
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
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="dic">参数</param>
        /// <param name="cmdType">命令类别</param>
        /// <returns></returns>
        public virtual bool ExecuteSql(string sql, Dictionary<string, string> dic = null, CommandType cmdType = CommandType.Text)
        {
            DynamicParameters param = ConvertDicToParam(dic, null, out string _);
            return con.Execute(sql, param, commandType: cmdType, transaction: DbTransaction) > 0;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procedureName">存储过程</param>
        /// <param name="dic">参数</param>
        /// <returns></returns>
        public virtual bool ExecuteProcedure(string procedureName, Dictionary<string, string> dic = null)
        {
            return ExecuteSql(procedureName, dic, CommandType.StoredProcedure);
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
        /// <summary>
        /// 根据SQL语句，或存储过程 查询实体
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dic"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public virtual IEnumerable<TResult> QueryBySql<TResult>(string sql, Dictionary<string, string> dic = null, CommandType cmdType = CommandType.Text)
        {
            DynamicParameters param = ConvertDicToParam(dic, null, out string _);
            return con.Query<TResult>(sql, param, commandType: cmdType, transaction: DbTransaction);
        }
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
            if(RepoXmlProvider.Statu != RepoXmlStatu.Loaded)
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
        /// <summary>
        /// 获取事务提供
        /// </summary>
        /// <returns></returns>
        public TransactionProvider GetTransaction()
        {
            return new TransactionProvider(con);
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
}
