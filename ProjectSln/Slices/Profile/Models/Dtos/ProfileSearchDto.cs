using Main.Global.Helpers.Querying.Uri;

namespace Main.Slices.Profile.Models.Dtos
{
    public sealed record ProfileSearchDto : RequestParams
    {
        public string Id { get; init; }
        public double Range { get; init; }
    }
}