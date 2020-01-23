using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace terminus.shared.models
{
    public class JournalEntryViewModel
    {
        public Guid id { get; set; }

        public string description { get; set; }


        public string companyId { get; set; }
      
        [MaxLength(10)]
        public string source { get; set; }

        [MaxLength(36)]
        public string sourceId { get; set; }

        [Display(Name = "Entry Date")]
        public DateTime transactionDate { get; set; }


        [MaxLength(36)]
        public string detailId { get; set; }

        [Required]
        public decimal amount { get; set; }

        [MaxLength(1), Required]
        [Display(Name ="Debit/Credit")]
        public string type { get; set; }

        public int lineNumber { get; set; }

        public string accountId { get; set; }
        public string ledgerAccountId { get; set; }

        public string accountCode { get; set; }
        public string accountName { get; set; }

        [MaxLength(200), Required]
        [Display(Name ="Description")]
        public string detailDescription { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        public List<GLAccount> gLAccounts { get; set; }
            
        [Display(Name = "Posting Date")]
        public DateTime postingDate { get; set; }

        }
}