using Homestead.Slices.Accounts.Services.Account;
using Homestead.Slices.Rota.Services;
using Main.Global.Library.RabbitMQ;
using Main.Slices.Accounts.EntityFramework_Jwt;
using Main.Slices.Accounts.Services;
using Main.Slices.Discovery;
using Main.Slices.Discovery.DapperOrm.Context;
using Main.Slices.Rota.Neo4J;
using Main.Slices.Rota.Services;
using Microsoft.AspNetCore.Identity;
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
                new ProfileService(logger, searchCtx));
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