using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class LocationDto
    {
        public double Latitude { get; init; }
        public double Longitude { get; init; }
        public string Name { get; init; } = "";
    }
}
