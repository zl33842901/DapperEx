using System;
using xLiAd.DapperEx.RepositoryPg;
using Xunit;

namespace xLiad.DapperEx.PostgreSql.Test
{
    public class InsertTest
    {
        string constring = "Host=127.0.0.1;Port=5432;Database=zhanglei;Username=postgres;Password=zhanglei";
        RepositoryPg<NewsTest> RepoNews => new RepositoryPg<NewsTest>(constring);
        RepositoryPg<DictInfo> RepoDict => new RepositoryPg<DictInfo>(constring);
        RepositoryPg<News2> RepoNews2 => new RepositoryPg<News2>(constring);
        [Fact]
        public void TestDictInfo()
        {
            var rst = RepoDict.Add(new DictInfo()
            {
                DictName = "测试名称",
                DictType = 99,
                Remark = "测试备注",
                CreateTime = DateTime.Now,
                OrderNum = OrderEnum.optionA,
                Deleted = false
            });
            Assert.True(rst > 0);
        }
        [Fact]
        public void TestNews()
        {
            NewsTest ttest = new NewsTest()
            {
                Content = "大侠走好",
                Title = "向您致敬",
                Author = new Author() { BirthDay = DateTime.Now, Id = 11, Name = "金大侠" }
            };
            var rst = RepoNews.Add(ttest);
            Assert.True(rst > 0);
        }
        [Fact]
        public void TestNews2()
        {
            News2 news2 = new News2()
            {
                Content = "三打白骨精",
                Title = "俺老孙来也",
                Author = new Author[] { new Author() { BirthDay = DateTime.Now, Id = 12, Name = "悟空" }, new Author() { BirthDay = DateTime.Today, Id = 15, Name = "八戒" } }
            };
            var rst = RepoNews2.Add(news2);
            Assert.True(rst > 0);
        }
    }
}
