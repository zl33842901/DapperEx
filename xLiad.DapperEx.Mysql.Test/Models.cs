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




    [Table("bid_suptbank")]
    public class BidSuptbank
    {
        #region 字段
        /// <summary>
        /// 自增字段
        /// </summary>
        [Identity, Key]
        public int id { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        public string bank_name { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string bank_corpname { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        public string bank_accno { get; set; }
        /// <summary>
        /// 开户行信息
        /// </summary>
        public string bank_deposit { get; set; }
        /// <summary>
        /// 机构简称
        /// </summary>
        public string bank_insshort { get; set; }
        /// <summary>
        /// 开户行代码
        /// </summary>
        public string bank_depositcode { get; set; }
        /// <summary>
        /// 开户行地址
        /// </summary>
        public string bank_depositaddr { get; set; }
        /// <summary>
        /// 同城交换号
        /// </summary>
        public string bank_localsno { get; set; }
        /// <summary>
        /// 全国联行号
        /// </summary>
        public string bank_nationlno { get; set; }
        /// <summary>
        /// 银行联行号
        /// </summary>
        public string bank_bankIno { get; set; }
        /// <summary>
        /// 银行代码
        /// </summary>
        public string bank_no { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string bank_detailno { get; set; }
        /// <summary>
        /// 账户类型
        /// </summary>
        public string bank_accounttype { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string bank_remark { get; set; }
        /// <summary>
        /// 进度信息
        /// </summary>
        public string bank_audprogress { get; set; }
        /// <summary>
        /// 最新申请人
        /// </summary>
        public string bank_proposer { get; set; }
        /// <summary>
        /// 最新申请时间
        /// </summary>
        public DateTime bank_aptime { get; set; }
        /// <summary>
        /// 最新更新时间
        /// </summary>
        public DateTime bank_uptime { get; set; }
        /// <summary>
        /// 最新状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 删除标志
        /// </summary>
        public byte is_deleted { get; set; }
        /// <summary>
        /// 是否设置当前银行信息为竞标申请默认选项。默认0否
        /// </summary>
        public byte bank_default { get; set; }
        #endregion
    }
}
