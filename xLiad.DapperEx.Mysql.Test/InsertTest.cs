using System;
using System.Collections.Generic;
using System.Text;
using xLiAd.DapperEx.RepositoryMysql;
using Xunit;

namespace xLiad.DapperEx.Mysql.Test
{
    public class InsertTest
    {
        string conn = "server=172.16.101.40;User Id=root;password=cig@2017;Database=dapperExTest;CharSet=utf8;";
        RepositoryMysql<DictInfo> RepoDict => new RepositoryMysql<DictInfo>(conn);
        RepositoryMysql<TestStamp> repoStamp => new RepositoryMysql<TestStamp>(conn);

        [Fact]
        public void TestDictInfo()
        {
            var repository = RepoDict;
            //Add 方法，当类有标识字段(Identity特性)时，返回标识ID；否则返回影响行数
            var rst = repository.Add(new DictInfo()
            {
                DictName = "哈哈哈",
                DictType = 99,
                Remark = "啪啪",
                CreateTime = DateTime.Now,
                OrderNum = OrderEnum.optionA,
                Deleted = false
            });
            Assert.True(rst > 0);
        }
        [Fact]
        public void Testtt()
        {
            var repository = RepoDict;
            var rst = new DictInfo()
            {
                DictName = "哈哈哈",
                DictType = 99,
                Remark = "啪啪",
                CreateTime = DateTime.Now,
                OrderNum = OrderEnum.optionA,
                Deleted = false
            };
            var count = repository.AddTrans(new DictInfo[] { rst });
        }
        [Fact]
        public void TestTimeStamp()
        {
            var repoStamp = this.repoStamp;
            var rst = repoStamp.Add(new TestStamp()
            {
                CreateTime = DateTime.Now,
                Name = "测试名称"
            });
            Assert.True(rst > 0);
        }
    }
}
