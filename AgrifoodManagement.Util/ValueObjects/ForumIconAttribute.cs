using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.ValueObjects
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ForumIconAttribute : Attribute
    {
        public string SvgIcon { get; }
        public string Color { get; set; }

        public ForumIconAttribute(string svgIcon, string color)
        {
            SvgIcon = svgIcon;
            Color = color;
        }
    }
}
