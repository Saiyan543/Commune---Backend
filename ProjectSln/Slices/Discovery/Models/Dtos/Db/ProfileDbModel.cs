using Main.Slices.Discovery.Models.Dtos.Base;

namespace Main.Slices.Discovery.Models.Dtos.Db
{
    public record ProfileDbModel : ProfileBase
    {
        public double Latitude { get; init; }
        public double Longitude { get; init; }
    }
}