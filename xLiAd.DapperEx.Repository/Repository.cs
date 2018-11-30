using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using xLiAd.DapperEx.MsSql.Core;
using xLiAd.DapperEx.MsSql.Core.Core.SetQ;
using xLiAd.DapperEx.MsSql.Core.Model;

namespace xLiAd.DapperEx.Repository
{
    public class Repository<T>
    {
        protected SqlConnection con;
        public Repository(string connectionString)
        {
            con = new SqlConnection(connectionString);
        }
        public Repository(SqlConnection _con)
        {
            con = _con;
        }
        public void Dispose() { if (con != null) con.Dispose(); }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public virtual List<T> All() { return con.QuerySet<T>().ToList(); }
        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public List<T> Where(Expression<Func<T, bool>> predicate) { return con.QuerySet<T>().Where(predicate).ToList(); }
        /// <summary>
        /// 根据条件获取数据并投影。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="selector">投影表达式</param>
        /// <returns></returns>
        public List<TResult> WhereSelect<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T,TResult>> selector)
        {
            return con.QuerySet<T>().Where(predicate).Select(selector).ToList();
        }
        /// <summary>
        /// 根据条件排序查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="order">排序字段</param>
        /// <param name="top">取前 top 条，为0时取全部，默认为0.</param>
        /// <returns></returns>
        public List<T> WhereOrder<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T,TKey>> order, int top =0)
        {
            var q = con.QuerySet<T>().Where(predicate).OrderBy(order);
            if (top > 0)
                return q.Top(top).ToList();
            return q.ToList();
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
            var q = con.QuerySet<T>().Where(predicate).OrderBy(order);
            if(top > 0)
                return q.Top(top).Select(selector).ToList();
            return q.Select(selector).ToList();
        }
        /// <summary>
        /// 单条数据插入  请在标识属性上加 Identity 特性
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int Add(T obj)
        {
            var r = con.CommandSet<T>().Insert(obj);
            return r;
        }
        /// <summary>
        /// 多条数据插入  请在标识属性上加 Identity 特性
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public virtual int Add(IEnumerable<T> objs)
        {
            var r = con.CommandSet<T>().Insert(objs);
            return r;
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
            var q = con.QuerySet<T>().Where(filter);
            Order<T> qq;
            if (desc)
                qq = q.OrderByDescing(orderBy);
            else
                qq = q.OrderBy(orderBy);
            var rst = qq.PageList(pageindex, pagesize);
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
            var q = con.QuerySet<T>().Where(filter);
            Order<T> qq = q;
            foreach(var order in orders)
            {
                if (order.Item2 == SortOrder.Descending)
                    qq = qq.OrderByDescing(order.Item1);
                else
                    qq = qq.OrderBy(order.Item1);
            }
            var rst = qq.PageList(pageindex, pagesize);
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
            var q = con.QuerySet<T>().Where(filter);
            Order<T> qq = q;
            foreach (var order in orders)
            {
                if (order.Item2 == SortOrder.Descending)
                    qq = qq.OrderByDescing(order.Item1);
                else
                    qq = qq.OrderBy(order.Item1);
            }
            var rst = qq.Select(selector).PageList(pageindex, pagesize);
            return rst;
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public T Find(Expression<Func<T, bool>> predicate)
        {
            return con.QuerySet<T>().Where(predicate).Get();
        }
        /// <summary>
        /// 根据主键获取一条数据（实体类需要设置主键 在主键属性上加 Key特性）
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find<TKey>(TKey id)
        {
            return con.QuerySet<T>().Get(id);
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
            return con.QuerySet<T>().Where(predicate).Select(keySelector).Get();
        }
        /// <summary>
        /// 根据条件取得数据数量
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> predicate) { return con.QuerySet<T>().Where(predicate).Count(); }
        /// <summary>
        /// 取得数据总数量
        /// </summary>
        public int CountAll { get { return con.QuerySet<T>().Count(); } }
        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public virtual int Delete(Expression<Func<T, bool>> predicate)
        {
            return con.CommandSet<T>().Where(predicate).Delete();
        }
        /// <summary>
        /// 根据主键删除数据（实体类需要设置主键 在主键属性上加 Key特性）
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns></returns>
        public int Delete<TKey>(TKey id)
        {
            return con.CommandSet<T>().Delete(id);
        }
        /// <summary>
        /// 根据主键更新一条数据的 除主键外的全部属性字段
        /// </summary>
        /// <param name="TObject">实体</param>
        /// <returns></returns>
        public virtual int Update(T TObject)
        {
            return con.CommandSet<T>().Update(TObject);
        }
        /// <summary>
        /// 根据主键更新一条数据的指定字段
        /// </summary>
        /// <param name="d">实体</param>
        /// <param name="efdbd">指定字段</param>
        /// <returns></returns>
        public virtual int Update(T d, params Expression<Func<T, object>>[] efdbd)
        {
            return con.CommandSet<T>().Update(d, efdbd);
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
            return con.CommandSet<T>().Where(predicate).Update(key, value);
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
            return con.CommandSet<T>().Where(predicate).Update(d, efdbd);
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
