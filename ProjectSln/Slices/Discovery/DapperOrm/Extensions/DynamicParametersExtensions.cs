using Dapper;
using System.Data;

namespace Main.Slices.Discovery.DapperOrm.Extensions
{
    public static class DynamicParametersExtensions
    {
        public static DynamicParameters Fill(this DynamicParameters parameters, params (string, object, DbType)[] vals)
        {
            foreach (var val in vals)
                parameters.Add(val.Item1, val.Item2, val.Item3);
            // name, value, type

            return parameters;
        }
    }
}