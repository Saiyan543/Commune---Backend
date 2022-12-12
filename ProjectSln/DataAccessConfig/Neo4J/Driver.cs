using Neo4j.Driver;

namespace Main.DataAccessConfig.Neo4J
{
    public static class Driver
    {
        public static IDriver Neo4jDriver { get; private set; }

        public static void Register(string url, string username, string password) =>
            Neo4jDriver = GraphDatabase.Driver(url,
                AuthTokens.Basic(username, password));
    }
}