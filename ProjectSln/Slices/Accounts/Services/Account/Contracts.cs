using Main.Global.Library.ApiController.Responses;
using Main.Slices.Accounts.Dependencies.IdentityCore.Models;
using Main.Slices.Accounts.Models.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Main.Slices.Accounts.Services.Account
{
    public interface IAccountService
    {
        Task<IdentityResult> DeleteUser(string id);

        Task<BaseResponse> GetUserAccountById(string id);

        Task<IEnumerable<UserAccountDto>> QueryAccounts(EFCoreQueryDto dto, bool trackChanges);

        Task<IdentityResult> RegisterUser(UserForRegistrationDto dto, string role);

        Task<IdentityResult> UpdatePassword(string id, UpdatePasswordDto dto);

        Task<IdentityResult> UpdateUser(string id, UserForUpdateDto dto);
    }
}