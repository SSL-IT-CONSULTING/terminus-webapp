using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class JournalEntryDtl : TBase
    {
        [Key, MaxLength(36)]
        public string id { get; set; }

        [Column(TypeName = "decimal(14,4)")]
        public decimal amount { get; set; }

        [MaxLength(1)]
        public string type { get; set; }

        public int lineNumber { get; set; }

        [ForeignKey("account")]
        public Guid accountId { get; set; }

        public GLAccount account { get; set; }

        [MaxLength(200)]
        public string description { get; set; }

        [MaxLength(1000)]
        public string remarks { get; set; }

        [MaxLength(100)]
        public string reference { get; set; }
    }
}
