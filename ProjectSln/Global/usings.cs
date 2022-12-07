global using AutoMapper;
using Homestead.Slices.Accounts.Services.Account;
using Homestead.Slices.Discovery.ProfileService;
using Homestead.Slices.Rota.Services;
using Homestead.Slices.Rota.Services.Contract;

using Main.Global.Library.RabbitMQ;
using Main.Slices.Accounts.Dependencies.IdentityCore.Configuration.Models.DbModels;
using Main.Slices.Accounts.Dependencies.IdentityCore.Context;
using Main.Slices.Accounts.Services.Account;
using Main.Slices.Accounts.Services.Authentication;
using Main.Slices.Discovery.DapperOrm.Context;
using Main.Slices.Discovery.ProfileService;
using Main.Slices.Rota.Dependencies.Neo4J;
using Main.Slices.Rota.Services.Contract;
using Main.Slices.Rota.Services.Message;

