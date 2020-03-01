using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace terminus.shared.models
{
    public class rptGLDetailViewModel
    {

        public Guid id { get; set; }
        [MaxLength(200)]
        public string glaccount { get; set; }
        [MaxLength(50)]
        public string documentId { get; set; }
        public DateTime postingDate { get; set; }
        public DateTime transactionDate { get; set; }
        public Decimal DRAmount { get; set; }
        public Decimal CRAmount { get; set; }
        [MaxLength(500)]
        public string description { get; set; }
        [MaxLength(200)]
        public string reference { get; set; }
        [MaxLength(200)]
        public string remarks { get; set; }


    }
}
