using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    [Table("Expenses")]

    public class Expense: TransactionBase

    {
        public Guid id { get; set; }

        [ForeignKey("vendor")]
        [MaxLength(36)]
        public string vendorId { get; set; }

        public Vendor vendor { get; set; }
        
        [MaxLength(200)]
        public string vendorOther { get; set; }

    }
}
