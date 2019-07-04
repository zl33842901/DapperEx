using System;
using System.Collections.Generic;
using System.Text;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 在 Update(Entity) 方法里，不更新（用来放在CreateTime 等字段上，不影响Update其他重载）
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NoUpdateAttribute : Attribute
    {
    }
}
