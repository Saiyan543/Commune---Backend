
using Main.Global.Helpers;
using Main.Global.Library.ApiController.Responses;
using Main.Global.Library.AutoMapper;
using Main.Slices.Accounts.Dependencies.IdentityCore.Context;
using Main.Slices.Accounts.Dependencies.IdentityCore.Extensions;
using Main.Slices.Accounts.Dependencies.IdentityCore.Models;
using Main.Slices.Accounts.Models.Dtos;
using Main.Slices.Accounts.Models.Responses;
using Main.Slices.Accounts.Services.Account;
using Microsoft.EntityFrameworkCore;

namespace Homestead.Slices.Accounts.Services.Account
{
    public partial class AccountService : EFCoreBase<User>, IAccountService
    {
        public async Task<IEnumerable<UserAccountDto>> QueryAccounts(EFCoreQueryDto dto, bool trackChanges)
        {
            var result = await FindAll(trackChanges)
              .Search(dto.searchBy, dto.searchTerm)
              .Sort(dto.sortBy)
              .ToListAsync();

            return result.ResultOrEmpty().Map<User, UserAccountDto>();
        }

        public async Task<BaseResponse> GetUserAccountById(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
            if (result is null)
                return new AccountNotFoundResponse(id);
            return new Response<UserAccountDto>(result.Map<User, UserAccountDto>());
        }
    }
}