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
                DictName = "��������",
                DictType = 99,
                Remark = "���Ա�ע",
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
                Content = "�����ߺ�",
                Title = "�����¾�",
                Author = new Author() { BirthDay = DateTime.Now, Id = 11, Name = "�����" }
            };
            var rst = RepoNews.Add(ttest);
            Assert.True(rst > 0);
        }
        [Fact]
        public void TestNews2()
        {
            News2 news2 = new News2()
            {
                Content = "����׹Ǿ�",
                Title = "��������Ҳ",
                Author = new Author[] { new Author() { BirthDay = DateTime.Now, Id = 12, Name = "���" }, new Author() { BirthDay = DateTime.Today, Id = 15, Name = "�˽�" } }
            };
            var rst = RepoNews2.Add(news2);
            Assert.True(rst > 0);
        }
    }
}
