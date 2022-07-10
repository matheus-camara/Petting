using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Customers.Localization.Config
{
    public static class LocalizationConfig
    {
        public static IServiceCollection AddLocalizationConfig(this IServiceCollection services)
        {
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("pt-BR")
                };

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.SetDefaultCulture("en-US");

                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    var languages = context.Request.Headers["Accept-Language"].ToString();
                    var currentLanguage = languages.Split(',').FirstOrDefault();
                    var defaultLanguage = string.IsNullOrEmpty(currentLanguage) ? "en-US" : currentLanguage;

                    if (!supportedCultures.Where(s => s.Name.Equals(defaultLanguage)).Any())
                    {
                        defaultLanguage = "en-US";
                    }

                    return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(defaultLanguage, defaultLanguage));
                }));
            });

            return services;
        }
    }
}
