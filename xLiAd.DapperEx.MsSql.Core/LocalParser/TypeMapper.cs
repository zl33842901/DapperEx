using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace xLiAd.DapperEx.MsSql.Core.LocalParser
{
    /// <summary>
    /// TypeMapper Interface
    /// </summary>
    public interface ITypeMapper
    {
        MemberInfo FindMember(MemberInfo[] properties, DbDataInfo dataInfo);
        MethodInfo FindConvertMethod(Type csharpType, Type dbType);
        DbDataInfo FindConstructorParameter(DbDataInfo[] dataInfos, ParameterInfo parameterInfo);
        ConstructorInfo FindConstructor(Type csharpType);
    }
    /// <summary>
    /// Default TypeMapper
    /// </summary>
    public class TypeMapper : ITypeMapper
    {
        /// <summary>
        /// Find parametric constructors.
        /// If there is no default constructor, the constructor with the most parameters is returned.
        /// </summary>
        public ConstructorInfo FindConstructor(Type csharpType)
        {
            var constructor = csharpType.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                var constructors = csharpType.GetConstructors();
                constructor = constructors.Where(a => a.GetParameters().Length == constructors.Max(s => s.GetParameters().Length)).FirstOrDefault();
            }
            return constructor;
        }
        /// <summary>
        /// Returns field information based on parameter information
        /// </summary>
        public DbDataInfo FindConstructorParameter(DbDataInfo[] dataInfos, ParameterInfo parameterInfo)
        {
            foreach (var item in dataInfos)
            {
                if (item.DataName.Equals(parameterInfo.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
                else if (//SqlMapper.MatchNamesWithUnderscores && 
                    item.DataName.Replace("_", "").Equals(parameterInfo.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        /// Returns attribute information based on field information
        /// </summary>
        public MemberInfo FindMember(MemberInfo[] properties, DbDataInfo dataInfo)
        {
            foreach (var item in properties)
            {
                if (item.Name.Equals(dataInfo.DataName, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
                else if (//SqlMapper.MatchNamesWithUnderscores && 
                    item.Name.Equals(dataInfo.DataName.Replace("_", ""), StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        /// Return type conversion function.
        /// </summary>
        public MethodInfo FindConvertMethod(Type csharpType, Type dbType)
        {
            bool eq;
            if (GetUnderlyingType(csharpType) == typeof(bool))
            {
                eq = GetUnderlyingType(dbType) == typeof(bool);
                return !IsNullableType(csharpType) ? (eq ? DataConvertMethod.ToBooleanMethod : DataConvertMethod.ToBooleanFromOther) : (eq ? DataConvertMethod.ToBooleanNullableMethod : DataConvertMethod.ToBooleanNullableFromOther);
            }
            if (GetUnderlyingType(csharpType).IsEnum)
            {
                return !IsNullableType(csharpType) ? DataConvertMethod.ToEnumMethod.MakeGenericMethod(csharpType) : DataConvertMethod.ToEnumNullableMethod.MakeGenericMethod(GetUnderlyingType(csharpType));
            }
            if (GetUnderlyingType(csharpType) == typeof(char))
            {
                eq = GetUnderlyingType(dbType) == typeof(char);
                return !IsNullableType(csharpType) ? (eq ? DataConvertMethod.ToCharMethod : DataConvertMethod.ToCharFromOther) : (eq ? DataConvertMethod.ToCharNullableMethod : DataConvertMethod.ToCharNullableFromOther);
            }
            if (csharpType == typeof(string))
            {
                return DataConvertMethod.ToStringMethod;
            }
            if (GetUnderlyingType(csharpType) == typeof(Guid))
            {
                eq = GetUnderlyingType(dbType) == typeof(Guid);
                return !IsNullableType(csharpType) ? (eq ? DataConvertMethod.ToGuidMethod : DataConvertMethod.ToGuidFromOther) : (eq ? DataConvertMethod.ToGuidNullableMethod : DataConvertMethod.ToGuidNullableFromOther);
            }
            if (GetUnderlyingType(csharpType) == typeof(DateTime))
            {
                eq = GetUnderlyingType(dbType) == typeof(DateTime);
                return !IsNullableType(csharpType) ? (eq ? DataConvertMethod.ToDateTimeMethod : DataConvertMethod.ToDateTimeFromOther) : (eq ? DataConvertMethod.ToDateTimeNullableMethod : DataConvertMethod.ToDateTimeNullableFromOther);
            }
            if (GetUnderlyingType(csharpType) == typeof(byte) || GetUnderlyingType(csharpType) == typeof(sbyte))
            {
                eq = GetUnderlyingType(dbType) == typeof(byte);
                return !IsNullableType(csharpType) ? (eq ? DataConvertMethod.ToByteMethod : DataConvertMethod.ToByteFromOther) : (eq ? DataConvertMethod.ToByteNullableMethod : DataConvertMethod.ToByteNullableFromOther);
            }
            if (GetUnderlyingType(csharpType) == typeof(short) || GetUnderlyingType(csharpType) == typeof(ushort))
            {
                eq = GetUnderlyingType(dbType) == typeof(short);
                return !IsNullableType(csharpType) ? (eq ? DataConvertMethod.ToIn16Method : DataConvertMethod.ToIn16FromOther) : (eq ? DataConvertMethod.ToIn16NullableMethod : DataConvertMethod.ToIn16NullableFromOther);
            }
            if (GetUnderlyingType(csharpType) == typeof(int) || GetUnderlyingType(csharpType) == typeof(uint))
            {
                eq = GetUnderlyingType(dbType) == typeof(int);
                return !IsNullableType(csharpType) ? (eq ? DataConvertMethod.ToIn32Method : DataConvertMethod.ToIn32FromOther) : (eq ? DataConvertMethod.ToIn32NullableMethod : DataConvertMethod.ToIn32NullableFromOther);
            }
            if (GetUnderlyingType(csharpType) == typeof(long) || GetUnderlyingType(csharpType) == typeof(ulong))
            {
                eq = GetUnderlyingType(dbType) == typeof(long);
                return !IsNullableType(csharpType) ? (eq ? DataConvertMethod.ToIn64Method : DataConvertMethod.ToIn64FromOther) : (eq ? DataConvertMethod.ToIn64NullableMethod : DataConvertMethod.ToIn64NullableFromOther);
            }
            if (GetUnderlyingType(csharpType) == typeof(float))
            {
                eq = GetUnderlyingType(dbType) == typeof(float);
                return !IsNullableType(csharpType) ? (eq ? DataConvertMethod.ToFloatMethod : DataConvertMethod.ToFloatFromOther) : (eq ? DataConvertMethod.ToFloatNullableMethod : DataConvertMethod.ToFloatNullableFromOther);
            }
            if (GetUnderlyingType(csharpType) == typeof(double))
            {
                eq = GetUnderlyingType(dbType) == typeof(double);
                return !IsNullableType(csharpType) ? (eq ? DataConvertMethod.ToDoubleMethod : DataConvertMethod.ToDoubleFromOther) : (eq ? DataConvertMethod.ToDoubleNullableMethod : DataConvertMethod.ToDoubleNullableFromOther);
            }
            if (GetUnderlyingType(csharpType) == typeof(decimal))
            {
                eq = GetUnderlyingType(dbType) == typeof(decimal);
                return !IsNullableType(csharpType) ? (eq ? DataConvertMethod.ToDecimalMethod : DataConvertMethod.ToDecimalFromOther) : (eq ? DataConvertMethod.ToDecimalNullableMethod : DataConvertMethod.ToDecimalNullableFromOther);
            }
            return !IsNullableType(csharpType) ? DataConvertMethod.ToObjectMethod.MakeGenericMethod(csharpType) : DataConvertMethod.ToObjectNullableMethod.MakeGenericMethod(Nullable.GetUnderlyingType(GetUnderlyingType(csharpType)));
        }
        private Type GetUnderlyingType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            return underlyingType ?? type;
        }
        private bool IsNullableType(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }
    }
    public static class DataConvertMethod
    {
        #region Method Field
        public static MethodInfo ToObjectMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToObject));
        public static MethodInfo ToByteMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToByte));
        public static MethodInfo ToByteFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToByteFromOther));
        public static MethodInfo ToIn16Method = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToInt16));
        public static MethodInfo ToIn16FromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToInt16FromOther));
        public static MethodInfo ToIn32Method = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToInt32));
        public static MethodInfo ToIn32FromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToInt32FromOther));
        public static MethodInfo ToIn64Method = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToInt64));
        public static MethodInfo ToIn64FromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToInt64FromOther));
        public static MethodInfo ToFloatMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToFloat));
        public static MethodInfo ToFloatFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToFloatFromOther));
        public static MethodInfo ToDoubleMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToDouble));
        public static MethodInfo ToDoubleFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToDoubleFromOther));
        public static MethodInfo ToDecimalMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToDecimal));
        public static MethodInfo ToDecimalFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToDecimalFromOther));
        public static MethodInfo ToBooleanMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToBoolean));
        public static MethodInfo ToBooleanFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToBooleanFromOther));
        public static MethodInfo ToCharMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToChar));
        public static MethodInfo ToCharFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToCharFromOther));
        public static MethodInfo ToStringMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToString));
        public static MethodInfo ToDateTimeMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToDateTime));
        public static MethodInfo ToDateTimeFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToDateTimeFromOther));
        public static MethodInfo ToEnumMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToEnum));
        public static MethodInfo ToGuidMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToGuid));
        public static MethodInfo ToGuidFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToGuidFromOther));
        #endregion

        #region NullableMethod Field
        public static MethodInfo ToObjectNullableMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertObjectNullable));
        public static MethodInfo ToByteNullableMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToByteNullable));
        public static MethodInfo ToByteNullableFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToByteNullableFromOther));
        public static MethodInfo ToIn16NullableMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToInt16Nullable));
        public static MethodInfo ToIn16NullableFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToInt16NullableFromOther));
        public static MethodInfo ToIn32NullableMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToInt32Nullable));
        public static MethodInfo ToIn32NullableFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToInt32NullableFromOther));
        public static MethodInfo ToIn64NullableMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToInt64Nullable));
        public static MethodInfo ToIn64NullableFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToInt64NullableFromOther));
        public static MethodInfo ToFloatNullableMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToFloatNullable));
        public static MethodInfo ToFloatNullableFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToFloatNullableFromOther));
        public static MethodInfo ToDoubleNullableMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToDoubleNullable));
        public static MethodInfo ToDoubleNullableFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToDoubleNullableFromOther));
        public static MethodInfo ToBooleanNullableMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToBooleanNullable));
        public static MethodInfo ToBooleanNullableFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToBooleanNullableFromOther));
        public static MethodInfo ToDecimalNullableMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToDecimalNullable));
        public static MethodInfo ToDecimalNullableFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToDecimalNullableFromOther));
        public static MethodInfo ToCharNullableMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToCharNullable));
        public static MethodInfo ToCharNullableFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToCharNullableFromOther));
        public static MethodInfo ToDateTimeNullableMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToDateTimeNullable));
        public static MethodInfo ToDateTimeNullableFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToDateTimeNullableFromOther));
        public static MethodInfo ToEnumNullableMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToEnumNullable));
        public static MethodInfo ToGuidNullableMethod = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToGuidNullable));
        public static MethodInfo ToGuidNullableFromOther = typeof(DataConvertMethod).GetMethod(nameof(DataConvertMethod.ConvertToGuidNullableFromOther));
        #endregion

        #region Define Convert
        public static T ConvertToObject<T>(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            var data = dr.GetValue(i);
            try
            {
                return (T)Convert.ChangeType(data, typeof(T));
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static byte ConvertToByte(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetByte(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static byte ConvertToByteFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToByte(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static short ConvertToInt16(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetInt16(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static short ConvertToInt16FromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToInt16(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static int ConvertToInt32(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetInt32(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static int ConvertToInt32FromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToInt32(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static long ConvertToInt64(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetInt64(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static long ConvertToInt64FromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToInt64(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static float ConvertToFloat(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetFloat(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static float ConvertToFloatFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToSingle(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static double ConvertToDouble(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetDouble(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static double ConvertToDoubleFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToDouble(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static bool ConvertToBoolean(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetBoolean(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static bool ConvertToBooleanFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToBoolean(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static decimal ConvertToDecimal(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetDecimal(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static decimal ConvertToDecimalFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToDecimal(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static char ConvertToChar(this IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetChar(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static char ConvertToCharFromOther(this IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToChar(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static string ConvertToString(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var obj = dr.GetValue(i);
                return obj.ToString();
                //var result = dr.GetString(i);
                //return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static DateTime ConvertToDateTime(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetDateTime(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static DateTime ConvertToDateTimeFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToDateTime(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static T ConvertToEnum<T>(IDataRecord dr, int i) where T : struct
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            var value = dr.GetValue(i);
            if (Enum.TryParse(value.ToString(), out T result)) return result;
            return default;
        }
        public static Guid ConvertToGuid(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetGuid(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static Guid ConvertToGuidFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return (Guid)Convert.ChangeType(data, typeof(Guid));
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        #endregion

        #region Define Nullable Convert
        public static T ConvertObjectNullable<T>(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var data = dr.GetValue(i);
                return (T)Convert.ChangeType(data, typeof(T));
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static byte? ConvertToByteNullable(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetByte(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static byte? ConvertToByteNullableFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToByte(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static short? ConvertToInt16Nullable(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetInt16(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static short? ConvertToInt16NullableFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToInt16(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static int? ConvertToInt32Nullable(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetInt32(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static int? ConvertToInt32NullableFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToInt32(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static long? ConvertToInt64Nullable(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetInt64(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static long? ConvertToInt64NullableFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToInt64(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static float? ConvertToFloatNullable(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetFloat(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static float? ConvertToFloatNullableFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToSingle(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static double? ConvertToDoubleNullable(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetDouble(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static double? ConvertToDoubleNullableFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToDouble(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static bool? ConvertToBooleanNullable(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetBoolean(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static bool? ConvertToBooleanNullableFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToBoolean(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static decimal? ConvertToDecimalNullable(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetDecimal(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static decimal? ConvertToDecimalNullableFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToDecimal(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static char? ConvertToCharNullable(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetChar(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static char? ConvertToCharNullableFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToChar(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static DateTime? ConvertToDateTimeNullable(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetDateTime(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static DateTime? ConvertToDateTimeNullableFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return Convert.ToDateTime(data);
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static T? ConvertToEnumNullable<T>(IDataRecord dr, int i) where T : struct
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            var value = dr.GetValue(i);
            if (Enum.TryParse(value.ToString(), out T result)) return result;
            return default;
        }
        public static Guid? ConvertToGuidNullable(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
            {
                return default;
            }
            try
            {
                var result = dr.GetGuid(i);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        public static Guid? ConvertToGuidNullableFromOther(IDataRecord dr, int i)
        {
            if (dr.IsDBNull(i))
                return default;
            var data = dr.GetValue(i);
            try
            {
                return (Guid)Convert.ChangeType(data, typeof(Guid));
            }
            catch (Exception e)
            {
                throw new Exception("exception column index: " + i + "; name: " + dr.GetName(i), e);
            }
        }
        #endregion
    }
}
