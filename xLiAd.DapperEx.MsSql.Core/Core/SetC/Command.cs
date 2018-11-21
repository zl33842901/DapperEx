using System;
using System.Data;
using System.Linq.Expressions;
using Dapper;
using xLiAd.DapperEx.MsSql.Core.Core.Interfaces;
using xLiAd.DapperEx.MsSql.Core.Model;

namespace xLiAd.DapperEx.MsSql.Core.Core.SetC
{
    /// <summary>
    /// 指令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Command<T> : ICommand<T>, IInsert<T>
    {
        protected readonly SqlProvider<T> SqlProvider;
        protected readonly IDbConnection DbCon;
        private readonly IDbTransaction _dbTransaction;
        protected DataBaseContext<T> SetContext { get; set; }

        protected Command(IDbConnection conn, SqlProvider<T> sqlProvider)
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
        }

        protected Command(IDbConnection conn, SqlProvider<T> sqlProvider, IDbTransaction dbTransaction)
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
            _dbTransaction = dbTransaction;
        }

        public int Update(T entity)
        {
            SqlProvider.FormatUpdate(entity);

            return Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }
        public int Delete<TKey>(TKey id)
        {
            SqlProvider.FormatDelete(id);

            return Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        public int Update(Expression<Func<T, T>> updateExpression)
        {
            SqlProvider.FormatUpdate(updateExpression);

            return Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }
        public int Update(T model, params Expression<Func<T, object>>[] updateExpression)
        {
            SqlProvider.FormatUpdateZhanglei(model, updateExpression);

            return Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }
        public int Update<TKey>(Expression<Func<T, TKey>> expression, TKey value)
        {
            SqlProvider.FormatUpdateZhanglei(expression, value);

            return Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        public int Delete()
        {
            SqlProvider.FormatDelete();

            return Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        //public int Insert(T entity)
        //{
        //    SqlProvider.FormatInsert(entity);

        //    return Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        //}

        public int Insert(T entity)
        {
            SqlProvider.FormatInsert(entity, out bool isHaveIdentity);

            var r = Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
            if (r > 0)
            {
                if (isHaveIdentity)
                    return SqlProvider.Params.Get<int>("@id");
                else
                    return r;
            }
            else
                return 0;
        }

        private int Exec(string sqlString, DynamicParameters param, IDbTransaction dbTransaction)
        {
            try
            {
                return DbCon.Execute(sqlString, param, dbTransaction);
            }
            catch(Exception e)
            {
                throw new Exception($"{e.Message} sql:{sqlString} params:{param}", e);
            }
        }
    }
}
