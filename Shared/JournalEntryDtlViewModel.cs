using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace terminus.shared.models
{
    public class JournalEntryDtlViewModel
    {
       
            [Key, MaxLength(36)]
            public string id { get; set; }

            [Required]
            [Display(Name = "Amount")]
            public decimal amount { get; set; }

            [MaxLength(1), Required]
            [Display(Name = "Debit/Credit")]
            public string type { get; set; }

            public int lineNumber { get; set; }

            [Required]
            [Display(Name = "GL Account")]
            public string accountId { get; set; }

            public string accountCode { get; set; }

            public string accountName { get; set; }
 
            public string description { get; set; }

            public string remarks { get; set; }

            public string reference { get; set; }

    }
}
