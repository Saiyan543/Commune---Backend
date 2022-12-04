using Main.Global.Helpers.Querying.Uri;

namespace Main.Slices.Discovery.Models.Dtos.In
{
    public record ProfileSearchDto : RequestParams
    {
        public string id { get; init; }
        public double range { get; init; }
    }
}