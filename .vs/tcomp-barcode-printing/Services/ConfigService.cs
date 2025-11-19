using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace TcompEdniffDataSync.Services
{
    internal static class ConfigService
    {
        private static readonly IConfigurationRoot config;

        static ConfigService()
        {
            try
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                Console.WriteLine($"Config file path: {filePath}");

                config = new ConfigurationBuilder()
                    .AddJsonFile(filePath, optional: false, reloadOnChange: true)
                    .Build();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Config initialization failed: {ex}");
                throw;
            }
        }

        public static string ApiBaseUrl => config["ApiSettings:BaseUrl"] ?? string.Empty;
        public static string ConnectionString => config.GetConnectionString("DefaultConnection") ?? string.Empty;
    }
}
