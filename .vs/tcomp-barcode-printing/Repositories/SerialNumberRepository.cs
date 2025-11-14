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

        async Task<List<SerialNumber>> ISerialNumber.GetSerialNumbersAsync(string serialNumber, string orderNo)
        {
            MessageBox.Show("" + serialNumber + " "+ orderNo);

            string apiUrl = $"{ConfigService.ApiBaseUrl}SerialNumber/getSerialNumbersForBarcodePrint";

            var payload = new { serialNumber = serialNumber, orderNo= orderNo };
            string jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl, content);
            response.EnsureSuccessStatusCode();

            string responseJson = await response.Content.ReadAsStringAsync();
            var serialNumbers = JsonConvert.DeserializeObject<List<SerialNumber>>(responseJson);

            return serialNumbers ?? new List<SerialNumber>();
        }
    }
}
