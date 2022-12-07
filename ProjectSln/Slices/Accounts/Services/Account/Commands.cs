using AutoMapper;
using Main.Global;
using Main.Global.Library.AutoMapper;
using Main.Global.Library.RabbitMQ;
using Main.Global.Library.RabbitMQ.Messages;
using Main.Slices.Accounts.Dependencies.IdentityCore.Context;
using Main.Slices.Accounts.Dependencies.IdentityCore.Models;
using Main.Slices.Accounts.Models.Dtos;
using Main.Slices.Accounts.Services.Account;
using Microsoft.AspNetCore.Identity;

namespace Homestead.Slices.Accounts.Services.Account
{
    public partial class AccountService : EFCoreBase<User>, IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IPublisher _publisher;
        private readonly Serilog.ILogger _logger;

        public AccountService(Serilog.ILogger logger,
            UserManager<User> userManager, IdentityContext context, IPublisher publisher) 
            : base(context)
        {
            _userManager = userManager;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task<IdentityResult> UpdatePassword(string id, UpdatePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            return result.Succeeded ? IdentityResult.Success : IdentityResult.Failed(result.Errors.ToArray());
        }

        public async Task<IdentityResult> UpdateUser(string id, UserForUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var result = await _userManager.UpdateAsync(dto.Map(user));
            return result.Succeeded ? IdentityResult.Success : IdentityResult.Failed(result.Errors.ToArray());
        }

        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto dto, string role)
        {

            var result = await _userManager.CreateAsync(dto.Map<UserForRegistrationDto, User>(), dto.Password);
            if (!result.Succeeded)
                return IdentityResult.Failed(result.Errors.ToArray());

            var user = await _userManager.FindByEmailAsync(dto.Email);
            var roleResult = await _userManager.AddToRoleAsync(user, role);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return IdentityResult.Failed(roleResult.Errors.ToArray());
            }
           
            _publisher.Publish(new Register(user.Id, user.UserName, role));
            return result;
        }

        public async Task<IdentityResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return IdentityResult.Failed(new IdentityError() { Code = "UserNotFound", Description = "User not found" });
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                _publisher.Publish(new Delete(id));
            return result;
        }
    }
}