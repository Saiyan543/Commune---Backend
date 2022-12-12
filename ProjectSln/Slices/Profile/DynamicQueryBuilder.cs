using Main.Global.Helpers.Querying.Uri;
using System.Text;

namespace Main.Slices.Profile
{
    public static class DynamicQueryBuilder
    {
        private const string SELECT = "SELECT * FROM Profile as p JOIN Days d ON d.ProfileId = p.Id JOIN Location l ON l.ProfileId = p.Id";

        public static string Build(ValueFilter[] conditions, string[] boolArr) =>
            Order(
                boolArr.Any() ? true : false,
                conditions.Any() ? true : false,
                boolArr,
                conditions,
                new StringBuilder().Append(SELECT)
                ).ToString().Replace("  ", " ").Trim();

        private static StringBuilder Order(bool anyBools, bool anyComparisons, string[] bools, ValueFilter[] conditions, StringBuilder sb) =>
             (anyBools, anyComparisons) switch
             {
                 (true, true) => NoPrePend(conditions, ValueCondition, PrePend(bools, BoolCondition, sb)),
                 (true, false) => PrePend(bools, BoolCondition, sb),
                 (false, true) => NoPrePend(conditions, ValueCondition, sb),
                 _ => sb
             };

        private static StringBuilder NoPrePend<T>(T[] strArr, Func<T, string> operation, StringBuilder sb)
        {
            for (int i = 1; i < strArr.Length; i++)
                sb.Append(" AND " + operation(strArr[i]));
            return sb;
        }

        private static StringBuilder PrePend<T>(T[] strArr, Func<T, string> operation, StringBuilder sb)
        {
            sb.Append(" WHERE " + operation(strArr[0]));
            for (int i = 1; i < strArr.Length; i++)
                sb.Append(" AND " + operation(strArr[i]));
            return sb;
        }

        private static Func<string, string> BoolCondition = (field) =>
        field.Trim().ToLower() switch
        {
            "m" => "d.Monday = TRUE",
            "tu" => "d.Tuesday = TRUE",
            "w" => "d.Wednesday = TRUE",
            "th" => "d.Thursday = TRUE",
            "f" => "d.Friday = TRUE",
            "sa" => "d.Saturday = TRUE",
            "su" => "d.Sunday = TRUE",
            "sia" => "c.SIA = TRUE",
            "cctv" => "c.CCTV = TRUE",
            "looking" => "s.ActivelyLooking = TRUE",

            _ => throw new ArgumentException("Invalid field")
        };

        private static Func<ValueFilter, string> ValueCondition = (filter) =>
            filter.Field.Trim().ToLower() switch
            {
                "monthsofexperience" => "b.MonthsOfExperience " + filter.Operator + " " + filter.Value + " ",
                _ => throw new ArgumentException("Invalid field")
            };
    }
}