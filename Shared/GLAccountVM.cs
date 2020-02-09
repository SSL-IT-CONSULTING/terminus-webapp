using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace terminus.shared.models
{
    public class GLAccountVM
    {

        [Key]
        public Guid accountId { get; set; }

        [MaxLength(120)]
        public string accountCode { get; set; }

        [MaxLength(1000)]
        public string accountDesc { get; set; }

        public decimal balance { get; set; }

        public string companyid { get; set; }

        public string revenue { get; set; }
        public string expense { get; set; }

        public string cashAccount { get; set; }
        public string outputVatAccount { get; set; }

        public int rowOrder { get; set; }
    }
}
