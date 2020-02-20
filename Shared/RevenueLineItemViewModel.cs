using System;
using System.ComponentModel.DataAnnotations;

namespace terminus.shared.models
{
    public class RevenueLineItemViewModel
    {
        public Guid Id { get; set; }

        [MaxLength(200)]
        public string description { get; set; }


        [Display(Name = "Debit")]
        [MaxLength(36), Required]
        public string debitAccountId { get; set; }

        [MaxLength(120)]
        public string debitAccountCode { get; set; }

        [MaxLength(1000)]
        public string debitAccountName { get; set; }

        [Display(Name = "Credit")]
        [MaxLength(36), Required]
        public string creditAccountId { get; set; }

        [MaxLength(120)]
        public string creditAccountCode { get; set; }

        [MaxLength(1000)]
        public string creditAccountName { get; set; }

        [Display(Name = "Balance")]
        public decimal billBalance { get; set; }

        [Display(Name = "Amount")]
        public decimal amount { get; set; }

        public string cashOrCheck { get; set; }

        [MaxLength(300)]
        public string bankName { get; set; }

        [MaxLength(300)]
        public string branch { get; set; }

        public DateTime? checkDate { get; set; }

        public decimal checkAmount { get; set; }

        public Guid billingLineItemId { get; set; }

        [Display(Name = "Amount")]
        public decimal amountApplied { get; set; }

        [MaxLength(40)]
        public string billLineType { get; set; }

    }
}