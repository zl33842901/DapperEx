using System;
using xLiAd.DapperEx.RepositoryOracle;
using Xunit;

namespace xLiad.DapperEx.Oracle.Test
{
    public class QueryTest
    {
        string connstring = "DATA SOURCE=172.16.8.170:1521/aaa;PASSWORD=aaa;PERSIST SECURITY INFO=True;USER ID=SYSADM";
        RepositoryOracle<NewsTest> RepoNews => new RepositoryOracle<NewsTest>(connstring);
        [Fact]
        public void TestSql()
        {
            var sql = @"SELECT T.DESCR
                , T.COUNTRY
                ,T.DESCR1
                FROM PS_YC_COUNTRY_VW T";
            var result = RepoNews.QueryBySql<Model1>(sql);
        }
    }
}
