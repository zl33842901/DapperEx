using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using xLiAd.DapperEx.Repository;
using xLiAd.DapperEx.RepositoryMysql;
using Xunit;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace xLiad.DapperEx.Mysql.Test
{
    public class QueryTest
    {
        string conn = "server=172.16.101.40;User Id=root;password=cig@2017;Database=dapperExTest;CharSet=utf8;";
        RepositoryMysql<DictInfo> RepoDict => new RepositoryMysql<DictInfo>(conn);
        [Fact]
        public void QueryTestWhere()
        {
            var ida = new List<int>() { 3, 2 };
            var rep = RepoDict;
            var rst = rep.Where(x => ida.Contains(x.DictID));
            Assert.Equal(2, rst.Count);
        }
        [Fact]
        public void QueryBid()
        {
            string cn = "server=172.16.101.113;User Id=root;password=abc,123;Database=smartbid;CharSet=utf8;";
            var bidSuptbankRepository = new RepositoryMysql<BidSuptbank>(cn);
            var id = 4;
            var res = bidSuptbankRepository.UpdateWhere(x => x.id == id, x => x.is_deleted, 1); //is_deleted �� 1 ���Ͳ�һ��ʱ��֮ǰ�Ǳ���ġ�
        }

        [Fact]
        public void QueryBid2()
        {
            string cn = "server=172.16.101.113;User Id=root;password=abc,123;Database=smartbid;CharSet=utf8;";
            var bidSuptbankRepository = new RepositoryMysql<BidSuptbank>(cn);
            var res = bidSuptbankRepository.Where(x => x.bank_default == false);
        }


        [Fact]
        public void TestWhereAndFind()
        {
            var repository = RepoDict;
            var rst = repository.Where(x => x.DictID > 1);
            var model = repository.Find(2);//Find �Ǹ���������ȡ��¼������������Ҫ�� Key��ǡ�
            Assert.Equal(model.Remark, rst.Find(x => x.DictID == 2)?.Remark);
        }
        [Fact]
        public void TestWhereAndAll()
        {
            var repository = RepoDict;
            var rst = repository.Where(x => x.DictID > 1 && !x.Deleted);
            var model = rst.FirstOrDefault();
            Assert.False(null == model);
            var all = repository.All();
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
            var repository = RepoDict;
            var rst = repository.Where(x => x.CreateTime > new DateTime(2018, 12, 1), x => x.DictID, x => x.DictName);//ֻ��ѯ DictID DictName �����ֶ�
            Assert.False(0 == rst.Count);
            Assert.Equal(0, rst.Count(x => !string.IsNullOrEmpty(x.Remark)));
            var first = rst.First();
            var model = repository.Find(first.DictID);
            Assert.Equal(first.DictName, model.DictName);
        }
        [Fact]
        public void TestWhereSelect()
        {
            var repository = RepoDict;
            var rst = repository.WhereSelect(x => x.DictID >= 3, x => x.DictName);//ͶӰ��ѯ��ֻ��ѯĳЩ�ֶΣ���ͶӰΪ��������
            var rstfull = repository.Where(x => x.DictID >= 3);
            Assert.False(0 == rstfull.Count);
            Assert.Equal(rst.Count, rstfull.Count);
            bool r = rst.Contains(rstfull.First().DictName);
            Assert.True(r);

            var ll2 = repository.WhereSelect(x => x.DictName.Contains("����") && x.Deleted, x => new { x.DictName, x.DictID }).ToDictionary(x => x.DictID, x => x.DictName);
        }
        [Fact]
        public void TestWhereOrderSelect()
        {
            var repository = RepoDict;
            var id = 4;
            //WhereOrderSelect ��ѯ����ͶӰ
            var lrst = repository.WhereOrderSelect(x => x.DictID > id && x.DictName.Contains("����") && x.CreateTime > new DateTime(2018, 1, 1), x => x.DictID, x => new { i1 = x.DictID, i2 = x.DictName });
            bool b = lrst.Count > 0;
            Assert.True(b);
            b = !lrst.First().i2.Contains("����");
            Assert.False(b);
        }
        [Fact]
        public void TestExpression()
        {
            var repository = RepoDict;
            Expression<Func<DictInfo, bool>> expression = x => x.DictID > 4;
            expression = expression.And(x => x.DictName.Contains("����"));
            var rst = repository.Where(expression);
            bool b = rst.Count > 0;
            Assert.True(b);
        }
        [Fact]
        public void TestTimeStamp()
        {
            RepositoryMysql<TestStamp> repoStamp = new RepositoryMysql<TestStamp>(conn);
            var rst = repoStamp.Where(x => true);
            bool b = rst.Any(x => string.IsNullOrEmpty(x.ROWVERSION));
            Assert.False(b);
        }
        [Fact]
        public void TestPageList()
        {
            var repository = RepoDict;
            var rst = repository.PageList(x => x.DictID > 1, x => x.DictID, 1, 2, true);
            bool b = rst.Items.Count == 2;
            Assert.True(b);
            Assert.Equal(rst.Total, repository.Count(x => x.DictID > 1));
        }
        [Fact]
        public void TestTransaction()
        {
            var repository = RepoDict;
            var trans = repository.GetTransaction();//ֻ������ repository ������Ӷ���
            var trepo = trans.GetRepository<DictInfo>();//��������Ĳִ�����
            var trepo2 = trans.GetRepository<TestStamp>();//��������Ĳִ�����
            bool rd = new System.Random().Next(1, 20000) > 10000;//���������ֵ�����������״��벻�״�������
            try
            {
                trepo.UpdateWhere(x => x.DictID == 1, x => x.CreateTime, DateTime.Now);
                var i = 0;
                var t = trepo.Where(x => x.DictID == 2);
                if (rd)
                {
                    var j = 5 / i;
                }
                trepo2.UpdateWhere(x => x.Id == 1, x => x.Name, "�޸ĺ�Ĳ�������");
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
        [Fact]
        public void TestPageListSql()
        {
            var repository = RepoDict;
            var l = repository.PageListBySql("select * from DictInfo", 2, 10);
        }
    }
}
