using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class RestCountry
    {
        public string Name { get; set; }
        public List<string> CallingCodes { get; set; }
    }

    public class CountryCode
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }
}
