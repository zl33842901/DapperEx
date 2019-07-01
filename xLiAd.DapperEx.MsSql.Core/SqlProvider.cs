using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Dapper;
using xLiAd.DapperEx.MsSql.Core.Core.Dialect;
using xLiAd.DapperEx.MsSql.Core.Core.Expression;
using xLiAd.DapperEx.MsSql.Core.Core.Interfaces;
using xLiAd.DapperEx.MsSql.Core.Helper;
using xLiAd.DapperEx.MsSql.Core.Model;

namespace xLiAd.DapperEx.MsSql.Core
{
    public class SqlProvider<T>
    {
        internal DataBaseContext<T> Context { get; set; }

        internal readonly ISqlDialect Dialect;
        public SqlProvider(ISqlDialect dialect = null)
        {
            Params = new DynamicParameters();
            Dialect = dialect ?? new SqlServerDialect();
        }

        public string SqlString { get; private set; }

        public DynamicParameters Params { get; private set; }

        private SqlProvider<T> FormatGetDo(IWhereExpression whereParams, IFieldAnyExpression fieldAnyExpression)
        {
            var selectSql = ResolveExpression.Instance(Dialect).ResolveSelect(typeof(T).GetPropertiesInDb(true), Context.QuerySet.SelectExpression, 1, false);

            var fromTableSql = FormatTableName();

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            var orderbySql = ResolveExpression.Instance(Dialect).ResolveOrderBy(Context.QuerySet.OrderbyExpressionList);

            var limitSql = ResolveExpression.Instance(Dialect).ResolveLimit(1);
            if (fieldAnyExpression != null)
            {
                string selectDistinctSql = ResolveExpression.Instance(Dialect).ResolveSelect(typeof(T).GetPropertiesInDb(true), Context.QuerySet.SelectExpression, 1, true);
                var di = fieldAnyExpression.WhereParam.ToDictionary();
                foreach (var i in di)
                {
                    Params.Add(i.Key, i.Value);
                }
                SqlString = $"{selectDistinctSql} from ({selectSql} ,jsonb_array_elements({fieldAnyExpression.ListFieldName})  as \"{ResolveExpression.FieldAnyColumnName}\" {fromTableSql} {whereSql} {orderbySql}) as {ResolveExpression.FieldAnyTableName} where {fieldAnyExpression.WhereClause} {limitSql}";
            }
            else
                SqlString = $"{selectSql} {fromTableSql} {whereSql} {orderbySql} {limitSql}";

            return this;
        }
        public SqlProvider<T> FormatGet(IFieldAnyExpression fieldAnyExpression)
        {
            var whereParams = ResolveExpression.Instance(Dialect).ResolveWhere(Context.QuerySet.WhereExpression);
            return FormatGetDo(whereParams, fieldAnyExpression);
        }
        public SqlProvider<T> FormatGet<TKey>(TKey id, IFieldAnyExpression fieldAnyExpression)
        {
            var whereParams = ResolveExpression.Instance(Dialect).ResolveWhere<T, TKey>(id);
            return FormatGetDo(whereParams, fieldAnyExpression);
        }

        public SqlProvider<T> FormatToList(LambdaExpression[] selector, IFieldAnyExpression fieldAnyExpression)
        {
            string selectSql;
            if (selector == null)
                selectSql = ResolveExpression.Instance(Dialect).ResolveSelect(typeof(T), Context.QuerySet.TopNum, false, Context.QuerySet.SelectExpression);
            else
                selectSql = ResolveExpression.Instance(Dialect).ResolveSelect(typeof(T), Context.QuerySet.TopNum, false, selector);

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.Instance(Dialect).ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            var orderbySql = ResolveExpression.Instance(Dialect).ResolveOrderBy(Context.QuerySet.OrderbyExpressionList);

            var limitSql = ResolveExpression.Instance(Dialect).ResolveLimit(Context.QuerySet.TopNum);
            if(fieldAnyExpression != null)
            {
                string selectDistinctSql;
                if (selector == null)
                    selectDistinctSql = ResolveExpression.Instance(Dialect).ResolveSelect(typeof(T), Context.QuerySet.TopNum, true, Context.QuerySet.SelectExpression);
                else
                    selectDistinctSql = ResolveExpression.Instance(Dialect).ResolveSelect(typeof(T), Context.QuerySet.TopNum, true, selector);
                var di = fieldAnyExpression.WhereParam.ToDictionary();
                foreach(var i in di)
                {
                    Params.Add(i.Key, i.Value);
                }
                SqlString = $"{selectDistinctSql} from ({selectSql} ,jsonb_array_elements({fieldAnyExpression.ListFieldName})  as \"{ResolveExpression.FieldAnyColumnName}\" {fromTableSql} {whereSql} {orderbySql}) as {ResolveExpression.FieldAnyTableName} where {fieldAnyExpression.WhereClause} {limitSql}";
            }
            else
                SqlString = $"{selectSql} {fromTableSql} {whereSql} {orderbySql} {limitSql}";

            return this;
        }
        //public SqlProvider<T> FormatToList(LambdaExpression[] selector)
        //{
        //    var selectSql = ResolveExpression.Instance(Dialect).ResolveSelect(typeof(T), Context.QuerySet.TopNum, false, selector);

        //    var fromTableSql = FormatTableName();

        //    var whereParams = ResolveExpression.Instance(Dialect).ResolveWhere(Context.QuerySet.WhereExpression);

        //    var whereSql = whereParams.SqlCmd;

        //    Params = whereParams.Param;

        //    var orderbySql = ResolveExpression.Instance(Dialect).ResolveOrderBy(Context.QuerySet.OrderbyExpressionList);

        //    var limitSql = ResolveExpression.Instance(Dialect).ResolveLimit(Context.QuerySet.TopNum);

        //    SqlString = $"{selectSql} {fromTableSql} {whereSql} {orderbySql} {limitSql}";

        //    return this;
        //}
        public SqlProvider<T> FormatToListZhanglei(Type type, IFieldAnyExpression fieldAnyExpression = null)
        {
            var selectSql = ResolveExpression.Instance(Dialect).ResolveSelect(type, Context.QuerySet.TopNum, false, Context.QuerySet.SelectExpression);

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.Instance(Dialect).ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            var orderbySql = ResolveExpression.Instance(Dialect).ResolveOrderBy(Context.QuerySet.OrderbyExpressionList);

            var limitSql = ResolveExpression.Instance(Dialect).ResolveLimit(Context.QuerySet.TopNum);
            if (fieldAnyExpression != null)
            {
                var selectDistinctSql = ResolveExpression.Instance(Dialect).ResolveSelect(type, Context.QuerySet.TopNum, true, Context.QuerySet.SelectExpression);
                var di = fieldAnyExpression.WhereParam.ToDictionary();
                foreach (var i in di)
                {
                    Params.Add(i.Key, i.Value);
                }
                SqlString = $"{selectDistinctSql} from ({selectSql} ,jsonb_array_elements({fieldAnyExpression.ListFieldName})  as \"{ResolveExpression.FieldAnyColumnName}\" {fromTableSql} {whereSql} {orderbySql}) as {ResolveExpression.FieldAnyTableName} where {fieldAnyExpression.WhereClause} {limitSql}";
            }
            else
                SqlString = $"{selectSql} {fromTableSql} {whereSql} {orderbySql} {limitSql}";

            return this;
        }

        public SqlProvider<T> FormatToPageList(Type type, int pageIndex, int pageSize, IFieldAnyExpression fieldAnyExpression)
        {
            var orderbySql = ResolveExpression.Instance(Dialect).ResolveOrderBy(Context.QuerySet.OrderbyExpressionList);
            if (string.IsNullOrEmpty(orderbySql))
                throw new Exception("分页查询需要排序条件");

            var selectSql = ResolveExpression.Instance(Dialect).ResolveSelect(type, pageSize, false, Context.QuerySet.SelectExpression);

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.Instance(Dialect).ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            var limitSql = ResolveExpression.Instance(Dialect).ResolveLimit(pageSize);

            if (fieldAnyExpression != null)
            {
                var selectDistinctSql = ResolveExpression.Instance(Dialect).ResolveSelect(type, pageSize, true, Context.QuerySet.SelectExpression);
                var di = fieldAnyExpression.WhereParam.ToDictionary();
                foreach (var i in di)
                {
                    Params.Add(i.Key, i.Value);
                }
                string newTable = $"({selectSql} ,jsonb_array_elements({fieldAnyExpression.ListFieldName})  as \"{ResolveExpression.FieldAnyColumnName}\" {fromTableSql} {whereSql} {orderbySql}) as {ResolveExpression.FieldAnyTableName}";
                string newNewTable = $"{selectDistinctSql} from {newTable} where {fieldAnyExpression.WhereClause} {limitSql}";

                SqlString = $"SELECT COUNT(1) from ({newNewTable}) as {ResolveExpression.FieldAnyTableName};";
                SqlString += $@"{selectDistinctSql}
            FROM    ( SELECT *
                      ,ROW_NUMBER() OVER ( {orderbySql} ) AS ROWNUMBER
                      from {newTable} where {fieldAnyExpression.WhereClause}
                    ) T
            WHERE   ROWNUMBER > {(pageIndex - 1) * pageSize}
                    AND ROWNUMBER <= {pageIndex * pageSize} {orderbySql} {limitSql};";
            }
            else
            {
                SqlString = $"SELECT COUNT(1) {fromTableSql} {whereSql};";
                if(Dialect.pageListDialectEnum == PageListDialectEnum.Mysql)
                {
                    SqlString += $@"{selectSql} {fromTableSql} {whereSql} {orderbySql} limit {(pageIndex - 1) * pageSize},{pageSize}";
                }
                else
                {
                    SqlString += $@"{selectSql}
            FROM    ( SELECT *
                      ,ROW_NUMBER() OVER ( {orderbySql} ) AS ROWNUMBER
                      {fromTableSql}
                      {whereSql}
                    ) T
            WHERE   ROWNUMBER > {(pageIndex - 1) * pageSize}
                    AND ROWNUMBER <= {pageIndex * pageSize} {orderbySql} {limitSql};";
                }
            }

            return this;
        }

        public SqlProvider<T> FormatCount()
        {
            var selectSql = "SELECT COUNT(1)";

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.Instance(Dialect).ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"{selectSql} {fromTableSql} {whereSql} ";

            return this;
        }

        public SqlProvider<T> FormatExists()
        {
            var selectSql = Dialect.IsUseLimitInsteadOfTop ? "SELECT 1" : "SELECT TOP 1 1";

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.Instance(Dialect).ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            var limitSql = Dialect.IsUseLimitInsteadOfTop ? " Limit 1" : string.Empty;

            SqlString = $"{selectSql} {fromTableSql} {whereSql} {limitSql}";

            return this;
        }

        public SqlProvider<T> FormatDelete()
        {
            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.Instance(Dialect).ResolveWhere(Context.CommandSet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"DELETE {fromTableSql} {whereSql }";

            return this;
        }
        public SqlProvider<T> FormatDelete<TKey>(TKey id)
        {
            var fromTableSql = FormatTableName();

            var where = ResolveExpression.Instance(Dialect).ResolveWhere<T, TKey>(id);

            var whereSql = where.SqlCmd;

            Params = where.Param;

            SqlString = $"DELETE {fromTableSql} {whereSql}";

            return this;
        }

        //public SqlProvider<T> FormatInsert(T entity)
        //{
        //    var paramsAndValuesSql = FormatInsertParamsAndValues(entity);

        //    var ifnotexistsWhere = ResolveExpression.ResolveWhere(Context.CommandSet.IfNotExistsExpression, "INE_");

        //    Params.AddDynamicParams(ifnotexistsWhere.Param);

        //    SqlString = Context.CommandSet.IfNotExistsExpression != null ? $"IF NOT EXISTS ( SELECT  1 FROM {FormatTableName(false)} {ifnotexistsWhere.SqlCmd} ) INSERT INTO {FormatTableName(false)} {paramsAndValuesSql}" : $"INSERT INTO {FormatTableName(false)} {paramsAndValuesSql}";
        //    return this;
        //}
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isHaveIdentity">代表是否含有自增标识</param>
        /// <returns></returns>
        public SqlProvider<T> FormatInsert(T entity, out IdentityTypeEnum isHaveIdentity,out PropertyInfo identityProperty, bool multiInsert = false)
        {
            //标识属性
            identityProperty = typeof(T).GetPropertiesInDb(false).FirstOrDefault(x => x.CustomAttributes.Any(b => b.AttributeType == typeof(IdentityAttribute)));
            var paramsAndValuesSql = multiInsert ? FormatInsertParamsAndValues(entity,null) : FormatInsertParamsAndValues(entity, identityProperty);

            var ifnotexistsWhere = ResolveExpression.Instance(Dialect).ResolveWhere(Context.CommandSet.IfNotExistsExpression, "INE_");

            Params.AddDynamicParams(ifnotexistsWhere.Param);

            SqlString = Context.CommandSet.IfNotExistsExpression != null ? $"IF NOT EXISTS ( SELECT  1 FROM {FormatTableName(false)} {ifnotexistsWhere.SqlCmd} ) INSERT INTO {FormatTableName(false)} {paramsAndValuesSql}" : $"INSERT INTO {FormatTableName(false)} {paramsAndValuesSql}";

            if (!multiInsert)
            {
                if (identityProperty != null)
                {
                    if(identityProperty.PropertyType == typeof(Guid))
                    {
                        isHaveIdentity = IdentityTypeEnum.Guid;
                        //Params.Add("@id", dbType: DbType.Guid, direction: ParameterDirection.Output);
                        //SqlString = SqlString + ";SELECT @id=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        isHaveIdentity = IdentityTypeEnum.Int;
                        //Params.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                        //SqlString = SqlString + ";SELECT @id=SCOPE_IDENTITY()";
                    }
                }
                else
                {
                    isHaveIdentity = IdentityTypeEnum.NoIdentity;
                }
            }
            else
                isHaveIdentity = IdentityTypeEnum.NoIdentity;
            return this;
        }

        public SqlProvider<T> FormatUpdate(Expression<Func<T, T>> updateExpression)
        {
            var update = ResolveExpression.Instance(Dialect).ResolveUpdate(updateExpression);

            var where = ResolveExpression.Instance(Dialect).ResolveWhere(Context.CommandSet.WhereExpression);

            var whereSql = where.SqlCmd;

            Params = where.Param;
            Params.AddDynamicParams(update.Param);

            SqlString = $"UPDATE {FormatTableName(false)} {update.SqlCmd} {whereSql}";

            return this;
        }

        public SqlProvider<T> FormatUpdate(T entity)
        {
            var update = ResolveExpression.Instance(Dialect).ResolveUpdate<T>(a => entity);

            var where = ResolveExpression.Instance(Dialect).ResolveWhere(entity);

            var whereSql = where.SqlCmd;

            Params = where.Param;
            Params.AddDynamicParams(update.Param);

            SqlString = $"UPDATE {FormatTableName(false)} {update.SqlCmd} {whereSql}";

            return this;
        }
        public SqlProvider<T> FormatUpdateNotDefault(T entity)
        {
            var update = ResolveExpression.Instance(Dialect).ResolveUpdateNotDefault<T>(a => entity);

            var where = ResolveExpression.Instance(Dialect).ResolveWhere(entity);

            var whereSql = where.SqlCmd;

            Params = where.Param;
            Params.AddDynamicParams(update.Param);

            SqlString = $"UPDATE {FormatTableName(false)} {update.SqlCmd} {whereSql}";

            return this;
        }
        public SqlProvider<T> FormatUpdateZhanglei(T entity, IEnumerable<LambdaExpression> expressionList)
        {
            var update = ResolveExpression.Instance(Dialect).ResolveUpdateZhanglei<T>(expressionList, entity);

            IWhereExpression where;
            if(Context.CommandSet.WhereExpression == null)
                where = ResolveExpression.Instance(Dialect).ResolveWhere(entity);
            else
                where = ResolveExpression.Instance(Dialect).ResolveWhere(Context.CommandSet.WhereExpression);

            var whereSql = where.SqlCmd;

            Params = where.Param;
            Params.AddDynamicParams(update.Param);

            SqlString = $"UPDATE {FormatTableName(false)} {update.SqlCmd} {whereSql}";

            return this;
        }
        public SqlProvider<T> FormatUpdateZhanglei<TKey>(Expression<Func<T,TKey>> expression, TKey value)
        {
            MemberExpression m = expression.Body as MemberExpression;
            if (m == null)
                throw new FieldAccessException("Field Not Found");

            var update = ResolveExpression.Instance(Dialect).ResolveUpdateZhanglei<T>(expression , value);

            var where = ResolveExpression.Instance(Dialect).ResolveWhere(Context.CommandSet.WhereExpression);

            var whereSql = where.SqlCmd;

            Params = where.Param;
            Params.AddDynamicParams(update.Param);

            SqlString = $"UPDATE {FormatTableName(false)} {update.SqlCmd} {whereSql}";

            return this;
        }

        public SqlProvider<T> FormatSum(LambdaExpression lambdaExpression)
        {
            var selectSql = ResolveExpression.Instance(Dialect).ResolveSum(typeof(T).GetPropertiesInDb(true), lambdaExpression);

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.Instance(Dialect).ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"{selectSql} {fromTableSql} {whereSql} ";

            return this;
        }

        public SqlProvider<T> FormatUpdateSelect(Expression<Func<T, T>> updator)
        {
            var update = ResolveExpression.Instance(Dialect).ResolveUpdate(updator);

            var selectSql = ResolveExpression.Instance(Dialect).ResolveSelectOfUpdate(typeof(T).GetPropertiesInDb(true), Context.QuerySet.SelectExpression);

            var where = ResolveExpression.Instance(Dialect).ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = where.SqlCmd;

            Params = where.Param;
            Params.AddDynamicParams(update.Param);

            var topSql = Context.QuerySet.TopNum.HasValue && !Dialect.IsUseLimitInsteadOfTop ? " TOP " + Context.QuerySet.TopNum.Value : "";

            var limitSql = Context.QuerySet.TopNum.HasValue && Dialect.IsUseLimitInsteadOfTop ? $" Limit {Context.QuerySet.TopNum.Value}" : "";

            SqlString = $"UPDATE {topSql} {FormatTableName(false)} WITH ( UPDLOCK, READPAST ) {update.SqlCmd} {selectSql} {whereSql} {limitSql}";

            return this;
        }

        private string FormatTableName(bool isNeedFrom = true)
        {
            Type typeOfTableClass;
            switch (Context.OperateType)
            {
                case EOperateType.Query:
                    typeOfTableClass = Context.QuerySet.TableType;
                    break;
                case EOperateType.Command:
                    typeOfTableClass = Context.CommandSet.TableType;
                    break;
                default:
                    throw new Exception("error EOperateType");
            }

            TableAttribute att;

            var tableName = typeOfTableClass.GetTableAttributeName(out att);

            var sc = att?.Schema ?? string.Empty;

            if (!string.IsNullOrEmpty(sc))
                sc = $"{sc}.";

            SqlString = isNeedFrom ? $" FROM {sc}{Dialect.ParseTableName(tableName)} " : $" {sc}{Dialect.ParseTableName(tableName)} ";

            return SqlString;
        }
        /// <summary>
        /// 生成列参数和值
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="identityProperty">标识列（没有或批量插入时 这个应为null）</param>
        /// <returns></returns>
        private string FormatInsertParamsAndValues(T entity, PropertyInfo identityProperty)
        {
            var paramSqlBuilder = new StringBuilder(100);
            var valueSqlBuilder = new StringBuilder(100);

            var properties = entity.GetProperties(false);

            var isAppend = false;
            foreach (var property in properties)
            {
                if (property.CustomAttributes.Any(b => b.AttributeType == typeof(IdentityAttribute)))
                    continue;
                bool isJsonColumn = property.CustomAttributes.Any(b => b.AttributeType == typeof(JsonColumnAttribute));//是否是JSON列
                if (isAppend)
                {
                    paramSqlBuilder.Append(",");
                    valueSqlBuilder.Append(",");
                }

                var name = property.GetColumnAttributeName(Dialect);//带方言符号的名称  如 [Title]
                var namenw = property.Name;//不带方言符号的纯名称  如  Title

                paramSqlBuilder.Append(name);

                object paramValue;
                if(isJsonColumn && Dialect.SupportJsonColumn && Dialect.HasSerializer)
                {
                    valueSqlBuilder.Append("@" + namenw + "::jsonb");
                    paramValue = Dialect.Serializer(property.GetValue(entity));
                }
                else
                {
                    valueSqlBuilder.Append("@" + namenw);
                    paramValue = property.GetValue(entity);
                }
                Params.Add("@" + namenw, paramValue);

                isAppend = true;
            }

            return Dialect.FormatInsertValues(identityProperty?.GetColumnAttributeName(Dialect), paramSqlBuilder.ToString(), valueSqlBuilder.ToString());

            //string outputString = string.Empty;
            //if(identityProperty != null)
            //{
            //    outputString = $" OUTPUT INSERTED.{identityProperty.GetColumnAttributeName(Dialect)} as insertedid ";
            //}
            //return $"({paramSqlBuilder}) {outputString} VALUES  ({valueSqlBuilder})";
        }
    }
}
