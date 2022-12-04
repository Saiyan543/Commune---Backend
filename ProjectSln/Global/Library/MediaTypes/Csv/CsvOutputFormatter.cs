using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace Main.Global.Library.MediaTypes.Csv
{
    public abstract class CsvOutputFormatter<T> : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type? type)
        {
            if (typeof(T).IsAssignableFrom(type)
                || typeof(IEnumerable<T>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }

            return false;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context,
            Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();

            if (context.Object is IEnumerable<T>)
            {
                foreach (var entity in (IEnumerable<object>)context.Object)
                {
                    FormatCsv(buffer, entity);
                }
            }
            else
            {
                FormatCsv(buffer, context.Object);
            }

            await response.WriteAsync(buffer.ToString());
        }

        protected static void FormatCsv(StringBuilder buffer, object entity)
        {
        }
    }
}