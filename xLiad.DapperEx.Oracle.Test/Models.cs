using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace xLiad.DapperEx.Oracle.Test
{
    public class Model1 {
        public string DESCR { get; set; }
        public string DESCR1 { get; set; }
        public string COUNTRY { get; set; }
    }
    public enum OrderEnum : int
    {
        optionA = 1,
        optionB = 2
    }
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
    }
    [Table("News2", Schema = "public")]
    public class News2
    {
        [Key]
        [Identity]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        [JsonColumn]
        public Author[] Author { get; set; }
    }
    [Table("News", Schema = "public")]
    public class NewsTest
    {
        [Key]
        [Identity]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        [JsonColumn]
        public Author Author { get; set; }
    }
    [Table("DictInfo", Schema = "public")]
    public class DictInfo
    {
        [Identity]
        [Key]
        public int DictID { get; set; }
        public string DictName { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// NotMapped 不与数据库表发生对应关系
        /// </summary>
        [NotMapped]
        public string nouse { get; set; }
        /// <summary>
        /// 只读属性自动 NotMapped
        /// </summary>
        public string DictName2 => DictName;
        public DateTime CreateTime { get; set; }
        public bool Deleted { get; set; }
        public OrderEnum? OrderNum { get; set; }
        public int? DictType { get; set; }
        /// <summary>
        /// 数组、列表属性 自动 NotMapped  除非有 JsonColumn 特性
        /// </summary>
        public List<int> TestList { get; set; }
        /// <summary>
        /// 私有读或写属性 自动 NotMapped
        /// </summary>
        public int privatesetAutoNotMapped { get; private set; }
        [NotMapped]
        public string Table { get; set; }
    }
}
