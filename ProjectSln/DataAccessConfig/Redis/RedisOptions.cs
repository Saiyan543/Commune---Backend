namespace Main.DataAccessConfig.Redis
{
    public record RedisOptions
    {
        public string Section = "RedisOptions";
        public string? ChannelPrefix { get; set; }
        public string? ClientName { get; set; }
        public string? Password { get; set; }
        public string? User { get; set; }
        public ICollection<string> Hosts { get; set; }
        public ICollection<int> Ports { get; set; }
    }
}