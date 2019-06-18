using System;
using System.Collections.Generic;
using System.Text;
using xLiAd.DapperEx.RepositoryMysql;
using Xunit;

namespace xLiad.DapperEx.Mysql.Test
{
    public class UpdateTest
    {
        string conn = "server=172.16.101.40;User Id=root;password=cig@2017;Database=dapperExTest;CharSet=utf8;";
        RepositoryMysql<DictInfo> RepoDict => new RepositoryMysql<DictInfo>(conn);
        [Fact]
        public void TestUpdate()
        {
            var repository = RepoDict;
            var ddxx = new DictInfo()
            {
                DictID = 11,
                CreateTime = DateTime.Now,
                DictType = 101,
                DictName = "某层某层会议室",
                Remark = "今天交流的内容是DapperEx使用",
                Deleted = true,
                OrderNum = OrderEnum.optionB
            };
            var rst = repository.Update(ddxx);
            Assert.Equal(1, rst);
            var ddyy = repository.Find(11);
            Assert.Equal("今天交流的内容是DapperEx使用", ddyy.Remark);
        }
        [Fact]
        public void TestUpdateModelField()
        {
            var repository = RepoDict;
            var ddxx = new DictInfo()
            {
                DictID = 1,
                CreateTime = DateTime.Now,
                Remark = "PHP是最好的语言"
            };
            var rst = repository.Update(ddxx, x => x.CreateTime, x => x.Remark);
            Assert.Equal(1, rst);
            var ddyy = repository.Find(1);
            Assert.Equal("PHP是最好的语言", ddyy.Remark);
        }
        [Fact]
        public void TestUpdateWhere()
        {
            var repository = RepoDict;
            var rst = repository.UpdateWhere(x => x.DictID == 2 || x.DictID == 12, x => x.Remark, "Python是最好的语言");
            Assert.Equal(2, rst);
            var ddyy = repository.Find(2);
            Assert.Equal("Python是最好的语言", ddyy.Remark);
            ddyy = repository.Find(12);
            Assert.Equal("Python是最好的语言", ddyy.Remark);
        }
        [Fact]
        public void TestUpdateWhereMulti()
        {
            var repository = RepoDict;
            var rst = repository.UpdateWhere(x => x.DictID > 12 && x.DictID < 15,
                new DictInfo()
                {
                    DictName = "C++是最好的语言",
                    OrderNum = OrderEnum.optionB
                }, x => x.DictName, x => x.OrderNum);
            var l = repository.Where(x => x.DictID > 12 && x.DictID < 15);
            foreach (var i in l)
            {
                Assert.Equal("C++是最好的语言", i.DictName);
                Assert.Equal(OrderEnum.optionB, i.OrderNum);
            }
        }
    }
}
