﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Dapper;
using xLiAd.DapperEx.MsSql.Core.Core.Expression;
using xLiAd.DapperEx.MsSql.Core.Core.Interfaces;
using xLiAd.DapperEx.MsSql.Core.Helper;
using xLiAd.DapperEx.MsSql.Core.Model;

namespace xLiAd.DapperEx.MsSql.Core
{
    public class SqlProvider<T>
    {
        internal DataBaseContext<T> Context { get; set; }

        public SqlProvider()
        {
            Params = new DynamicParameters();
        }

        public string SqlString { get; private set; }

        public DynamicParameters Params { get; private set; }

        public SqlProvider<T> FormatGet()
        {
            var selectSql = ResolveExpression.ResolveSelect(typeof(T).GetPropertiesInDb(), Context.QuerySet.SelectExpression, 1);

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            var orderbySql = ResolveExpression.ResolveOrderBy(Context.QuerySet.OrderbyExpressionList);

            SqlString = $"{selectSql} {fromTableSql} {whereSql} {orderbySql}";

            return this;
        }
        public SqlProvider<T> FormatGet<TKey>(TKey id)
        {
            var selectSql = ResolveExpression.ResolveSelect(typeof(T).GetPropertiesInDb(), Context.QuerySet.SelectExpression, 1);

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere<T,TKey>(id);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            var orderbySql = ResolveExpression.ResolveOrderBy(Context.QuerySet.OrderbyExpressionList);

            SqlString = $"{selectSql} {fromTableSql} {whereSql} {orderbySql}";

            return this;
        }

        public SqlProvider<T> FormatToList()
        {
            //var selectSql = ResolveExpression.ResolveSelect(typeof(T).GetPropertiesInDb(), Context.QuerySet.SelectExpression, Context.QuerySet.TopNum);
            var selectSql = ResolveExpression.ResolveSelectZhanglei(typeof(T), Context.QuerySet.SelectExpression, Context.QuerySet.TopNum);

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            var orderbySql = ResolveExpression.ResolveOrderBy(Context.QuerySet.OrderbyExpressionList);

            SqlString = $"{selectSql} {fromTableSql} {whereSql} {orderbySql}";

            return this;
        }
        public SqlProvider<T> FormatToList(IEnumerable<LambdaExpression> selector)
        {
            //var selectSql = ResolveExpression.ResolveSelect(typeof(T).GetPropertiesInDb(), Context.QuerySet.SelectExpression, Context.QuerySet.TopNum);
            var selectSql = ResolveExpression.ResolveSelectZhanglei(typeof(T), selector, Context.QuerySet.TopNum);

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            var orderbySql = ResolveExpression.ResolveOrderBy(Context.QuerySet.OrderbyExpressionList);

            SqlString = $"{selectSql} {fromTableSql} {whereSql} {orderbySql}";

            return this;
        }
        public SqlProvider<T> FormatToListZhanglei(Type type)
        {
            var selectSql = ResolveExpression.ResolveSelectZhanglei(type, Context.QuerySet.SelectExpression, Context.QuerySet.TopNum);

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            var orderbySql = ResolveExpression.ResolveOrderBy(Context.QuerySet.OrderbyExpressionList);

            SqlString = $"{selectSql} {fromTableSql} {whereSql} {orderbySql}";

            return this;
        }

        public SqlProvider<T> FormatToPageList(int pageIndex, int pageSize)
        {
            var orderbySql = ResolveExpression.ResolveOrderBy(Context.QuerySet.OrderbyExpressionList);
            if (string.IsNullOrEmpty(orderbySql))
                throw new Exception("分页查询需要排序条件");

            var selectSql = ResolveExpression.ResolveSelectZhanglei(typeof(T), Context.QuerySet.SelectExpression, pageSize);

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"SELECT COUNT(1) {fromTableSql} {whereSql};";
            SqlString += $@"{selectSql}
            FROM    ( SELECT *
                      ,ROW_NUMBER() OVER ( {orderbySql} ) AS ROWNUMBER
                      {fromTableSql}
                      {whereSql}
                    ) T
            WHERE   ROWNUMBER > {(pageIndex - 1) * pageSize}
                    AND ROWNUMBER <= {pageIndex * pageSize} {orderbySql};";

            return this;
        }

        public SqlProvider<T> FormatCount()
        {
            var selectSql = "SELECT COUNT(1)";

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"{selectSql} {fromTableSql} {whereSql} ";

            return this;
        }

        public SqlProvider<T> FormatExists()
        {
            var selectSql = "SELECT TOP 1 1";

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"{selectSql} {fromTableSql} {whereSql}";

            return this;
        }

        public SqlProvider<T> FormatDelete()
        {
            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(Context.CommandSet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"DELETE {fromTableSql} {whereSql }";

            return this;
        }
        public SqlProvider<T> FormatDelete<TKey>(TKey id)
        {
            var fromTableSql = FormatTableName();

            var where = ResolveExpression.ResolveWhere<T, TKey>(id);

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
            identityProperty = typeof(T).GetPropertiesInDb().FirstOrDefault(x => x.CustomAttributes.Any(b => b.AttributeType == typeof(IdentityAttribute)));
            var paramsAndValuesSql = multiInsert ? FormatInsertParamsAndValues(entity,null) : FormatInsertParamsAndValues(entity, identityProperty);

            var ifnotexistsWhere = ResolveExpression.ResolveWhere(Context.CommandSet.IfNotExistsExpression, "INE_");

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
            var update = ResolveExpression.ResolveUpdate(updateExpression);

            var where = ResolveExpression.ResolveWhere(Context.CommandSet.WhereExpression);

            var whereSql = where.SqlCmd;

            Params = where.Param;
            Params.AddDynamicParams(update.Param);

            SqlString = $"UPDATE {FormatTableName(false)} {update.SqlCmd} {whereSql}";

            return this;
        }

        public SqlProvider<T> FormatUpdate(T entity)
        {
            var update = ResolveExpression.ResolveUpdate<T>(a => entity);

            var where = ResolveExpression.ResolveWhere(entity);

            var whereSql = where.SqlCmd;

            Params = where.Param;
            Params.AddDynamicParams(update.Param);

            SqlString = $"UPDATE {FormatTableName(false)} {update.SqlCmd} {whereSql}";

            return this;
        }
        public SqlProvider<T> FormatUpdateZhanglei(T entity, IEnumerable<LambdaExpression> expressionList)
        {
            var update = ResolveExpression.ResolveUpdateZhanglei<T>(expressionList, entity);

            IWhereExpression where;
            if(Context.CommandSet.WhereExpression == null)
                where = ResolveExpression.ResolveWhere(entity);
            else
                where = ResolveExpression.ResolveWhere(Context.CommandSet.WhereExpression);

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

            var update = ResolveExpression.ResolveUpdateZhanglei<T>(expression , value);

            var where = ResolveExpression.ResolveWhere(Context.CommandSet.WhereExpression);

            var whereSql = where.SqlCmd;

            Params = where.Param;
            Params.AddDynamicParams(update.Param);

            SqlString = $"UPDATE {FormatTableName(false)} {update.SqlCmd} {whereSql}";

            return this;
        }

        public SqlProvider<T> FormatSum(LambdaExpression lambdaExpression)
        {
            var selectSql = ResolveExpression.ResolveSum(typeof(T).GetPropertiesInDb(), lambdaExpression);

            var fromTableSql = FormatTableName();

            var whereParams = ResolveExpression.ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = whereParams.SqlCmd;

            Params = whereParams.Param;

            SqlString = $"{selectSql} {fromTableSql} {whereSql} ";

            return this;
        }

        public SqlProvider<T> FormatUpdateSelect(Expression<Func<T, T>> updator)
        {
            var update = ResolveExpression.ResolveUpdate(updator);

            var selectSql = ResolveExpression.ResolveSelectOfUpdate(typeof(T).GetPropertiesInDb(), Context.QuerySet.SelectExpression);

            var where = ResolveExpression.ResolveWhere(Context.QuerySet.WhereExpression);

            var whereSql = where.SqlCmd;

            Params = where.Param;
            Params.AddDynamicParams(update.Param);

            var topSql = Context.QuerySet.TopNum.HasValue ? " TOP " + Context.QuerySet.TopNum.Value : "";
            SqlString = $"UPDATE {topSql} {FormatTableName(false)} WITH ( UPDLOCK, READPAST ) {update.SqlCmd} {selectSql} {whereSql}";

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

            var tableName = typeOfTableClass.GetTableAttributeName();

            SqlString = isNeedFrom ? $" FROM {tableName} " : $" {tableName} ";

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

            var properties = entity.GetProperties();

            var isAppend = false;
            foreach (var property in properties)
            {
                if (property.CustomAttributes.Any(b => b.AttributeType == typeof(IdentityAttribute)))
                    continue;
                if (isAppend)
                {
                    paramSqlBuilder.Append(",");
                    valueSqlBuilder.Append(",");
                }

                var name = property.GetColumnAttributeName();

                paramSqlBuilder.Append(name);
                valueSqlBuilder.Append("@" + name);

                Params.Add("@" + name, property.GetValue(entity));

                isAppend = true;
            }
            string outputString = string.Empty;
            if(identityProperty != null)
            {
                outputString = $" OUTPUT INSERTED.{identityProperty.GetColumnAttributeName()} as insertedid ";
            }
            return $"({paramSqlBuilder}) {outputString} VALUES  ({valueSqlBuilder})";
        }
    }
}
