using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Formatters
{
    public static class EnumFormatter
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var attribute = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();

            return attribute?.Name ?? enumValue.ToString();
        }
    }
}
