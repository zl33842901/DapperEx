using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using xLiAd.DapperEx.Repository;
using System.Linq.Expressions;
using System.Linq;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace xLiAd.DapperEx.MsSql.Core.Samples
{
    public enum OrderEnum
    {
        asdfasdf = 1,
        ggggjkl = 2
    }
    public class DictInfo
    {
        [Identity]
        [Key]
        public int DictID { get; set; }
        public string DictName { get; set; }
        public string Remark { get; set; }
        [NotMapped]
        public string asdfasdf { get; set; }
        public string DictName2 => DictName;
        public DateTime CreateTime { get; set; }
        public bool Deleted { get; set; }
        public OrderEnum OrderNum { get; set; }
        public int? DictType { get; set; }
    }
    /// <summary>
    /// 本框架部分源码取自于 Sikiro.DapperLambdaExtension.MsSql 在此对作者 陈珙 致敬。
    /// 本 ORM 优势：
    /// 1，支持 Select 出部分字段匿名类型
    /// 2，支持部分字段更新
    /// 3，支持根据条件更新、删除
    /// 4，支持枚举字段检索
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var con = new SqlConnection(
                " Data Source=127.0.0.1;Initial Catalog=zhanglei;Persist Security Info=True;User ID=sa;Password=feih#rj87");

            RepDict repository = new RepDict(con);
            repository.DoSomething();

            var id = 106071;
            var enumList = new int?[] { 104, 102 };
            var ll4 = repository.Where(x => enumList.Contains(x.DictType));

            Expression<Func<DictInfo, bool>> expression = x=>x.DictID > id;
            expression = expression.And(x => x.DictName.Contains("总监"));
            var zzz = repository.Where(expression);
            var lll = repository.Where(x => x.Deleted == true && x.OrderNum == OrderEnum.asdfasdf);
            var ll2 = repository.Where(x => x.DictName.Contains("总监") && x.Deleted);
            var ll = repository.Where(x => x.Deleted == true && x.Deleted);
            var llll = repository.Where(x => !x.Deleted);
            var lllll = repository.Where(x => true);
            var llllll = repository.Where(x => false);
            var lrst = repository.WhereOrderSelect(x => x.DictID > id && x.DictName.Contains("总监") && x.CreateTime < new DateTime(2018,1,1), x => x.DictID, x => new { i1 = x.DictID, i2 = x.DictName });


            //var idd = repository.Add(new DictInfo() { DictID = 9999, DictName = "哇哈哈" });

            var r = repository.Delete(9999);
            var l = repository.PageList(x => x.DictID >= id, x => x.DictID, 1, 5, true);
            l.Items[0].Remark = "修改个备注111";
            r = repository.Update(l.Items.First(), x=>x.Remark);

            var r1 = repository.UpdateWhere(x => x.DictID == id, x => x.Remark, "哈哈哈哈");
            r1 = repository.Delete(x => x.DictID == 65656);

            var r2 = repository.Count(x=>id < x.DictID);

            var r3 = repository.Find(id);
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
                tc.CommandSet<DictInfo>().Delete(9999);
                tc.CommandSet<DictInfo>().Where(a => a.DictID == 106085).Delete();
                tc.CommandSet<DictInfo>().Insert(new DictInfo
                {
                    DictID = 9999,
                    DictName = "哇哈哈",
                    CreateTime = DateTime.Now
                });
            });
            con.Dispose();
            return 1;
        }
    }
}
