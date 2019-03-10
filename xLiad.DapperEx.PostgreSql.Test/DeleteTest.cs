using System;
using System.Collections.Generic;
using System.Text;
using xLiAd.DapperEx.RepositoryPg;
using Xunit;

namespace xLiad.DapperEx.PostgreSql.Test
{
    public class DeleteTest
    {
        string constring = "Host=127.0.0.1;Port=5432;Database=zhanglei;Username=postgres;Password=zhanglei";
        RepositoryPg<NewsTest> RepoNews => new RepositoryPg<NewsTest>(constring);
        RepositoryPg<DictInfo> RepoDict => new RepositoryPg<DictInfo>(constring);
        RepositoryPg<News2> RepoNews2 => new RepositoryPg<News2>(constring);
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
            var rst = RepoDict.Delete(x => x.DictID > 4 && x.DictName == "哈哈哈");
            Assert.True(rst == 1 || rst == 0);
            Assert.Equal(0, RepoDict.Count(x => x.DictID == 5));
        }
    }
}
