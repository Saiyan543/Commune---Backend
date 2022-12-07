using Main.Global.Helpers;
using Main.Slices.Rota.Dependencies.Neo4J;
using Main.Slices.Rota.Dependencies.Neo4J.Extensions;
using Main.Slices.Rota.Dependencies.Redis.Extensions;
using Main.Slices.Rota.Models.Db;
using Main.Slices.Rota.Models.enums;
using Main.Slices.Rota.Services.Rota;
using Microsoft.IdentityModel.Tokens;
using Neo4j.Driver;
using StackExchange.Redis;

namespace Homestead.Slices.Rota.Services.Rota
{

    public partial class RotaService : IRotaService
    {
        public async Task X()
        {

            const string Set = @"Match (s:User{role:'Security'}) SET s.available = true";
            await _driver.RunAsync(Set, new {});

            const string AllClubIds_Names = @"MATCH (c:User{role:'Club'}) RETURN c.id, c.name";
            var allClubIds_Names = await _driver.ReadAsync(AllClubIds_Names, null, (s) => (s["c.id"].As<string>(), s["c.name"].As<string>()));
            

            foreach (var clubId_NameAt_n in allClubIds_Names)
            {
                var clubRotaAt_n = 
                    await _redis.HashGetAsync(clubId_NameAt_n.Item1, DateToChange)
                    .ContinueWith(x => x.Result
                    .Deserialize<RetrieveClubUp>());
                

                if (clubRotaAt_n.Start is null) 
                {
                    await Increment(RotaPrefix+clubId_NameAt_n.Item1, new NullShift().Serialize(), new InitClubUp().Serialize());
                    continue;
                }
                const string QueryB = @"MATCH (s:User{available:true, role:'Security'})-[:Contract]-(c:User{id:$id, role:'Club'}) RETURN s.id, s.name LIMIT $personel";
                var allAvailableContractedBouncersIds_Names = await _driver.ReadAsync(QueryB, 
                    new { id = clubId_NameAt_n.Item1, personel = clubRotaAt_n.Personel },
                        (res) => (res["s.id"].As<string>(), res["s.name"].As<string>()));

                if (allAvailableContractedBouncersIds_Names.IsNullOrEmpty())
                    continue;

                List<Personel> personelForShift = new();
                List<string> unavailable = new();
                foreach (var sec in allAvailableContractedBouncersIds_Names)
                {
                    var secRotaAt_n = await GetUpComingRota<RetrieveSecurityUp>(sec.Item1, DateToChange);
                    if (secRotaAt_n.Start is null)
                    {
                        unavailable.Add(sec.Item1);
                        await Increment(sec.Item1, string.Empty, new InitSecurityUp().Serialize());
                    }
                    
                    if (secRotaAt_n.Start >= clubRotaAt_n.Start && clubRotaAt_n.End <= secRotaAt_n.End)
                    {
                        personelForShift.Add(new Personel(sec.Item1, sec.Item2, Attendance.Unconfirmed));
                        unavailable.Add(sec.Item1);
                    }
                }

                await _redis.Write(
                   async (db) => await db.HashSetAsync(clubId_NameAt_n.Item1, DateToChange, new Shift(clubId_NameAt_n.Item1,clubId_NameAt_n.Item2, clubRotaAt_n.Start, clubRotaAt_n.End, EventStatus.Ok, personelForShift).Serialize()),
                   async (db) => await db.HashDeleteAsync(clubId_NameAt_n.Item1, DateToDelete),
                   async (db) => await db.HashSetAsync(clubId_NameAt_n.Item1, DateToCreate, new InitClubUp().Serialize()));
                
                foreach (var sec in personelForShift)
                {
                    await _redis.Write(
                    async (db) => await db.HashSetAsync(sec.SecurityId, DateToChange, clubId_NameAt_n.Item1),
                    async (db) => await db.HashDeleteAsync(sec.SecurityId, DateToDelete),
                    async (db) => await db.HashSetAsync(sec.SecurityId, DateToCreate, new InitSecurityUp().Serialize()));
                }


                using (var session = _driver.AsyncSession(ConfigBuilder => ConfigBuilder.WithDatabase("neo4j")))
                {
                    await session.ExecuteWriteAsync(async tx =>
                    {
                        foreach (var a in unavailable)
                        {
                            await tx.RunAsync(@"MATCH (s:User{role:'Security', id:$id}) SET s.available = false", new {id = a});
                        }
                    });
                }


            }
            const string ReturnUnavailable = @"MATCH (s:User{role:'Security'}) WHERE s.available = true RETURN s.id";
            var unpicked = await _driver.ReadAsync(ReturnUnavailable,
                new { }, (rec) => rec["s.id"].As<string>());

            foreach (var un in unpicked)
            {
                await Increment(un, "void", new InitSecurityUp().Serialize());
            }



        }
        public async Task Increment(string locationId, string shift, string newRota)
            => await _redis.Write(
               async (db) => await db.HashSetAsync(locationId, DateToChange, shift),
               async (db) => await db.HashDeleteAsync(locationId, DateToDelete),
               async (db) => await db.HashSetAsync(locationId, DateToCreate, newRota));


    }



}



public record RetrieveSecurityUp
{
    public DateTime? Start { get; init; }
    public DateTime? End { get; init; }
}


public record RetrieveClubUp
{
    public DateTime? Start { get; init; }
    public DateTime? End { get; init; }
    public int Personel { get; init; }
}



public record InitSecurityUp
{
    public DateTime? Start { get; init; } = null;
    public DateTime? End { get; init; } = null;

}

public record InitClubUp
{
    public DateTime? Start { get; init; } = null;
    public DateTime? End { get; init; } = null;
    public int Personel { get; init; } = default;

}

public class NullShift : Shift
{
    public NullShift()
        : base(null, null, null, null, EventStatus.Null, Enumerable.Empty<Personel>().ToList())
    { }
}


public class Shift
{
    public Shift(string? ClubId, string? ClubName, DateTime? Start, DateTime? End, EventStatus Status, List<Personel> Personel)
    {

        this.ClubName = ClubName == null ? string.Empty : ClubName;

        this.ClubId = ClubId == null ? string.Empty : ClubId;

        this.Start = Start;

        this.End = End;

        this.Status = Status;

        this.Personel = Personel;
    }


    public string ClubName { get; set; }

    public string ClubId { get; set; }

    public DateTime? Start { get; set; }

    public DateTime? End { get; set; }

    public EventStatus Status { get; set; }

    public List<Personel> Personel { get; set; }

}

public record class Personel
{
    public Personel(string SecurityId, string SecurityName, Attendance Attendance)
    {
        this.SecurityName = SecurityName;
        this.SecurityId = SecurityId;
        this.Attendance = Attendance;
    }

    public string SecurityName { get; init; }
    public string SecurityId { get; init; }
    public Attendance Attendance { get; set; }

}






