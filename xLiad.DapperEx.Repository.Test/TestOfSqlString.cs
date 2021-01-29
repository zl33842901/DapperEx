using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using xLiAd.DapperEx.Repository;
using Xunit;

namespace xLiad.DapperEx.Repository.Test
{
    public class TestOfSqlString
    {
        private SqlConnection Conn => new SqlConnection("Data Source=127.0.0.1;Initial Catalog=zhanglei;Persist Security Info=True;User ID=sa;Password=zhanglei");
        [Fact]
        public void TestWhere()
        {
            var repository = new Repository<DictInfo>(Conn);
            var ida = new List<int>() { 106071, 106072 };
            var rst = repository.Where(x => ida.Contains(x.DictID));
            Assert.NotNull(repository.SqlString);
            Assert.Contains("SELECT  [DictID] [DictID],[DictName] [DictName],[Remark] [Remark],[CreateTime] [CreateTime],[Deleted] [Deleted],[OrderNum] [OrderNum],[DictType] [DictType]  FROM [DictInfo]   WHERE [DictID] IN @DictID", repository.SqlString);
            var a = repository.ParamsString;
            Assert.Contains("106071", a);
        }
    }
}
