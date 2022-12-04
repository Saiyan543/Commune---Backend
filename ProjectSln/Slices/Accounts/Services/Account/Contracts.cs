using Main.Global.Library.ApiController.Responses;
using Main.Slices.Accounts.Dependencies.IdentityCore.Configuration.Models.Dtos;
using Main.Slices.Accounts.Models.Dtos.In;
using Main.Slices.Accounts.Models.Dtos.Out;
using Microsoft.AspNetCore.Identity;

namespace Main.Slices.Accounts.Services.Account
{
    public interface IAccountService
    {
        Task<IdentityResult> DeleteUser(string id);

        Task<BaseResponse> GetUserAccountById(string id);

        Task<IEnumerable<UserAccountDto>> QueryAccounts(EFCoreQueryDto dto, bool trackChanges);

        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);

        Task<IdentityResult> UpdatePassword(string id, UpdatePasswordDto inDto);

        Task<IdentityResult> UpdateUser(string id, UserForUpdateDto update);
    }
}