using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using xLiAd.DapperEx.Repository;
using System.Linq.Expressions;
using System.Linq;

namespace xLiAd.DapperEx.MsSql.Core.Samples
{
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
    }
    class Program
    {
        static void Main(string[] args)
        {
            var con = new SqlConnection(
                " Data Source=127.0.0.1;Initial Catalog=zhanglei;Persist Security Info=True;User ID=sa;Password=feih#rj87");

            Repository<DictInfo> repository = new Repository<DictInfo>(con);

            var id = 106071;
            var lrst = repository.WhereOrderSelect(x => x.DictID > id, x => x.DictID, x => new { i1 = x.DictID, i2 = x.DictName });


            //var idd = repository.Add(new DictInfo() { DictID = 9999, DictName = "哇哈哈" });

            //var r = repository.Delete(9999);
            //var ll = repository.PageList(x => x.DictID >= id, x => x.DictID, 1, 5, true);
            //ll.Items[0].Remark = "修改个备注111";
            //r = repository.Update(ll.Items.First(), x=>x.Remark);

            //var r1 = repository.UpdateWhere(x => x.DictID == id, x => x.Remark, "哈哈哈哈");
            //r1 = repository.Delete(x => x.DictID == 65656);

            var r2 = repository.Count(x=>id < x.DictID);

            var r3 = repository.Find(id);
        }
    }
}
