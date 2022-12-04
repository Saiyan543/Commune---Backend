namespace Main.Slices.Rota.Models.Db
{
    public record SecurityRotaModel(string SecurityName, string SecurityId, DateTime? Start, DateTime? End, bool Working);
}
