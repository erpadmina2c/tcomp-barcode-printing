using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcomp_barcode_printing.Models
{
    public  class SerialNumber
    {

        public int sn_id { get; set; }

        public int sn_order_no { get; set; }

        public string sn_serial_no { get; set; }

        public string sn_original_no { get; set; }

      
        public int sn_category { get; set; }

      
        public string sn_category_name { get; set; } = string.Empty;

     
        public int sn_make { get; set; }

      
        public string sn_make_name { get; set; } = string.Empty;

     
        public int sn_model { get; set; }


        public string sn_model_name { get; set; } = string.Empty;

       
        public int sn_processor { get; set; }

       
        public string sn_processor_name { get; set; } = string.Empty;

      
        public int sn_hard_disk { get; set; }

      
        public string sn_hard_disk_name { get; set; } = string.Empty;

       
        public int sn_hard_disk_size { get; set; }

       
        public int sn_ram_type { get; set; }

   
        public string sn_ram_type_name { get; set; } = string.Empty;

     
        public int sn_ram_size { get; set; }

      
        public decimal sn_purchase_price { get; set; }

      
        public string sn_message { get; set; } = string.Empty;

      
        public string sn_remarks { get; set; } = string.Empty;

     
        public string sn_received_yn { get; set; } = null;

      
        public int sn_received_by { get; set; }

       
        public string sn_received_by_name { get; set; }

      
        public DateTime sn_received_on { get; set; }

    
        public string sn_sold_yn { get; set; } = null;

      
        public int sn_so_id { get; set; } 

     
        public int sn_sold_by { get; set; }

     
        public string sn_sold_by_name { get; set; }

       
        public DateTime sn_sold_on { get; set; }

     
        public int sn_prod_status { get; set; }

     
        public string sn_prod_status_name { get; set; }

      
        public int sn_invoice_status { get; set; }

      
        public string sn_invoice_status_name { get; set; }

       
        public string sn_invoice_no { get; set; }

       
        public int sn_sales_category { get; set; }

       
        public string sn_sales_category_name { get; set; }

       
        public string sn_send_to_edniff { get; set; }

      
        public int sn_cre_by { get; set; }

       
        public string sn_cre_by_name { get; set; } = string.Empty;
      
        public DateTime sn_cre_date { get; set; } = DateTime.Now;
    }
}
