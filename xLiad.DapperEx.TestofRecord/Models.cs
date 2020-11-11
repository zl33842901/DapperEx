using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace xLiad.DapperEx.Repository.Test
{
    [Table("DictInfo")]
    public record DictInfo
    {
        [Identity]
        [Key]
        public int DictID { get; init; }
        public string DictName { get; init; }
        public string Remark { get; init; }
        /// <summary>
        /// NotMapped 不与数据库表发生对应关系
        /// </summary>
        [NotMapped]
        public string nouse { get; init; }
        /// <summary>
        /// 只读属性自动 NotMapped
        /// </summary>
        public string DictName2 => DictName;
        [NoUpdate, AutoDateTimeWhenInsert]
        public DateTime? CreateTime { get; init; }
        public bool Deleted { get; init; }
        /// <summary>
        /// 支持枚举属性
        /// </summary>
        public OrderEnum? OrderNum { get; init; }
        public int? DictType { get; init; }
        /// <summary>
        /// 数组、列表属性 自动 NotMapped
        /// </summary>
        public List<int> TestList { get; init; }
        /// <summary>
        /// 私有读或写属性 自动 NotMapped
        /// </summary>
        public int privatesetAutoNotMapped { get; private set; }
        [NotMapped]
        public string Table { get; init; }
    }
    public enum OrderEnum : int
    {
        optionA = 1,
        optionB = 2
    }
    [Table("DictInfo")]
    public class DictInfo2
    {
        [Identity]
        [Key]
        public int? DictID { get; set; }
        public string DictName { get; set; }
        public OrderEnum OrderNum { get; set; }
        public int DictType { get; set; }
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

    public class Articles
    {
        [Identity]
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int DictID { get; set; }
        [NotMapped]
        public string DictName { get; set; }
    }
    public class Author
    {
        [Identity]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int AId { get; set; }
    }
}
