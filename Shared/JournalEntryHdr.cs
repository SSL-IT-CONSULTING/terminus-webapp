using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class JournalEntryHdr:TBase
    {
        [Key]
        public Guid id { get; set; }

        [MaxLength(200)]
        public string description { get; set; }

        public IEnumerable<JournalEntryDtl> JournalDetails { get; set; }

        [ForeignKey("company")]
        [MaxLength(10)]
        public string companyId { get; set; }
        public Company company { get; set; }

        [MaxLength(10)]
        public string source { get; set; }

        [MaxLength(36)]
        public string sourceId { get; set; }

        public DateTime transactionDate { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        public DateTime postingDate { get; set; }

        [MaxLength(100)]
        public string reference { get; set; }

        [MaxLength(20)]
        public string documentId { get; set; }



    }
}
