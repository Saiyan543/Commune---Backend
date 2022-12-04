namespace Main.Slices.Rota
{
    public sealed class RepeatingService : BackgroundService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public RepeatingService(Serilog.ILogger logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        private readonly PeriodicTimer _timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _timer.WaitForNextTickAsync(stoppingToken)
                && !stoppingToken.IsCancellationRequested)
            {
                //_logger.Lo("RepeatingService running at: {time}", DateTimeOffset.Now);

                using (var scope = _serviceProvider.CreateScope())
                {
                    //var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();
                    //await scopedProcessingService.DoWork(stoppingToken);
                }
            }
        }
    }
}
//        public async Task DeleteExpiredRota(IEnumerable<string> allIds)
//        {
//            for (int i = 0; i < allIds.Count(); i++)
//            {
//                // make sure it doesn't spill over to the day after
//                await _redis.HashDeleteAsync(i + RotaPrefix, DateTime.UtcNow.ToShortDateString());
//            }
//        }

//        public async Task Update()
//        {
//            var time = "";
//            var allIds = await _driver.ReadAsync(@"MATCH (u:Club) RETURN u.id", null, (s) => s["u.id"].As<string>());
//            await DeleteExpiredRota(allIds);


//            var securityRotas = await _redis
//                .HashGetAsync("", DateTime.UtcNow.AddDays(7).ToString())
//                .ContinueWith(x => x.Result
//                .RedisDeserialize<IEnumerable<SecurityRotaModel>>());

//            var clubRotas = await _redis
//                .HashGetAsync("", DateTime.UtcNow.AddDays(7).ToString())
//                .ContinueWith(x => x.Result
//                .RedisDeserialize<IEnumerable<ClubRotaModel>>());



//            for (int i = 0; i < clubRotas.Count(); i++)
//            {
//                var club = clubRotas.ElementAt(i);
//                var result = await _driver.ReadAsync(
//                    @"MATCH (s:S{available:'true'})-[:Contract]-(c:C{id:$id}) 
//                        Return s Limit $personel", new { id = clubRotas.ElementAt(i).Id },
//                    (res) => res["s.id"].As<string>());


//                if (result.IsNullOrEmpty())
//                {
//                    continue;
//                }

//                var res = securityRotas.Where(x => x.SecurityId.Equals(result));

//                var cRota = new ClubShiftModel();
//                foreach (var r in res)
//                {
//                    if (r.Start <= clubRotas.ElementAt(i).Start && r.End >= clubRotas.ElementAt(i).End)
//                    {
//                        SecurityShiftModel sRota = new(club.Name, club.Id, club.Start, club.End, status.Unconfirmed);
//                        await _redis.HashSetAsync("Prefix" + r.SecurityId, time, JsonConvert.SerializeObject(sRota));

//                        var sec = new Security(r.SecurityName, r.SecurityId, status.Unconfirmed);
//                        cRota.Personel.Add(sec);

//                        await _driver.RunAsync(@"Match(:User{id:$id}) SET available = false", new { id = r.SecurityId });
//                        continue;
//                    }

//                }

//                await _redis.HashSetAsync("Prefix" + club.Id, time, JsonConvert.SerializeObject(cRota));

//            }






//        }
//    }










//    // security






//    // 
//}