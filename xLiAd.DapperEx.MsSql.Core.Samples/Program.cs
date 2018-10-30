using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using xLiAd.DapperEx.Repository;
using System.Linq.Expressions;

namespace xLiAd.DapperEx.MsSql.Core.Samples
{
    public class DictInfo
    {
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
            var lrst = repository.WhereOrderSelect(x => x.DictID > id, x => x.DictID, x => new { x.DictID, x.DictName });
        }
    }
}
