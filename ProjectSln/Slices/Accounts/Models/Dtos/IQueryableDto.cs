namespace Main.Slices.Accounts.Models.Dtos
{
    public record IQueryableDto(int pageNumber, int pageSize, string searchBy, string searchTerm, string sortBy);

    //?pageNumber=1&pageSize=5&searchBy=firstname&searchTerm=jimmy&sortBy=lastname
}