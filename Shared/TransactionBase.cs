using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class TransactionBase:TBase
    {

        [ForeignKey("company")]
        [MaxLength(10)]
        public string companyId { get; set; }

        public Company company { get; set; }
        
        [ForeignKey("account")]
        public Guid accountId { get; set; }
        public GLAccount account { get; set; }

        public DateTime transactionDate { get; set; }

        [MaxLength(200)]
        public string description { get; set; }
        
        [MaxLength(500)]
        public string remarks { get; set; }

        public decimal amount { get; set; }

        [MaxLength(100)]
        public string reference { get; set; }

        [MaxLength(1)]
        public string cashOrCheck { get; set; }

        public CheckDetails checkDetails { get; set; }

        public GLAccount cashAccount { get; set; }

        public JournalEntryHdr journalEntry { get; set; }

        [MaxLength(36)]
        public string receiptNo { get; set; }

        public decimal taxAmount { get; set; }

        public DateTime? dueDate { get; set; }

        public decimal beforeTax { get; set; }
    }
}
