using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class PropertyDirectory:TBase
    {
        public Guid id { get; set; }

        [ForeignKey("property")]
        [MaxLength(36)]
        public string propertyId { get; set; }
        public Property property { get; set; }
        
        [MaxLength(36)]
        [ForeignKey("tenant")]
        public string tenandId { get; set; }
        public Tenant tenant { get; set; }

        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }

        [ForeignKey("company")]
        [MaxLength(10)]
        public string companyId { get; set; }
        public Company company { get; set; }

        public decimal monthlyRate { get; set; }
        [ForeignKey("revenueAccount")]
        public Guid? revenueAccountId { get; set; }

        public GLAccount revenueAccount { get; set; }

        [MaxLength(12)]
        public string status { get; set; }


    }
}
