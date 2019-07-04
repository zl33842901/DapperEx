using System;
using System.Collections.Generic;
using System.Text;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 在插入时，有这个标记的属性，当为 null 或 DateTime.MinValue 时，自动更改为默认值
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoDateTimeWhenInsertAttribute : Attribute
    {
        public AutoDateTimeEnum DefaultValue { get; set; } = AutoDateTimeEnum.Now;
    }
    public enum AutoDateTimeEnum
    {
        Now = 0,
        TimeStampMin = 1
    }
    public static class AutoDateTimeValue
    {
        public static DateTime Now => DateTime.Now;
        public static DateTime TimeStampMin => new DateTime(1970, 1, 1, 8, 0, 1);
        public static DateTime[] Values => new DateTime[] { Now, TimeStampMin };
    }
    public static class AutoDateTimeOffsetValue
    {
        public static DateTimeOffset Now => DateTimeOffset.Now;
        public static DateTimeOffset TimeStampMin => new DateTimeOffset(1970, 1, 1, 0, 0, 1, TimeSpan.Zero);
        public static DateTimeOffset[] Values => new DateTimeOffset[] { Now, TimeStampMin };
    }
}
