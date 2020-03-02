using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace terminus.shared.models
{
    public class BillingViewModel
    {
        [Key]
        public string billId { get; set; }
        [MaxLength(36)]
        public string documentId { get; set; }
        public DateTime dateDue { get; set; }
        public DateTime transactionDate { get; set; }
        [MaxLength(12)]
        public string status { get; set; }
        public string propertyDirectoryId { get; set; }
        public string propertyId { get; set; }
        public string propertyDesc { get; set; }
        public string tenantId { get; set; }
        public string tenantName { get; set; } 
        public decimal totalAmount { get; set; }
        public decimal amountPaid { get; set; }
        public decimal balance { get; set; }


        [MaxLength(10)]
        public string billType { get; set; }

        [MaxLength(8)]
        public string MonthYear { get; set; }

        public List<Property> properties { get; set; }

    }
}
