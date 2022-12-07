using AutoMapper;
using Homestead.Slices.Accounts.Services.Account;
using Homestead.Slices.Discovery.ProfileService;
using Homestead.Slices.Rota.Services;
using Homestead.Slices.Rota.Services.Contract;
using Homestead.Slices.Rota.Services.Rota;
using Main.Global.Library.RabbitMQ;
using Main.Slices.Accounts.Dependencies.IdentityCore.Context;
using Main.Slices.Accounts.Dependencies.IdentityCore.Models;
using Main.Slices.Accounts.Services.Account;
using Main.Slices.Accounts.Services.Authentication;
using Main.Slices.Discovery.DapperOrm.Context;
using Main.Slices.Discovery.ProfileService;
using Main.Slices.Rota.Dependencies.Neo4J;
using Main.Slices.Rota.Services.Contract;
using Main.Slices.Rota.Services.Message;
using Main.Slices.Rota.Services.Rota;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Main.Global
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAccountService> _account;
        private readonly Lazy<IAuthenticationService> _authentication;
        private readonly Lazy<IProfileService> _profile;
        private readonly Lazy<IMessageService> _message;
        private readonly Lazy<IRotaService> _rota;
        private readonly Lazy<IContractService> _contract;

        public ServiceManager(
            IMapper mapper,
            IdentityContext idpCtx,
            UserManager<User> userManager,
            Serilog.ILogger logger,
            IConfiguration configuration,
            DapperContext searchCtx,
            IPublisher publisher,
            IDatabase redis     
            )
        {
            _account = new Lazy<IAccountService>(() =>
                new AccountService(logger, userManager, idpCtx, publisher));
            _authentication = new Lazy<IAuthenticationService>(() =>
                new AuthenticationService(userManager, logger, configuration));
            _profile = new Lazy<IProfileService>(() =>
                new ProfileService(logger, mapper, searchCtx));
            _message = new Lazy<IMessageService>(() =>
                new MessageService(logger, redis, Driver.Neo4jDriver));
            _rota = new Lazy<IRotaService>(() =>
                new RotaService(logger, redis, Driver.Neo4jDriver));
            _contract = new Lazy<IContractService>(() =>
                new ContractService(logger, Driver.Neo4jDriver));
        }

        public IAccountService Account => _account.Value;
        public IAuthenticationService Authentication => _authentication.Value;
        public IProfileService Profile => _profile.Value;
        public IMessageService Message => _message.Value;
        public IRotaService Rota => _rota.Value;
        public IContractService Contract => _contract.Value;
    }

    public interface IServiceManager
    {
        public IAccountService Account { get; }

        public IAuthenticationService Authentication { get; }

        public IProfileService Profile { get; }

        public IMessageService Message { get; }

        public IRotaService Rota { get; }

        public IContractService Contract { get; }
    }
}