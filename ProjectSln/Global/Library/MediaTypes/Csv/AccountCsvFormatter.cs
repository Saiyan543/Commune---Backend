using System.Text;

namespace Main.Global.Library.MediaTypes.Csv
{
    public sealed class AccountCsvFormatter : CsvOutputFormatter<object>
    {
        public AccountCsvFormatter()
            : base()
        {
        }

        public new void FormatCsv(StringBuilder buffer, object account)
        {
            // buffer.AppendLine($"{account.Id},\"{account.Name},\"{account.FullAddress}\"");
        }
    }
}