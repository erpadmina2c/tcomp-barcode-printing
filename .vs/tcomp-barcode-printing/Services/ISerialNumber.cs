using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcomp_barcode_printing.Models;

namespace tcomp_barcode_printing.Services
{
    public interface ISerialNumber
    {
        Task<List<ListSerialNumber>> GetSerialNumbersAsync(string orderNo, string serialNumber);

        Task<List<ListSerialNumber>> getSerialNumberForSpecificationTag(string serialNumber);
    }
}
