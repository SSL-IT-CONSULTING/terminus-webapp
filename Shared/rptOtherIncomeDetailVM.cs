using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace terminus.shared.models
{
   public class rptOtherIncomeDetailVM
    {

        public Guid id { get; set; }
        
        public DateTime transactionDate { get; set; }

        [MaxLength(200)]
        public string gLAccount { get; set; }
        public decimal amount { get; set; }
        [MaxLength(500)]
        public string  description { get; set; }
        [MaxLength(100)]
        public string  documentID { get; set; }
        [MaxLength(1000)]
        public string comments { get; set; }

    }
}
