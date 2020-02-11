using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class Billing:TBase
    {
        [Key]
        public Guid billId { get; set; }

        [MaxLength(36)]
        public string documentId { get; set; }

        public DateTime dateDue { get; set; }

        public DateTime transactionDate { get; set; }

        [MaxLength(12)]
        public string status { get; set; }

        [ForeignKey("propertyDirectory")]
        public Guid propertyDirectoryId { get; set; }
        public PropertyDirectory propertyDirectory { get; set; }

        [ForeignKey("company")]
        [MaxLength(10)]
        public string companyId { get; set; }
        public Company company { get; set; }

        public decimal totalAmount { get; set; }
        public decimal amountPaid { get; set; }
        public decimal balance { get; set; }

        public IEnumerable<Revenue> collections { get; set; }

        public ICollection<BillingLineItem> billingLineItems { get; set; }

    }
}
