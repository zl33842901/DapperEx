using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
//using Dapper;
using xLiAd.DapperEx.MsSql.Core.Core.Interfaces;
using xLiAd.DapperEx.MsSql.Core.Helper;
using xLiAd.DapperEx.MsSql.Core.Model;

namespace xLiAd.DapperEx.MsSql.Core.Core.SetC
{
    /// <summary>
    /// 指令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Command<T> : ICommand<T>, IInsert<T>
    {
        public event DapperExExceptionHandler ErrorHappened;
        protected readonly SqlProvider<T> SqlProvider;
        protected readonly IDbConnection DbCon;
        private readonly IDbTransaction _dbTransaction;
        protected DataBaseContext<T> SetContext { get; set; }
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
        public Dapper.DynamicParameters Params { get; private set; }
        private void SetSql()
        {
            if (SqlProvider.SqlString != null)
                this.SqlString = SqlProvider.SqlString;
            if (SqlProvider.Params != null)
                this.Params = SqlProvider.Params;
        }
        /// <summary>
        /// 新建立命令器
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="sqlProvider">SQL转换器</param>
        /// <param name="dbTransaction">事务</param>
        /// <param name="throws">是否抛出错误</param>
        protected Command(IDbConnection conn, SqlProvider<T> sqlProvider, IDbTransaction dbTransaction = null, bool throws = true)
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
            _dbTransaction = dbTransaction;
            Throws = throws;
        }
        #region Update
        public async Task<int> UpdateAsync(T entity)
        {
            SqlProvider.FormatUpdate(entity);
            SetSql();
            var result = await ExecAsync(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            return result;
        }
        public int Update(T entity)
        {
            SqlProvider.FormatUpdate(entity);
            SetSql();
            var result = Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            return result;
        }
        #endregion
        #region UpdateNotDefault
        public async Task<int> UpdateNotDefaultAsync(T entity)
        {
            SqlProvider.FormatUpdateNotDefault(entity);
            SetSql();
            var result = await ExecAsync(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            return result;
        }
        public int UpdateNotDefault(T entity)
        {
            SqlProvider.FormatUpdateNotDefault(entity);
            SetSql();
            var result = Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            return result;
        }
        #endregion
        #region Delete
        public async Task<int> DeleteAsync<TKey>(TKey id)
        {
            SqlProvider.FormatDelete(id);
            SetSql();
            var result = await ExecAsync(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            return result;
        }
        public int Delete<TKey>(TKey id)
        {
            SqlProvider.FormatDelete(id);
            SetSql();
            var result = Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            return result;
        }
        #endregion
        #region Update
        public async Task<int> UpdateAsync(Expression<Func<T, T>> updateExpression)
        {
            SqlProvider.FormatUpdate(updateExpression);
            SetSql();
            var result = await ExecAsync(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            return result;
        }
        public int Update(Expression<Func<T, T>> updateExpression)
        {
            SqlProvider.FormatUpdate(updateExpression);
            SetSql();
            var result = Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            return result;
        }
        #endregion
        #region Update
        public async Task<int> UpdateAsync(T model, params Expression<Func<T, object>>[] updateExpression)
        {
            SqlProvider.FormatUpdateZhanglei(model, updateExpression);
            SetSql();
            var result = await ExecAsync(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            return result;
        }
        public int Update(T model, params Expression<Func<T, object>>[] updateExpression)
        {
            SqlProvider.FormatUpdateZhanglei(model, updateExpression);
            SetSql();
            var result = Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            return result;
        }
        #endregion
        #region Update
        public async Task<int> UpdateAsync<TKey>(Expression<Func<T, TKey>> expression, TKey value)
        {
            SqlProvider.FormatUpdateZhanglei(expression, value);
            SetSql();
            var result = await ExecAsync(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            return result;
        }
        public int Update<TKey>(Expression<Func<T, TKey>> expression, TKey value)
        {
            SqlProvider.FormatUpdateZhanglei(expression, value);
            SetSql();
            var result = Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            return result;
        }
        #endregion
        #region Delete
        public async Task<int> DeleteAsync()
        {
            SqlProvider.FormatDelete();
            SetSql();
            var result = await ExecAsync(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            return result;
        }
        public int Delete()
        {
            SqlProvider.FormatDelete();
            SetSql();
            var result = Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            return result;
        }
        #endregion
        #region Insert
        public async Task<int> InsertAsync(T entity)
        {
            SqlProvider.FormatInsert(entity, out var isHaveIdentity, out var property);
            SetSql();
            int result;
            if (isHaveIdentity == System.ComponentModel.DataAnnotations.IdentityTypeEnum.Int)
            {
                result = await DbCon.ExecuteScalarAsync<int>(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            }
            else if(isHaveIdentity == System.ComponentModel.DataAnnotations.IdentityTypeEnum.Guid)
            {
                var gi = await DbCon.ExecuteScalarAsync<Guid>(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
                property.SetValue(entity, gi);
                result = 1;
            }
            else
            {
                result = await ExecAsync(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            }
            return result;
        }
        public int Insert(T entity)
        {
            SqlProvider.FormatInsert(entity, out var isHaveIdentity, out var property);
            SetSql();
            int result;
            if (isHaveIdentity == System.ComponentModel.DataAnnotations.IdentityTypeEnum.Int)
            {
                result = DbCon.ExecuteScalar<int>(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            }
            else if (isHaveIdentity == System.ComponentModel.DataAnnotations.IdentityTypeEnum.Guid)
            {
                var gi = DbCon.ExecuteScalar<Guid>(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
                property.SetValue(entity, gi);
                result = 1;
            }
            else
            {
                result = Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            }
            return result;
        }
        #endregion
        #region Insert
        public async Task<int> InsertAsync(IEnumerable<T> entitys)
        {
            if (entitys.Count() < 1)
                return 0;
            SqlProvider.FormatInsert(entitys.First(), out var _,out var _, true);
            SetSql();
            foreach(var item in entitys)
            {
                SqlProvider.SetAutoDateTime(item);
            }
            var result = await DbCon.ExecuteAsync(SqlProvider.SqlString, entitys, _dbTransaction);
            return result;
        }
        public int Insert(IEnumerable<T> entitys)
        {
            if (entitys.Count() < 1)
                return 0;
            SqlProvider.FormatInsert(entitys.First(), out var _, out var _, true);
            SetSql();
            foreach (var item in entitys)
            {
                SqlProvider.SetAutoDateTime(item);
            }
            var result = DbCon.Execute(SqlProvider.SqlString, entitys, _dbTransaction);
            return result;
        }
        #endregion
        private async Task<int> ExecAsync(string sqlString, Dapper.DynamicParameters param, IDbTransaction dbTransaction)
        {
            try
            {
                return await DbCon.ExecuteAsync(sqlString, param, dbTransaction);
            }
            catch(Exception e)
            {
                CallEvent(sqlString, param, e.Message);
                if (Throws)
                    throw new Exception($"{e.Message} sql:{sqlString} params:{param}", e);
                else
                    return 0;
            }
        }
        private int Exec(string sqlString, Dapper.DynamicParameters param, IDbTransaction dbTransaction)
        {
            try
            {
                return DbCon.Execute(sqlString, param, dbTransaction);
            }
            catch (Exception e)
            {
                CallEvent(sqlString, param, e.Message);
                if (Throws)
                    throw new Exception($"{e.Message} sql:{sqlString} params:{param}", e);
                else
                    return 0;
            }
        }
        private void CallEvent(string sqlString, Dapper.DynamicParameters param, string message)
        {
            try
            {
                var args = new DapperExEventArgs(sqlString, param, message);
                ErrorHappened?.Invoke(this, args);
            }
            catch { }
        }
    }
}
