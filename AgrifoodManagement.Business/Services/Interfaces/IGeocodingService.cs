using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Services.Interfaces
{
    public interface IGeocodingService
    {
        /// <summary>
        /// Returns (latitude, longitude) for the given address, or (null,null) if not found.
        /// </summary>
        Task<(double? lat, double? lon)> GeocodeAddressAsync(string address);
    }
}
