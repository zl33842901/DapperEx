﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using xLiAd.DapperEx.Repository;
using System.Linq.Expressions;
using System.Linq;
using xLiAd.DapperEx.MsSql.Core.Helper;
using System.Collections.Generic;
using xLiAd.DapperEx.MsSql.Core.Core;

namespace xLiAd.DapperEx.MsSql.Core.Samples
{
    public enum OrderEnum : int
    {
        optionA = 1,
        optionB = 2
    }
    public class DictInfo
    {
        [Identity]
        [Key]
        public int? DictID { get; set; }
        public string DictName { get; set; }
        public string Remark { get; set; }
        [NotMapped]
        public string nouse { get; set; }
        public string DictName2 => DictName;
        public DateTime CreateTime { get; set; }
        public bool Deleted { get; set; }
        public OrderEnum? OrderNum { get; set; }
        public int? DictType { get; set; }
        public List<int> TestList { get; set; }
        public int tttttttt { get; private set; }
        [NotMapped]
        public string Table { get; set; }
    }
    [Table("Articles")]
    public class TTTTTTtest
    {
        [Key]
        [Identity]
        public int Id { get; set; }
        public string title { get; set; }
        public int DictID { get; set; }
    }
    public class TestStamp
    {
        [Key]
        [Identity]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 这是一个 timestamp 字段
        /// </summary>
        [Timestamp]
        public string ROWVERSION { get; set; }
    }
    /// <summary>
    /// 本 ORM 优势：
    /// 0，使用表达式写条件、排序 等能提高工作效率（避免字段名称写错，简化语法。）
    /// 1，支持 Select 出部分字段匿名类型
    /// 2，支持部分字段更新
    /// 3，支持根据条件更新、删除
    /// 4，支持枚举字段检索
    /// 5，支持事务
    /// </summary>
    class Program
    {
        private static void Log(object sender, DapperExEventArgs eventArgs)
        {
            Console.Write(eventArgs.ToString());
        }
        static void Main(string[] args)
        {
            //连接字符串
            var con = new SqlConnection("Data Source=127.0.0.1;Initial Catalog=zhanglei;Persist Security Info=True;User ID=sa;Password=zhanglei");
            #region 需要使用类似 mybatis 的XML方式存SQL语句时，需要下边这个对象，否则不需要。
            var xmlPath = System.IO.Directory.GetCurrentDirectory() + "\\sql.xml";
            RepoXmlProvider repoXmlProvider = new RepoXmlProvider(xmlPath);
            #endregion
            Repository<DictInfo> repository = new Repository<DictInfo>(con, repoXmlProvider, Log, true);
            Repository<TTTTTTtest> rep2 = new Repository<TTTTTTtest>(con);

            //timestamp 字段测试
            //Repository<TestStamp> repoStamp = new Repository<TestStamp>(con);
            //repoStamp.Add(new TestStamp()
            //{
            //    CreateTime = DateTime.Now,
            //    Name = "我我我asdf",
            //    ROWVERSION = "aaaa"
            //});
            //var feijfwo = repoStamp.Where(x => x.Name.Contains("我"));
            //return;

            #region 增
            //var mmmmm = new TTTTTTtest() { title = "sdfijwefi" };
            //var iiiis = rep2.Add(mmmmm);
            //Console.Write(iiiis);
            ////当类有标识字段(Identity特性)时，返回标识ID；否则返回影响行数
            //var idd = repository.Add(new DictInfo() { DictID = 126119, DictName = "哇哈哈", CreateTime = DateTime.Now });
            //var count = repository.AddTrans(new List<DictInfo>() {
            //    new DictInfo() { DictName = "康师傅5", CreateTime = DateTime.Now },
            //    new DictInfo() { DictName = "康师傅6", CreateTime = DateTime.Now }
            //});
            //return;
            #endregion

            #region 删
            //这种方式需要类定义主键字段(Key特性)
            //var r = repository.Delete(1234);
            //var rdf1 = repository.Delete(x => x.DictID > 106093 && x.DictName == "哈哈哈");
            //var eifwjo = repository.DeleteTrans(new int[] { 106097, 106098 });
            #endregion

            #region 改
            //var trans = repository.GetTransaction();
            //var trepo = trans.GetRepository<DictInfo>();
            //var trepo2 = trans.GetRepository<TTTTTTtest>();
            //try
            //{
            //    trepo.UpdateWhere(x => x.DictID == 100018, x => x.CreateTime, DateTime.Now);
            //    var i = 0;
            //    var tijwoeifj = trepo.Where(x => x.DictID == 100018); //如果要使用
            //    //var j = 5 / i;
            //    trepo2.UpdateWhere(x => x.Id == 3, x => x.title, "个人资料类别222");
            //    trans.Commit();
            //}
            //catch (Exception eeeee)
            //{
            //    trans.Rollback();
            //}
            //var ddxx = new DictInfo() { DictID = 100020, CreateTime = DateTime.Now, DictType = 101, DictName = "某层某层会议室", Remark = "今天交流的内容是DapperEx使用", Deleted = true, OrderNum = OrderEnum.optionA };
            //var r443 = repository.Update(ddxx); //
            //var r44 = repository.Update(ddxx, x => x.Remark,x=>x.CreateTime);
            //var r1 = repository.UpdateWhere(x => x.DictID == 102098 && (x.DictName == "哈哈哈" || x.DictName == "啦啦"), x => x.Remark, "更新后的备注");
            //r1 = repository.UpdateWhere(x => x.DictID > 102099 && x.DictID < 104066, new DictInfo() { DictName = "哈哈哈", OrderNum = OrderEnum.optionB }, x => x.DictName, x => x.OrderNum);

            //var eifjwoeif = repository.UpdateTrans(new DictInfo[]
            //{
            //    new DictInfo()
            //    {
            //        DictID = 106084, CreateTime = DateTime.Now, Deleted = true, DictName = "老总", DictType = 3, OrderNum = OrderEnum.optionB, Remark = "老总，请加薪"
            //    },
            //    new DictInfo()
            //    {
            //        DictID = 106086, CreateTime = DateTime.MinValue, Deleted = false, DictName = "老总秘书", DictType = 2, OrderNum = OrderEnum.optionA, Remark = "老总秘书，请加薪"
            //    }
            //});
            #endregion

            #region 查
            var id = 106071;
            var idaaa = new List<int?>() { 106071, 106072 };
            var idbbb = Enumerable.Range(106071, 2200).Select(x=> new int?(x)).ToArray();
            ////总数
            //var ccc = repository.CountAll;
            ////数量
            var aaa = 106071;
            var bbb = "哈哈哈";
            var ccc = OrderEnum.optionA;
            //var r223 = repository.Where(x => x.DictID == 106071 && idbbb.Contains(x.DictID));
            var r225 = repository.WhereSelect(x => x.DictID >= 106071, x => x.DictName);
            var r228 = repository.Where(x => x.DictID >= 106071);
            return;
            //var r224 = repository.All();
            //var r2 = repository.Where(x => x.DictID == aaa);
            //return;
            ////主键获取记录
            //var r3 = repository.Find(id);
            ////普通查询
            //var idfjso = repository.Where(x => !x.Deleted);
            //var enumList = new int?[] { 104, 102 };
            //var ll4 = repository.Where(x => enumList.Contains(x.DictType));
            ////表达式拼接
            //Expression<Func<DictInfo, bool>> expression = x => x.DictID > id;
            //expression = expression.And(x => x.DictName.Contains("总监"));
            //var zzz = repository.Where(expression);
            ////枚举字段查询
            //var lll = repository.Where(x => x.Deleted == true && x.OrderNum == OrderEnum.optionA);
            ////查询并投影
            //var ll2 = repository.WhereSelect(x => x.DictName.Contains("老总") && x.Deleted, x => new { x.DictName, x.DictID }).ToDictionary(x => x.DictID, x => x.DictName);
            //var lrst = repository.WhereOrderSelect(x => x.DictID > id && x.DictName.Contains("总监") && x.CreateTime < new DateTime(2018, 1, 1), x => x.DictID, x => new { i1 = x.DictID, i2 = x.DictName });
            //查询实体方式
            //var dflkjsdf = repository.Where(new QueryDict() { Name = "技术副总裁", startdate = new DateTime(2016,1,1) }.Expression);
            //var feifjwo = repository.Where(x => x.CreateTime > new DateTime(2018, 12, 1), x => x.DictID, x => x.DictName);

            //var kk = repository.Where(x => x.DictID > 5555, x => x.DictID);
            //var jsldkfjsl = repository.PageList(x => x.CreateTime > new DateTime(2018, 12, 1), 1, 10, new Tuple<Expression<Func<DictInfo, object>>, SortOrder>(x => x.Deleted, SortOrder.Descending), new Tuple<Expression<Func<DictInfo, object>>, SortOrder>(x => x.DictID, SortOrder.Descending));

            //repository.PageList(x => x.DictID > 5555, x => x.DictID, 1, 10, true);
            //return;
            #endregion

            #region 分页查询

            var l = repository.PageList(x => x.DictID >= id, x => x.DictID, 1, 5, true);
            #endregion

            #region 执行XML
            //var lbfs = repository.ExecuteXml("DictSelect");
            //var rsss = repository.ExecuteXml("deleteUser", new Dictionary<string, string>() { { "id", "4321" } });
            //var oiehfwie = repository.QueryXml<DictInfo>("DictSelect");

            var rrrr = repository.QueryBySql<string>("select top 1 dictid from dictinfo");
            #endregion

            #region 事务
            //var repo = new RepDict(con);
            //repo.DoSomething();
            #endregion
        }
    }
    public class RepDict : Repository<DictInfo>
    {
        public RepDict(string connectionString) : base(connectionString)
        {
        }

        public RepDict(SqlConnection _con) : base(_con)
        {
        }
        public int DoSomething()
        {
            con.Transaction(tc =>
            {
                tc.CommandSet<DictInfo>().Delete(106089);
                tc.CommandSet<DictInfo>().Where(a => a.DictID == 106085).Delete();
                tc.CommandSet<DictInfo>().Insert(new DictInfo
                {
                    DictName = "哇哈哈",
                    CreateTime = DateTime.Now
                });
            });
            return 1;
        }
    }

    public class QueryDict
    {
        public string Name { get; set; }
        public string Bak { get; set; }
        public bool? delete { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public Expression<Func<DictInfo, bool>> Expression
        {
            get
            {
                Expression<Func<DictInfo, bool>> e = null;
                if (!string.IsNullOrWhiteSpace(Name))
                    e = e.And(x => x.DictName.Contains(Name));
                if (!string.IsNullOrWhiteSpace(Bak))
                    e = e.And(x => x.Remark.Contains(Bak));
                if (delete != null)
                    e = e.And(x => x.Deleted == delete);
                if (startdate != null)
                    e = e.And(x => x.CreateTime > startdate.Value);
                if (enddate != null)
                    e = e.And(x => x.CreateTime < enddate.Value);
                return e;
            }
        }
    }
}
