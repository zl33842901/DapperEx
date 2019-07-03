using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;
using xLiAd.DapperEx.MsSql.Core.Core.Expression;
using xLiAd.DapperEx.MsSql.Core.Core.SetQ;
using xLiAd.DapperEx.Repository;

namespace xLiAd.DapperEx.RepositoryPg
{
    public class RepositoryPg<T> : RepositoryBase<T>, IRepositoryPg<T>
    {
        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="repoXmlProvider"></param>
        /// <param name="exceptionHandler">抛错时的委托</param>
        /// <param name="throws">是否抛出错误，强烈建议保持默认值 true 不然报错时会返回正常数据。</param>
        public RepositoryPg(string connectionString, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(new Npgsql.NpgsqlConnection(connectionString), repoXmlProvider, exceptionHandler, throws)
        {

        }
        public RepositoryPg(IDbConnection _con, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(_con, repoXmlProvider, exceptionHandler, throws)
        {

        }
        public RepositoryPg(IDbTransaction _tran, RepoXmlProvider repoXmlProvider = null, MsSql.Core.Core.DapperExExceptionHandler exceptionHandler = null, bool throws = true)
            : base(_tran.Connection, repoXmlProvider, exceptionHandler, throws, _tran)
        {

        }
        protected override ISqlDialect Dialect
        {
            get
            {
                var dialect = new PostgreSqlDialect();
                dialect.SetSerializeFunc(Newtonsoft.Json.JsonConvert.SerializeObject, Newtonsoft.Json.JsonConvert.DeserializeObject);
                return dialect;
            }
        }

        /// <summary>
        /// 获取事务提供
        /// </summary>
        /// <returns></returns>
        public override TransactionProviderBase GetTransaction()
        {
            if (DbTransaction != null)
                throw new Exception("已有事务实例的仓储不允许执行此操作。");
            else
                return new TransactionProviderPg(con, this.RepoXmlProvider, ExceptionHandler, Throws);
        }

        protected override QuerySet<T> QuerySet
        {
            get {
                var q = base.QuerySet;
                q.SetSerializeFunc(Newtonsoft.Json.JsonConvert.SerializeObject, Newtonsoft.Json.JsonConvert.DeserializeObject);
                if(this.FieldAnyExpression != null)
                {
                    q.FieldAnyExpression = this.FieldAnyExpression;
                    this.FieldAnyExpression = null;
                }
                return q;
            }
        }
        private IFieldAnyExpression FieldAnyExpression { get; set; }
        /// <summary>
        /// 此方式支持除分页投影之外的所有查询方法。
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="Field"></param>
        /// <param name="Any"></param>
        /// <returns></returns>
        public IRepositoryPg<T> FieldAny<TField>(Expression<Func<T,IList<TField>>> Field, Expression<Func<TField,bool>> Any)
        {
            this.FieldAnyExpression = new FieldAnyExpression<T, TField>(Field, Any, Dialect);
            return this;
        }

        public override TResult FindField<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> keySelector)
        {
            if (this.FieldAnyExpression != null && typeof(TResult).IsClass && typeof(TResult) != typeof(string))
                return WhereSelect(predicate, keySelector).FirstOrDefault();
            else
                return base.FindField(predicate, keySelector);
        }
    }
}
