using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tcomp_barcode_printing.Models;
using tcomp_barcode_printing.Services;
using TcompEdniffDataSync.Services;

namespace tcomp_barcode_printing.Repositories
{
    internal class SerialNumberRepository : ISerialNumber
    {
        private readonly HttpClient httpClient;
        public SerialNumberRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        async Task<List<ListSerialNumber>> ISerialNumber.GetSerialNumbersAsync(string orderNo, string serialNumber)
        {

            string apiUrl = $"{ConfigService.ApiBaseUrl}SerialNumber/getSerialNumbersForBarcodePrint";

            var form = new MultipartFormDataContent();
            form.Add(new StringContent(orderNo ?? "0"), "orderNo");
            form.Add(new StringContent(serialNumber.ToString() ?? ""), "serialNumber");

            var response = await httpClient.PostAsync(apiUrl, form);
            string responseJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<ListSerialNumber>>(responseJson)
                   ?? new List<ListSerialNumber>();
        }


    }
}
