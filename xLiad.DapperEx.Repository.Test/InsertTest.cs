using System;
using System.Data.SqlClient;
using xLiAd.DapperEx.Repository;
using Xunit;

namespace xLiad.DapperEx.Repository.Test
{
    public class InsertTest
    {
        private SqlConnection Conn => new SqlConnection("Data Source=127.0.0.1;Initial Catalog=zhanglei;Persist Security Info=True;User ID=sa;Password=zhanglei");
        [Fact]
        public void TestDictInfo()
        {
            var repository = new Repository<DictInfo>(Conn);
            //Add 方法，当类有标识字段(Identity特性)时，返回标识ID；否则返回影响行数
            var rst = repository.Add(new DictInfo()
            {
                DictName = "测试名称",
                DictType = 99,
                Remark = "测试备注",
                OrderNum = OrderEnum.optionA,
                Deleted = false
            });
            Assert.True(rst > 0);
        }
        [Fact]
        public void TestTimeStamp()
        {
            Repository<TestStamp> repoStamp = new Repository<TestStamp>(Conn);
            var rst = repoStamp.Add(new TestStamp()
            {
                CreateTime = DateTime.Now,
                Name = "测试名称"
            });
            Assert.True(rst > 0);
        }
        [Fact]
        public void TestDictInfoMulti()
        {
            var repository = new Repository<DictInfo>(Conn);
            //Add 方法，当类有标识字段(Identity特性)时，返回标识ID；否则返回影响行数
            var rst = repository.Add(new DictInfo[] {
                new DictInfo()
                {
                DictName = "测试名称",
                DictType = 99,
                Remark = "测试备注",
                OrderNum = OrderEnum.optionA,
                Deleted = false
                },
                new DictInfo()
                {
                DictName = "测试名称",
                DictType = 99,
                Remark = "测试备注",
                OrderNum = OrderEnum.optionA,
                Deleted = false
                }
            });
            Assert.True(rst > 0);
        }
    }
}
