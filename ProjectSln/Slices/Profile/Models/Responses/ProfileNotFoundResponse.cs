using Main.Global.Library.ApiController.Responses;

namespace Main.Slices.Profile.Models.Responses
{
    public class ProfileNotFoundResponse : NotFoundResponse
    {
        public ProfileNotFoundResponse(string id)
            : base($"Profile with Id: {id} Not found. Time: {DateTime.UtcNow}")
        { }
    }
}