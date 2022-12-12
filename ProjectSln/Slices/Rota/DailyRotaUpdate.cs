using Main.DataAccessConfig.Neo4J;
using Main.DataAccessConfig.Neo4J.Extensions;
using Main.DataAccessConfig.Redis.Extensions;
using Main.Global.Helpers;
using Main.Slices.Rota.Models.Rota;
using Main.Slices.Rota.Models.Rota.enums;
using Neo4j.Driver;
using StackExchange.Redis;

namespace Main.Slices.Rota
{
    public interface IDailyRotaUpdate
    { public Task Init(); }

    public sealed class DailyRotaUpdate : IDailyRotaUpdate
    {
        private static string DateToDelete = DateTime.UtcNow.Subtract(TimeSpan.FromDays(0)).ToShortDateString();
        private static string DateToChange = DateTime.UtcNow.AddDays(7).ToShortDateString();
        private static string DateToCreate = DateTime.UtcNow.AddDays(14).ToShortDateString();
        private readonly IDatabase _redis;
        private readonly IDriver _driver;

        public DailyRotaUpdate(IDatabase redis)
        {
            _redis = redis;
            _driver = Driver.Neo4jDriver;
        }

        public async Task Init()
        {
            await _driver.RunAsync(@"Match (s:User{role:'Security'}) SET s.available = true", new { });

            const string AllClubIds_Names = @"MATCH (c:User{role:'Club'}) RETURN c.id, c.name";
            var allClubIds_Names = await _driver.ReadAsync(AllClubIds_Names, null, (s) => (s["c.id"].As<string>(), s["c.name"].As<string>()));

            foreach (var clubId_NameAt_n in allClubIds_Names)
            {
                var clubRotaAt_n =
                    await _redis.HashGetAsync(clubId_NameAt_n.Item1, DateToChange)
                    .ContinueWith(x => x.Result
                    .Deserialize<Schedule_Club>());

                if (clubRotaAt_n.Start is null)
                {
                    await Increment(clubId_NameAt_n.Item1, new NullShift().Serialize(), new Schedule_Club(null, null, default).Serialize());
                    continue;
                }

                var allAvailableContractedBouncersIds_Names = await _driver.ReadAsync(
                    @"MATCH (s:User{available:true, role:'Security'})-[:Contract]-(c:User{id:$id, role:'Club'}) RETURN s.id, s.name LIMIT $personel",
                    new { id = clubId_NameAt_n.Item1, personel = clubRotaAt_n.Personel },
                        (res) => (res["s.id"].As<string>(), res["s.name"].As<string>()));

                if (allAvailableContractedBouncersIds_Names is null)
                    continue;

                List<Personel> personelForShift = new();
                List<string> unavailable = new();
                foreach (var sec in allAvailableContractedBouncersIds_Names)
                {
                    var secRotaAt_n = await _redis
                        .HashGetAsync(sec.Item1, DateToChange)
                        .ContinueWith(x => x.Result
                        .Deserialize<Schedule_Security>());

                    if (secRotaAt_n.Start is null)
                    {
                        unavailable.Add(sec.Item1);
                        await Increment(sec.Item1, string.Empty, new Schedule_Security(null, null).Serialize());
                    }

                    if (secRotaAt_n.Start >= clubRotaAt_n.Start && clubRotaAt_n.End <= secRotaAt_n.End)
                    {
                        personelForShift.Add(new Personel(sec.Item1, sec.Item2, Attendance.Unconfirmed));
                        unavailable.Add(sec.Item1);
                    }
                }

                await Increment(clubId_NameAt_n.Item1,
                    new Shift(clubId_NameAt_n.Item1, clubId_NameAt_n.Item2, clubRotaAt_n.Start, clubRotaAt_n.End, EventStatus.Ok, personelForShift).Serialize(),
                    new Schedule_Club(null, null, default).Serialize());

                foreach (var sec in personelForShift)
                {
                    await Increment(sec.SecurityId, clubId_NameAt_n.Item1, new Schedule_Security(null, null).Serialize());
                }

                using (var session = _driver.AsyncSession(ConfigBuilder => ConfigBuilder.WithDatabase("neo4j")))
                {
                    await session.ExecuteWriteAsync(async tx =>
                    {
                        foreach (var id in unavailable)
                        {
                            await tx.RunAsync(@"MATCH (s:User{role:'Security', id:$id}) SET s.available = false", new { id });
                        }
                    });
                }
            }

            var unpicked = await _driver.ReadAsync(@"MATCH (s:User{role:'Security'}) WHERE s.available = true RETURN s.id",
                new { }, (rec) => rec["s.id"].As<string>());

            foreach (var un in unpicked)
            {
                await Increment(un, "void", new Schedule_Security(null, null).Serialize());
            }
        }

        public async Task Increment(string locationId, string shift, string newRota)
            => await _redis.Write(
               async (db) => await db.HashSetAsync(locationId, DateToChange, shift),
               async (db) => await db.HashDeleteAsync(locationId, DateToDelete),
               async (db) => await db.HashSetAsync(locationId, DateToCreate, newRota));
    }
}