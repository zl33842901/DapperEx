using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using xLiAd.DapperEx.Repository;
using Xunit;

namespace xLiad.DapperEx.Repository.Test
{
    public class UpdateTest
    {
        private SqlConnection Conn => new SqlConnection("Data Source=127.0.0.1;Initial Catalog=zhanglei;Persist Security Info=True;User ID=sa;Password=zhanglei");
        [Fact]
        public void TestUpdate()
        {
            var repository = new Repository<DictInfo>(Conn);
            var ddxx = new DictInfo()
            {
                DictID = 106092,
                CreateTime = DateTime.Now,
                DictType = 101,
                DictName = "某层某层会议室",
                Remark = "今天交流的内容是DapperEx使用",
                Deleted = true,
                OrderNum = OrderEnum.optionB
            };
            var rst = repository.Update(ddxx);
            Assert.Equal(1, rst);
            var ddyy = repository.Find(106092);
            Assert.Equal("今天交流的内容是DapperEx使用", ddyy.Remark);
        }
        [Fact]
        public void TestUpdateModelField()
        {
            var repository = new Repository<DictInfo>(Conn);
            var ddxx = new DictInfo()
            {
                DictID = 106091,
                CreateTime = DateTime.Now,
                Remark = "PHP是最好的语言"
            };
            var rst = repository.Update(ddxx, x => x.CreateTime, x => x.Remark);
            Assert.Equal(1, rst);
            var ddyy = repository.Find(106091);
            Assert.Equal("PHP是最好的语言", ddyy.Remark);
        }
        [Fact]
        public void TestUpdateWhere()
        {
            var repository = new Repository<DictInfo>(Conn);
            var rst = repository.UpdateWhere(x => x.DictID == 106088 || x.DictID == 106089, x => x.Remark, "Python是最好的语言");
            Assert.Equal(2, rst);
            var ddyy = repository.Find(106088);
            Assert.Equal("Python是最好的语言", ddyy.Remark);
            ddyy = repository.Find(106089);
            Assert.Equal("Python是最好的语言", ddyy.Remark);
        }
        [Fact]
        public void TestUpdateWhereMulti()
        {
            var repository = new Repository<DictInfo>(Conn);
            var rst = repository.UpdateWhere(x => x.DictID > 102099 && x.DictID < 104066,
                new DictInfo()
                {
                    DictName = "C++是最好的语言",
                    OrderNum = OrderEnum.optionB
                }, x => x.DictName, x => x.OrderNum);
            var l = repository.Where(x => x.DictID > 102099 && x.DictID < 104066);
            foreach(var i in l)
            {
                Assert.Equal("C++是最好的语言", i.DictName);
                Assert.Equal(OrderEnum.optionB, i.OrderNum);
            }
        }
        [Fact]
        public void TestUpdateNotDefault()
        {
            var repository = new Repository<DictInfo>(Conn);
            var ddxx = new DictInfo()
            {
                DictID = 106092,
                CreateTime = DateTime.Now,
                DictType = null,
                DictName = "某层某层会议室",
                Remark = null,
                Deleted = false,
                OrderNum = OrderEnum.optionB
            };
            var rst = repository.UpdateNotDefault(ddxx);
            Assert.Equal(1, rst);
            var ddyy = repository.Find(106092);
            Assert.Equal("今天交流的内容是DapperEx使用", ddyy.Remark);
        }
    }
}
