using AutoMapper;
using Main.Global.Library.AutoMapper;
using Main.Global.Library.RabbitMQ;
using Main.Global.Library.RabbitMQ.Messages;
using Main.Global.Models;
using Main.Slices.Accounts.Dependencies.IdentityCore.Configuration.Models.DbModels;
using Main.Slices.Accounts.Dependencies.IdentityCore.Context;
using Main.Slices.Accounts.Models.Dtos.In;
using Main.Slices.Accounts.Services.Account;
using Microsoft.AspNetCore.Identity;

namespace Homestead.Slices.Accounts.Services.Account
{
    public partial class AccountService : EFCoreBase<User>, IAccountService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IPublisher _publisher;

        public AccountService(IMapper mapper,
            UserManager<User> userManager, IdentityContext context, IPublisher publisher) : base(context)
        {
            _mapper = mapper;
            _userManager = userManager;
            _publisher = publisher;
        }

        public async Task<IdentityResult> UpdatePassword(string id, UpdatePasswordDto inDto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var update = await _userManager.ChangePasswordAsync(user, inDto.CurrentPassword, inDto.NewPassword);
            return update.Succeeded ? IdentityResult.Success : IdentityResult.Failed(update.Errors.ToArray());
        }

        public async Task<IdentityResult> UpdateUser(string id, UserForUpdateDto inDto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var update = await _userManager.UpdateAsync(_mapper.Map(inDto, user));
            return update.Succeeded ? IdentityResult.Success : IdentityResult.Failed(update.Errors.ToArray());
        }

        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto inDto)
        {
            string role = new Role(inDto.Role).Value;

            if (role == string.Empty)
                return IdentityResult.Failed(new IdentityError { Description = "Invalid Role" });


            var result = await _userManager.CreateAsync(inDto.Map<UserForRegistrationDto, User>(), inDto.Password);
            if (!result.Succeeded)
                return IdentityResult.Failed(result.Errors.ToArray());

            var user = await _userManager.FindByEmailAsync(inDto.Email);
            var result2 = await _userManager.AddToRoleAsync(user, role);

            if (!result2.Succeeded)
                return IdentityResult.Failed(result.Errors.ToArray());
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