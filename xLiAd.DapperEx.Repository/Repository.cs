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
        private SqlConnection con;
        public Repository(string connectionString)
        {
            con = new SqlConnection(connectionString);
        }
        public Repository(SqlConnection _con)
        {
            con = _con;
        }
        public void Dispose() { if (con != null) con.Dispose(); }
        public virtual List<T> All() { return con.QuerySet<T>().ToList(); }
        public List<T> Where(Expression<Func<T, bool>> predicate) { return con.QuerySet<T>().Where(predicate).ToList(); }
        public List<TResult> WhereSelect<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T,TResult>> selector)
        {
            return con.QuerySet<T>().Where(predicate).Select(selector).ToList();
        }
        public List<T> WhereOrder<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T,TKey>> order, int top =0)
        {
            var q = con.QuerySet<T>().Where(predicate).OrderBy(order);
            if (top > 0)
                return q.Top(top).ToList();
            return q.ToList();
        }
        public List<TResult> WhereOrderSelect<TKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, Expression<Func<T,TResult>> selector, int top =0)
        {
            var q = con.QuerySet<T>().Where(predicate).OrderBy(order);
            if(top > 0)
                return q.Top(top).Select(selector).ToList();
            return q.Select(selector).ToList();
        }
        public virtual int Add(T obj)
        {
            var r = con.CommandSet<T>().Insert(obj);
            return r;
        }
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
        public T Find(Expression<Func<T, bool>> predicate)
        {
            return con.QuerySet<T>().Where(predicate).Get();
        }
        public T Find<TKey>(TKey id)
        {
            return con.QuerySet<T>().Get(id);
        }
        public TResult FindField<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> keySelector)
        {
            return con.QuerySet<T>().Where(predicate).Select(keySelector).Get();
        }
        public int Count(Expression<Func<T, bool>> predicate) { return con.QuerySet<T>().Where(predicate).Count(); }
        public int CountAll { get { return Count(x=>true); } }
        public virtual int Delete(Expression<Func<T, bool>> predicate)
        {
            return con.CommandSet<T>().Where(predicate).Delete();
        }
        /// <summary>
        /// 删除某个数字ID的数据   注：必须有KEY定义，不然报错。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete<TKey>(TKey id)
        {
            return con.CommandSet<T>().Delete(id);
        }
        public virtual int Update(T TObject)
        {
            return con.CommandSet<T>().Update(TObject);
        }
        public virtual int Update(T d, params Expression<Func<T, object>>[] efdbd)
        {
            return con.CommandSet<T>().Update(d, efdbd);
        }
        public int UpdateWhere<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> key, TKey value)
        {
            return con.CommandSet<T>().Where(predicate).Update(key, value);
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
