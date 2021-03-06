using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace terminus.shared.models
{
    public class rptRevenue
    {

        public Guid id { get; set; }

        [MaxLength(100)]
        public string documentID { get; set; }

        public DateTime transactionDate { get; set; }

        public DateTime postingDate { get; set; }

        [MaxLength(1000)]
        public string description { get; set; }
        [MaxLength(1000)]
        public string reference { get; set; }
        [MaxLength(1000)]
        public string remarks { get; set; }
        [MaxLength(100)]
        public string gLAccount { get; set; }

        public decimal amount { get; set; }

    }
}
