using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace 恒温测试机.Utils
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtendUtils
    {

        /// <summary>
        /// 设置某一位的值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index">要设置的位， 值从低到高为 0-7</param>
        /// <param name="flag">要设置的值 true / false</param>
        /// <returns></returns>
        public static void set_bit(ref this byte data, int index, bool flag)
        {
            index++;
            if (index > 8 || index < 1)
                throw new ArgumentOutOfRangeException();
            int v = index < 2 ? index : (2 << (index - 2));
            data = flag ? (byte)(data | v) : (byte)(data & ~v);
        }

        /// <summary>
        /// 获取数据中某一位的值
        /// </summary>
        /// <param name="input">传入的数据类型,可换成其它数据类型,比如Int</param>
        /// <param name="index">要获取的第几位的序号,从0开始 0-7</param>
        /// <returns>返回值为-1表示获取值失败</returns>
        public static int get_bit(this byte input, int index)
        {
            return ((input & (1 << index)) > 0) ? 1 : 0;
        }
    }

    public static class ObjectExtensions
    {
        /// <summary>
        /// 非NULL字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string AsString(this object source)
        {
            if (source == null || source == DBNull.Value)
            {
                return string.Empty;
            }
            return Convert.ToString(source).Trim();
        }

        /// <summary>
        /// 非NULL int
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int AsInt(this object source)
        {
            if (source == null || source == DBNull.Value)
            {
                return 0;
            }
            return Convert.ToInt32(source);
        }

        /// <summary>
        /// 可NULL int
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int? AsIntNullable(this object source)
        {
            if (source == null || source == DBNull.Value)
            {
                return null;
            }
            return Convert.ToInt32(source);
        }
        /// <summary>
        /// 非NULL Long
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static long AsLong(this object source)
        {
            if (source == null || source == DBNull.Value)
            {
                return 0;
            }
            return Convert.ToInt64(source);
        }

        /// <summary>
        /// 非NULL Decimal
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal AsDecimal(this object source)
        {
            if (source == null || source == DBNull.Value)
            {
                return 0;
            }
            return Convert.ToDecimal(source);
        }

        /// <summary>
        /// 非NULL Double
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double AsDouble(this object source)
        {
            if (source == null || source == DBNull.Value)
            {
                return 0;
            }
            return Convert.ToDouble(source);
        }

        /// <summary>
        /// 非NULL bool
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool AsBool(this object source)
        {
            if (source == null || source == DBNull.Value)
            {
                return false;
            }
            return Convert.ToBoolean(source);
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime AsDateTime(this object source)
        {
            if (source == null || source == DBNull.Value || source.AsString().IsNullOrEmpty())
            {
                return default(DateTime);
            }
            return Convert.ToDateTime(source);
        }

        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime? AsDateTimeNullable(this object source)
        {
            if (source == null || source == DBNull.Value)
            {
                return null;
            }
            return Convert.ToDateTime(source);
        }

        /// <summary>
        /// 枚举转换
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T AsEnum<T>(this object source) where T : struct, IConvertible
        {
            var type = typeof(T);
            if (!Enum.IsDefined(type, source.AsInt()))
                throw new Exception(source + "转为枚举类型的" + type.Name + "失败！");

            return (T)Enum.Parse(type, source.AsString());
        }

        public static object DeepClone(this object obj)
        {
            if (obj == null)
                return null;
            using (var memory = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memory, obj);
                memory.Position = 0;
                return binaryFormatter.Deserialize(memory);
            }
        }

        public static T DeepClone<T>(this T sourceObject) where T : class
        {
            if (sourceObject == null)
                return null;
            return DeepClone((object)sourceObject) as T;
        }

        //public static T ConvertType<T>(this object sourceObject) where T : class
        //{
        //    if (sourceObject == null)
        //        return null;
        //    return sourceObject.ToJson().JsonToObject<T>();
        //}

        public static long ObjectSize(this object obj)
        {
            if (obj == null)
                return 0;
            using (var memory = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memory, obj);
                return memory.Length;
            }
        }
        public static bool IsValueNull(this object obj)
        {
            if (obj == null)
                return true;
            if (obj == DBNull.Value)
                return true;
            return string.IsNullOrEmpty(obj.ToString());
        }
        //public static List<TTo> Convert<TFrom, TTo>(this List<TFrom> sourceCollection) where TFrom : TTo
        //{
        //    List<TTo> result = new List<TTo>();
        //    foreach (TFrom sourceObject in sourceCollection)
        //    {
        //        result.Add(sourceObject);
        //    }
        //    return result;
        //}

        //public static string ToBase64(object obj)
        //{
        //    if (obj == null) return string.Empty;
        //    using (MemoryStream memory = new MemoryStream())
        //    {
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        formatter.Serialize(memory, obj);
        //        return Convert.ToBase64String(memory.ToArray());
        //    }
        //}

        //public static T DeserializeFromBase64<T>(string base64String)
        //{
        //    if (string.IsNullOrEmpty(base64String))
        //        return default(T);
        //    using (MemoryStream memory = new MemoryStream(Convert.FromBase64String(base64String)))
        //    {
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        return (T)formatter.Deserialize(memory);
        //    }
        //}


    }

    /// <summary>
    /// 字符串对象扩展
    /// </summary>
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 尝试将当前包含日期时间信息的字符串类型转换成 <see cref="DateTime"/> 类型。转换失败则抛出异常。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string source)
        {
            DateTime result;
            if (!DateTime.TryParse(source, out result))
                throw new Exception(source + "转DateTime类型失败，请检查！");

            return result;
        }

        /// <summary>
        /// 尝试将当前包含日期时间信息的字符串类型转换成 <see cref="DateTime"/> 类型。转换失败则抛出异常，字符串为空则返回NULL。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime? ToDateTimeNullable(this string source)
        {
            if (IsNullOrEmpty(source))
                return null;

            DateTime result;
            if (!DateTime.TryParse(source, out result))
                throw new Exception(source + "转DateTime类型失败，请检查！");

            return result;
        }
        public static DateTime? ToDateTimeNullable(this string source, DateTime? defaultValue)
        {
            DateTime dateTime;
            if (DateTime.TryParse(source, out dateTime))
                return dateTime;

            return defaultValue;
        }

        /// <summary>
        /// 尝试将当前字符串类型转换成 <see cref="bool"/> 。等于 "true" 或等于 "y" 或等于 "t" 返回 true（忽略大小写）。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool ToBool(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return false;

            return source.ToLower() == "true" || source.ToLower() == "y" || source.ToLower() == "t" || source.ToInt(0) > 0;
        }

        /// <summary>
        /// 尝试将当前包含数值信息的字符串类型转换成 <see cref="int"/> 类型，转换失败则抛出异常，字符串为空则返回0。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ToInt(this string source)
        {
            if (IsNullOrEmpty(source))
                return 0;

            int result;
            if (!int.TryParse(source, out result))
                throw new Exception(source + "转Int类型失败，请检查！");

            return result;

        }
        public static int ToInt(this string source, int defaultValue)
        {
            int result;
            if (int.TryParse(source, out result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// 尝试将当前包含数值信息的字符串类型转换成可为空的 <see cref="int"/> 类型，转换失败则抛出异常，字符串为空则返回0。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int? ToIntNullable(this string source)
        {
            if (IsNullOrEmpty(source))
                return null;

            int result;
            if (!int.TryParse(source, out result))
                throw new Exception(source + "转Int类型失败，请检查！");

            return result;
        }
        public static int? ToIntNullable(this string source, int? defaultValue)
        {
            int result;
            if (int.TryParse(source, out result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// 尝试将当前包含数值信息的字符串类型转换成 <see cref="long"/> 类型，转换失败则抛出异常，字符串为空则返回0。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static long ToLong(this string source)
        {
            if (IsNullOrEmpty(source))
                return 0;

            long result;
            if (!long.TryParse(source, out result))
                throw new Exception(source + "转Long类型失败，请检查！");

            return result;
        }
        public static long ToLong(this string source, long defaultValue)
        {
            long result;
            if (long.TryParse(source, out result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// 尝试将当前包含数值信息的字符串类型转换成可为空的 <see cref="long"/> 类型，转换失败则抛出异常，字符串为空则返回NULL。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static long? ToLongNullable(this string source)
        {
            if (IsNullOrEmpty(source))
                return null;

            long result;
            if (!long.TryParse(source, out result) && !IsNullOrEmpty(source))
                throw new Exception(source + "转Long类型失败，请检查！");

            return result;
        }
        public static long? ToLongNullable(this string source, long? defaultValue)
        {
            long result;
            if (long.TryParse(source, out result))
                return result;

            return defaultValue;
        }

        public static Guid ToGuid(this string source)
        {
            if (string.IsNullOrEmpty(source))
                throw new Exception("空字符不能转为Guid！");
            Guid result;
            if (Guid.TryParse(source, out result))
            {
                return result;
            }
            throw new Exception(string.Concat("GUID “", source, "”不是标准的Guid格式字符串！"));
        }

        /// <summary>
        /// 尝试将当前包含数值信息的字符串类型转换成 <see cref="Decimal"/> 类型。转换失败则抛出异常。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string source)
        {
            if (IsNullOrEmpty(source))
                return 0;

            //对于末尾为.的自动补0
            if (source.Last() == '.')
            {
                source += "0";
            }
            decimal result;
            if (!decimal.TryParse(source, out result))
                throw new Exception(source + "转Decimal类型失败，请检查！");

            return result;
        }
        public static decimal ToDecimal(this string source, decimal defaultValue)
        {
            decimal result;
            if (decimal.TryParse(source, out result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// 尝试将当前包含数值信息的字符串类型转换成可为空的 <see cref="Decimal"/> 类型。转换失败则抛出异常，字符串为空则返回NULL。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal? ToDecimalNullable(this string source)
        {
            if (IsNullOrEmpty(source))
                return null;

            decimal result;
            if (!decimal.TryParse(source, out result))
                throw new Exception(source + "转Decimal类型失败，请检查！");

            return result;
        }
        public static decimal? ToDecimalNullable(this string source, decimal? defaultValue)
        {
            decimal result;
            if (decimal.TryParse(source, out result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// 尝试将当前包含数值信息的字符串类型转换成 <see cref="Double"/> 类型。转换失败则抛出异常。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double ToDouble(this string source)
        {
            if (IsNullOrEmpty(source))
                return 0;

            double result;
            if (!double.TryParse(source, out result))
                throw new Exception(source + "转Double类型失败，请检查！");

            return result;
        }
        public static double ToDouble(this string source, Double defaultValue)
        {
            double result;
            if (double.TryParse(source, out result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// 尝试将当前包含数值信息的字符串类型转换成 <see cref="TimeSpan"/> 类型。转换失败则抛出异常。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this string source)
        {
            return TimeSpan.Parse(source);
        }

        /// <summary>
        /// 当字符串超过指定长度时，在指定长度处加上换行符，使字符串换行。
        /// </summary>
        /// <returns></returns>
        public static string ToLineFeed(this string source, int length)
        {
            source = source.TrimEnd();
            if (source.Length <= length)
                return source;
            var time = source.Length / length;
            for (int i = 1; i <= time; i++)
            {
                var insertLength = i * length + (i - 1) * 4;
                source = source.Insert(insertLength, "\r\n");
            }
            return source;
        }

        /// <summary>
        /// 银行卡号密文处理。
        /// </summary>
        /// <returns></returns>
        public static string ToSecretBankCardNo(this string source)
        {
            if (string.IsNullOrWhiteSpace(source) || (source.Length < 15))
                return source;
            var head = source.Substring(0, 6);
            var end = source.Substring(source.Length - 4, 4);
            return head + "********" + end;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string PhoneEncryption(this string source)
        {
            if (string.IsNullOrWhiteSpace(source) || (source.Length < 7))
                return source;
            var head = source.Substring(0, 3);
            var end = source.Substring(7, source.Length - 7);
            return head + "****" + end;
        }

        /// <summary>
        /// 按指定长度(单字节)截取字符串
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="len">截取字节数</param>
        /// <returns>string</returns>
        public static string SubstringByByte(this string str, int startIndex, int len)
        {
            var encoding = Encoding.GetEncoding("GB2312");
            byte[] bytes = encoding.GetBytes(str);
            return encoding.GetString(bytes, startIndex, len);

        }

        public static string ToTrim(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return "";
            return source.Trim();
        }

        public static string ToSqlParams(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return "";
            return source.Replace("'", "''");
        }

        /// <summary>
        /// 转为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultEnum">转换失败默认值</param>
        /// <returns></returns>
        public static T StringToEnum<T>(this string name, T defaultEnum)
        {
            if (string.IsNullOrEmpty(name))
            {
                return defaultEnum;
            }
            Type MyEnum = typeof(T);
            object o = null;
            var sn = Enum.GetNames(MyEnum);
            name = name.ToLower();
            foreach (var statusName in sn)
            {
                if (statusName.ToLower() == name)
                {
                    o = Enum.Parse(MyEnum, statusName);
                    break;
                }
            }
            if (o == null)
                o = defaultEnum;
            return (T)o;
        }

        /// <summary>
        /// 字符串转为枚举
        /// 注意与ToEnum的区别
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T StringToEnum<T>(this string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return default(T);
            }
            Type MyEnum = typeof(T);
            object o = null;
            var sn = Enum.GetNames(MyEnum);
            name = name.ToLower();
            foreach (var statusName in sn)
            {
                if (statusName.ToLower() == name)
                {
                    o = Enum.Parse(MyEnum, statusName);
                    break;
                }
            }
            if (o == null)
                o = default(T);
            return (T)o;
        }

        ///// <summary>
        ///// 枚举的键字符串转化为枚举
        ///// 值为"0"/"1"/"2"等数值型字符串
        ///// </summary>
        //public static T ToEnum<T>(this string source)
        //{
        //    return source.ToInt().ToEnum<T>();
        //}

        public static bool IsValidHourMinuteFormat(this string str)
        {
            if (string.IsNullOrEmpty(str.Trim()))
                return false;
            return Regex.IsMatch(str, "^([01][0-9]|2[0-3]):([0-5][0-9])$|([01][0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])$");
        }

        public static List<long> ToLongList(this string str, string fix = ",")
        {
            List<long> list = new List<long>();
            var arr = str.Split(new string[] { fix }, StringSplitOptions.RemoveEmptyEntries);
            long l;
            foreach (var a in arr)
            {
                long.TryParse(a, out l);
                list.Add(l);
            }
            return list;
        }
        public static List<int> ToIntList(this string str, string fix = ",")
        {
            List<int> list = new List<int>();
            var arr = str.Split(new string[] { fix }, StringSplitOptions.RemoveEmptyEntries);
            int l;
            foreach (var a in arr)
            {
                int.TryParse(a, out l);
                list.Add(l);
            }
            return list;
        }
        public static List<DateTime> ToDateTimeList(this string str, string fix = ",")
        {
            List<DateTime> list = new List<DateTime>();
            var arr = str.Split(new string[] { fix }, StringSplitOptions.RemoveEmptyEntries);
            DateTime l;
            foreach (var a in arr)
            {
                DateTime.TryParse(a, out l);
                list.Add(l);
            }
            return list;
        }
        public static string ToJoinString<T>(this List<T> list, string fix = ",")
        {
            return string.Join(fix, list);
        }

        //加密显示
        public static string SensitiveEncrypt(this string str)
        {
            str = str.Replace("\n", "");
            var count = str.Length - 16;
            if (count <= 0)
                return str;

            var newStr = "";
            newStr += str.Substring(0, 8);
            newStr += new string('*', count);
            newStr += str.Substring(str.Length - 8, 8);
            return newStr;
        }

        public static bool IsValidPhoneTelNumberFormat(this string str)
        {
            if (string.IsNullOrEmpty(str.Trim()))
                return false;
            return Regex.IsMatch(str, @"^(\d{7,8})|(\d{11})|(\d{7,8}\-\d{3,4})");
        }
        public static bool IsValidNumberFormat(this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str.Trim()))
                return false;
            if (str.Length > maxLength)
                return false;
            return Regex.IsMatch(str, @"^\d*$");
        }
        public static bool IsValidEnglishNumberFormat(this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str.Trim()))
                return false;
            if (str.Length > maxLength)
                return false;
            return Regex.IsMatch(str, @"[A-Za-z0-9]{1," + maxLength + "}");
        }

        /// <summary>
        /// 截取指定长度字符串
        /// </summary>
        public static string Truncate(this string str, int len)
        {
            if (string.IsNullOrEmpty(str) || str.Length <= len)
                return str;
            return str.Substring(0, len);

        }
    }
}
