using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Linq.Expressions;
using xLiAd.DapperEx.Repository;
using xLiAd.DapperEx.MsSql.Core.Helper;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace xLiad.DapperEx.Repository.TestFramework
{
    [TestClass]
    public class QueryTest
    {
        private SqlConnection Conn => new SqlConnection("Data Source=127.0.0.1;Initial Catalog=zhanglei;Persist Security Info=True;User ID=sa;Password=zhanglei");
        [TestMethod]
        public void TestWhere()
        {
            var repository = new Repository<DictInfo>(Conn);
            var ida = new List<int>() { 106071, 106072 };
            var rst = repository.Where(x => ida.Contains(x.DictID));
            Assert.AreEqual(2, rst.Count);
        }
        [TestMethod]
        public void TestWhere2()
        {
            var repository = new Repository<DictInfo>(Conn);
            int? id = 106071;
            var rst = repository.Where(x => x.DictID == id);
            Assert.AreEqual(1, rst.Count);
        }
        [TestMethod]
        public void TestWhere3()
        {
            var repository = new Repository<DictInfo2>(Conn);
            int? id = 106071;
            var rst = repository.Where(x => x.DictID == id);
            Assert.AreEqual(1, rst.Count);
        }
        [TestMethod]
        public void TestWhere4()
        {
            var repository = new Repository<DictInfo2>(Conn);
            int id = 106071;
            var rst = repository.Where(x => x.DictID == id);
            Assert.AreEqual(1, rst.Count);
        }
        [TestMethod]
        public void TestWhere5()
        {
            var repository = new Repository<DictInfo>(Conn);
            OrderEnum orderEnum = OrderEnum.optionA;
            var rst = repository.Where(x => x.OrderNum == orderEnum);
            Assert.AreNotEqual(0, rst.Count);
            foreach (var item in rst)
            {
                Assert.AreEqual(OrderEnum.optionA, item.OrderNum);
            }
        }
        [TestMethod]
        public void TestWhere6()
        {
            var repository = new Repository<DictInfo>(Conn);
            OrderEnum? orderEnum = OrderEnum.optionA;
            var rst = repository.Where(x => x.OrderNum == orderEnum);
            Assert.AreNotEqual(0, rst.Count);
            foreach (var item in rst)
            {
                Assert.AreEqual(OrderEnum.optionA, item.OrderNum);
            }
        }
        [TestMethod]
        public void TestWhere7()
        {
            var repository = new Repository<DictInfo2>(Conn);
            OrderEnum orderEnum = OrderEnum.optionA;
            var rst = repository.Where(x => x.OrderNum == orderEnum);
            Assert.AreNotEqual(0, rst.Count);
            foreach (var item in rst)
            {
                Assert.AreEqual(OrderEnum.optionA, item.OrderNum);
            }
        }
        [TestMethod]
        public void TestWhere8()
        {
            var repository = new Repository<DictInfo2>(Conn);
            OrderEnum? orderEnum = OrderEnum.optionA;
            var rst = repository.Where(x => x.OrderNum == orderEnum);
            Assert.AreNotEqual(0, rst.Count);
            foreach (var item in rst)
            {
                Assert.AreEqual(OrderEnum.optionA, item.OrderNum);
            }
        }
        [TestMethod]
        public void TestWhere9()
        {
            var repository = new Repository<DictInfo2>(Conn);
            var rst = repository.Where(x => x.OrderNum == null);
            Assert.AreEqual(0, rst.Count);
        }
        [TestMethod]
        public void TestWhere10()
        {
            var repository = new Repository<DictInfo>(Conn);
            var rst = repository.Where(x => x.OrderNum == null);
            Assert.AreEqual(0, rst.Count);
        }
        [TestMethod]
        public void TestWhere11()
        {
            var repository = new Repository<DictInfo>(Conn);
            var rst = repository.Where(x => x.DictID == null);
            Assert.AreEqual(0, rst.Count);
        }
        [TestMethod]
        public void TestWhere12()
        {
            var repository = new Repository<DictInfo2>(Conn);
            var rst = repository.Where(x => x.DictID == null);
            Assert.AreEqual(0, rst.Count);
        }
        [TestMethod]
        public void TestWhereAndFind()
        {
            var repository = new Repository<DictInfo>(Conn);
            var rst = repository.Where(x => x.DictID > 106087);
            var model = repository.Find(106088);//Find 是根据主键获取记录，所以主键上要有 Key标记。
            Assert.AreEqual(model.Remark, rst.Find(x => x.DictID == 106088)?.Remark);
        }
        [TestMethod]
        public void TestWhereAndAll()
        {
            var repository = new Repository<DictInfo>(Conn);
            var rst = repository.Where(x => x.DictID > 106091 && !x.Deleted);
            var model = rst.FirstOrDefault();
            Assert.IsFalse(null == model);
            var all = repository.All();
            var model2 = all.Find(x => x.DictID == model.DictID);
            Assert.IsFalse(null == model2);
            Assert.AreEqual(model.Deleted, model2.Deleted);
            Assert.AreEqual(model.CreateTime, model2.CreateTime);
            Assert.AreEqual(model.Remark, model2.Remark);
            Assert.AreEqual(model.DictName, model2.DictName);
        }
        [TestMethod]
        public void TestWhereWithSomeFields()
        {
            var repository = new Repository<DictInfo>(Conn);
            var rst = repository.Where(x => x.CreateTime > new DateTime(2018, 12, 1), x => x.DictID, x => x.DictName);//只查询 DictID DictName 两个字段
            Assert.IsFalse(0 == rst.Count);
            Assert.AreEqual(0, rst.Count(x => !string.IsNullOrEmpty(x.Remark)));
            var first = rst.First();
            var model = repository.Find(first.DictID);
            Assert.AreEqual(first.DictName, model.DictName);
        }
        [TestMethod]
        public void TestWhereSelect()
        {
            var repository = new Repository<DictInfo>(Conn);
            var rst = repository.WhereSelect(x => x.DictID >= 106071, x => x.DictName);//投影查询（只查询某些字段）可投影为匿名类型
            var rstfull = repository.Where(x => x.DictID >= 106071);
            Assert.IsFalse(0 == rstfull.Count);
            Assert.AreEqual(rst.Count, rstfull.Count);
            bool r = rst.Contains(rstfull.First().DictName);
            Assert.IsTrue(r);

            var ll2 = repository.WhereSelect(x => x.DictName.Contains("哈哈") && x.Deleted, x => new { x.DictName, x.DictID }).ToDictionary(x => x.DictID, x => x.DictName);
        }
        [TestMethod]
        public void TestWhereOrderSelect()
        {
            var repository = new Repository<DictInfo>(Conn);
            var id = 106071;
            //WhereOrderSelect 查询排序并投影
            var lrst = repository.WhereOrderSelect(x => x.DictID > id && x.DictName.Contains("老总") && x.CreateTime > new DateTime(2018, 1, 1), x => x.DictID, x => new { i1 = x.DictID, i2 = x.DictName });
            bool b = lrst.Count > 0;
            Assert.IsTrue(b);
            b = !lrst.First().i2.Contains("老总");
            Assert.IsFalse(b);
        }
        [TestMethod]
        public void TestExpression()
        {
            var repository = new Repository<DictInfo>(Conn);
            Expression<Func<DictInfo, bool>> expression = x => x.DictID > 106071;
            expression = expression.And(x => x.DictName.Contains("老总"));
            var rst = repository.Where(expression);
            bool b = rst.Count > 0;
            Assert.IsTrue(b);
        }
        [TestMethod]
        public void TestTimeStamp()
        {
            Repository<TestStamp> repoStamp = new Repository<TestStamp>(Conn);
            var rst = repoStamp.Where(x => true);
            bool b = rst.Any(x => string.IsNullOrEmpty(x.ROWVERSION));
            Assert.IsFalse(b);
        }
        [TestMethod]
        public void TestPageList()
        {
            var repository = new Repository<DictInfo>(Conn);
            var rst = repository.PageList(x => x.DictID > 5555, x => x.DictID, 1, 10, true);
            bool b = rst.Items.Count == 10;
            Assert.IsTrue(b);
            Assert.AreEqual(rst.Total, repository.Count(x => x.DictID > 5555));
        }
        [TestMethod]
        public void TestTransaction()
        {
            var repository = new Repository<DictInfo>(Conn);
            var trans = repository.GetTransaction();//只利用了 repository 里的连接对象。
            var trepo = trans.GetRepository<DictInfo>();//带有事务的仓储对象
            var trepo2 = trans.GetRepository<TestStamp>();//带有事务的仓储对象
            bool rd = new System.Random().Next(1, 20000) > 10000;//随机的正负值，用来试验抛错与不抛错的情况。
            try
            {
                trepo.UpdateWhere(x => x.DictID == 100018, x => x.CreateTime, DateTime.Now);
                var i = 0;
                var t = trepo.Where(x => x.DictID == 100018);
                if (rd)
                {
                    var j = 5 / i;
                }
                trepo2.UpdateWhere(x => x.Id == 1, x => x.Name, "修改后的测试名称");
                trans.Commit();
                if (rd)
                    Assert.IsTrue(false);
                else
                    Assert.IsTrue(true);
            }
            catch (Exception)
            {
                trans.Rollback();
                if (rd)
                    Assert.IsTrue(true);
                else
                    Assert.IsTrue(false);
            }
        }
    }
}
