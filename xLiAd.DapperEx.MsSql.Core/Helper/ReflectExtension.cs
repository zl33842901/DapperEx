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
        internal static PropertyInfo[] GetProperties(this object obj)
        {
            return obj.GetType().GetPropertiesInDb();
        }
        internal static PropertyInfo[] GetPropertiesInDb(this Type type)
        {
            var plist = type.GetProperties().Where(x => x.CanRead && x.CanWrite &&
                !Attribute.IsDefined(x, typeof(DatabaseGeneratedAttribute)) && !Attribute.IsDefined(x, typeof(NotMappedAttribute))
                && !typeof(IList).IsAssignableFrom(x.PropertyType)
                ).ToArray();
            return plist;
        }

        internal static PropertyInfo GetKeyPropertity(this object obj)
        {
            var properties = obj.GetType().GetProperties().Where(a => a.GetCustomAttribute<KeyAttribute>() != null).ToArray();

            if (!properties.Any())
                throw new LambdaExtensionException($"the {nameof(obj)} entity with no KeyAttribute Propertity");

            if (properties.Length > 1)
                throw new LambdaExtensionException($"the {nameof(obj)} entity with greater than one KeyAttribute Propertity");

            return properties.First();
        }
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
