using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Web.Models.Locator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgrifoodManagement.Web.Mappers
{
    public static class LocatorViewModelMapper
    {
        public static LocatorViewModel Map(IEnumerable<LocationDto> locations)
        {
            // Load the GeoJSON map once
            var worldPath = Path.Combine(Directory.GetCurrentDirectory(),
                                 "wwwroot", "scripts", "WorldMap.json");
            var worldText = File.ReadAllText(worldPath);
            var worldObj = JsonConvert.DeserializeObject<JObject>(worldText)!;

            var romaniaPath = Path.Combine(Directory.GetCurrentDirectory(),
                                 "wwwroot", "scripts", "RomaniaMap.json");
            var romaniaText = File.ReadAllText(romaniaPath);
            var romaniaObj = JsonConvert.DeserializeObject<JObject>(romaniaText)!;

            // Map each LocationDto → Location
            var vmLocations = locations
                .Select(d => new Location
                {
                    latitude = d.Latitude,
                    longitude = d.Longitude,
                    name = d.Name,
                    isInRomania = d.IsInRomania
                })
                .ToList();

            // Hard-coded continent colors
            var continentColors = new List<ContinentColor> 
            {
                new() { continent = "North America", color = "#71b081" },
                new() { continent = "South America", color = "#5a9a77" },
                new() { continent = "Africa", color = "#498770" },
                new() { continent = "Europe", color = "#39776c" },
                new() { continent = "Asia", color = "#266665" },
                new() { continent = "Australia", color = "#124f5e" }
            };

            return new LocatorViewModel
            {
                MapData = worldObj,
                RomaniaMapData = romaniaObj,
                Locations = vmLocations,
                ContinentColors = continentColors
            };
        }
    }
}
