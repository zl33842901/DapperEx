using System;
using System.Collections.Generic;
using System.Text;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 代表标识，自增
    /// </summary>
    public class IdentityAttribute : Attribute
    {

    }
    public enum IdentityTypeEnum : byte
    {
        /// <summary>
        /// 无标识
        /// </summary>
        NoIdentity = 0,
        /// <summary>
        /// Guid标识
        /// </summary>
        Guid = 1,
        /// <summary>
        /// 整型标识
        /// </summary>
        Int = 2
    }
}
