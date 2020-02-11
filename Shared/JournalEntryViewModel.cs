using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace terminus.shared.models
{
    public class JournalEntryViewModel
    {
        public Guid id { get; set; }

        [MaxLength(200)]
        public string description { get; set; }


        public string companyId { get; set; }

        [MaxLength(10)]
        public string source { get; set; }

        [MaxLength(36)]
        public string sourceId { get; set; }

        [Display(Name = "Entry Date")]
        public DateTime transactionDate { get; set; }


        [MaxLength(200), Required]
        [Display(Name = "Reference")]
        public string reference { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        [Display(Name = "Posting Date")]
        public DateTime postingDate { get; set; }


        public List<JournalEntryDtlViewModel> journalEntryDtls { get;set;}

        [MaxLength(20)]
        public string documentId { get; set; }


    }
}