using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class RevenueLineItem
    {
        [Key]
        public Guid id { get; set; }

        [ForeignKey("revenue")]
        public Guid revenueId { get; set; }
        public Revenue revenue { get; set; }

        [ForeignKey("billingLineItem")]
        public Guid billingLineItemId { get; set; }
        public BillingLineItem billingLineItem { get; set; }

        [ForeignKey("debitAccount")]
        public Guid debitAccountId { get; set; }
        public GLAccount debitAccount { get; set; }

        [ForeignKey("creditAccount")]
        public Guid creditAccountId { get; set; }
        public GLAccount creditAccount { get; set; }

        [MaxLength(200)]
        public string description { get; set; }

        public decimal amount { get; set; }

        [MaxLength(1)]
        public string cashOrCheck { get; set; }

        //public CheckDetails checkDetails { get; set; }

        [MaxLength(300)]
        public string bankName { get; set; }

        [MaxLength(300)]
        public string branch { get; set; }

        public DateTime? checkDate { get; set; }

    }
}
