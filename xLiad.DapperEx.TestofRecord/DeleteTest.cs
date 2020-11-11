using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using xLiAd.DapperEx.Repository;
using Xunit;

namespace xLiad.DapperEx.Repository.Test
{
    public class DeleteTest
    {
        private SqlConnection Conn => new SqlConnection("Data Source=127.0.0.1;Initial Catalog=zhanglei;Persist Security Info=True;User ID=sa;Password=zhanglei");
        [Fact]
        public void TestDelete()
        {
            var repository = new Repository<DictInfo>(Conn);
            //这种方式需要类定义主键字段(Key特性)
            var rst = repository.Delete(100020);
            Assert.True(rst == 1 || rst == 0);
            Assert.Equal(0, repository.Count(x => x.DictID == 100020));
        }
        [Fact]
        public void TestDeleteWhere()
        {
            var repository = new Repository<DictInfo>(Conn);
            var rst = repository.Delete(x => x.DictID > 106093 && x.DictName == "哈哈哈");
            Assert.True(rst == 1 || rst == 0);
            Assert.Equal(0, repository.Count(x => x.DictID == 106094));
        }
    }
}
