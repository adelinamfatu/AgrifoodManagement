using AgrifoodManagement.Business.Services.Interfaces;
using AgrifoodManagement.Util.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Services.Impl
{
    public class NominatimGeocodingService : IGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NominatimGeocodingService> _logger;

        public NominatimGeocodingService(HttpClient httpClient,
                                         ILogger<NominatimGeocodingService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            if (!httpClient.DefaultRequestHeaders.Contains("User-Agent"))
                httpClient.DefaultRequestHeaders.Add("User-Agent", "AgroFoodApp");
        }

        public async Task<(double? lat, double? lon)> GeocodeAddressAsync(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return (null, null);

            var query = Uri.EscapeDataString(address.Replace(",", " "));
            var url = $"https://nominatim.openstreetmap.org/search?q={query}&format=json&limit=1";

            try
            {
                var resp = await _httpClient.GetAsync(url);
                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Nominatim lookup failed for {Address}: {StatusCode}",
                                       address, resp.StatusCode);
                    return (null, null);
                }

                using var stream = await resp.Content.ReadAsStreamAsync();
                var results = await JsonSerializer.DeserializeAsync<List<NominatimResult>>(stream);

                var first = results?.FirstOrDefault();
                if (first != null
                    && double.TryParse(first.lat, out var lat)
                    && double.TryParse(first.lon, out var lon))
                {
                    return (lat, lon);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during geocoding lookup for {Address}", address);
            }

            return (null, null);
        }
    }
}
