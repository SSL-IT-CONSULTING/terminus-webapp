using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class Billing:TBase
    {
        [Key]
        public Guid billId { get; set; }

        [MaxLength(500)]
        public string billRefId { get; set; }

        public DateTime dateDue { get; set; }

        public decimal amountDue { get; set; }

        [MaxLength(12)]
        public string status { get; set; }

        [ForeignKey("propertyDirectory")]
        public Guid propertyDirectoryId { get; set; }
        public PropertyDirectory propertyDirectory { get; set; }

        [ForeignKey("company")]
        [MaxLength(10)]
        public string companyId { get; set; }
        public Company company { get; set; }

    }
}
