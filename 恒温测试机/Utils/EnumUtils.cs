using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace 恒温测试机.Utils
{
    /// <summary>
    /// 枚举说明属性
    /// </summary>
    public class EnumDescriptionAttribute : Attribute
    {
        public string Text { get; private set; }

        public EnumDescriptionAttribute(string text)
        {
            Text = text;
        }
    }
    /// <summary>
    /// Enum对象扩展
    /// </summary>
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum enumeration)
        {
            var type = enumeration.GetType();
            MemberInfo[] memInfo = type.GetMember(enumeration.ToString());

            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                if (attrs.Length > 0)
                    return ((EnumDescriptionAttribute)attrs[0]).Text;
            }
            return enumeration.ToString();
        }

        public static int? GetValueByDesc<T>(this string desc)
        {
            if (!(typeof(T).BaseType == typeof(Enum)))
                return null;

            foreach (var key in Enum.GetNames(typeof(T)))
            {
                var type = typeof(T);
                MemberInfo[] memInfo = type.GetMember(key);

                if (memInfo.Length > 0)
                {
                    object[] attrs = memInfo[0].GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                    if (attrs.Length > 0 && ((EnumDescriptionAttribute)attrs[0]).Text == desc)
                    {
                        return (int)Enum.Parse(typeof(T), key);
                    }
                }

            }
            return null;

        }
    }
}
