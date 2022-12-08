using Main.Global.Library.MediaTypes.Csv;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Main.Global.Library.MediaTypes
{
    public static class ServiceExtensions
    {
        public static IMvcBuilder AddCustomCSVFormatters(this IMvcBuilder builder) =>
            builder.AddMvcOptions(config => config.OutputFormatters.Add(new AccountCsvFormatter()));

        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config.OutputFormatters
                        .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();

                if (systemTextJsonOutputFormatter != null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/apiroot+json");
                }

                var xmlOutputFormatter = config.OutputFormatters
                        .OfType<XmlDataContractSerializerOutputFormatter>()?
                        .FirstOrDefault();

                if (xmlOutputFormatter != null)
                {
                    xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/apiroot+xml");
                }
            });
        }
    }
}