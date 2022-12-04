namespace Main.Slices.Accounts.Dependencies.IdentityCore.Configuration.Models.Dtos
{
    public record EFCoreQueryDto(int pageNumber, int pageSize, string searchBy, string searchTerm, string sortBy);

    //?pageNumber=1&pageSize=5&searchBy=firstname&searchTerm=jimmy&sortBy=lastname
}