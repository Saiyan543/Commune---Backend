using Main.Slices.Accounts.Models.Dtos;
using System.Text;

namespace Main.Global.Library.MediaTypes.Csv
{
    public sealed class AccountCsvFormatter : CsvOutputFormatter<UserAccountDto>
    {
        public AccountCsvFormatter()
            : base()
        {
        }

        public new void FormatCsv(StringBuilder buffer, UserAccountDto account)
        {
            buffer.AppendLine($"{account.Id},\"{account.UserName},\"{account.Email}\"");
        }
    }
}