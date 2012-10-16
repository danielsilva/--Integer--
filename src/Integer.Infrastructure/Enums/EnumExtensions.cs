using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Integer.Infrastructure.Enums
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum element)
        {
            Type type = element.GetType();
            MemberInfo[] memberInfo = type.GetMember(element.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] descriptionAttribute = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (descriptionAttribute != null && descriptionAttribute.Length > 0)
                {
                    return ((DescriptionAttribute)descriptionAttribute[0]).Description;
                }
            }

            return element.ToString();
        }

        public static IEnumerable<KeyValuePair<int, string>> GetDescriptions(this Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ApplicationException("O método GetDescriptions só pode ser utilizado para enumeradores.");
            }

            var list = new List<KeyValuePair<int, string>>();
            foreach (var enumItem in Enum.GetValues(enumType))
            {
                var item = new KeyValuePair<int, string>((int)enumItem, ((Enum)enumItem).GetDescription());
                list.Add(item);
            }
            return list;
        }
    }
}
