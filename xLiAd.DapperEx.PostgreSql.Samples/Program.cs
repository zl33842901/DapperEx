using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using xLiAd.DapperEx.RepositoryPg;

namespace xLiAd.DapperEx.PostgreSql.Samples
{
    [Table("News", Schema="public")]
    public class TTTTTTtest
    {
        [Key]
        [Identity]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var constring = "Host=127.0.0.1;Port=5432;Database=zhanglei;Username=postgres;Password=zhanglei";
            RepositoryPg<TTTTTTtest> repository = new RepositoryPg<TTTTTTtest>(constring);

            TTTTTTtest ttest = new TTTTTTtest()
            {
                Content = "小喇叭开始广播啦",
                Title = "哒滴滴哒滴滴"
            };
            var iiid = repository.Add(ttest);

            var l = repository.Where(x => true);
            var r225 = repository.WhereSelect(x => x.Id >= 3, x => x.Title);
            var id = 1;
            var r3 = repository.Find(id);
        }
    }
}
