using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Formatters
{
    public static class SplitPascalCase
    {
        public static string Format(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return System.Text.RegularExpressions.Regex.Replace(
                input,
                "(\\B[A-Z])",
                " $1"
            );
        }
    }
}
