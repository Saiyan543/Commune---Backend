﻿using Homestead.Slices.Rota.Dependencies.Neo4J.Options;
using Main.Slices.Rota.Dependencies.Neo4J;

namespace Main.Slices.Rota.Dependencies.Neo4J.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureNeo4jDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var neo4jConfig = new Neo4jOptions();
            configuration.Bind("Neo4jOptions", neo4jConfig);
            Driver.Register(neo4jConfig.Uri, neo4jConfig.Username, neo4jConfig.Password);
        }
    }
}