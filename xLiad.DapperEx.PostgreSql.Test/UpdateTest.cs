using System;
using System.Collections.Generic;
using System.Text;
using xLiAd.DapperEx.RepositoryPg;
using Xunit;

namespace xLiad.DapperEx.PostgreSql.Test
{
    public class UpdateTest
    {
        string constring = "Host=127.0.0.1;Port=5432;Database=zhanglei;Username=postgres;Password=zhanglei";
        RepositoryPg<NewsTest> RepoNews => new RepositoryPg<NewsTest>(constring);
        RepositoryPg<DictInfo> RepoDict => new RepositoryPg<DictInfo>(constring);
        RepositoryPg<News2> RepoNews2 => new RepositoryPg<News2>(constring);
        [Fact]
        public void TestUpdate()
        {
            var ddxx = new DictInfo()
            {
                DictID = 1,
                CreateTime = DateTime.Now,
                DictType = 101,
                DictName = "某层某层会议室",
                Remark = "今天交流的内容是DapperEx使用",
                Deleted = true,
                OrderNum = OrderEnum.optionB
            };
            var rst = RepoDict.Update(ddxx);
            Assert.Equal(1, rst);
            var ddyy = RepoDict.Find(1);
            Assert.Equal("今天交流的内容是DapperEx使用", ddyy.Remark);
        }
        [Fact]
        public void TestUpdateModelField()
        {
            var ddxx = new DictInfo()
            {
                DictID = 8,
                CreateTime = DateTime.Now,
                Remark = "PHP是最好的语言"
            };
            var rst = RepoDict.Update(ddxx, x => x.CreateTime, x => x.Remark);
            Assert.Equal(1, rst);
            var ddyy = RepoDict.Find(8);
            Assert.Equal("PHP是最好的语言", ddyy.Remark);
        }
        [Fact]
        public void TestUpdateWhere()
        {
            var rst = RepoDict.UpdateWhere(x => x.DictID == 9 || x.DictID == 10, x => x.Remark, "Python是最好的语言");
            Assert.Equal(2, rst);
            var ddyy = RepoDict.Find(9);
            Assert.Equal("Python是最好的语言", ddyy.Remark);
            ddyy = RepoDict.Find(10);
            Assert.Equal("Python是最好的语言", ddyy.Remark);
        }
        [Fact]
        public void TestUpdateWhereMulti()
        {
            var rst = RepoDict.UpdateWhere(x => x.DictID > 8 && x.DictID < 11,
                new DictInfo()
                {
                    DictName = "C++是最好的语言",
                    OrderNum = OrderEnum.optionB
                }, x => x.DictName, x => x.OrderNum);
            var l = RepoDict.Where(x => x.DictID > 8 && x.DictID < 11);
            foreach (var i in l)
            {
                Assert.Equal("C++是最好的语言", i.DictName);
                Assert.Equal(OrderEnum.optionB, i.OrderNum);
            }
        }
    }
}
