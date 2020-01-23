using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class GLAccount:TBase
    {
        [Key]
        public Guid accountId { get; set; }

        [MaxLength(120)]
        public string accountCode { get; set; }

        [MaxLength(1000)]
        public string accountDesc { get; set; }

        public decimal balance { get; set; }

        [ForeignKey("company")]
        [MaxLength(10)]
        public string companyId { get; set; }

        public Company company { get; set; }

        public bool revenue { get; set; }
        public bool expense { get; set; }

        public bool cashAccount { get; set; }
        public bool outputVatAccount { get; set; }

        public int rowOrder { get; set; }

    }
}
