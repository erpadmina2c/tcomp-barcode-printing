using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using tcomp_barcode_printing.Repositories;
using tcomp_barcode_printing.Services;
using TcompEdniffDataSync.Services;

namespace tcomp_barcode_printing
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();

            // Register Typed HttpClient for SerialNumberRepository
            services.AddHttpClient<ISerialNumber, SerialNumberRepository>(client =>
            {
                client.BaseAddress = new Uri(ConfigService.ApiBaseUrl);
            });

            var provider = services.BuildServiceProvider();

            var serialNumberService = provider.GetRequiredService<ISerialNumber>();

            Application.Run(new Form1(serialNumberService));
        }
    }
}
