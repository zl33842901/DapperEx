using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace xLiad.DapperEx.Mysql.Test
{
    [Table("DictInfo")]
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
        [NoUpdate]
        public DateTime CreateTime { get; set; }
        public bool Deleted { get; set; }
        /// <summary>
        /// 支持枚举属性
        /// </summary>
        public OrderEnum? OrderNum { get; set; }
        public int? DictType { get; set; }
        /// <summary>
        /// 数组、列表属性 自动 NotMapped
        /// </summary>
        public List<int> TestList { get; set; }
        /// <summary>
        /// 私有读或写属性 自动 NotMapped
        /// </summary>
        public int privatesetAutoNotMapped { get; private set; }
        [NotMapped]
        public string Table { get; set; }
    }
    public enum OrderEnum : int
    {
        optionA = 1,
        optionB = 2
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

    public class TestTimeStamp
    {
        [Key]
        [Identity]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }
    [Table("TestTimeStamp")]
    public class TestTimeStamp2
    {
        [Key]
        [Identity]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
