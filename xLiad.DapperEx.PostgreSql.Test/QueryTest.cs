using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using xLiAd.DapperEx.RepositoryPg;
using Xunit;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace xLiad.DapperEx.PostgreSql.Test
{
    /// <summary>
    /// 本 ORM 优势：
    /// 0，使用表达式写条件、排序 等能提高工作效率（避免字段名称写错，简化语法。）
    /// 1，支持 Select 出部分字段匿名类型
    /// 2，支持部分字段更新
    /// 3，支持根据条件更新、删除
    /// 4，支持枚举字段检索
    /// 5，支持事务
    /// </summary>
    public class QueryTest
    {
        string constring = "Host=127.0.0.1;Port=5432;Database=zhanglei;Username=postgres;Password=zhanglei";
        RepositoryPg<NewsTest> RepoNews => new RepositoryPg<NewsTest>(constring);
        RepositoryPg<DictInfo> RepoDict => new RepositoryPg<DictInfo>(constring);
        RepositoryPg<News2> RepoNews2 => new RepositoryPg<News2>(constring);
        [Fact]
        public void TestWhere()
        {
            //var ida = new List<int>() { 1, 2 };
            //var rst = RepoDict.Where(x => ida.Contains(x.DictID));
            //Assert.Equal(2, rst.Count);
        }
        [Fact]
        public void TestWhereAndFind()
        {
            var rst = RepoDict.Where(x => x.DictID > 8);
            var model = RepoDict.Find(9);//Find 是根据主键获取记录，所以主键上要有 Key标记。
            Assert.Equal(model.Remark, rst.Find(x => x.DictID == 9)?.Remark);
        }
        [Fact]
        public void TestWhereAndAll()
        {
            var rst = RepoDict.Where(x => x.DictID > 5 && !x.Deleted);
            var model = rst.FirstOrDefault();
            Assert.False(null == model);
            var all = RepoDict.All();
            var model2 = all.Find(x => x.DictID == model.DictID);
            Assert.False(null == model2);
            Assert.Equal(model.Deleted, model2.Deleted);
            Assert.Equal(model.CreateTime, model2.CreateTime);
            Assert.Equal(model.Remark, model2.Remark);
            Assert.Equal(model.DictName, model2.DictName);
        }
        [Fact]
        public void TestWhereWithSomeFields()
        {
            var rst = RepoDict.Where(x => x.CreateTime > new DateTime(2018, 12, 1), x => x.DictID, x => x.DictName);//只查询 DictID DictName 两个字段
            Assert.False(0 == rst.Count);
            Assert.Equal(0, rst.Count(x => !string.IsNullOrEmpty(x.Remark)));
            var first = rst.First();
            var model = RepoDict.Find(first.DictID);
            Assert.Equal(first.DictName, model.DictName);
        }
        [Fact]
        public void TestWhereSelect()
        {
            var rst = RepoDict.WhereSelect(x => x.DictID >= 7, x => x.DictName);//投影查询（只查询某些字段）可投影为匿名类型
            var rstfull = RepoDict.Where(x => x.DictID >= 7);
            Assert.False(0 == rstfull.Count);
            Assert.Equal(rst.Count, rstfull.Count);
            bool r = rst.Contains(rstfull.First().DictName);
            Assert.True(r);

            var ll2 = RepoDict.WhereSelect(x => x.DictName.Contains("哈哈") && x.Deleted, x => new { x.DictName, x.DictID }).ToDictionary(x => x.DictID, x => x.DictName);
        }
        [Fact]
        public void TestWhereOrderSelect()
        {
            var id = 3;
            //WhereOrderSelect 查询排序并投影
            var lrst = RepoDict.WhereOrderSelect(x => x.DictID > id && x.DictName.Contains("老总") && x.CreateTime > new DateTime(2018, 1, 1), x => x.DictID, x => new { i1 = x.DictID, i2 = x.DictName });
            bool b = lrst.Count > 0;
            Assert.True(b);
            b = !lrst.First().i2.Contains("老总");
            Assert.False(b);
        }
        [Fact]
        public void TestExpression()
        {
            Expression<Func<DictInfo, bool>> expression = x => x.DictID > 6;
            expression = expression.And(x => x.DictName.Contains("老总"));
            var rst = RepoDict.Where(expression);
            bool b = rst.Count > 0;
            Assert.True(b);
        }
        [Fact]
        public void TestClassField()
        {
            var rst = RepoNews.Where(x => true);
            bool b = rst.Any(x => x.Author == null);
            Assert.False(b);
            var rr = RepoNews2.Where(x => true);
            bool bb = rr.Any(x => x.Author.Length < 1);
            Assert.False(b);
        }
        [Fact]
        public void TestPageList()
        {
            var rst = RepoDict.PageList(x => x.DictID > 1, x => x.DictID, 1, 3, true);
            bool b = rst.Items.Count == 3;
            Assert.True(b);
            Assert.Equal(rst.Total, RepoDict.Count(x => x.DictID > 1));
        }
        [Fact]
        public void TestTransaction()
        {
            var trans = RepoDict.GetTransaction();//只利用了 RepoDict 里的连接对象。
            var trepo = trans.GetRepository<DictInfo>();//带有事务的仓储对象
            var trepo2 = trans.GetRepository<NewsTest>();//带有事务的仓储对象
            bool rd = new System.Random().Next(1, 20000) > 10000;//随机的正负值，用来试验抛错与不抛错的情况。
            try
            {
                trepo.UpdateWhere(x => x.DictID == 2, x => x.CreateTime, DateTime.Now);
                var i = 0;
                var t = trepo.Where(x => x.DictID == 2);
                if (rd)
                {
                    var j = 5 / i;
                }
                trepo2.UpdateWhere(x => x.Id == 1, x => x.Title, "修改后的测试标题");
                trans.Commit();
                if (rd)
                    Assert.True(false);
                else
                    Assert.True(true);
            }
            catch (Exception)
            {
                trans.Rollback();
                if (rd)
                    Assert.True(true);
                else
                    Assert.True(false);
            }
        }
    }
}
