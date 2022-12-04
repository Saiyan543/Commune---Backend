using Main.Slices.Accounts.Dependencies.IdentityCore.Configuration.Models.DbModels;

namespace Main.Slices.Accounts.Dependencies.IdentityCore.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<User> Search(this IQueryable<User> users, string searchBy, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return users;

            return searchBy.Trim().ToLower() switch
            {
                "firstname" => users.Where(x => x.FirstName.ToLower().Contains(searchTerm)),
                "lastname" => users.Where(x => x.LastName.ToLower().Contains(searchTerm)),
                "username" => users.Where(x => x.UserName.ToLower().Contains(searchTerm)),
                "email" => users.Where(x => x.Email.ToLower().Contains(searchTerm)),
                _ => users
            };
        }

        public static IQueryable<User> Sort(this IQueryable<User> users, string sortBy)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return users.OrderBy(e => e.LastName);

            return sortBy.Trim().ToLower() switch
            {
                "firstname" => users.OrderBy(x => x.FirstName),
                "lastname" => users.OrderBy(x => x.LastName),
                "username" => users.OrderBy(x => x.UserName),
                "email" => users.OrderBy(x => x.Email),
                _ => users
            };
        }
    }
}