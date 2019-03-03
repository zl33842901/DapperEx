using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using xLiAd.DapperEx.RepositoryPg;

namespace xLiAd.DapperEx.PostgreSql.Samples
{
    public enum OrderEnum : int
    {
        optionA = 1,
        optionB = 2
    }
    [Table("News", Schema="public")]
    public class TTTTTTtest
    {
        [Key]
        [Identity]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
    [Table("DictInfo", Schema = "public")]
    public class DictInfo
    {
        //[Identity]
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
    class Program
    {
        static void Main(string[] args)
        {
            var constring = "Host=127.0.0.1;Port=5432;Database=zhanglei;Username=postgres;Password=zhanglei";
            RepositoryPg<TTTTTTtest> repository = new RepositoryPg<TTTTTTtest>(constring);
            RepositoryPg<DictInfo> repod = new RepositoryPg<DictInfo>(constring);
            //TTTTTTtest ttest = new TTTTTTtest()
            //{
            //    Content = "小喇叭开始广播啦",
            //    Title = "哒滴滴哒滴滴"
            //};
            //var iiid = repository.Add(ttest);

            //var l = repository.Where(x => true);
            //var r225 = repository.WhereSelect(x => x.Id >= 3, x => x.Title);
            //var id = 1;
            //var r3 = repository.Find(id);

            //var r = repository.Delete(2);

            //var ddxx = new TTTTTTtest() { Id = 6, Title = "更新后的标题2", Content="更新后的内容" };
            //var r443 = repository.Update(ddxx, x=>x.Content);

            //var l = repository.PageList(x => x.Id > 1, x=>x.Id , 2, 3, true);
            var trans = repository.GetTransaction();
            var trepo = trans.GetRepository<DictInfo>();
            var trepo2 = trans.GetRepository<TTTTTTtest>();
            try
            {
                trepo.UpdateWhere(x => x.DictID == 2, x => x.CreateTime, DateTime.Now);
                var i = 0;
                var tijwoeifj = trepo.Where(x => x.DictID == 3); //如果要使用
                //var j = 5 / i;
                trepo2.UpdateWhere(x => x.Id == 3, x => x.Title, "个人资料类别");
                trans.Commit();
            }
            catch (Exception eeeee)
            {
                trans.Rollback();
            }
        }
    }
}
