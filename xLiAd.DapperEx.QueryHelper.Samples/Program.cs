using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using xLiAd.DapperEx.Repository;

namespace xLiAd.DapperEx.QueryHelper.Samples
{
    public class DictInfo
    {
        //[Identity]
        [Key]
        public int DictID { get; set; }
        public string DictName { get; set; }
        public string Remark { get; set; }
        [NotMapped]
        public string nouse { get; set; }
        public string DictName2 => DictName;
        public DateTime CreateTime { get; set; }
        public bool Deleted { get; set; }
        public int OrderNum { get; set; }
        public int? DictType { get; set; }
        public List<int> TestList { get; set; }
        public int tttttttt { get; private set; }
    }
    public class Articles
    {
        [Identity]
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int DictID { get; set; }
        [NotMapped]
        public string bbbb { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var con = new SqlConnection("Data Source=127.0.0.1;Initial Catalog=zhanglei;Persist Security Info=True;User ID=sa;Password=feih#rj87");
            Repository<DictInfo> repository = new Repository<DictInfo>(con, null);

            var q = new QueryParamProvider<DictInfo>();
            q.AddItem(x => x.DictName, QueryParamProviderOprater.Contains);
            q.AddItem(x => x.DictID, QueryParamProviderOprater.LessThanOrEqual, null, null);
            q.AddItem(x => x.CreateTime, QueryParamProviderOprater.GreaterThanOrEqual, null, "startTime");
            q.AddItem(x => x.CreateTime, QueryParamProviderOprater.LessThan, null, "endTime");
            var nv = new System.Collections.Specialized.NameValueCollection();
            //nv.Add("DictName", "");
            //nv.Add("DictID", "106071");
            //nv.Add("startTime", "2018-12-1");
            //nv.Add("endTime", "2019-2-1");
            var ee = q.GetExpression(nv);

            //var l = repository.Where(ee);



            Repository<Articles> repos = new Repository<Articles>(con, null);
            QueryParamJoiner<Articles> qq = new QueryParamJoiner<Articles>();
            qq.AddItem<DictInfo, int>(repository, x => x.DictName, QueryParamJoinerOprater.Contains, x => x.DictID, x => x.DictID);
            var nv2 = new System.Collections.Specialized.NameValueCollection();
            nv2.Add("DictName", "技术副");
            var aa = qq.GetExpression(nv2);
            var l2 = repos.Where(aa);

            l2.LeftJoin(repository, x => x.DictID, x => x.DictID, (x, y) => { x.bbbb = y.DictName; }, out var _, x=>x.DictName);
        }
    }
}
