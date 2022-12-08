using Main.Global.Helpers.Location.Coordinates;
using System.Text;

namespace Main.Global.Helpers.Location
{
    public static class GeoLocation
    {
        private static Polygon Area;

        static GeoLocation()
        {
            Coordinate Chesham = new Coordinate(51.70190397403934, -0.6077561976064242);
            Coordinate Harlow = new Coordinate(51.75974184386442, 0.09536880767493451);
            Coordinate Rochester = new Coordinate(51.378185809815974, 0.46615738467877615);
            Coordinate Woking = new Coordinate(51.32586815773875, -0.5459581014391172);

            Area = new Polygon(new Coordinate[4] { Chesham, Harlow, Rochester, Woking });
        }

        public static bool IsInRange(double maxDistance, Coordinate coordinateFrom, Coordinate coordinateTo)
          => coordinateFrom.DistanceTo(coordinateTo, UnitOfLength.Kilometers) <= maxDistance;

        public static Coordinate GetCoordinateFromPostCode(string Address)
        {
            // A proxy for using a public maps Api.
            return Area.GeneratePoint();
        }

        public static string GetPostCodeFromCoordinate(Coordinate coordinate)
        {
            // A proxy for using a public maps Api.

            var rand = new Random();
            List<string> Letters = new() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            StringBuilder sb = new();
            for (int i = 0; i <= 7; i++)
            {
                if (i == 3)
                {
                    sb.Append(" ");
                    continue;
                }
                sb.Append(Letters[rand.Next(0, 25)]);
            }
            return sb.ToString();
        }
    }
}