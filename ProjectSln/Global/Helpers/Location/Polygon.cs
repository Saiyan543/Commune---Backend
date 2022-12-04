using Main.Global.Helpers.Location.Coordinates;

namespace Main.Global.Helpers.Location
{
    public struct Polygon
    {
        public Polygon(Coordinate[] coordinates)
        {
            MinLat = coordinates.Min(x => x.Latitude);
            MinLon = coordinates.Min(x => x.Longitude);
            MaxLat = coordinates.Max(x => x.Latitude);
            MaxLon = coordinates.Max(x => x.Longitude);
        }

        public double MinLat { get; set; }
        public double MaxLat { get; set; }
        public double MinLon { get; set; }
        public double MaxLon { get; set; }

        public Coordinate GeneratePoint()
        {
            var random = new Random();
            var latitude = random.NextDouble() * (MaxLat - MinLat) + MinLat;
            var longitude = random.NextDouble() * (MaxLon - MinLon) + MinLon;
            return new Coordinate(latitude, longitude);
        }
    }
}