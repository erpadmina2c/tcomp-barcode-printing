using Microsoft.Extensions.Configuration;
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
                config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)           // FIX 1: Set correct folder
                    .AddJsonFile("appsettings.json", false, true)     // FIX 2: Use only filename
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
