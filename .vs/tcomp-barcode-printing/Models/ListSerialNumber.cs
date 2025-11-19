using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcomp_barcode_printing.Models
{
    public class ListSerialNumber
    {
        [Key]
        [Display(Name = "ID")]
        public int id { get; set; }

        [Display(Name = "Order No")]
        public int order_no { get; set; }

        [Display(Name = "Original No")]
        public string original_no { get; set; }

        [Display(Name = "Serial Number")]
        public string serial_no { get; set; }

        [Display(Name = "Make")]
        public string make { get; set; }

        [Display(Name = "Model")]
        public string model { get; set; }

        [Display(Name = "Processor")]
        public string processor { get; set; }

        [Display(Name = "Hard Disk")]
        public string hard_disk { get; set; }

        [Display(Name = "Hard Disk Size")]
        public string hard_disk_size { get; set; }

        [Display(Name = "RAM Type")]
        public string ram_type { get; set; }

        [Display(Name = "RAM Size")]
        public string ram_size { get; set; }

        [Display(Name = "Send To TComp")]
        public string send_to_tcomp { get; set; }

        [Display(Name = "Created Date")]
        public DateTime cre_date { get; set; }

        [Display(Name = "Message")]
        public string message { get; set; } = string.Empty;
    }
}
