using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using xLiAd.DapperEx.MsSql.Core.Model;

namespace xLiAd.DapperEx.Repository
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// 释放资源
        /// </summary>
        void Dispose();
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        List<T> All();
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<T>> AllAsync();
        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        List<T> Where(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        Task<List<T>> WhereDistinctAsync(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        List<T> WhereDistinct(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 只获取指定字段
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="efdbd"></param>
        /// <returns></returns>
        List<T> Where(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] efdbd);
        /// <summary>
        /// 只获取指定字段
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="efdbd"></param>
        /// <returns></returns>
        Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] efdbd);
        /// <summary>
        /// 只获取指定字段
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="efdbd"></param>
        /// <returns></returns>
        Task<List<T>> WhereDistinctAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] efdbd);
        /// <summary>
        /// 只获取指定字段
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="efdbd"></param>
        /// <returns></returns>
        List<T> WhereDistinct(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] efdbd);
        /// <summary>
        /// 根据条件获取数据并投影。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="selector">投影表达式</param>
        /// <returns></returns>
        List<TResult> WhereSelect<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        /// <summary>
        /// 根据条件获取数据并投影。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="selector">投影表达式</param>
        /// <returns></returns>
        Task<List<TResult>> WhereSelectAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        /// <summary>
        /// 根据条件获取数据并投影。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="selector">投影表达式</param>
        /// <returns></returns>
        Task<List<TResult>> WhereSelectDistinctAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        /// <summary>
        /// 根据条件获取数据并投影。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="selector">投影表达式</param>
        /// <returns></returns>
        List<TResult> WhereSelectDistinct<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        /// <summary>
        /// 根据条件排序查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="order">排序字段</param>
        /// <param name="top">取前 top 条，为0时取全部，默认为0.</param>
        /// <param name="desc">是否倒序</param>
        /// <returns></returns>
        List<T> WhereOrder<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, int top = 0, bool desc = false);
        /// <summary>
        /// 根据条件排序查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="order">排序字段</param>
        /// <param name="top">取前 top 条，为0时取全部，默认为0.</param>
        /// <param name="desc">是否倒序</param>
        /// <returns></returns>
        Task<List<T>> WhereOrderAsync<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, int top = 0, bool desc = false);
        /// <summary>
        /// 根据条件排序查询 并投影
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="order">排序字段</param>
        /// <param name="selector">投影表达式</param>
        /// <param name="top">取前 top 条，为0时取全部，默认为0.</param>
        /// <param name="desc">是否倒序</param>
        /// <returns></returns>
        List<TResult> WhereOrderSelect<TKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, Expression<Func<T, TResult>> selector, int top = 0, bool desc = false);
        /// <summary>
        /// 根据条件排序查询 并投影
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="order">排序字段</param>
        /// <param name="selector">投影表达式</param>
        /// <param name="top">取前 top 条，为0时取全部，默认为0.</param>
        /// <param name="desc">是否倒序</param>
        /// <returns></returns>
        Task<List<TResult>> WhereOrderSelectAsync<TKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, Expression<Func<T, TResult>> selector, int top = 0, bool desc = false);
        /// <summary>
        /// 根据条件排序去重查询 并投影
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="order">排序字段</param>
        /// <param name="selector">投影表达式</param>
        /// <param name="top">取前 top 条，为0时取全部，默认为0.</param>
        /// <param name="desc">是否倒序</param>
        /// <returns></returns>
        Task<List<TResult>> WhereOrderSelectDistinctAsync<TKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, Expression<Func<T, TResult>> selector, int top = 0, bool desc = false);
        /// <summary>
        /// 根据条件排序去重查询 并投影
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="order">排序字段</param>
        /// <param name="selector">投影表达式</param>
        /// <param name="top">取前 top 条，为0时取全部，默认为0.</param>
        /// <param name="desc">是否倒序</param>
        /// <returns></returns>
        List<TResult> WhereOrderSelectDistinct<TKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, Expression<Func<T, TResult>> selector, int top = 0, bool desc = false);
        /// <summary>
        /// 单条数据插入  请在标识属性上加 Identity 特性
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int Add(T obj);
        /// <summary>
        /// 单条数据插入  请在标识属性上加 Identity 特性
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task<int> AddAsync(T obj);
        /// <summary>
        /// 多条数据插入  请在标识属性上加 Identity 特性
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        int Add(IEnumerable<T> objs);
        /// <summary>
        /// 多条数据插入  请在标识属性上加 Identity 特性
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        Task<int> AddAsync(IEnumerable<T> objs);
        /// <summary>
        /// 多条数据插入(事务操作)  请在标识属性上加 Identity 特性
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        int AddTrans(IEnumerable<T> objs);
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
        PageList<T> PageList<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> orderBy, int pageindex = 1, int pagesize = 50, bool desc = false);
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
        Task<PageList<T>> PageListAsync<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> orderBy, int pageindex = 1, int pagesize = 50, bool desc = false);
        /// <summary>
        /// 根据条件分页（多排序条件）
        /// </summary>
        /// <param name="filter">条件表达式</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页条数</param>
        /// <param name="orders">排序字段</param>
        /// <returns></returns>
        PageList<T> PageList(Expression<Func<T, bool>> filter, int pageindex = 1, int pagesize = 50, params Tuple<Expression<Func<T, object>>, SortOrder>[] orders);
        /// <summary>
        /// 根据条件分页（多排序条件）
        /// </summary>
        /// <param name="filter">条件表达式</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页条数</param>
        /// <param name="orders">排序字段</param>
        /// <returns></returns>
        Task<PageList<T>> PageListAsync(Expression<Func<T, bool>> filter, int pageindex = 1, int pagesize = 50, params Tuple<Expression<Func<T, object>>, SortOrder>[] orders);
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
        PageList<T> PageList<TKey1, TKey2>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey1>> order1, bool order1Desc, Expression<Func<T, TKey2>> order2, bool order2Desc, int pageindex = 1, int pagesize = 50);
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
        Task<PageList<T>> PageListAsync<TKey1, TKey2>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey1>> order1, bool order1Desc, Expression<Func<T, TKey2>> order2, bool order2Desc, int pageindex = 1, int pagesize = 50);
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
        PageList<TResult> PageListSelect<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector, int pageindex = 1, int pagesize = 50, params Tuple<Expression<Func<T, object>>, SortOrder>[] orders);
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
        Task<PageList<TResult>> PageListSelectAsync<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector, int pageindex = 1, int pagesize = 50, params Tuple<Expression<Func<T, object>>, SortOrder>[] orders);
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据主键获取一条数据（实体类需要设置主键 在主键属性上加 Key特性）
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T Find<TKey>(TKey id);
        /// <summary>
        /// 根据主键获取一条数据（实体类需要设置主键 在主键属性上加 Key特性）
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> FindAsync<TKey>(TKey id);
        /// <summary>
        /// 根据条件获取一条数据并投影
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="keySelector">投影表达式</param>
        /// <returns></returns>
        TResult FindField<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> keySelector);
        /// <summary>
        /// 根据条件获取一条数据并投影
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="keySelector">投影表达式</param>
        /// <returns></returns>
        Task<TResult> FindFieldAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> keySelector);
        /// <summary>
        /// 根据条件取得数据数量
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据条件取得数据数量
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 是否存在符合条件的记录
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        bool Exist(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 是否存在符合条件的记录
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> ExistAsync(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 取得数据总数量
        /// </summary>
        int CountAll { get; }
        /// <summary>
        /// 取得数据总数量
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync(bool setSql = true);
        /// <summary>
        /// 取得数据总数量
        /// </summary>
        /// <returns></returns>
        int Count(bool setSql = true);
        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        int Delete(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据主键删除数据（实体类需要设置主键 在主键属性上加 Key特性）
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns></returns>
        int Delete<TKey>(TKey id);
        /// <summary>
        /// 根据主键删除数据（实体类需要设置主键 在主键属性上加 Key特性）
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns></returns>
        Task<int> DeleteAsync<TKey>(TKey id);
        /// <summary>
        /// 根据主键删除数据(事务操作)（实体类需要设置主键 在主键属性上加 Key特性）
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="idList"></param>
        /// <returns></returns>
        int DeleteTrans<TKey>(IEnumerable<TKey> idList);
        /// <summary>
        /// 根据主键更新一条数据的 除主键外的全部属性字段
        /// </summary>
        /// <param name="TObject">实体</param>
        /// <returns></returns>
        int Update(T TObject);
        /// <summary>
        /// 根据主键更新一条数据的 除主键外的全部属性字段
        /// </summary>
        /// <param name="TObject">实体</param>
        /// <returns></returns>
        Task<int> UpdateAsync(T TObject);
        /// <summary>
        /// 根据主键更新一些数据的 除主键外的全部属性字段（事务操作）
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        int UpdateTrans(IEnumerable<T> entityList);
        /// <summary>
        /// 根据主键更新一条数据的指定字段
        /// </summary>
        /// <param name="d">实体</param>
        /// <param name="efdbd">指定字段</param>
        /// <returns></returns>
        int Update(T d, params Expression<Func<T, object>>[] efdbd);
        /// <summary>
        /// 根据主键更新一条数据的指定字段
        /// </summary>
        /// <param name="d">实体</param>
        /// <param name="efdbd">指定字段</param>
        /// <returns></returns>
        Task<int> UpdateAsync(T d, params Expression<Func<T, object>>[] efdbd);
        /// <summary>
        /// 根据条件更新某个字段
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="key">要更新的字段</param>
        /// <param name="value">字段要更新的值</param>
        /// <returns></returns>
        int UpdateWhere<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> key, TKey value);
        /// <summary>
        /// 根据条件更新某个字段
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="key">要更新的字段</param>
        /// <param name="value">字段要更新的值</param>
        /// <returns></returns>
        Task<int> UpdateWhereAsync<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> key, TKey value);
        /// <summary>
        /// 根据条件更新若干字段
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="d">存储字段值的实体（主键不作为更新条件）</param>
        /// <param name="efdbd">要更新的字段</param>
        /// <returns></returns>
        int UpdateWhere(Expression<Func<T, bool>> predicate, T d, params Expression<Func<T, object>>[] efdbd);
        /// <summary>
        /// 根据条件更新若干字段
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="d">存储字段值的实体（主键不作为更新条件）</param>
        /// <param name="efdbd">要更新的字段</param>
        /// <returns></returns>
        Task<int> UpdateWhereAsync(Expression<Func<T, bool>> predicate, T d, params Expression<Func<T, object>>[] efdbd);
        /// <summary>
        /// 根据主键更新一条数据的 除主键外的值不为默认的字段
        /// </summary>
        /// <param name="TObject"></param>
        /// <returns></returns>
        Task<int> UpdateNotDefaultAsync(T TObject);
        /// <summary>
        /// 根据主键更新一条数据的 除主键外的值不为默认的字段
        /// </summary>
        /// <param name="TObject"></param>
        /// <returns></returns>
        int UpdateNotDefault(T TObject);
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="cmdType">命令类别</param>
        /// <returns></returns>
        bool ExecuteSql(string sql, object param = null, CommandType cmdType = CommandType.Text);
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="cmdType">命令类别</param>
        /// <returns></returns>
        Task<bool> ExecuteSqlAsync(string sql, object param = null, CommandType cmdType = CommandType.Text);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procedureName">存储过程</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        bool ExecuteProcedure(string procedureName, object param = null);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procedureName">存储过程</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        Task<bool> ExecuteProcedureAsync(string procedureName, object param = null);
        /// <summary>
        /// 执行查询 返回第一条结果
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        TResult GetScalar<TResult>(string sql, Dictionary<string, string> dic = null);
        /// <summary>
        /// 执行查询 返回第一条结果
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        Task<TResult> GetScalarAsync<TResult>(string sql, Dictionary<string, string> dic = null);
        /// <summary>
        /// 根据SQL语句，或存储过程 查询实体
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        IEnumerable<TResult> QueryBySql<TResult>(string sql, object param = null, CommandType cmdType = CommandType.Text);
        /// <summary>
        /// 根据SQL语句，或存储过程 查询实体
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param">参数</param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> QueryBySqlAsync<TResult>(string sql, object param = null, CommandType cmdType = CommandType.Text);
        /// <summary>
        /// 从XML中获取SQL执行
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        bool ExecuteXml(string id, Dictionary<string, string> dic = null);
        /// <summary>
        /// 从XML中获取SQL执行
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        Task<bool> ExecuteXmlAsync(string id, Dictionary<string, string> dic = null);
        /// <summary>
        /// 从XML中获取SQL查询
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="id"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        IEnumerable<TResult> QueryXml<TResult>(string id, Dictionary<string, string> dic = null);
        /// <summary>
        /// 从XML中获取SQL查询
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="id"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> QueryXmlAsync<TResult>(string id, Dictionary<string, string> dic = null);
    }
}
