using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
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

        public async Task<List<ListSerialNumber>> getSerialNumberForSpecificationTag(string serialNumber)
        {
            return await Task.Run(() =>
            {
                List<ListSerialNumber> listserialNumbers = new List<ListSerialNumber>();

                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigService.ConnectionString))
                    using (SqlCommand cmd = new SqlCommand("getSerialNumberForSpecificationTag", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@serialno", serialNumber);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var sn = new ListSerialNumber
                                {
                                    id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                                    order_no = reader["order_no"] != DBNull.Value ? Convert.ToInt32(reader["order_no"]) : 0,
                                    original_no = reader["original_no"]?.ToString(),
                                    serial_no = reader["serial_no"]?.ToString(),
                                    make = reader["make"]?.ToString(),
                                    model = reader["model"]?.ToString(),
                                    processor = reader["processor"]?.ToString(),
                                    hard_disk = reader["hard_disk"]?.ToString(),
                                    hard_disk_size = reader["hard_disk_size"]?.ToString(),
                                    ram_type = reader["ram_type"]?.ToString(),
                                    ram_size = reader["ram_size"]?.ToString(),
                                    send_to_tcomp = reader["send_to_tcomp"]?.ToString(),
                                    keyboard = reader["keyboard"]?.ToString(),
                                    lcd = reader["lcd"]?.ToString(),
                                    cre_date = reader["cre_date"] != DBNull.Value
                                                ? Convert.ToDateTime(reader["cre_date"])
                                                : DateTime.MinValue,
                                    message = reader["message"]?.ToString() ?? "Success"
                                };

                                listserialNumbers.Add(sn);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                return listserialNumbers;
            });
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
