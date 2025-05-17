namespace AgrifoodManagement.Web.Models.Locator
{
    public class LocatorViewModel
    {
        public List<Location> Locations { get; set; }
        public List<ContinentColor> ContinentColors { get; set; }
        public object MapData { get; set; }
    }

    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string name { get; set; }
    }

    public class ContinentColor
    {
        public string continent { get; set; }
        public string color { get; set; }
    }
}
