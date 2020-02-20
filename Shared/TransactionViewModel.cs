using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class TransactionViewModel:TBaseViewModel
    {
        [Required]
        public DateTime transactionDate { get; set; }

        [Display(Name = "Details")]
        [MaxLength(36), Required]
        public string glAccountId { get; set; }

        [MaxLength(120)]
        public string glAccountCode { get; set; }


        [MaxLength(1000)]
        public string glAccountName { get; set; }


        [MaxLength(200)]
        public string description { get; set; }

        [Required]
        [Display(Name ="Due date")]
        public DateTime? dueDate { get; set; }

        [MaxLength(500)]
        public string remarks { get; set; }

        //[Required]
        [Display(Name ="Amount")]
        public decimal amount { get; set; }

        [MaxLength(100),Required]

        [Display(Name = "Reference")]
        public string reference { get; set; }

        public string cashOrCheck { get; set; }

        [MaxLength(300)]
        public string bankName { get; set; }

        [MaxLength(300)]
        public string branch { get; set; }

        public DateTime? checkDate { get; set; }

        public decimal checkAmount { get; set; }

        [Display(Name = "Ledger")]
        [MaxLength(36), Required]
        public string cashAccountId { get; set; }

        [MaxLength(120)]
        public string cashAccountCode { get; set; }

        [MaxLength(1000)]
        public string cashAccountName { get; set; }


        public JournalEntryHdr journalEntry { get; set; }

        [MaxLength(36)]
        public string receiptNo { get; set; }

        //[Display(Name ="Billing Id")]
        //[MaxLength(36), Required]
        //public string billingDocumentId { get; set; }

        //[MaxLength(36)]
        //public string billingId { get; set; }

        public decimal taxAmount
        {
            get; set;

        }

        public decimal beforeTax { get; set; }


    }
}
