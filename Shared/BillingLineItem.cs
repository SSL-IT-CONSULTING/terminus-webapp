using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class BillingLineItem
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(200)]
        public string description { get; set; }

        public decimal amount { get; set; }

        public int lineNo { get; set; }

        [ForeignKey("billing")]
        public Guid billingId { get; set; }
        public Billing billing { get; set; }

        public bool generated { get; set; }

        public decimal amountPaid { get; set; }

        [MaxLength(40)]
        public string billLineType { get; set; }

    }
}
