using System;
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


        }
    }
}
