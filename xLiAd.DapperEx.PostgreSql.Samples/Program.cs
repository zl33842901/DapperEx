using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using xLiAd.DapperEx.RepositoryPg;

namespace xLiAd.DapperEx.PostgreSql.Samples
{
    [Table("News")]
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
            var l = repository.Where(x => true);
        }
    }
}
