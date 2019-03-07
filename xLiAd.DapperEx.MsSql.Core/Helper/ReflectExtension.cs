using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using xLiAd.DapperEx.MsSql.Core.Core;

namespace xLiAd.DapperEx.MsSql.Core.Helper
{
    internal static class ReflectExtension
    {
        internal static PropertyInfo[] GetProperties(this object obj, bool forSelect)
        {
            return obj.GetType().GetPropertiesInDb(forSelect);
        }
        /// <summary>
        /// 获取某个类型的所有与数据库对应的属性（字段）
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="forSelect">是 Select 语句（而不是insert、update语句）吗？</param>
        /// <returns></returns>
        internal static PropertyInfo[] GetPropertiesInDb(this Type type, bool forSelect)
        {
            var plist = type.GetProperties().Where(x => x.CanRead && x.CanWrite //可读可写
                && x.SetMethod.IsPublic && x.GetMethod.IsPublic //且读写方法对外
                && !Attribute.IsDefined(x, typeof(DatabaseGeneratedAttribute)) //没有标记为计算值、标识位
                && !Attribute.IsDefined(x, typeof(NotMappedAttribute)) //没有标记为不对应字段
                && (!typeof(IList).IsAssignableFrom(x.PropertyType) || Attribute.IsDefined(x, typeof(JsonColumnAttribute))) //不是数组（但标记了Jsonb字段的除外）
                && (!x.PropertyType.IsClass || Attribute.IsDefined(x, typeof(JsonColumnAttribute))) //不是类（但标记了Jsonb字段的除外）
                ).ToArray();
            if (!forSelect)
            {
                ////insert\update 语句不取 timestamp 类型的字段
                plist = plist.Where(x => !Attribute.IsDefined(x, typeof(TimestampAttribute))).ToArray();
            }
            return plist;
        }

        /// <summary>
        /// 获取JsonColumn 标记的属性
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        internal static PropertyInfo[] GetJsonColumnProperty(PropertyInfo[] properties)
        {
            var ps = properties.Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(JsonColumnAttribute))).ToArray();
            return ps;
        }
        /// <summary>
        /// 获取某个类型的字段里，具有 JsonColumn 标记的
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static PropertyInfo[] GetJsonColumnProperty(this Type type)
        {
            return GetJsonColumnProperty(type.GetPropertiesInDb(true));
        }
        /// <summary>
        /// 某个类型是否有 JsonColumn 标记的字段
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool HasJsonColumn(this Type type)
        {
            return type.GetJsonColumnProperty().Length > 0;
        }

        /// <summary>
        /// 获取主键
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static PropertyInfo GetKeyPropertity(this object obj)
        {
            var properties = obj.GetType().GetProperties().Where(a => a.GetCustomAttribute<KeyAttribute>() != null).ToArray();

            if (!properties.Any())
                throw new LambdaExtensionException($"the {nameof(obj)} entity with no KeyAttribute Propertity");

            if (properties.Length > 1)
                throw new LambdaExtensionException($"the {nameof(obj)} entity with greater than one KeyAttribute Propertity");

            return properties.First();
        }

        /// <summary>
        /// 获取主键  这个方法有空应该和上边那个方法合并一下。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static PropertyInfo GetKeyPropertity(this Type type)
        {
            var properties = type.GetProperties().Where(a => a.GetCustomAttribute<KeyAttribute>() != null).ToArray();

            if (!properties.Any())
                throw new LambdaExtensionException($"the {nameof(type)} entity with no KeyAttribute Propertity");

            if (properties.Length > 1)
                throw new LambdaExtensionException($"the {nameof(type)} entity with greater than one KeyAttribute Propertity");

            return properties.First();
        }
    }
}
