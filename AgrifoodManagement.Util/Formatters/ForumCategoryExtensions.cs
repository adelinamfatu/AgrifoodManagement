using AgrifoodManagement.Util.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Formatters
{
    public static class ForumCategoryExtensions
    {
        public static string GetIcon(this ForumCategory category)
        {
            var memberInfo = category.GetType().GetMember(category.ToString()).FirstOrDefault();
            var attribute = memberInfo?.GetCustomAttributes(typeof(ForumIconAttribute), false)
                                      .Cast<ForumIconAttribute>()
                                      .FirstOrDefault();

            if (attribute == null)
                return string.Empty;

            return $"<span style='color: {attribute.Color};'>{attribute.SvgIcon}</span>";
        }
    }
}
