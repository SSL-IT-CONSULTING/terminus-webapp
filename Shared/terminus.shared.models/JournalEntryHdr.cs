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

        [MaxLength(120)]
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


    }
}
