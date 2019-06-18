using System;
using System.Collections.Generic;
using System.Text;
using xLiAd.DapperEx.RepositoryMysql;
using Xunit;

namespace xLiad.DapperEx.Mysql.Test
{
    public class DeleteTest
    {
        string conn = "server=172.16.101.40;User Id=root;password=cig@2017;Database=dapperExTest;CharSet=utf8;";
        RepositoryMysql<DictInfo> RepoDict => new RepositoryMysql<DictInfo>(conn);
        [Fact]
        public void TestDelete()
        {
            //这种方式需要类定义主键字段(Key特性)
            var rst = RepoDict.Delete(4);
            Assert.True(rst == 1 || rst == 0);
            Assert.Equal(0, RepoDict.Count(x => x.DictID == 4));
        }
        [Fact]
        public void TestDeleteWhere()
        {
            var rst = RepoDict.Delete(x => x.DictID > 9 && x.DictName == "哈哈哈");
            Assert.True(rst == 1 || rst == 0);
            Assert.Equal(0, RepoDict.Count(x => x.DictID == 10));
        }
    }
}
