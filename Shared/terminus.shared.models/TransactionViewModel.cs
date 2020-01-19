using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace terminus.shared.models
{
    public class TransactionViewModel:TBaseViewModel
    {
        [Required]
        public DateTime transactionDate { get; set; }

        [MaxLength(36), Required]
        public string glAccountId { get; set; }

        [MaxLength(120)]
        public string glAccountCode { get; set; }


        [MaxLength(1000)]
        public string glAccountName { get; set; }


        [MaxLength(200)]
        public string description { get; set; }

        public DateTime? dueDate { get; set; }

        [MaxLength(500)]
        public string remarks { get; set; }

        [Required]
        public decimal amount { get; set; }

        [MaxLength(100)]
        public string reference { get; set; }

        public string cashOrCheck { get; set; }

        [MaxLength(300)]
        public string bankName { get; set; }

        [MaxLength(300)]
        public string branch { get; set; }

        public DateTime? checkDate { get; set; }

        public decimal checkAmount { get; set; }

        [MaxLength(36), Required]
        public string cashAccountId { get; set; }

        [MaxLength(120)]
        public string cashAccountCode { get; set; }

        [MaxLength(1000)]
        public int cashAccountName { get; set; }


        public JournalEntryHdr journalEntry { get; set; }

        [MaxLength(36)]
        public string receiptNo { get; set; }

        public decimal taxAmount
        {
            get; set;

        }


    }
}
