using Main.Global.Helpers.Querying.Uri;
using Main.Slices.Profile;

namespace Project_Tests
{
    public sealed class DynamicQueryBuilderTests
    {
        [Fact]
        public void Test1()
        {

            RequestParams @params = new() { Booleans = "m tu w", Comparisons = new ValueFilter[] { new ValueFilter() { Field = "MonthsOfExperience", Operator = ">", Value = "12" } } };
            var query = DynamicQueryBuilder.Build(@params.Comparisons, @params.SplitBools());
                
            var expect = "SELECT * FROM Profile as p JOIN Days d ON d.ProfileId = p.Id JOIN Location l ON l.ProfileId = p.Id WHERE MonthsOfExperience > 12 AND d.Monday = TRUE AND d.Tuesday = TRUE AND d.Wednesday = TRUE";

            Assert.Equal(expect, query);
        }
    }
}